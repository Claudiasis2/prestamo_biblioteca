using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Frontend;
using ProyectoConjuntoMaterialPrestamo.Frontend.Menus;
using ProyectoConjuntoMaterialPrestamo.Models;

class Program
{
    public static void Cargar()
    {
        var directorio = new DirectoryInfo(AppContext.BaseDirectory);

        while (directorio != null &&
               !File.Exists(Path.Combine(directorio.FullName, ".env")))
        {
            directorio = directorio.Parent;
        }

        if (directorio == null)
            throw new FileNotFoundException("No se encontró el archivo .env");

        Env.Load(Path.Combine(directorio.FullName, ".env"));
    }

    public static async Task Main()
    {
        Cargar();
        IConsola consola = new ConsolaReal();
        

        try
        {
            await MenuPrincipal.Menu(consola);
        }
        catch (ArgumentException ex)
        {
            consola.Escribir(ex.Message);
        }
        catch (SqlException ex)
        {
            consola.Escribir("La operación SQL ha fallado.");
            consola.Escribir(ex.Message);
        }
        catch (DbUpdateException)
        {
            consola.Escribir("No se pudieron guardar los cambios.");
        }

    }


}

