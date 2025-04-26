using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GestionVentasAPI.Models;

namespace GestionVentasAPI.Services
{
    public class ProductoService
    {
        private readonly string _connectionString;

        public ProductoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");

        }

        public async Task<List<Producto>> ObtenerTodosLosProductosAsync()
        {
            var productos = new List<Producto>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerTodosLosProductos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            productos.Add(new Producto
                            {
                                idproducto = (int)reader["idproducto"],
                                Nombre = reader["nombre"].ToString(),
                                Detalle = reader["detalle"].ToString(),
                                Precio = (decimal)reader["precio"],
                                Disponible = (int)reader["disponible"],
                                TipoProducto = reader["tipo_producto"].ToString()
                            });
                        }
                    }
                }
            }
            return productos;
        }

        public async Task<Producto> ObtenerProductoPorIdAsync(int idproducto)
        {
            Producto producto = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerProductoPorId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            producto = new Producto
                            {
                                idproducto = (int)reader["idproducto"],
                                Nombre = reader["nombre"].ToString(),
                                Detalle = reader["detalle"].ToString(),
                                Precio = (decimal)reader["precio"],
                                Disponible = (int)reader["disponible"],
                                TipoProducto = reader["tipo_producto"].ToString()
                            };
                        }
                    }
                }
            }
            return producto;
        }

        public async Task CrearProductoAsync(Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@detalle", producto.Detalle ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@disponible", producto.Disponible);
                    cmd.Parameters.AddWithValue("@tipo_producto", producto.TipoProducto ?? (object)DBNull.Value);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> ActualizarProductoAsync(int idproducto, Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@detalle", producto.Detalle ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@disponible", producto.Disponible);
                    cmd.Parameters.AddWithValue("@tipo_producto", producto.TipoProducto ?? (object)DBNull.Value);

                    await con.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> EliminarProductoAsync(int idproducto)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    await con.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }






    }
}
