using AdminBookStoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AdminBookStoreApp.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/GetAllAuthors";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Author>>().Result;
            return View(data);
        }
        public IActionResult Edit(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://localhost:44305/api/Admin/GetAuthorDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Author>().Result;


            return View(result);

        }

        [HttpPost]
        public IActionResult Edit(Author author)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/EditAuthor";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the error response, for example, by displaying an error message
                ModelState.AddModelError(string.Empty, "Failed to update author");
                return View(author);
            }
        }

    }
}
