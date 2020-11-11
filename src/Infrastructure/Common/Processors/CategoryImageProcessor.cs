using System;
using System.IO;
using Application.Common.Interfaces;
using System.Drawing;
using System.Linq;
using Application.Categories.Queries.CategoryDetail;

namespace Infrastructure.Common.Processors
{
    public class CategoryImageProcessor : ICategoryImageProcessor
    {
        public const int BrokenBytesAmount = 78;

        public void Process(CategoryDetailViewModel category)
        {
            if (!IsValidImage(category.Picture)) {

                category.Picture = category.Picture.Skip(BrokenBytesAmount).ToArray();
            }
        }

        private bool IsValidImage(byte[] bytes)
        {
            try {
                using var ms = new MemoryStream(bytes);
                Image.FromStream(ms);
            } catch (ArgumentException) {
                return false;
            }

            return true;
        }
    }
}