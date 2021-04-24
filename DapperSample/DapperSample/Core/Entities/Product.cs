using System;
using System.Text.Json.Serialization;

namespace DapperSample.Core.Entities
{
    public class Product
    {
        //[JsonIgnore]//hide results from request/reponse
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
