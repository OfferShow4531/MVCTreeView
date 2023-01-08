using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTreeView.Data;
using MVCTreeView.Models;
using MVCTreeView.Models.MenuModels;

namespace MVCTreeView.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationDbContext locationDbContext;
        public LocationController(LocationDbContext locationDbContext) 
        {
            this.locationDbContext = locationDbContext;          
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await locationDbContext.locations.ToListAsync();
            return View(employees);
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddLocations addLocationRequest)
        {
            var location = new Location()
            {
                Name= addLocationRequest.Name,
                Description= addLocationRequest.Description,
                ParentId = addLocationRequest.ParentId,
                MenuNumber = addLocationRequest.MenuNumber,

            };
            await locationDbContext.locations.AddAsync(location);
            await locationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int Id) 
        {
            var loc = await locationDbContext.locations.FirstOrDefaultAsync(x => x.Id == Id);
            if(loc != null)
            {
                var viewModel = new UpdateLocations()
                {
                    Id = loc.Id,
                    Name = loc.Name,
                    Description = loc.Description,
                    ParentId = loc.ParentId,
                    MenuNumber = loc.MenuNumber,
                };
                return await Task.Run(() => View("View", viewModel));
            }

           

            return RedirectToAction("Index");
        }
        [HttpPost] 
        public async Task<IActionResult> View(UpdateLocations updateLocations)
        {
            var loc = await locationDbContext.locations.FindAsync(updateLocations.Id);
            if(loc != null)
            {
                loc.Name = updateLocations.Name;
                loc.Description = updateLocations.Description;
                loc.ParentId = updateLocations.ParentId;
                loc.MenuNumber = updateLocations.MenuNumber;

                await locationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateLocations updateLocations)
        {
            var loc = await locationDbContext.locations.FindAsync(updateLocations.Id);
            if(loc != null)
            {
                locationDbContext.locations.Remove(loc);
                await locationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
