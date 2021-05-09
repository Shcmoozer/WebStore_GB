using System;

namespace WebStore.Domain.ViewModels
{
    public record ProductViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public decimal Price { get; init; }

        public string Brand { get; init; }
    }

    public class PageViewModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}