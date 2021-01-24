using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("staff")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        //[Route("all")]
        public IActionResult Index() => View(_EmployeesData.Get());

        //[Route("Info(id:{id})")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.Get(id);
            if (employee is null) return NotFound();
            return View(employee);
        }

    }
}
