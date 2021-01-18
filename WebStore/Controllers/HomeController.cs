using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee
            {
                Id = 1,
                LastName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                Age = 37,
                Phone = "79097897986",
                Salary = 5700
            },
            new Employee
            {
                Id = 2,
                LastName = "Петров",
                FirstName = "Петр",
                Patronymic = "Петрович",
                Age = 19,
                Phone = "79385238515",
                Salary = 3200
            },
            new Employee
            {
                Id = 3,
                LastName = "Сидоров",
                FirstName = "Сидор",
                Patronymic = "Сидорович",
                Age = 26,
                Phone = "79236358565",
                Salary = 7300
            }
        };

        public IActionResult Index() => View(/*"SecondView"*/);

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }

        public IActionResult Employees()
        {
            return View(__Employees);
        }
    }
}
