using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myWebApp.services;
using myWebApp.models;
namespace myWebApp.controller
{
     //[Route("api/[controller]")]
     [Route("[controller]")]

        [ApiController]
    public class productsController : ControllerBase
    {
        public productsController(JsonFileProductService productService){
            this.productService = productService;
        }

        public JsonFileProductService productService{get;}

        [HttpGet]
        public IEnumerable<Product> Get(){
                return productService.GetProducts();
        }

        [Route("Rate")]
        [HttpGet]
        public ActionResult Get(
            [FromQuery] string ProductId,
            [FromQuery] int Rating)
            {
            productService.AddRating(ProductId, Rating);
            return Ok();
        }

    }
}