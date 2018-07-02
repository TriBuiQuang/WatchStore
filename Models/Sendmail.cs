using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWatchStore.Models
{
    public class Sendmail
    {
        [DataType(DataType.EmailAddress), Display(Name = "To")]
        [Required]
        public string FromEmail { get; set; }
        public string Pass { get; set; }

        [Display(Name = "Body")]
        [DataType(DataType.MultilineText)]
        public string EMailBody { get; set; }
        [Display(Name = "Subject")]
        public string EmailSubject { get; set; }
       
        
    }
}