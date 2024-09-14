using DemoRazorP.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//Referencia al namepace para gestion de datos
using System.Data.SqlClient;
//Referencia al Modelo


namespace DemoRazorP.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        //Definir variable para acceder al archvio appsevise.json
        private readonly IConfiguration configuracion;

        //Lista de Objetos de la clase "Cliente"
        public List<Cliente> listaClinetes = new List<Cliente>();
        //Definiendo el constructor
        public IndexModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }
        public void OnGet()
        {
            try {
                //Definir la cadena de conexion
                string cadena = configuracion.GetConnectionString("CadenaConexion");
                //Crear un objeto de tipo "SqlConnection"
                SqlConnection conexion = new SqlConnection(cadena);
                //Abrir Conexion
                conexion.Open();
                //Crear Objeto de "SlqCommand"
                SqlCommand commando = new SqlCommand("Select * From Clientes", conexion);
                //Crear Objeto  "SqlDataReader"
                SqlDataReader lector = commando.ExecuteReader();
                //Recorrer el DataReader
                while (lector.Read())
                {
                    //Crear un objeto de tipo clase cliente
                    Cliente newCliente = new Cliente();
                    newCliente.codCliente = lector.GetInt32(0);
                    newCliente.nomcliente = lector.GetString(1);
                    newCliente.Direccion = lector.GetString(2);
                    newCliente.Telefono = lector.GetString(3);
                    newCliente.fechaCom = lector.GetDateTime(4).ToString("dd/MM/yyyy");

                    //Agregar Objeto a la lista 
                    listaClinetes.Add(newCliente);
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
