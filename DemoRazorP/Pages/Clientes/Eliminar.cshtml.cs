using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoRazorP.Pages.Clientes
{
    public class EliminarModel : PageModel
    {
        public readonly IConfiguration configuracion;

        public EliminarModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }

        public void OnGet()
        {
        }
    }
}
