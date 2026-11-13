using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System.Data;

public static class UsuarioEF
{


    private static Usuario Convertir(Usuario fila)
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

    public static async Task<List<Usuario>> BuscarAsync(int id = 0, string email = "", string nombre = "")
    {
        try
        {

            using var db = ConexionEf.CrearContexto();
            var query = db.Usuarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(u =>
                    u.NombreUsuario.Contains(nombre) ||
                    u.PrimerApellido.Contains(nombre) ||
                    u.SegundoApellido.Contains(nombre));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }

            if (id > 0)
            {
                query = query.Where(u => u.IdUsuario == id);
            }

            var filas = query.ToList();

            return filas.Select(fila => Convertir(fila)).ToList();
        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al buscar usuario.", ex);
        }
    }

    public static async Task<List<Usuario>> ListarAsync()
    {
        using var db = ConexionEf.CrearContexto();
        var filas = db.Usuarios
        .OrderBy(u => u.IdUsuario).ToList();
        return filas.Select(fila => Convertir(fila))
        .ToList();
    }


    public static async Task<int> InsertarAsync(Usuario u)
    {
        try
        {
            using var db = ConexionEf.CrearContexto();
            var fila = new Usuario
            {
                Email = u.Email,
                NombreUsuario = u.NombreUsuario,
                PrimerApellido = u.PrimerApellido,
                SegundoApellido = u.SegundoApellido,
                Activo = u.Activo,
                Password = u.Password
            };
            db.Usuarios.Add(fila);
            db.SaveChanges();
            return fila.IdUsuario;
        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al insertar usuario.", ex);
        }
    }

    public static async Task<bool> ModificarAsync(Usuario u)
    {
        try
        {

            using var db = ConexionEf.CrearContexto();
            var fila = db.Usuarios.SingleOrDefault(q => q.IdUsuario == u.IdUsuario);
            if (fila is null)
                return false;
            if (u.Email != null && u.Email != "")
                fila.Email = u.Email;
            if (u.NombreUsuario != null && u.NombreUsuario != "")
                fila.NombreUsuario = u.NombreUsuario;
            if (u.PrimerApellido != null && u.PrimerApellido != "")
                fila.PrimerApellido = u.PrimerApellido;
            if (u.PrimerApellido != null && u.PrimerApellido != "")
                fila.SegundoApellido = u.SegundoApellido;
            db.SaveChanges();
            return true;

        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al insertar usuario.", ex);
        }
    }

    public static async Task<bool> BorrarAsync(int id)
    {
        try
        {
            using var db = ConexionEf.CrearContexto();
            var fila = db.Usuarios.SingleOrDefault(f => f.IdUsuario == id);

            db.Usuarios.All(f => f.IdUsuario != id);
            if (fila is null)
                return false;
            bool tienePrestamo = db.Prestamos
            .Any(p => p.Fkusuario == id);
            if (tienePrestamo)
                return false;
            bool tieneSolicitud = db.SolicitudPrestamos
            .Any(s => s.Fkusuario == id);
            if (tieneSolicitud)
                return false;
            db.Usuarios.Remove(fila);
            db.SaveChanges();
            return true;
        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al eliminar el registro.", ex);
        }
    }

    public static async Task<bool> LoginAsync(string Email, string Password)
    {
        try
        {
            using var db = ConexionEf.CrearContexto();
            var fila = db.Usuarios.SingleOrDefault(q => q.Email == Email && q.Password == Password);
            if (fila is null)
                return false;
            return true;
        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al insertar usuario.", ex);
        }
    }

}
