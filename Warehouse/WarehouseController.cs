using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GakkoAppVertical.Warehouse;

[Route("api/warehouse")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private IWarehouseService warehouseService;
    
    public WarehouseController(IWarehouseService warehouseService)
    {
        this.warehouseService = warehouseService;
    }

    [HttpGet]
    public IActionResult AddProductWarehouse([FromBody] ProductWarehouse productWarehouse)
    {
        warehouseService.AddProductWarehouse(productWarehouse);
        return Ok();
    }

}
