using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Application.ViewModels
{
    public class AppRoleViewModel
    {
        public Guid? Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
    }
}
