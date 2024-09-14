using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DemoRazorP.Modelos;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlClient;

namespace DemoRazorP.Pages.Producto
{
    public class AgregarpModel : PageModel
    {
        //Definir Variable para acceder al archivo appservise.json
        private readonly IConfiguration configuracion;

        //Crar un Objoeto de Tipo Productos
        public Productos newProducto = new Productos();

        //Crear una Variable para el manejo de errores
        public string mensajeError = "";
        public string mensajeExito = "";

        //Definiendo el Constructor
        public AgregarpModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }


        public void OnGet()
        {
        }

        //Agregar Metodo "Onpost"
        public void OnPost()
        {
            newProducto.nomProducto = Request.Form["nombre"];
            newProducto.tipoProducto = Request.Form["tipoProducto"];
            newProducto.extProducto = Request.Form["extProducto"];
            newProducto.preProducto = Request.Form["preProducto"];

            if (newProducto.nomProducto.Length == 0 || newProducto.tipoProducto.Length == 0 || newProducto.extProducto.Length == 0 || newProducto.preProducto.Length == 0)
            {
                mensajeError = "Todos los campos son Requeridos";
                return;
            }
            try
            {
                // Definimos una variable y le asignamos la candena de conexion definida en el archivo appsettings.json
                String cadena = configuracion.GetConnectionString("CadenaConexion");

                //Creamos un objeto de la clase SqlConnection Indicando como parametro
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrimos la Conexion
                conexion.Open();

                //Crear el Query para insertar los datos
                String query = "INSERT INTO Producto (nomProducto, tipoProducto, extProducto, preProducto) VALUES (@nomProducto, @tipoProducto,@extProducto,@preProducto);";

                //Creamos un objeto de la Clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);

                //Pasando los Datos Ingresado a los Parametros
                comando.Parameters.AddWithValue("@nomProducto", newProducto.nomProducto);
                comando.Parameters.AddWithValue("@tipoProducto", newProducto.tipoProducto);
                comando.Parameters.AddWithValue("@extProducto", newProducto.extProducto);
                comando.Parameters.AddWithValue("@preProducto",newProducto.preProducto);

                //Le indicamos a Sql Server que ejecute el comando especificado anteriormente
                comando.ExecuteNonQuery();

                //Cerramos la conexion
                conexion.Close();

            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return;
            }
            //Limpiamos los Controles
            newProducto.nomProducto = "";
            newProducto.tipoProducto = "";
            newProducto.extProducto = "";
            newProducto.preProducto = " ";
            mensajeExito = "Producto Agregado Correctamente.";

            //Al finalizar pasar a la pagina Principal de Clientes
            Response.Redirect("/Producto");
        }
    }
}
