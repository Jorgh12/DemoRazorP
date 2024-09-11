using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

//Agregamos la Clase Cliente
using DemoRazorP.Modelos;
using System.Data.SqlClient;
using System.Data;

namespace DemoRazorP.Pages.Clientes
{
    public class ModificarModel : PageModel
    {
        //Definimos la variable para acceder al parametro de configuracion
        private readonly IConfiguration configuracion;

        //Crear un Objeto de tipo Cliente
        public Cliente newCliente = new Cliente();

        //Crear variables para el manejo de errores
        public string mensajeError = "";
        public string mensajeExito = "";

        //Creamos el constructor
        public ModificarModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }
        public void OnGet()
        {
            //Obtenemos el codigo del cliente desde la pagina
            string codCliente = Request.Query["id"];

            try
            {
                //Definimos una variable y le asignamos la cadena de conexion definia en el archivo appsettings.json
                string cadena = configuracion.GetConnectionString("CadenaConexon");

                //Creamos el objeto de la Clase SqlConnection
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrimos la conexion
                conexion.Open();

                //Creamos un objeto de la clase SqlCommand con el dato cargado en el control TexBox
                SqlCommand comando = new SqlCommand("Select * From Clientes where CodCliente = @codCliente", conexion);

                //Pasar los datos del codigo del cliente al parametro 
                comando.Parameters.AddWithValue("@codCliente", codCliente);

                //Creamos un Objeto de la Clase SqlDataReader y lo inicializamos
                SqlDataReader registro = comando.ExecuteReader();

                //Recorremos el SqlDataReader
                if(registro.Read())
                {
                    newCliente.codCliente = registro.GetInt32(0);
                    newCliente.nomcliente = registro.GetString(1);
                    newCliente.Direccion = registro.GetString(2);
                    newCliente.Telefono = registro.GetString(3);
                    newCliente.fechaCom = registro.GetDateTime(4).ToString("yyyy-MM-dd");
                }

                //Cerramos la Conexion
                conexion.Close();

            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                throw;
            }
        }

        //Agregar Metodo "OnPost"
        public void OnPost()
        {
            newCliente.codCliente = int.Parse(Request.Form["codigo"]);
            newCliente.nomcliente = Request.Form["nombre"];
            newCliente.Direccion = Request.Form["direccion"];
            newCliente.Telefono = Request.Form["telefono"];
            newCliente.fechaCom = Request.Form["fechacompra"];

            if(newCliente.nomcliente.Length == 0 || newCliente.Direccion.Length == 0 || newCliente.Telefono.Length == 0 || newCliente.fechaCom.Length == 0)
            {
                mensajeError = "Todos los Campos son Requeridos";
                return;
            }

            try
            {
                //Definimos una variable y le asignamos la cadena de conexion ya definida en el archivo json
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                //Creamos un objeto de la clase SqlConnection indicando como parametro la cadena de conexion
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrimos la conexion
                conexion.Open();
                //Creamos el Query
                String query = "Update Clientes Set nombre = @nombre, direccion = @direccion, telefono = @telefono, FechaPrimeraCompra = @fechacompra Where" + "CodClente = @codCliente";

                //Creamos un objeto de la Clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);

                //Pasar datos de los controles a los parametros
                comando.Parameters.AddWithValue("@nombre", newCliente.nomcliente);
                comando.Parameters.AddWithValue("@direcion", newCliente.Direccion);
                comando.Parameters.AddWithValue("@telefono", newCliente.Telefono);
                comando.Parameters.AddWithValue("@fechacompra", DateTime.Parse(newCliente.fechaCom));
                comando.Parameters.AddWithValue("@codCliente", newCliente.codCliente);

                //Ejecutamos el comando anterior
                comando.ExecuteNonQuery();

                //Crerramos la conexion
                conexion.Close();
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                throw;
            }
            //Redirigir a la pagina Index
            Response.Redirect("/Clientes/Index");

        }
    }
}
