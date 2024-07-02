using AdminBookStoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace AdminBookStoreApp.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/GetAllBooks";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Books>>().Result;

            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://localhost:44305/api/Admin/GetBooksDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Books>().Result;

            HttpClient clientA = new HttpClient();
            string URLA = "https://localhost:44305/api/Admin/GetAllAuthors";

            HttpResponseMessage responseAuthors = client.GetAsync(URLA).Result;
            var data = responseAuthors.Content.ReadAsAsync<List<Author>>().Result;
            // Fetch the authors from the database
            var authors = data.Select(a => new {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName  // Combine first and last names
            }).ToList();

            // Create a SelectList with FullName as the text and Id as the value
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");


            HttpClient clientP = new HttpClient();
            string URLP = "https://localhost:44305/api/Admin/GetAllPublishers";

            HttpResponseMessage responsePublishers = clientP.GetAsync(URLP).Result;
            var publishers = responsePublishers.Content.ReadAsAsync<List<Publisher>>().Result;

            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");


            return View(result);

        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://localhost:44305/api/Admin/GetBooksDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Books>().Result;


            return View(result);

        }


        [HttpPost]
        public IActionResult Update(Books book)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/EditBooks";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the error response, for example, by displaying an error message
                ModelState.AddModelError(string.Empty, "Failed to update author");
                return View(book);
            }
        }

        public IActionResult Delete(string id)
        {

            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://localhost:44305/api/Admin/DeleteBooks";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Books>().Result;
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Create()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/GetAllAuthors";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Author>>().Result;
            // Fetch the authors from the database
            var authors = data.Select(a => new {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName  // Combine first and last names
            }).ToList();

            // Create a SelectList with FullName as the text and Id as the value
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");


            HttpClient clientP = new HttpClient();
            string URLP = "https://localhost:44305/api/Admin/GetAllPublishers";

            HttpResponseMessage responsePublishers = clientP.GetAsync(URLP).Result;
            var publishers = responsePublishers.Content.ReadAsAsync<List<Publisher>>().Result;

            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");
            return View();

        }
        [HttpPost]
        public IActionResult Create(Books author)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44305/api/Admin/CreateBooks";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the error response, for example, by displaying an error message
                ModelState.AddModelError(string.Empty, "Failed to create author");
                return View(author);
            }
        }
    }
}
