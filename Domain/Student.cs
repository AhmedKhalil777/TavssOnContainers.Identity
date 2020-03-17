using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Domain
{
    public enum Departments
    {
         IS , IT , CS , G , BI , SE
    }
    public class Student 
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public Departments Department { get; set; }
        [Required]
        [Range(1,4)]
        public int StudyYear { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

    }
}
