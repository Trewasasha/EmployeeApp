using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using EmployeeApp.Models;
using Xamarin.Forms;

namespace EmployeeApp.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private Employee currentEmployee;
        private Employee originalEmployee;
        private bool isEditMode;
        private string errorMessage;
        private List<string> companyNames;
        private List<string> positionTitles;
        private bool isBusy;

        private string firstNameError;
        private string lastNameError;
        private string middleNameError;
        private string birthDateError;
        private string phoneNumberError;

        public Employee CurrentEmployee
        {
            get => currentEmployee;
            set
            {
                if (currentEmployee != null)
                    currentEmployee.PropertyChanged -= OnEmployeePropertyChanged;

                currentEmployee = value;

                if (currentEmployee != null)
                    currentEmployee.PropertyChanged += OnEmployeePropertyChanged;

                OnPropertyChanged();
                ValidateAllFields();
            }
        }

        private void OnEmployeePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateAllFields();
        }

        public bool IsEditMode
        {
            get => isEditMode;
            set
            {
                isEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDeleteEnabled));
                ((Command)DeleteCommand)?.ChangeCanExecute();
            }
        }

        public bool IsDeleteEnabled => isEditMode;

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string FirstNameError
        {
            get => firstNameError;
            set
            {
                firstNameError = value;
                OnPropertyChanged();
            }
        }

        public string LastNameError
        {
            get => lastNameError;
            set
            {
                lastNameError = value;
                OnPropertyChanged();
            }
        }

        public string MiddleNameError
        {
            get => middleNameError;
            set
            {
                middleNameError = value;
                OnPropertyChanged();
            }
        }

        public string BirthDateError
        {
            get => birthDateError;
            set
            {
                birthDateError = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumberError
        {
            get => phoneNumberError;
            set
            {
                phoneNumberError = value;
                OnPropertyChanged();
            }
        }

        public List<string> CompanyNames
        {
            get => companyNames;
            set
            {
                companyNames = value;
                OnPropertyChanged();
            }
        }

        public List<string> PositionTitles
        {
            get => positionTitles;
            set
            {
                positionTitles = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
                ((Command)SaveCommand)?.ChangeCanExecute();
                ((Command)DeleteCommand)?.ChangeCanExecute();
            }
        }

        public bool IsFormValid
        {
            get
            {
                if (CurrentEmployee == null)
                    return false;

                bool isFirstNameValid = !string.IsNullOrWhiteSpace(CurrentEmployee.FirstName);
                bool isLastNameValid = !string.IsNullOrWhiteSpace(CurrentEmployee.LastName);
                bool isMiddleNameValid = !string.IsNullOrWhiteSpace(CurrentEmployee.MiddleName);
                bool isBirthDateValid = CurrentEmployee.BirthDate != default(DateTime) &&
                                       CurrentEmployee.BirthDate <= DateTime.Now;
                bool isPhoneValid = string.IsNullOrWhiteSpace(CurrentEmployee.PhoneNumber) ||
                                   ValidatePhoneNumber(CurrentEmployee.PhoneNumber);

                bool result = isFirstNameValid && isLastNameValid && isMiddleNameValid &&
                              isBirthDateValid && isPhoneValid;

                System.Diagnostics.Debug.WriteLine($"=== IsFormValid ===");
                System.Diagnostics.Debug.WriteLine($"Имя: {isFirstNameValid} ('{CurrentEmployee.FirstName ?? "null"}')");
                System.Diagnostics.Debug.WriteLine($"Фамилия: {isLastNameValid} ('{CurrentEmployee.LastName ?? "null"}')");
                System.Diagnostics.Debug.WriteLine($"Отчество: {isMiddleNameValid} ('{CurrentEmployee.MiddleName ?? "null"}')");
                System.Diagnostics.Debug.WriteLine($"Дата рождения: {isBirthDateValid} ({CurrentEmployee.BirthDate})");
                System.Diagnostics.Debug.WriteLine($"Телефон: {isPhoneValid} ('{CurrentEmployee.PhoneNumber ?? "null"}')");
                System.Diagnostics.Debug.WriteLine($"Результат: {result}");

                return result;
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public event EventHandler<Employee> SaveCompleted;
        public event EventHandler CancelCompleted;
        public event EventHandler DeleteCompleted;

        public EmployeeViewModel()
        {
            InitializeData();
            InitializeCommands();
        }

        private void InitializeData()
        {
            CompanyNames = EmployeeService.Instance.GetCompanies()
                                                  .Select(c => c.Name)
                                                  .ToList();

            PositionTitles = EmployeeService.Instance.GetPositions()
                                                   .Select(p => p.Title)
                                                   .ToList();

            System.Diagnostics.Debug.WriteLine($"Загружено компаний: {CompanyNames?.Count ?? 0}");
            System.Diagnostics.Debug.WriteLine($"Загружено должностей: {PositionTitles?.Count ?? 0}");

            CurrentEmployee = new Employee { BirthDate = DateTime.Now };
            IsEditMode = false;
            ClearErrors();
        }

        private void InitializeCommands()
        {
            SaveCommand = new Command(SaveEmployee, () => IsFormValid && !IsBusy);
            CancelCommand = new Command(CancelEdit);
            DeleteCommand = new Command(DeleteEmployee, () => IsEditMode && !IsBusy);
        }

        public void LoadEmployee(Employee employee)
        {
            if (employee != null)
            {
                originalEmployee = employee;
                CurrentEmployee = employee.Clone();
                IsEditMode = true;
                ErrorMessage = "";
                ClearErrors();
                ((Command)SaveCommand)?.ChangeCanExecute();
                ((Command)DeleteCommand)?.ChangeCanExecute();
            }
        }

        public void ResetForm()
        {
            CurrentEmployee = new Employee { BirthDate = DateTime.Now };
            originalEmployee = null;
            IsEditMode = false;
            ErrorMessage = "";
            ClearErrors();
            ((Command)SaveCommand)?.ChangeCanExecute();
            ((Command)DeleteCommand)?.ChangeCanExecute();
        }

        private void ClearErrors()
        {
            FirstNameError = "";
            LastNameError = "";
            MiddleNameError = "";
            BirthDateError = "";
            PhoneNumberError = "";
        }

        private void ValidateAllFields()
        {
            if (CurrentEmployee == null) return;

            // Валидация имени
            if (string.IsNullOrWhiteSpace(CurrentEmployee.FirstName))
            {
                FirstNameError = "Имя обязательно для заполнения";
            }
            else if (CurrentEmployee.FirstName.Length < 2)
            {
                FirstNameError = "Имя должно содержать минимум 2 символа";
            }
            else if (CurrentEmployee.FirstName.Length > 50)
            {
                FirstNameError = "Имя не должно превышать 50 символов";
            }
            else if (!Regex.IsMatch(CurrentEmployee.FirstName, @"^[\p{L}\s\-'\.]+$"))
            {
                FirstNameError = "Имя может содержать буквы, дефис, пробел и апостроф";
            }
            else
            {
                FirstNameError = "";
            }

            // Валидация фамилии
            if (string.IsNullOrWhiteSpace(CurrentEmployee.LastName))
            {
                LastNameError = "Фамилия обязательна для заполнения";
            }
            else if (CurrentEmployee.LastName.Length < 2)
            {
                LastNameError = "Фамилия должна содержать минимум 2 символа";
            }
            else if (CurrentEmployee.LastName.Length > 50)
            {
                LastNameError = "Фамилия не должна превышать 50 символов";
            }
            else if (!Regex.IsMatch(CurrentEmployee.LastName, @"^[\p{L}\s\-'\.]+$"))
            {
                LastNameError = "Фамилия может содержать буквы, дефис, пробел и апостроф";
            }
            else
            {
                LastNameError = "";
            }

            // Валидация отчества
            if (string.IsNullOrWhiteSpace(CurrentEmployee.MiddleName))
            {
                MiddleNameError = "Отчество обязательно для заполнения";
            }
            else if (CurrentEmployee.MiddleName.Length < 2)
            {
                MiddleNameError = "Отчество должно содержать минимум 2 символа";
            }
            else if (CurrentEmployee.MiddleName.Length > 50)
            {
                MiddleNameError = "Отчество не должно превышать 50 символов";
            }
            else if (!Regex.IsMatch(CurrentEmployee.MiddleName, @"^[\p{L}\s\-'\.]+$"))
            {
                MiddleNameError = "Отчество может содержать буквы, дефис, пробел и апостроф";
            }
            else
            {
                MiddleNameError = "";
            }

            // Валидация даты рождения
            if (CurrentEmployee.BirthDate == default(DateTime))
            {
                BirthDateError = "Дата рождения обязательна для заполнения";
            }
            else if (CurrentEmployee.BirthDate > DateTime.Now)
            {
                BirthDateError = "Дата рождения не может быть в будущем";
            }
            else if (CurrentEmployee.BirthDate < DateTime.Now.AddYears(-120))
            {
                BirthDateError = "Некорректная дата рождения";
            }
            else
            {
                BirthDateError = "";
            }

            // Валидация телефона
            if (!string.IsNullOrWhiteSpace(CurrentEmployee.PhoneNumber))
            {
                if (!ValidatePhoneNumber(CurrentEmployee.PhoneNumber))
                {
                    PhoneNumberError = "Введите номер в формате: +7(XXX)XXX-XX-XX";
                }
                else
                {
                    PhoneNumberError = "";
                }
            }
            else
            {
                PhoneNumberError = "";
            }

            ((Command)SaveCommand)?.ChangeCanExecute();
            ((Command)DeleteCommand)?.ChangeCanExecute();

            OnPropertyChanged(nameof(IsFormValid));

            System.Diagnostics.Debug.WriteLine($"=== ПОСЛЕ ВАЛИДАЦИИ ===");
            System.Diagnostics.Debug.WriteLine($"FirstNameError: '{FirstNameError}'");
            System.Diagnostics.Debug.WriteLine($"LastNameError: '{LastNameError}'");
            System.Diagnostics.Debug.WriteLine($"MiddleNameError: '{MiddleNameError}'");
            System.Diagnostics.Debug.WriteLine($"BirthDateError: '{BirthDateError}'");
            System.Diagnostics.Debug.WriteLine($"PhoneNumberError: '{PhoneNumberError}'");
            System.Diagnostics.Debug.WriteLine($"IsFormValid: {IsFormValid}");
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true;

            string pattern = @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private async void SaveEmployee()
        {
            ValidateAllFields();

            if (!IsFormValid)
            {
                ErrorMessage = "Заполните все обязательные поля корректно";
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = "";

                if (IsEditMode && originalEmployee != null)
                {
                    CurrentEmployee.Id = originalEmployee.Id;
                    EmployeeService.Instance.UpdateEmployee(CurrentEmployee);
                }
                else
                {
                    EmployeeService.Instance.AddEmployee(CurrentEmployee);
                }

                SaveCompleted?.Invoke(this, CurrentEmployee);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при сохранении: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CancelEdit()
        {
            CancelCompleted?.Invoke(this, EventArgs.Empty);
        }

        private async void DeleteEmployee()
        {
            if (originalEmployee == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Подтверждение удаления",
                $"Удалить сотрудника {originalEmployee.LastName} {originalEmployee.FirstName}?",
                "Да",
                "Нет");

            if (confirm)
            {
                try
                {
                    IsBusy = true;
                    EmployeeService.Instance.DeleteEmployee(originalEmployee.Id);
                    DeleteCompleted?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Ошибка при удалении: {ex.Message}";
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}