using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    public class WorkSpacesController : ODataController
    {
        private readonly AppDbContext _context;

        public WorkSpacesController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery(MaxExpansionDepth = 3)] 
        public IQueryable<WorkSpace> Get()
        {
            return _context.WorkSpaces.AsNoTracking();
        }

        [EnableQuery]
        public SingleResult<WorkSpace> Get([FromODataUri] int key)
        {
            var result = _context.WorkSpaces.Where(w => w.Id == key);
            return SingleResult.Create(result);
        }
    }
}