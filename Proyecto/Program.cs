using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;

class Program
{
    public static async Task Main()
    {
        try
        {
            /*
            using var db = ConexionEf.CrearContexto();
            var filas = await db.Usuarios.ToListAsync<Usuario>();
            foreach (var fila in filas)
            {
                Console.WriteLine($"{fila.IdUsuario} {fila.NombreUsuario} {fila.PrimerApellido} {fila.SegundoApellido} {fila.Email}");
            }*/
            
            GestionUsuario.Menu();
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("La operación SQL ha fallado.");
            Console.WriteLine(ex.Message);
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("No se pudieron guardar los cambios.");
        }

    }


}

