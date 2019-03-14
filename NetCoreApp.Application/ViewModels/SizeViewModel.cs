using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Application.ViewModels
{
    public class SizeViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
