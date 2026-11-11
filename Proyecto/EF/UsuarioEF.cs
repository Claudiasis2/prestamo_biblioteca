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

    public static Usuario? BuscarAsync(int id)
    {
        try
        {
            using var db = ConexionEf.CrearContexto();
            var fila = db.Usuarios
            .SingleOrDefault(u => u.IdUsuario == id);
            return fila is null ? null : Convertir(fila);
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

}
