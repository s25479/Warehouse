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

    [HttpPost]
    public async Task<IActionResult> AddProductWarehouse([FromBody] ProductWarehouse productWarehouse)
    {
        try
        {
            return await warehouseService.AddProductWarehouse(productWarehouse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
