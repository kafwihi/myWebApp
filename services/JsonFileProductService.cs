using System.Text.Json;
using myWebApp.models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//@using Microsoft.AspNetCore.Mvc.ViewEngines
//@inject IWebHostEnvironment Environment
//@inject ICompositeViewEngine Engine
namespace myWebApp.services
{
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment){
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment{get;}

        private string JsonFileName{
            get {return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");
            }
         }

        public IEnumerable<Product> GetProducts(){
            using (var JsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<Product[]>(JsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions{
                        PropertyNameCaseInsensitive = true
                    }
                );

            }
        }

        public void AddRating(string productId, int rating){
            var products = GetProducts();

            var query = products.First(x => x.Id == productId);
            if(query.Ratings == null){
                query.Ratings = new int[] {rating};
            }
            else
            {
                var ratings = query.Ratings.ToList();
                ratings.Add(rating);
                query.Ratings = ratings.ToArray();

            }

            using(var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<Product>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }
                    ),
                    products
                );
            }
        }
    }
}