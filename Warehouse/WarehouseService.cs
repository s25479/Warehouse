using Microsoft.AspNetCore.Mvc;

namespace GakkoAppVertical.Warehouse;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository warehouseRepository;
    
    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        this.warehouseRepository = warehouseRepository;
    }

    public async Task<IActionResult> AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        var product = await warehouseRepository.GetProduct(productWarehouse.IdProduct.Value);
        if (product == null)
            return BadRequest();

        if (!await warehouseRepository.HasWarehouse(productWarehouse.IdWarehouse.Value))
            return BadRequest();

        if (productWarehouse.Amount.Value <= 0)
            return BadRequest();

        var order = await warehouseRepository.GetOrderCreatedBefore(
            productWarehouse.IdProduct.Value,
            productWarehouse.Amount.Value,
            productWarehouse.CreatedAt);
        if (order == null || order.FulfilledAt != null)
            return BadRequest();

        if (await warehouseRepository.HasOrderedProductInWarehouse(order.IdOrder))
            return BadRequest();

        return new ObjectResult(warehouseRepository.AddProductToWarehouse(productWarehouse.IdWarehouse.Value, productWarehouse.IdProduct.Value, order.IdOrder, productWarehouse.Amount.Value, product.Price)){StatusCode = StatusCodes.Status200OK};
    }

    private ObjectResult BadRequest()
    {
        return new ObjectResult(null){StatusCode = StatusCodes.Status400BadRequest};
    }
}
