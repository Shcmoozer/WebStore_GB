using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _CartServices;

        public CartViewComponent(ICartService CartServices) => _CartServices = CartServices;

        public IViewComponentResult Invoke() => View(_CartServices.GetViewModel());
    }
}