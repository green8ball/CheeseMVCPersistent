using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Menus";
            IList<Menu> menus = context.Menus.Include(c => c.Name).ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (addMenuViewModel.Name != null)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            } else
            {
                return View("Add");
            }
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);
            List<Cheese> cheesesList = context.Cheeses.ToList();
            List<SelectListItem> cheeses = new List<SelectListItem>();
            foreach (Cheese cheese in cheesesList)
            {
                cheeses.Add(new SelectListItem
                {
                    Value = cheese.ID.ToString(),
                    Text = cheese.Name.ToString()
                });
            }
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel
            {
                Menu = menu,
                Cheeses = cheeses
            };
            return View("AddItem");
    }

        public IActionResult ViewMenu(int id)
        {
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();
            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Items = items
            };
            return View("ViewMenu", viewMenuViewModel);
        }

        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
    }
}