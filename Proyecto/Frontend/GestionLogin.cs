using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;

static class GestionLogin
{
    static async public void Login()
    {
        Console.Clear();
        Console.WriteLine("LOGGIN");
        string Email = LeerTexto($"Email: ");
        string Password = LeerTexto($"Password: ");
        if (await LoginServicio.LoginAsync(Email, Password))
        {
            Console.WriteLine("BIENVENIDO!!");
        }
        else
        {
            Console.WriteLine("Login incorrecto.");
        }
        SalirPantalla();

    }

    static string LeerTexto(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine()?.Trim() ?? "";
    }

    public static void SalirPantalla()
    {
        Console.WriteLine("\nPulsa una tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
}