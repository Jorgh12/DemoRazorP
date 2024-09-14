using System.Data.SqlTypes;

namespace DemoRazorP.Modelos
{
    public class Venta
    {
        //Propiedades para acceder a cada una de las tablas
        public int codVenta {  get; set; }
        public string fechaVenta { get; set;}
        public int codCliente { get; set;}
        public string nomCliente { get; set;}
        public int cantVenta { get;  set;}
        public SqlMoney presVenta { get; set; }
        public SqlMoney totalVenta { get; set; }
    }
}
