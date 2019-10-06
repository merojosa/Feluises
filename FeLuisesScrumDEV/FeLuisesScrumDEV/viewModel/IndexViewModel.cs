using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeLuisesScrumDEV.Models;

namespace FeLuisesScrumDEV.viewModel
{
    public class IndexViewModel
    {
        public Project Project { get; set; }
        public List<Module> AssociatedModules { get; set; }
        public IndexViewModel()
        { //New constructor
            AssociatedModules = new List<Module>();
        }
    }
}