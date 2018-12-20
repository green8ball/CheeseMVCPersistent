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
            IList<Menu> menus = context.Menus.ToList();
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
            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (addMenuItemViewModel.cheeseID != null)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.cheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.menuID).ToList();

                if (existingItems.Count == 0)
                {
                    CheeseMenu cheeseMenu = new CheeseMenu
                    {
                        MenuID = addMenuItemViewModel.menuID,
                        CheeseID = addMenuItemViewModel.cheeseID
                    };
                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();
                    return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.menuID);
                }
                else
                {
                    return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.menuID);
                }
            }
            else
            {
                return View("AddItem");
            }
        }

        public IActionResult ViewMenu(int id)
        {
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();
            Menu menu = context.Menus.Single(m => m.ID == id);
            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Items = items,
                Menu = menu
            };
            return View(viewMenuViewModel);
        }

        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
    }
}