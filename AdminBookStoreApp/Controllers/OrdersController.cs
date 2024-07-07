﻿using AdminBookStoreApp.Models;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AdminBookStoreApp.Controllers
{
    public class OrdersController : Controller
    {

        public OrdersController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string url = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetAllActiveOrders";
            HttpResponseMessage response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }

        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            string url = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetDetailsForOrder";
            var model = new
            {
                Id = id,
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            var data = response.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }

        public FileContentResult CreateInvoice(string id)
        {

            HttpClient client = new HttpClient();
            string url = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetDetailsForOrder";
            var model = new
            {
                Id = id,
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            var result = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.Email);

            StringBuilder sb = new StringBuilder();
            var total = 0.0;
            foreach (var item in result.bookInOrders)
            {
                sb.AppendLine("Book " + item.Book.Title + " has quantity " + item.Quantity + " with price " + item.Book.Price);
                total += item.Quantity * item.Book.Price;
            }
            document.Content.Replace("{{ProductList}}", sb.ToString());

            document.Content.Replace("{{TotalPrice}}", total.ToString() + "$");

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());
            //document.Save(stream, new DocxSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        public IActionResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");
                worksheet.Cell(1, 1).Value = "OrderID";
                worksheet.Cell(1, 2).Value = "Customer UserName";
                worksheet.Cell(1, 3).Value = "Total Price";
                HttpClient client = new HttpClient();
                string URL = "https://bookstoreweb20240707160345.azurewebsites.net/api/Admin/GetAllActiveOrders";

                HttpResponseMessage response = client.GetAsync(URL).Result;
                var data = response.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 0; i < data.Count(); i++)
                {
                    var item = data[i];
                    worksheet.Cell(i + 2, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = item.User.Email;
                    var total = 0.0;
                    for (int j = 0; j < item.bookInOrders.Count(); j++)
                    {
                        worksheet.Cell(1, 4 + j).Value = "Book - " + (j + 1);
                        worksheet.Cell(i + 2, 4 + j).Value = item.bookInOrders.ElementAt(j).Book.Title;
                        total += (item.bookInOrders.ElementAt(j).Quantity * item.bookInOrders.ElementAt(j).Book.Price);
                    }
                    worksheet.Cell(i + 2, 3).Value = total;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

    }
}
