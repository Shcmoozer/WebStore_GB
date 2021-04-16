using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    //[Route("staff")]
    [Authorize]
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

        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        #region Edit

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            if (id <= 0) return BadRequest();

            var employee = _EmployeesData.Get((int)id);

            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.LastName,
                LastName = employee.FirstName,
                MiddleName = employee.Patronymic,
                Age = employee.Age,
                Salary = employee.Salary,
                Phone = employee.Phone
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            //if(ModelState.IsValid)

            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (model.Name == "Усама" && model.MiddleName == "бен" && model.LastName == "Ладен")
                ModelState.AddModelError("", "Террористов не берем!");

            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = model.Id,
                    FirstName = model.LastName,
                    LastName = model.Name,
                    Patronymic = model.MiddleName,
                    Age = model.Age,
                    Salary = model.Salary,
                    Phone = model.Phone
                };

                if (employee.Id == 0)
                    _EmployeesData.Add(employee);
                else
                    _EmployeesData.Update(employee);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        #endregion


        #region Delete

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var employee = _EmployeesData.Get(id);

            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                MiddleName = employee.Patronymic,
                Age = employee.Age,
                Salary = employee.Salary,
                Phone = employee.Phone
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
