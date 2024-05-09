using Microsoft.AspNetCore.Mvc;

namespace GakkoAppVertical.Warehouse;

public interface IWarehouseService
{
    Task<IActionResult> AddProductToWarehouse(ProductWarehouse productWarehouse);
    Task<IActionResult> AddProductToWarehouseStored(ProductWarehouse productWarehouse);
}
