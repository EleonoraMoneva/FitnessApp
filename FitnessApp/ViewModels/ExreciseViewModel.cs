using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.ViewModels
{
    public class ExreciseViewModel
    {
        public string EName { get; set; }
        public IFormFile ProfileImage { get; set; }

    }
}
