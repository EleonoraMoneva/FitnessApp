using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Models
{
    public class Trainer
    {
        [Key]

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Bdate { get; set; }
        public string Adr { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Number { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }


        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

    }
}
