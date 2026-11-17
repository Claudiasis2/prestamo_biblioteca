using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Frontend;
using ProyectoConjuntoMaterialPrestamo.Frontend.Menus;
using ProyectoConjuntoMaterialPrestamo.Models;

class Program
{
    public static async Task Main()
    {
        Env.Load();
        try
        {
            IConsola consola = new ConsolaReal();
            await MenuPrincipal.Menu(consola);
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

