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
    public async Task<IActionResult> AddProductToWarehouse([FromBody] ProductWarehouse productWarehouse)
    {
        try
        {
            return await warehouseService.AddProductToWarehouse(productWarehouse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("stored/")]
    public async Task<IActionResult> AddProductToWarehouseStored([FromBody] ProductWarehouse productWarehouse)
    {
        try
        {
            return await warehouseService.AddProductToWarehouseStored(productWarehouse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
