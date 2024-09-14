using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

//Agregamos la Clase Cliente
using DemoRazorP.Modelos;
using System.Data.SqlClient;
using System.Data;

namespace DemoRazorP.Pages.Producto
{
    public class ModificarpModel : PageModel
    {
        //Definimos la variable para acceder al parametro de configuracion
        private readonly IConfiguration configuracion;

        //Crear un Objeto de tipo Cliente
        public Productos newProducto = new Productos();

        //Crear variables para el manejo de errores
        public string mensajeError = "";
        public string mensajeExito = "";

        //Creamos el constructor
        public ModificarpModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }
        public void OnGet()
        {
            //Obtenemos el codigo del cliente desde la pagina
            string codProducto = Request.Query["id"];
            if (string.IsNullOrEmpty(codProducto))
            {
                return;
            }

            try
            {
                //Definimos una variable y le asignamos la cadena de conexion definia en el archivo appsettings.json
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                //Creamos el objeto de la Clase SqlConnection
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrimos la conexion
                conexion.Open();

                //Creamos un objeto de la clase SqlCommand con el dato cargado en el control TexBox
                SqlCommand comando = new SqlCommand("Select * From Producto Where codProducto = @codProducto", conexion);

                //Pasar los datos del codigo del cliente al parametro 
                comando.Parameters.AddWithValue("@codProducto", codProducto);

                //Creamos un Objeto de la Clase SqlDataReader y lo inicializamos
                SqlDataReader registro = comando.ExecuteReader();

                //Recorremos el SqlDataReader
                if (registro.Read())
                {
                    newProducto.codProducto = registro.GetInt32(0);
                    newProducto.nomProducto = registro.GetString(1);
                    newProducto.tipoProducto = registro.GetString(2);
                    newProducto.extProducto = registro.GetInt32(3).ToString();
                    newProducto.preProducto = registro.GetDouble(4).ToString();
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
            newProducto.codProducto = int.Parse(Request.Form["codProducto"]);
            newProducto.nomProducto = Request.Form["nombre"];
            newProducto.tipoProducto = Request.Form["tipoProducto"];
            newProducto.extProducto = Request.Form["extProducto"];
            newProducto.preProducto = Request.Form["preProducto"];

            if (newProducto.nomProducto.Length == 0 || newProducto.tipoProducto.Length == 0 || newProducto.extProducto.Length == 0 || newProducto.preProducto.Length == 0)
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
                String query = "Update Producto Set nomProducto = @nomProducto, tipoProducto = @tipoProducto, extProducto = @extProducto, preProducto = @preProducto Where " + " codProducto = @codProducto";

                //Creamos un objeto de la Clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);

                //Pasar datos de los controles a los parametros
                comando.Parameters.AddWithValue("@nomProducto", newProducto.nomProducto);
                comando.Parameters.AddWithValue("@tipoProducto", newProducto.tipoProducto);
                comando.Parameters.AddWithValue("@extProducto", newProducto.extProducto);
                comando.Parameters.AddWithValue("@preProducto", newProducto.preProducto);
                comando.Parameters.AddWithValue("@codProducto", newProducto.codProducto);

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
            Response.Redirect("/Producto");

        }
    }
}
