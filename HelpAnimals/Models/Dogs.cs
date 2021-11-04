using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HelpAnimals.Models
{
    public class Dogs
    {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; }
        public string Breed { set; get; }
        public string Age { set; get; }
        public string Gender { set; get; }
        public string Size { set; get; }
        public string Status { set; get; }
        [NotMapped]
        public IFormFile Image { set; get; }

    }
}
