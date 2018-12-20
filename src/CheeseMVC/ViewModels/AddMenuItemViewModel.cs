using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        [Required]
        public int cheeseID { get; set; }
        [Required]
        public int menuID { get; set; }
        public Menu Menu { get; set; }
        public List<SelectListItem> Cheeses { get; set; }

        public AddMenuItemViewModel (Menu menu, Cheese cheese)
        {
            Menu = menu;
            Cheeses = new List<SelectListItem>();
            Cheeses.Add(new SelectListItem
            {
                Value = cheese.ID.ToString(),
                Text = cheese.Name
            });
        }

        public AddMenuItemViewModel()
        {

        }

    }
}