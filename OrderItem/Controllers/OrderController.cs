using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderItem.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderItem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/<OrderController>
        [HttpPost("{id}")]
        public Cart Post(int id, [FromBody]Object o)
        {
            string Token = HttpContext.Request.Headers["Authorization"].Single().Split(" ")[1];
            string menuItemName="";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                client.BaseAddress = new Uri("http://40.88.248.210/api/MenuItem/");
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // return JsonConvert.DeserializeObject<MyClass>(await response.Content.ReadAsStringAsync());
                    readTask.Wait();
                    //  return readTask.Result;
                    menuItemName = readTask.Result;
                }
            }
            if (menuItemName == null)
                return null;
            Cart cart = new Cart() { id = 1, userId = 1, menuItemId = id, menuItemName = menuItemName};
            return cart;
        }
    }
}
