﻿using ProductManagementApp.Models;
namespace ProductManagementApp.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }

}
