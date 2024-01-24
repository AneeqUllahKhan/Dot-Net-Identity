using TodoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace TodoApp.Controllers
{
[Authorize]
    public class TodosController : Controller
    {
        private readonly ApplicationDBContext dbContext;

        public TodosController(ApplicationDBContext dbContext)
        {
         this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve only the records associated with the current user
            var todos = await dbContext.Todos
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return View(todos);

        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Add(AddTodoViewModel addtodoRequest ) {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var todos = new Models.Domain.Todo()
            {
                Id = Guid.NewGuid(),
                Task = addtodoRequest.Task,
                // Set the UserId property
                UserId = userId
            };

            await dbContext.Todos.AddAsync(todos);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]

        public async Task<IActionResult> View(Guid id) 
        {

            var todos = await dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todos != null) {

                var viewModel = new UpdateTodoViewModel()
                {
                    Id = todos.Id,
                    Task = todos.Task
                };
                return await Task.Run(()=>View("View", viewModel));
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> View(UpdateTodoViewModel model)
        {
            var todos = await dbContext.Todos.FindAsync(model.Id);
            if (todos != null)
            {
                todos.Task = model.Task;

                await dbContext.SaveChangesAsync();
                

                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateTodoViewModel model)
        {
            var todos =  await dbContext.Todos.FindAsync(model.Id);
            if (todos != null)
            {
                dbContext.Todos.Remove(todos);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

    }
}
