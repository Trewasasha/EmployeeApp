using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EmployeeApp.Models
{
    public class Employee : INotifyPropertyChanged
    {
        private int id;
        private string firstName;
        private string lastName;
        private string middleName;
        private DateTime birthDate;
        private string company;
        private string position;
        private string phoneNumber;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        public string MiddleName
        {
            get => middleName;
            set
            {
                middleName = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                birthDate = value;
                OnPropertyChanged();
            }
        }

        public string Company
        {
            get => company;
            set
            {
                company = value;
                OnPropertyChanged();
            }
        }

        public string Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Employee Clone()
        {
            return new Employee
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                MiddleName = this.MiddleName,
                BirthDate = this.BirthDate,
                Company = this.Company,
                Position = this.Position,
                PhoneNumber = this.PhoneNumber
            };
        }
    }
}