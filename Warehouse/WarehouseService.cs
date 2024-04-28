namespace GakkoAppVertical.Warehouse;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository warehouseRepository;
    
    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        this.warehouseRepository = warehouseRepository;
    }

    public void AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        warehouseRepository.AddProductWarehouse(productWarehouse);
    }
}
