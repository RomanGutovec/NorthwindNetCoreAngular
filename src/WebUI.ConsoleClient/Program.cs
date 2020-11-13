using Application.Products.Queries.ProductsList;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Categories.Queries.CategoriesList;

namespace WebUI.ConsoleClient
{
    class Program
    {
        private static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task<ProductsListViewModel> GetProductsAsync(string path)
        {
            ProductsListViewModel products = null;
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                products = await response.Content.ReadAsAsync<ProductsListViewModel>();
            }
            return products;
        }

        private static async Task<CategoriesListViewModel> GetCategoriesAsync(string path)
        {
            CategoriesListViewModel categories = null;
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                categories = await response.Content.ReadAsAsync<CategoriesListViewModel>();
            }
            return categories;
        }

        private static void ShowProducts(ProductsListViewModel productsVw)
        {
            Console.WriteLine($"_____________{nameof(ShowProducts)}_______________");
            foreach (var product in productsVw.Products) {
                Console.WriteLine($"Name: {product.ProductName}\n" +
                                  $"Category: {product.CategoryName}");
            }
        }

        private static void ShowCategories(CategoriesListViewModel categoriesVm)
        {
            Console.WriteLine($"_____________{nameof(ShowCategories)}_______________");
            foreach (var category in categoriesVm.Categories) {
                Console.WriteLine($"Name: {category.Name}\n" +
                                  $"Description: {category.Description}");
            }
        }

        private static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var urlProducts = new Uri("https://localhost:5001/api/products");

                var products = await GetProductsAsync(urlProducts.ToString());
                ShowProducts(products);

                var urlCategories = new Uri("https://localhost:5001/api/categories");
                var categories = await GetCategoriesAsync(urlCategories.ToString());
                ShowCategories(categories);

            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}