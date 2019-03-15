using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();

        List<SlideViewModel> GetSlides(string groupAlias);

        SystemConfigViewModel GetSystemConfig(string code);
    }
}
