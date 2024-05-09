using System.Data.SqlClient;

namespace GakkoAppVertical.Warehouse;

public class WarehouseRepository : IWarehouseRepository
{
    private IConfiguration configuration;
    
    public WarehouseRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<Product> GetProduct(int productId)
    {
        using var connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = "SELECT * FROM Product WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", productId);

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Product
            {
                IdProduct = productId,
                Price = (Decimal)reader["Price"]
            };
        }
        return null!;
    }

    public async Task<bool> HasWarehouse(int warehouseId)
    {
        using var connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);                
        using var cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", warehouseId);

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        return reader.HasRows;
    }

    public async Task<Order> GetOrderCreatedBefore(int productId, int amount, DateTime beforeDate)
    {
        using var connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = "SELECT * FROM \"Order\" WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @BeforeDate";
        cmd.Parameters.AddWithValue("@IdProduct", productId);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@BeforeDate", beforeDate);

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Order
            {
                IdOrder = (int)reader["IdOrder"],
                FulfilledAt = (DateTime)reader["FulfilledAt"]
            };
        }
        return null!;
    }

    public async Task<bool> HasOrderedProductInWarehouse(int orderId)
    {
        using var connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);                
        using var cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@IdOrder", orderId);

        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        return reader.HasRows;
    }
}
