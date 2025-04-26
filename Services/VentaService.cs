using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GestionVentasAPI.Models;
using Microsoft.Extensions.Configuration;

namespace GestionVentasAPI.Services
{
    public class VentaService
    {
        private readonly string _connectionString;

        public VentaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");

        }

        public async Task<List<Venta>> ObtenerTodasLasVentasAsync()
        {
            var ventas = new List<Venta>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerTodasLasVentas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ventas.Add(new Venta
                            {
                                IDVenta = (int)reader["IDVenta"],
                                IdUsuario = (int)reader["IdUsuario"],
                                Fecha = (DateTime)reader["fecha"],
                                MontoTotal = (decimal)reader["monto_total"],
                                MetodoPago = reader["metodo_pago"].ToString()
                            });
                        }
                    }
                }
            }
            return ventas;
        }

        public async Task<Venta> ObtenerVentaPorIdAsync(int idVenta)
        {
            Venta venta = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerVentaPorId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDVenta", idVenta);
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            venta = new Venta
                            {
                                IDVenta = (int)reader["IDVenta"],
                                IdUsuario = (int)reader["IdUsuario"],
                                Fecha = (DateTime)reader["fecha"],
                                MontoTotal = (decimal)reader["monto_total"],
                                MetodoPago = reader["metodo_pago"].ToString()
                            };
                        }
                    }
                }
            }
            return venta;
        }

        public async Task<int> InsertarVentaAsync(Venta venta) 
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                    cmd.Parameters.AddWithValue("@monto_total", venta.MontoTotal);
                    cmd.Parameters.AddWithValue("@metodo_pago", venta.MetodoPago);

                    await con.OpenAsync();
                    
                    object result = await cmd.ExecuteScalarAsync();
                    return (int)result; 
                }
            }
        }

        public async Task<bool> ActualizarVentaAsync(int idVenta, Venta venta)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDVenta", idVenta);
                    cmd.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                    cmd.Parameters.AddWithValue("@monto_total", venta.MontoTotal);
                    cmd.Parameters.AddWithValue("@metodo_pago", venta.MetodoPago);

                    await con.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> EliminarVentaAsync(int idVenta)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDVenta", idVenta);
                    await con.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }



    }
}
