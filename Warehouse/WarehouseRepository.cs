using System.Data.SqlClient;

namespace GakkoAppVertical.Warehouse;

public class WarehouseRepository : IWarehouseRepository
{
    private IConfiguration configuration;
    
    public WarehouseRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
	public void AddProductWarehouse(ProductWarehouse productWarehouse)
    {
		
	}
}
