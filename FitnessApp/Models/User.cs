using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace FitnessApp.Models
{
    public class User
    {
        [Key]

        public int Id { get; set; }
        

        [Required]
        [StringLength(100)]
        public string FName { get; set; }

        [Required]
        [StringLength(100)]
        public string LName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [StringLength(500)]
        public string MainGoal { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string Career { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Adress { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }


        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FName, LName);
            }
        }
    }


}
