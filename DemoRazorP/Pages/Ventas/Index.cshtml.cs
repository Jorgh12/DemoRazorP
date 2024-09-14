using DemoRazorP.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoRazorP.Pages.Ventas
{
    public class IndexModel : PageModel
    {

        //Lista de Objetos de la Clase "Ventas"
        public List<Venta> ListaVentas = new List<Venta>();
        public void OnGet()
        {
        }
    }
}
