using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GestionVentasAPI.Models;

namespace GestionVentasAPI.Services
{
    public class DetalleVentaService
    {
        private readonly string _connectionString;

        public DetalleVentaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");

        }
        public async Task<List<DetalleVenta>> ObtenerTodosLosDetallesDeVentaAsync()
        {
            var detallesVenta = new List<DetalleVenta>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerTodosLosDetallesDeVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            detallesVenta.Add(new DetalleVenta
                            {
                                IdDetalledeventa = (int)reader["IdDetalledeventa"],
                                idproducto = (int)reader["idproducto"],
                                IdVenta = (int)reader["idVenta"],
                                Cantidad = (int)reader["Cantidad"],
                                Subtotal = (decimal)reader["subtotal"],
                                PrecioUnitario = (decimal)reader["preciounitario"]
                            });
                        }
                    }
                }
            }
            return detallesVenta;
        }

        public async Task<DetalleVenta> ObtenerDetalleVentaPorIdAsync(int idDetalleVenta)
        {
            DetalleVenta detalleVenta = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerDetalleVentaPorId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdDetalledeventa", idDetalleVenta);
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            detalleVenta = new DetalleVenta
                            {
                                IdDetalledeventa = (int)reader["IdDetalledeventa"],
                                idproducto = (int)reader["idproducto"],
                                IdVenta = (int)reader["idVenta"],
                                Cantidad = (int)reader["Cantidad"],
                                Subtotal = (decimal)reader["subtotal"],
                                PrecioUnitario = (decimal)reader["preciounitario"]
                            };
                        }
                    }
                }
            }
            return detalleVenta;
        }

        public async Task CrearDetalleVentaAsync(DetalleVenta detalleVenta)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarDetalleVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idproducto", detalleVenta.idproducto);
                    cmd.Parameters.AddWithValue("@idVenta", detalleVenta.IdVenta);
                    cmd.Parameters.AddWithValue("@Cantidad", detalleVenta.Cantidad);
                    cmd.Parameters.AddWithValue("@subtotal", detalleVenta.Subtotal);
                    cmd.Parameters.AddWithValue("@preciounitario", detalleVenta.PrecioUnitario);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> ActualizarDetalleVentaAsync(int idDetalleVenta, DetalleVenta detalleVenta)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarDetalleVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdDetalledeventa", idDetalleVenta);
                    cmd.Parameters.AddWithValue("@idproducto", detalleVenta.idproducto);
                    cmd.Parameters.AddWithValue("@idVenta", detalleVenta.IdVenta);
                    cmd.Parameters.AddWithValue("@Cantidad", detalleVenta.Cantidad);
                    cmd.Parameters.AddWithValue("@subtotal", detalleVenta.Subtotal);
                    cmd.Parameters.AddWithValue("@preciounitario", detalleVenta.PrecioUnitario);

                    await con.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> EliminarDetalleVentaAsync(int idDetalleVenta)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarDetalleVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdDetalledeventa", idDetalleVenta);
                    await con.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }


    }
}
