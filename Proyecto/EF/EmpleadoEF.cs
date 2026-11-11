using Microsoft.Data.SqlClient;
using ProyectoConjuntoMaterialPrestamo.Models;
using System.Data;

public static class EmpleadoEF
{
    public static async Task<int> InsertarAsync(Empleado e)
    {
        try
        {
            using var db = ConexionEf.CrearContexto();
            var fila = new Empleado
            {
                NombreEmpleado = e.NombreEmpleado,
                PrimerApellido = e.PrimerApellido,
                SegundoApellido = e.SegundoApellido,
                Email = e.Email
            };
            db.Empleados.Add(fila);
            db.SaveChanges();
            return fila.IdEmpleado;
        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al insertar empleado.", ex);
        }
    }

    public static async Task<List<Empleado>> BuscarAsync(int id = 0, string email = "", string nombre = "")
    {
        try
        {

            using var db = ConexionEf.CrearContexto();
            var query = db.Empleados.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(u =>
                    u.NombreEmpleado.Contains(nombre) ||
                    u.PrimerApellido.Contains(nombre) ||
                    u.SegundoApellido.Contains(nombre));
            }

            if (id > 0)
            {
                query = query.Where(u => u.IdEmpleado == id);
            }

            var filas = query.ToList();

            return filas.Select(fila => Convertir(fila)).ToList();
        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al buscar empleado.", ex);
        }
    }

    public static async Task<bool> ModificarAsync(Empleado e)
    {
        try
        {

            using var db = ConexionEf.CrearContexto();
            var fila = db.Empleados.SingleOrDefault(q => q.IdEmpleado == e.IdEmpleado);
            if (fila is null)
                return false;
            if (e.Email != null && e.Email != "")
                fila.Email = e.Email;
            if (e.NombreEmpleado != null && e.NombreEmpleado != "")
                fila.NombreEmpleado = e.NombreEmpleado;
            if (e.PrimerApellido != null && e.PrimerApellido != "")
                fila.PrimerApellido = e.PrimerApellido;
            if (e.SegundoApellido != null && e.SegundoApellido != "")
                fila.SegundoApellido = e.SegundoApellido;
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
            var fila = db.Empleados.SingleOrDefault(f => f.IdEmpleado == id);

            if (fila is null)
                return false;
            bool tienePrestamo = db.Prestamos
            .Any(p => p.Fkempleado == id);
            if (tienePrestamo)
                return false;            
            db.Empleados.Remove(fila);
            db.SaveChanges();
            return true;
        }
        catch (SqlException ex)
        {
            throw new ArgumentException("Error al eliminar el registro.", ex);
        }
    }

    public static async Task<List<Empleado>> ListarAsync()
    {
        using var db = ConexionEf.CrearContexto();
        var filas = db.Empleados
        .OrderBy(e => e.IdEmpleado).ToList();
        return filas.Select(fila => Convertir(fila))
        .ToList();
    }

    private static Empleado Convertir(Empleado fila)
    {
        return new Empleado
        {
            IdEmpleado = fila.IdEmpleado,
            NombreEmpleado = fila.NombreEmpleado,
            PrimerApellido = fila.PrimerApellido,
            SegundoApellido = fila.SegundoApellido,
            Email = fila.Email
        };
    }
}