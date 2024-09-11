using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DemoRazorP.Modelos;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlClient;

namespace DemoRazorP.Pages.Clientes
{
    public class AgregarModel : PageModel
    {
        //Definir Variable para acceder al archivo appservise.json
        private readonly IConfiguration configuracion;

        //Crar un Objoeto de Tipo Cliente
        public Cliente newCliente = new Cliente();

        //Crear una Variable para el manejo de errores
        public string mensajeError = "";
        public string mensajeExito = "";

        //Definiendo el Constructor
        public AgregarModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion; 
        }


        public void OnGet()
        {
        }

        //Agregar Metodo "Onpost"
        public void OnPost()
        {
            newCliente.nomcliente = Request.Form["nombre"];
            newCliente.Direccion = Request.Form["direccion"];
            newCliente.Telefono = Request.Form["telefono"];
            newCliente.fechaCom = Request.Form["fechacompra"];

            if(newCliente.nomcliente.Length == 0 || newCliente.Direccion.Length == 0 || newCliente.Telefono.Length == 0 || newCliente.fechaCom.Length == 0)
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
                String query = "Insert Into Clientes(NomCliente,Direccion,Telefono,FechaPrimeraCompra) values (@nombre, @direccion,@telefono,@fechaPrimeraCompra);";

                //Creamos un objeto de la Clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);

                //Pasando los Datos Ingresado a los Parametros
                comando.Parameters.AddWithValue("@nombre", newCliente.nomcliente);
                comando.Parameters.AddWithValue("@direccion", newCliente.Direccion);
                comando.Parameters.AddWithValue("@telefono", newCliente.Telefono);
                comando.Parameters.AddWithValue("@fechaPrimeraCompra", DateTime.Parse(newCliente.fechaCom));

                //Le indicamos a Sql Server que ejecute el comando especificado anteriormente
                comando.ExecuteNonQuery();

                //Cerramos la conexion
                conexion.Close();

            }catch (Exception ex)
            {
                mensajeError = ex.Message;
                return;
            }
            //Limpiamos los Controles
            newCliente.nomcliente = "";
            newCliente.Direccion = "";
            newCliente.Telefono = "";
            mensajeExito = "Cliente Agregado Correctamente.";

            //Al finalizar pasar a la pagina Principal de Clientes
            Response.Redirect("/Index");
        }
    }
}
