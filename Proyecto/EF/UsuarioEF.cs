using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System.Data;


namespace ProyectoConjuntoMaterialPrestamo.EF
{
    public static class UsuarioEF
    {



        public static List<Usuario> ListarAsync()
        {

            try
            {
                using var db = ConexionEf.CrearContexto();
                var filas = db.Usuarios
                .OrderBy(f => f.Email).ToList();
                return filas;
            }
            catch (SqlException ex)
            {
                throw new ArgumentException("Error al listar empleados por departamento.", ex);
            }
        }

        public static async Task<Usuario?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Usuarios
                .SingleOrDefaultAsync(f => f.IdUsuario == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }



        public static async Task<int> InsertarAsync(Usuario e)
        {
            try
            {
                using var db = ConexionEf.CrearContexto();
                var fila = new Usuario
                {
                    NombreUsuario = e.NombreUsuario,
                    PrimerApellido = e.PrimerApellido,
                    SegundoApellido = e.SegundoApellido,
                    Email = e.Email,
                    Activo = e.Activo,
                    Password = e.Password
                };

                
                db.Usuarios.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdUsuario;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<bool> BorrarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = db.Usuarios.SingleOrDefault(
                f => f.IdUsuario == id);
                if (fila is null) return false;
                db.Usuarios.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Usuario Convertir(Usuario fila)
        {
            return new Usuario
            {
                IdUsuario = fila.IdUsuario,
                NombreUsuario = fila.NombreUsuario,
                PrimerApellido = fila.PrimerApellido,
                SegundoApellido = fila.SegundoApellido,
                Email = fila.Email,
                Activo = fila.Activo
            };
        }

    }
}
