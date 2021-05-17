using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;
using WebStore.ViewModels;

namespace WebStore.Components
{
    //[ViewComponent(Name = "Название")]
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(string SectionId)
        {
            var section_id = int.TryParse(SectionId, out var id) ? id : (int?)null;

            var sections = GetSections(section_id, out var parent_section_id);

            ViewBag.SectionId = section_id;
            //ViewBag.ParentSectionId = parent_section_id;
            ViewData["ParentSectionId"] = parent_section_id;

            return View(new SelectableSectionsViewModel()
            {
                Sections = sections,
                SectionId = section_id,
                ParentSectionId = parent_section_id
            });
        }

        //public async Task<IViewComponentResult> InvokeAsync() => View();

        private IEnumerable<SectionViewModel> GetSections(int? SectionId, out int? ParentSectionId)
        {
            ParentSectionId = null;

            var sections = _ProductData.GetSections();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_sections_views = parent_sections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                    ProductsCount = s.ProductsCount,
                })
                .ToList();

            int OrderSortMethod(SectionViewModel a, SectionViewModel b) => Comparer<int>.Default.Compare(a.Order, b.Order);
            foreach (var parent_section in parent_sections_views)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);

                foreach (var child_section in childs)
                {
                    if (child_section.Id == SectionId)
                        ParentSectionId = child_section.ParentId;

                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        Parent = parent_section,
                        ProductsCount = child_section.ProductsCount,
                    });
                }
                    

                parent_section.ChildSections.Sort(OrderSortMethod);
            }
            parent_sections_views.Sort(OrderSortMethod);

            return parent_sections_views;
        }
    }
}
