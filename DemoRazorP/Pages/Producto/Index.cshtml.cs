
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//Referencia al namepace para gestion de datos
using System.Data.SqlClient;
//Referencia al Modelo
using DemoRazorP.Modelos;

namespace DemoRazorP.Pages.Producto
{
    public class IndexModel : PageModel
    {
        //Definir variable para acceder al archvio appsevise.json
        private readonly IConfiguration configuracion;

        //Lista de Objetos de la clase "Producto"
        public List<Productos> listaProducto = new List<Productos>();
        //Definiendo el constructor
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
                SqlCommand commando = new SqlCommand("Select * From Producto", conexion);
                //Crear Objeto  "SqlDataReader"
                SqlDataReader lector = commando.ExecuteReader();
                //Recorrer el DataReader
                while (lector.Read())
                {
                    //Crear un objeto de tipo clase producto
                    Productos newProducto = new Productos();
                    newProducto.codProducto = lector.GetInt32(0);
                    newProducto.nomProducto = lector.GetString(1);
                    newProducto.tipoProducto = lector.GetString(2);
                    newProducto.extProducto = lector.GetInt32(3).ToString();
                    newProducto.preProducto = lector.GetDouble(4).ToString();

                    //Agregar Objeto a la lista 
                    listaProducto.Add(newProducto);
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
