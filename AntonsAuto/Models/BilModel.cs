using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AntonsAuto.Models
{
    public class BilModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="Model navn")]
        [StringLength(20, ErrorMessage = "Navnet skal være mellem 3 og 20 bogstaver", MinimumLength = 3)]
        public string Navn { get; set; }

        [Display(Name = "Kørte kilometer")]
        [Range(0, 999999,ErrorMessage = "Tallet må ikke overstige 999.999")]
        public int AntalKilometer { get; set; }

        [Required]
        [Display(Name = "Fabrikant")]
        public int BilfabrikantID { get; set; }

        [Display(Name = "Fabrikant")]
        public Bilfabrikant Bilfabrikant { get; set; }
    }
}
