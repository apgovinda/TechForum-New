﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tech_Forum.Models
{
    public class Content
    {
        public string postid { get; set; }

        [Display(Name = "Content")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Content required")]
        [MinLength(200, ErrorMessage = "Content should be minimum of 200 characters")]
        public string content_ { get; set; }
        public System.DateTime date { get; set; }
        public string userid { get; set; }
    }
}
