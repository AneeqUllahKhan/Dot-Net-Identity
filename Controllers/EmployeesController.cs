﻿using TodoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace TodoApp.Controllers
{
[Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDBContext dbContext;

        public EmployeesController(ApplicationDBContext dbContext)
        {
         this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve only the records associated with the current user
            var employees = await dbContext.Employees
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return View(employees);

        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Add(AddEmployeeViewModel addemployeeRequest ) {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var employee = new Models.Domain.Employee()
            {
                Id = Guid.NewGuid(),
                Name = addemployeeRequest.Name,
                Email = addemployeeRequest.Email,
                Salary = addemployeeRequest.Salary,
                Department = addemployeeRequest.Department,
                DateOfBirth = addemployeeRequest.DateOfBirth,
                // Set the UserId property
                UserId = userId
            };

            await dbContext.Employees.AddAsync(employee);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]

        public async Task<IActionResult> View(Guid id) 
        {

            var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null) {

                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth,
                };
                return await Task.Run(()=>View("View", viewModel));
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await dbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await dbContext.SaveChangesAsync();
                

                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee =  await dbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                dbContext.Employees.Remove(employee);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

    }
}
