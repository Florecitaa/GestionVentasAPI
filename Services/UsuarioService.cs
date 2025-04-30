using GestionVentasAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionVentasAPI.Services
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            var usuarios = new List<Usuario>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerTodosLosUsuarios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new Usuario
                            {
                                IDUsuario = (int)reader["IDUsuario"],
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString(),
                                Correo = reader["correo"].ToString(),
                                Clave = reader["clave"].ToString(),
                                Rol = reader["Rol"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                Direccion = reader["direccion"].ToString(),
                                Cedula = reader["cedula"].ToString()
                            });
                        }
                    }
                }
            }
            return usuarios;
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            Usuario usuario = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerUsuarioPorId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDUsuario", id);
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                IDUsuario = (int)reader["IDUsuario"],
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString(),
                                Correo = reader["correo"].ToString(),
                                Clave = reader["clave"].ToString(),
                                Rol = reader["Rol"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                Direccion = reader["direccion"].ToString(),
                                Cedula = reader["cedula"].ToString()
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@telefono", usuario.Telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", usuario.Direccion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@cedula", usuario.Cedula); // Ensure not null

                    await con.OpenAsync();
                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627) // Check for duplicate key error numbers
                        {
                            throw new DuplicateCedulaException("A user with this Cedula already exists.", ex); //Wrap the exception
                        }
                        else
                        {
                            throw; // Re-throw other SQLExceptions
                        }
                    }
                }
            }
        }

        public async Task<bool> ActualizarUsuarioAsync(int id, Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDUsuario", id);
                    cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@telefono", usuario.Telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@direccion", usuario.Direccion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@cedula", usuario.Cedula ?? (object)DBNull.Value);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDUsuario", id);
                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }

        public async Task<Usuario> ValidarUsuarioAsync(string correo, string clave)
        {
            Usuario usuario = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ValidarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@Clave", clave);
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                IDUsuario = (int)reader["IDUsuario"],
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString(),
                                Correo = reader["correo"].ToString(),
                                Clave = reader["clave"].ToString(),
                                Rol = reader["Rol"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                Direccion = reader["direccion"].ToString(),
                                Cedula = reader["cedula"].ToString()
                            };
                        }
                    }
                }
            }
            return usuario;
        }
    }

    [Serializable] //custom exception.
    public class DuplicateCedulaException : Exception
    {
        public DuplicateCedulaException() { }
        public DuplicateCedulaException(string message) : base(message) { }
        public DuplicateCedulaException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateCedulaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
