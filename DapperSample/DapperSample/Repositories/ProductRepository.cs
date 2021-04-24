using Dapper;
using DapperSample.Core.Entities;
using DapperSample.Core.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperSample.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sqlserver");
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = "SELECT Id, Title, Description, Created FROM Products";

            var products = await connection.QueryAsync<Product>(sql);
            return products;
        }

        public async Task<Product> GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = "SELECT Id, Title, Description, Created FROM Products WHERE Id=@Id";

            var products = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            return products;
        }

        public async Task<int> Create(Product product)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"INSERT INTO Products (Title, Description, Created) 
                            VALUES (@Title, @Description, @Created)";

            var affectedRows = await connection.ExecuteAsync(sql, product);//new{Id=product.Id, Title=product.Title...}
            return affectedRows;
        }

        public async Task<int> Update(Product product)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"UPDATE Products SET Title=@Title, Description=@Description, Created=@Created
                            WHERE Id=@Id";

            var affectedRows = await connection.ExecuteAsync(sql, product);//new{Id=product.Id, Title=product.Title...}
            return affectedRows;
        }

        public async Task<int> Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"DELETE FROM Products WHERE Id=@Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows;
        }
    }
}
