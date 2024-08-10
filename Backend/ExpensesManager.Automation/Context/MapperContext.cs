
using ExpensesManager.DB.Models;
using ExpensesManager.Automation.Repositories.Mapper;

namespace ExpensesManager.Automation;
public class MapperContext
{
    public IMapperRepository MapperRepository { get; set; }
    public MapperContext(IMapperRepository mapperRepository)
    {
        this.MapperRepository = mapperRepository;
    }
}
