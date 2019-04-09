using System.Threading.Tasks;

namespace NetCoreApp.Services
{
    public interface IViewRenderService
    {
        /// <summary>
        /// Render Razor View to String
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
