using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Throw() => throw new ApplicationException("Test error!");

        public IActionResult SecondAction(string id) => Content($"Action with value id:{id}");

        public IActionResult Error404() => View();

        public IActionResult ErrorStatus(string code) => code switch
        {
            "404" => RedirectToAction(nameof(Error404)),
            _ => Content($"Error code: {code}")
        };

        public IActionResult Blog() => View();

        public IActionResult BlogSingle() => View();

        //public IActionResult Cart() => View();

       // public IActionResult Checkout() => View();

        public IActionResult ContactUs() => View();

        public IActionResult Login() => View();

        public IActionResult ProductDetails() => View();

        //public IActionResult Shop() => View();

        //public IActionResult NotFound404() => View();


        public IActionResult Test() => Content("Test");
    }
}
