namespace GakkoAppVertical.Warehouse;

public interface IWarehouseRepository
{
    Task<Product> GetProduct(int productId);
    Task<bool> HasWarehouse(int warehouseId);
    Task<Order> GetOrderCreatedBefore(int productId, int amount, DateTime beforeDate);
    Task<bool> HasOrderedProductInWarehouse(int orderId);
}
