using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesApiController> _Logger;

        public EmployeesApiController(
            IEmployeesData EmployeesData,
            ILogger<EmployeesApiController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        [HttpGet] // http://localhost:5001/api/employees
        public IEnumerable<Employee> Get() => _EmployeesData.Get();

        [HttpGet("{id}")] // http://localhost:5001/api/employees/5
        public Employee Get(int id) => _EmployeesData.Get(id);

        [HttpGet("employee")] // http://localhost:5001/api/employees/employee?LastName=Иванов&FirstName=Иван&Patronymic=Иванович
        public Employee GetByName(string LastName, string FirstName, string Patronymic) => _EmployeesData.GetByName(LastName, FirstName, Patronymic);

        [HttpPost]
        public int Add(Employee employee)
        {
            _Logger.LogInformation("Добавление сотрудника {0}", employee);
            return _EmployeesData.Add(employee);
        }

        [HttpPost("employee")] // post -> http://localhost:5001/api/employees/employee?LastName=Иванов&FirstName=Иван&Patronymic=Иванович&Age=37
        public Employee Add(string LastName, string FirstName, string Patronymic, int Age)
        {
            _Logger.LogInformation("Добавление сотрудника {0} {1} {2} {3} лет",
                LastName, FirstName, Patronymic, Age);
            return _EmployeesData.Add(LastName, FirstName, Patronymic, Age);
        }

        //[HttpPut("{id}")]// put -> http://localhost:5001/api/employees/5
        [HttpPut] // put -> http://localhost:5001/api/employees/5
        public void Update( /*int id, */ Employee employee)
        {
            _Logger.LogInformation("Редактирование сотрудника {0}", employee);
            _EmployeesData.Update(employee);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            _Logger.LogInformation("Удаление сотрудника id:{0}...", id);
            var result = _EmployeesData.Delete(id);
            _Logger.LogInformation("Удаление сотрудника id:{0} - {1}",
                id, result ? "выполнено" : "не найден");
            return result;
        }
    }
}