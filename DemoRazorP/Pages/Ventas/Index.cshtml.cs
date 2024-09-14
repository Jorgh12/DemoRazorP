using DemoRazorP.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DemoRazorP.Pages.Ventas
{
    public class IndexModel : PageModel
    {

        //Definir Variables para acceder al archivo appservise.json
        private readonly IConfiguration configuracion;

        //Lista de Objetos de la Clase "Ventas"
        public List<Venta> ListaVentas = new List<Venta>();

        public IndexModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }
        public void OnGet()
        {
            try
            {
                //Definir la cadena de conexion
                string cadena = configuracion.GetConnectionString("CadenaConexion");
                //Crear un objeto de tipo "SqlConnection"
                SqlConnection conexion = new SqlConnection(cadena);
                //Abrir Conexion
                conexion.Open();
                //Crear Objeto de "SlqCommand"
                SqlCommand commando = new SqlCommand("Select v.codVenta,[fechaVenta],c.NomCliente,[cantVenta],[totalVenta] From Clientes c Join Ventas v On c.codCliente = v.codCliente", conexion);
                //Crear Objeto  "SqlDataReader"
                SqlDataReader lector = commando.ExecuteReader();
                //Recorrer el DataReader
                while (lector.Read())
                {
                    //Crear un objeto de tipo clase cliente
                    Venta newVenta = new Venta();
                    newVenta.codVenta = lector.GetInt32(0);
                    newVenta.fechaVenta = lector.GetDateTime(1).ToString("dd/MM/yyyy");
                    newVenta.nomCliente = lector.GetString(2).ToString();
                    newVenta.cantVenta = lector.GetInt32(3);
                    newVenta.totalVenta = lector.GetSqlMoney(4);

                    //Agregar Objeto a la lista 
                    ListaVentas.Add(newVenta);
                }
                //Cerrar Conexión
                conexion.Close();
            }
            catch (Exception ex)
            {
                //Mensaje por Errores
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
    }
}
