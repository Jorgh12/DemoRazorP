using DemoRazorP.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace DemoRazorP.Pages.Ventas
{
    public class ModificarModel : PageModel
    {
        //Defenimos la variable para acceder al parametro de configuracion
        private readonly IConfiguration configuracion;

        //Crear un Objeto de Tipo Venta
        public Venta newVenta = new Venta();

        //Variable para el manejo de errores
        public string mensajeError = "";
        public string mensajeExito = "";

        //Creamos el constructor
        public ModificarModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }
        public void OnGet()
        {
            //obtenemos el codigo de la venta desde la pagina
            string codVenta = Request.Query["id"];
            if (string.IsNullOrEmpty(codVenta))
            {
                return;
            }

            try
            {
                //Definimos una variable y le asignamos la cadena de conexion
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                //Creamos el objeto de la Clase SqlConnectio
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrimos la conexion
                conexion.Open();

                //Creamos un Objeto de la Clase SqlCommand con el dato cargado
                SqlCommand comando = new SqlCommand("Select v.codVenta,[fechaVenta],c.NomCliente,[cantVenta],[totalVenta] From Clientes c Join Ventas v On c.codCliente = v.codCliente Where v.codVenta = @codVenta", conexion);

                //Pasar los Datos del Codigo de la venta seleccionada
                comando.Parameters.AddWithValue("@codVenta", codVenta);

                //Creamos un Objeto de la clase SqlDatareader y se inicializa
                SqlDataReader registro = comando.ExecuteReader();

                //Recorremos el SqlDataReader
                if (registro.Read())
                {
                    newVenta.codVenta = registro.GetInt32(0);
                    newVenta.fechaVenta = registro.GetDateTime(1).ToString("dd/MM/yyyy");
                    newVenta.nomCliente = registro.GetString(2);
                    newVenta.cantVenta = registro.GetInt32(3);
                    newVenta.totalVenta = registro.GetSqlMoney(4);
                }
                //Cerrar la Conexion
                conexion.Close();
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                throw;
            }
        }

        public void OnPost()
        {
            newVenta.codVenta = int.Parse(Request.Form["codigo"]);
            newVenta.fechaVenta = Request.Form["FechaVenta"];
            newVenta.nomCliente = Request.Form["nomCliente"];
            newVenta.cantVenta = int.Parse(Request.Form["cantidad"]);
            newVenta.totalVenta = SqlMoney.Parse(Request.Form["MontoTotal"]);

            if (newVenta.fechaVenta.Length == 0 || newVenta.nomCliente.Length == 0 || newVenta.cantVenta == 0 || newVenta.totalVenta == 0)
            {
                mensajeError = "Todos los Camps son Requerido";
                return;
            }
            try
            {
                //Definimos una Variable  y le asignamos la cadena
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                //Creamos un Objeto de clase SqlConnection
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrimos la Conexion
                conexion.Open();

                //Cramos el Query
                string query = "Update Ventas Set fechaVenta = @fechaVenta, NomCliente = @NomCliente, cantVenta = @cantVenta, totalVenta = @totalVenta Where " + " codVenta = @codVenta";

                //Creamos un objeto de la Clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);

                //Pasar Datos de los controles a los parametros
                comando.Parameters.AddWithValue("@fechaVenta", DateTime.Parse(newVenta.fechaVenta));
                comando.Parameters.AddWithValue("@NomCliente", newVenta.nomCliente);
                comando.Parameters.AddWithValue("@cantventa", newVenta.cantVenta);
                comando.Parameters.AddWithValue("@totalVenta", newVenta.totalVenta);

                //Ejecutamos el comando anterior
                comando.ExecuteNonQuery();

                //Cerramos la conexion
                conexion.Close();
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                throw;
            }
            //Redirigir a la pagina Index
            Response.Redirect("/Ventas");
        }
    }
}
