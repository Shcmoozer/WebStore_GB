﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View("SecondView");

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }
    }
}
