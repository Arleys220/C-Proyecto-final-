using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_final
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
    }

    public class ProductosController : ApiController
    {
        private string connectionString = "Data Source=tu_servidor;Initial Catalog=tu_basededatos;User ID=tu_usuario;Password=tu_contraseña";

        [HttpGet]
        public IHttpActionResult ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Productos";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Producto producto = new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"])
                        };
                        productos.Add(producto);
                    }

                    reader.Close();
                    return Ok(productos);
                }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult ObtenerProductoPorId(int id)
        {
            Producto producto = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Productos WHERE Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        producto = new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"])
                        };
                    }

                    reader.Close();
                    if (producto == null)
                        return NotFound();

                    return Ok(producto);
                }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
                
