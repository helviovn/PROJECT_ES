

using Dapper;
using Microsoft.Data.SqlClient;
using PROJECT_ES.Data;

namespace PROJECT_ES.Service;

public class CategoryRepository
{
    private readonly string _connectionString;

    public CategoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task AddCategoryAsync(String name)
    {
        //query para inserir na base de dados as categorias
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "INSERT INTO dbo.Category (Name) " +
                        "VALUES (@Name);" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new
            {
                Name = name
            };

            var id = await connection.ExecuteScalarAsync<int>(query,parameters);

            
        }
    }
    
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Category";
        
            var categories = await connection.QueryAsync<Category>(query);

            return categories;
        }
    }
    
    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT * FROM dbo.Category WHERE Id = @Id";
            
            var parameters = new { Id = id };
        
            var category = await connection.QueryFirstOrDefaultAsync<Category>(query, parameters);

            return category;
        }
    }
    
    
    public async Task DeleteMovieAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "DELETE FROM dbo.Category WHERE Id = @Id";

            var parameters = new { Id = id };

            await connection.ExecuteAsync(query, parameters);
        }
    }
    
    public async Task UpdateCategoryAsync(int id, string newName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
        
            var query = "UPDATE dbo.Category SET Name = @Name WHERE Id = @Id";
        
            await connection.ExecuteAsync(query, new { Id = id, Name = newName });
        }
    }



    
    
    
}