﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ToDoListBE.Controllers;
using ToDoListBE.Data;
using ToDoListBE.Data.Models;
using ToDoListBE.Dto;

namespace ToDoListBE.Test
{
    public class OverdueToDoTasksTest
    {
        private ToDoListDbContext _context;
        private TasksController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ToDoListDbContext>()
                .UseInMemoryDatabase("testdb")
                .Options;

            _context = new ToDoListDbContext(options);
            _controller = new TasksController(_context);
        }
        [Test]
        public async Task GetToDoOverdueTasks_ShouldReturnOverdueTasks()
        {
            var taskDto = new CreateTaskDto
            {
                Name = "Renew my NatGeo Subscription",
                DueDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd"),
                Description = "Must renew my subscription by the end of October"
            };
            await _controller.PostToDoTask(taskDto);
            var iActionResult = await _controller.GetToDoОverdueTasks();
            if (iActionResult is OkObjectResult okResult)
            {
                
                var returnedTasks = okResult.Value as List<TaskDto>;
                Assert.IsNotNull(returnedTasks); 
                Assert.AreEqual(1, returnedTasks.Count);
                Assert.AreEqual(returnedTasks[0].Name, taskDto.Name);
                Assert.AreEqual(returnedTasks[0].Description, taskDto.Description);

            }
            else
            {
                Assert.Fail("Expected Ok response");
            }


        }
        [Test]
        public async Task GetToDoOverdueTasks_ShouldNotReturnOverdueTasks()
        {
            var taskDto = new CreateTaskDto
            {
                Name = "Renew my NatGeo Subscription",
                DueDate = DateTime.UtcNow.AddDays(+8).ToString("yyyy-MM-dd"),
                Description = "Must renew my subscription by the end of October"
            };
            await _controller.PostToDoTask(taskDto);
            var iActionResult = await _controller.GetToDoОverdueTasks();
            if (iActionResult is OkObjectResult okResult)
            {

                var returnedTasks = okResult.Value as List<TaskDto>;
                Assert.IsNotNull(returnedTasks);
                Assert.AreEqual(0, returnedTasks.Count);

            }
            else
            {
                Assert.Fail("Expected Ok response");
            }


        }
        [Test]
        public async Task GetToDoOverdueTasks_ShouldReturnOverdueTasksIfDone()
        {
            await _context.ToDoTasks.AddRangeAsync(new List<ToDoTask>
            {
                new ToDoTask
                {
                    Description = "Go Xmas Shopping to the mall",
                    Name = "Xmas presents",
                    IsDone = true,
                    DueDate =  DateTime.UtcNow.AddDays(-8),
                }
            });
            await _context.SaveChangesAsync();
            var iActionResult = await _controller.GetToDoОverdueTasks();
            if (iActionResult is OkObjectResult okResult)
            {

                var returnedTasks = okResult.Value as List<TaskDto>;
                Assert.IsNotNull(returnedTasks);
                Assert.AreEqual(0, returnedTasks.Count);
            }
            else
            {
                Assert.Fail("Expected Ok response");
            }


        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}