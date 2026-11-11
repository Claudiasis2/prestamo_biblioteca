using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;

class Program
{
    public static async Task Main()
    {
        try
        {
            //GestionUsuario.Menu();
            GestionEmpleado.Menu();
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

