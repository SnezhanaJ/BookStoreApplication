using BookStore.Domain.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    public class PartnerPlatformController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://onlineeducationapp.azurewebsites.net/api/Admin/GetAllEnrollments";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Enrollment>>().Result;
            return View(data);
        }
    }
}
