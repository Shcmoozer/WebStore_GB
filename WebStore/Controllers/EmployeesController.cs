using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("staff")]
    public class EmployeesController : Controller
    {
        private List<Employee> _Employees;

        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }

        //[Route("all")]
        public IActionResult Index() => View(_Employees);

        //[Route("Info(id:{id})")]
        public IActionResult Details(int id)
        {
            var employee = _Employees.FirstOrDefault(e => e.Id == id);
            if (employee is null) return NotFound();
            return View(employee);
        }

    }
}
