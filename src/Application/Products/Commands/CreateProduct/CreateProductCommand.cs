using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
    }
}
