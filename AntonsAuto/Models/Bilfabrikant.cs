using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AntonsAuto.Models
{
    public class Bilfabrikant
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Fabrikant")]
        [StringLength(20,ErrorMessage ="Navnet skal være mellem 3 og 20 bogstaver", MinimumLength = 3)]
        public string Navn { get; set; }

        public ICollection<BilModel> BilModels { get; set; }
    }
}
