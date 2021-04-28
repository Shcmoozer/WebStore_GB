using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly List<Employee> _Employees;
        private int _CurrentMaxId;

        public InMemoryEmployeesData()
        {
            _Employees = TestData.Employees;
            _CurrentMaxId = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 1);
        }

        public IEnumerable<Employee> Get() => _Employees;

        public Employee Get(int id) => _Employees.FirstOrDefault(employee => employee.Id == id);

        public Employee GetByName(string LastName, string FirstName, string Patronymic) =>
            _Employees.FirstOrDefault(e => e.LastName == LastName && e.FirstName == FirstName && e.Patronymic == Patronymic);

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            

            if (_Employees.Contains(employee)) return employee.Id;
            

            employee.Id = ++_CurrentMaxId;
            _Employees.Add(employee);

            return employee.Id;
        }

        public Employee Add(string LastName, string FirstName, string Patronymic, int Age)
        {
            var employee = new Employee
            {
                LastName = LastName,
                FirstName = FirstName,
                Patronymic = Patronymic,
                Age = Age
            };
            Add(employee);
            return employee;
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee)) return;

            var db_item = Get(employee.Id);
            if (db_item is null) return;

            db_item.LastName = employee.LastName;
            db_item.FirstName = employee.FirstName;
            db_item.Patronymic = employee.Patronymic;
            db_item.Age = employee.Age;
            db_item.Phone = employee.Phone;
            db_item.Salary = employee.Salary;

        }

        public bool Delete(int id)
        {
            var item = Get(id);
            if (item is null) return false;
            return _Employees.Remove(item);

        }
    }
}
