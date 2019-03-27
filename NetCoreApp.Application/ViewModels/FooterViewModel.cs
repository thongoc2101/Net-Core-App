using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreApp.Application.ViewModels
{
    public class FooterViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Content { set; get; }
    }
}
