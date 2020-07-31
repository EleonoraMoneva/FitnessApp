using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitnessApp.Models
    
{ [Table("AspNetUsers")]
    public class AppUser : IdentityUser
    {
        [Display(Name = "Role")]
        public string Role { get; set; }
        public int? TrainerId { get; set; }
        [Display(Name = "Trainer")]
        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
