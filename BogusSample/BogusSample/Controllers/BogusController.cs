using Bogus;
using BogusSample.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BogusSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BogusController : ControllerBase
    {
        private readonly ILogger<BogusController> _logger;
        public BogusController(ILogger<BogusController> logger)
        {
            _logger = logger;
            Randomizer.Seed = new Random(123456);
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var fakeUsers = new Faker<User>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.FirstName, f => f.Person.FirstName)
                .RuleFor(a => a.LastName, f => f.Person.LastName)
                .RuleFor(a => a.Username, f => f.Person.UserName)
                .RuleFor(a => a.Email, f => f.Person.Email)
                .RuleFor(a => a.FullName, (f, p) => $"{p.FirstName} {p.LastName}")
                .RuleFor(a => a.Avatar, f => f.Person.Avatar)
                .RuleFor(a => a.DateOfBirth, f => f.Person.DateOfBirth);
            
            var users = fakeUsers.Generate(10);
            return Ok(users);
        }
        [HttpGet("Products")]
        public IActionResult GetProducts()
        {
            var fakeProducts = new Faker<Product>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Name, (f, p) => f.Company.CompanyName())
                .RuleFor(a => a.Description, f => f.Lorem.Paragraph(1))
                .RuleFor(a => a.Price, f => f.Random.Decimal(1m, 1000m))
                .RuleFor(a => a.Created, f => f.Date.BetweenOffset(DateTimeOffset.Now.AddMonths(-1), DateTimeOffset.Now))
                .RuleFor(a => a.Currency, f => f.Finance.Currency().Code);

            var products = fakeProducts.Generate(10);
            return Ok(products);
        }
        [HttpGet("UserProducts")]
        public IActionResult GetUserProducts()
        {
            var fakeProducts = new Faker<Product>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Name, (f, p) => f.Company.CompanyName())
                .RuleFor(a => a.Description, f => f.Lorem.Paragraph(1))
                .RuleFor(a => a.Price, f => f.Random.Decimal(1m, 1000m))
                .RuleFor(a => a.Created, f => f.Date.BetweenOffset(DateTimeOffset.Now.AddMonths(-1), DateTimeOffset.Now))
                .RuleFor(a => a.Currency, f => f.Finance.Currency().Code);

            var products = fakeProducts.Generate(10);

            var fakeUsers = new Faker<User>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.FirstName, f => f.Person.FirstName)
                .RuleFor(a => a.LastName, f => f.Person.LastName)
                .RuleFor(a => a.Username, f => f.Person.UserName)
                .RuleFor(a => a.Email, f => f.Person.Email)
                .RuleFor(a => a.FullName, (f, p) => $"{p.FirstName} {p.LastName}")
                .RuleFor(a => a.Avatar, f => f.Person.Avatar)
                .RuleFor(a => a.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(a => a.Products, f => f.PickRandom(products, 2));

            var users = fakeUsers.Generate(3);

            return Ok(users);
        }
    }
}
