using EmployeeApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmployeeApp.ViewModel
{
    public class EmployeeListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employees;
        private Employee selectedEmployee;
        private bool isRefreshing;

        public ObservableCollection<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }

        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                OnPropertyChanged();
                if (selectedEmployee != null)
                {
                    EditEmployeeCommand?.Execute(selectedEmployee);
                }
            }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public event EventHandler AddEmployeeRequested;
        public event EventHandler<Employee> EditEmployeeRequested;

        public EmployeeListViewModel()
        {
            LoadEmployees();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddEmployeeCommand = new Command(OnAddEmployee);
            EditEmployeeCommand = new Command<Employee>(OnEditEmployee);
            RefreshCommand = new Command(RefreshEmployees);
        }

        private void LoadEmployees()
        {
            Employees = EmployeeService.Instance.GetEmployees();
        }

        private void OnAddEmployee()
        {
            AddEmployeeRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnEditEmployee(Employee employee)
        {
            if (employee != null)
            {
                EditEmployeeRequested?.Invoke(this, employee);
            }
        }

        private void RefreshEmployees()
        {
            IsRefreshing = true;
            LoadEmployees();
            IsRefreshing = false;
        }

        public void RefreshList()
        {
            LoadEmployees();
            OnPropertyChanged(nameof(Employees));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}