﻿using AdminBookStoreApp.Models;
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
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetAllAuthors";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Author>>().Result;
            return View(data);
        }

        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetAuthorDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Author>().Result;


            return View(result);

        }
        public IActionResult Edit(string id)
        {
            HttpClient client = new HttpClient();
            //added in next aud
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetAuthorDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Author>().Result;


            return View(result);

        }
        public IActionResult Update(Author author)
        {
            HttpClient client = new HttpClient();
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/EditAuthor";

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
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/DeleteAuthor";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Author>().Result;
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Author author)
        {
            HttpClient client = new HttpClient();
            string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/CreateAuthor";

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
