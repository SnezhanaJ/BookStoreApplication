using AdminBookStoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AdminBookStoreApp.Controllers
{
    public class PublishersController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetAllPublishers";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Publisher>>().Result;
            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetPublishersDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Publisher>().Result;


            return View(result);

        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetPublishersDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Publisher>().Result;


            return View(result);

        }


        [HttpPost]
        public IActionResult Update(Publisher author)
        {
            HttpClient client = new HttpClient();
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/EditPublisher";

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

        public IActionResult Delete(string id)
        {

            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/DeletePublisher";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Publisher>().Result;
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Publisher author)
        {
            HttpClient client = new HttpClient();
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/CreatePublisher";

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
