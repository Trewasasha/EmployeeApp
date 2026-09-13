using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EmployeeApp.Models;

namespace EmployeeApp
{
    public class EmployeeService
    {
        private static EmployeeService instance;
        private ObservableCollection<Employee> employees;
        private List<Company> companies;
        private List<Position> positions;
        private int nextId;

        private EmployeeService()
        {
            InitializeData();
        }

        public static EmployeeService Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmployeeService();
                return instance;
            }
        }

        private void InitializeData()
        {
            // Инициализация компаний
            companies = new List<Company>
            {
                new Company { Id = 1, Name = "Microsoft" },
                new Company { Id = 2, Name = "Google" },
                new Company { Id = 3, Name = "Apple" },
                new Company { Id = 4, Name = "Amazon" },
                new Company { Id = 5, Name = "Facebook" },
                new Company { Id = 6, Name = "Oracle" },
                new Company { Id = 7, Name = "IBM" },
                new Company { Id = 8, Name = "Другая" }
            };

            // Инициализация должностей
            positions = new List<Position>
            {
                new Position { Id = 1, Title = "Разработчик" },
                new Position { Id = 2, Title = "Тестировщик" },
                new Position { Id = 3, Title = "Менеджер" },
                new Position { Id = 4, Title = "Дизайнер" },
                new Position { Id = 5, Title = "Аналитик" },
                new Position { Id = 6, Title = "Директор" },
                new Position { Id = 7, Title = "Специалист" }
            };

            // Инициализация сотрудников с корректными данными
            employees = new ObservableCollection<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "Иван",
                    LastName = "Петров",
                    MiddleName = "Сергеевич",
                    BirthDate = new DateTime(1990, 5, 15),
                    Company = "Microsoft",
                    Position = "Разработчик",
                    PhoneNumber = "+7(999)123-45-67"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Анна",
                    LastName = "Иванова",
                    MiddleName = "Алексеевна",
                    BirthDate = new DateTime(1985, 8, 20),
                    Company = "Google",
                    Position = "Менеджер",
                    PhoneNumber = "+7(888)987-65-43"
                },
                new Employee
                {
                    Id = 3,
                    FirstName = "Сергей",
                    LastName = "Смирнов",
                    MiddleName = "Николаевич",
                    BirthDate = new DateTime(1992, 3, 10),
                    Company = "Apple",
                    Position = "Тестировщик",
                    PhoneNumber = "+7(777)456-78-90"
                }
            };

            nextId = employees.Max(e => e.Id) + 1;
        }

        public ObservableCollection<Employee> GetEmployees()
        {
            return employees;
        }

        public List<Company> GetCompanies()
        {
            return companies;
        }

        public List<Position> GetPositions()
        {
            return positions;
        }

        public void AddEmployee(Employee employee)
        {
            employee.Id = nextId++;
            employees.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            var existing = employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existing != null)
            {
                existing.FirstName = employee.FirstName;
                existing.LastName = employee.LastName;
                existing.MiddleName = employee.MiddleName;
                existing.BirthDate = employee.BirthDate;
                existing.Company = employee.Company;
                existing.Position = employee.Position;
                existing.PhoneNumber = employee.PhoneNumber;
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                employees.Remove(employee);
            }
        }
    }
}