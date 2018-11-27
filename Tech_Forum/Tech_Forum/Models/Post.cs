using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tech_Forum.Models
{
    public class Post : Content
    {
        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title required")]
        public string title { get; set; }

        [Display(Name = "Domain")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Domain required")]
        public string domain { get; set; }

        [Display(Name = "Technology")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Technology required")]
        public string technology { get; set; }

        [Display(Name = "Tags")]
        public string tags { get; set; }
        public Nullable<double> rating { get; set; }

    }
}