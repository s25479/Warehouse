using Microsoft.AspNetCore.Mvc;

namespace GakkoAppVertical.Warehouse;

public interface IWarehouseService
{
    Task<IActionResult> AddProductWarehouse(ProductWarehouse productWarehouse);
}
