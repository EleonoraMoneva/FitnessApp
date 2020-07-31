using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Models
{
    public class Programme
    {
        public int Id { get; set; }      
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Equipment { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }



    }
}
