using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ubigrade.Application.Models
{
    public class NotenModel
    {
        [Display(Name = "Noten ID")]
        [Required(ErrorMessage = "Gültige Noten ID eingeben")]
        public int NId { get; set; }

        [Display(Name = "Notenbezeichnung")]
        [Required(ErrorMessage = "Notenbezeichnung eingeben")]
        public int Bezeichnung { get; set; }

        [Display(Name = "Mindestanforderung")]
        [Required(ErrorMessage = "Mindestanforderung eingeben")]
        public int Mindestanforderung { get; set; }
    }
}
