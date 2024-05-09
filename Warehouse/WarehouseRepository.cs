using System.Data.SqlClient;
using System.Data;

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

    public async Task<int> AddProductToWarehouse(int warehouseId, int productId, int orderId, int amount, decimal productPrice)
    {
        DateTime currentDateTime = DateTime.Now;

        using var connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = "UPDATE \"Order\" SET FulfilledAt = @CurrentDateTime WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@CurrentDateTime", currentDateTime);
        cmd.Parameters.AddWithValue("@IdOrder", orderId);

        await connection.OpenAsync();
        var transaction = await connection.BeginTransactionAsync();
        cmd.Transaction = (SqlTransaction)transaction;  

        try
        {
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt); SELECT @@IDENTITY";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@IdWarehouse", warehouseId);
            cmd.Parameters.AddWithValue("@IdProduct", productId);
            cmd.Parameters.AddWithValue("@IdOrder", orderId);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@Price", productPrice * amount);
            cmd.Parameters.AddWithValue("@CreatedAt", currentDateTime);

            var insertedId = (int)(decimal) await cmd.ExecuteScalarAsync();
            await transaction.CommitAsync();
            return insertedId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> AddProductToWarehouseStored(ProductWarehouse productWarehouse)
    {
        using var connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand("AddProductToWarehouse", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync();
        return (int)(decimal) await cmd.ExecuteScalarAsync();
    }
}
