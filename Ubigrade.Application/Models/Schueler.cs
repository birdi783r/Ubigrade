using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ubigrade.Application.Models
{
    public class SchuelerModel
    {
        [Display(Name = "Schueler ID:")]
        //[Range(1, 50, ErrorMessage = "Sie müssen einen gültigen Wert eingeben (1-50)")]
        [Required(ErrorMessage = "Geben Sie eine ID")]
        public int Checkpersonnumber { get; set; }

        [Display(Name = "Nachname:")]
        [Required(ErrorMessage = "Nachname muss eingegeben werden")]
        public string NName { get; set; }

        [Display(Name = "Vorname:")]
        [Required(ErrorMessage = "Vorname muss eingegeben werden")]
        public string VName { get; set; }

        [Display(Name = "Geschlecht:")]
        [Required(ErrorMessage = "Geschlecht muss eingegeben werden")]
        public string Geschlecht { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Email muss eingegeben werden")]
        public string EmailAdresse { get; set; }

        [Display(Name = "Schuljahr:")]
        [Required(ErrorMessage = "Schuljahr muss eingegeben werden")]
        public int Schuljahr { get; set; }
    }
}
