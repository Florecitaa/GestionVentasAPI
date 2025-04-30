namespace GestionVentasAPI.Models
{
    public class Venta
    {
        public int IDVenta { get; set; }  
        public int IdUsuario { get; set; }
        public decimal monto_total { get; set; }
        public string metodo_pago { get; set; }

    }
}
