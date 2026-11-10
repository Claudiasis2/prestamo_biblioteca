using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;


static class GestionUsuario
{
    static async public void Menu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("GESTION DE MATERIALES");
            Console.WriteLine("1 Listar | 2 Buscar | 3 Alta");
            Console.WriteLine("4 Salario | 5 Borrar");
            Console.WriteLine("0 Salir");
            string opcion = LeerTexto("Opción: ");
            if (opcion == "0") break;
            try
            {
                switch (opcion)
                {
                    case "1": //await ListarAsync();
                              break;
                    case "2": //await BuscarAsync(); 
                        break;
                    case "3": //await InsertarAsync();
                              break;
                    case "4": //await CambiarSalarioAsync(); 
                        break;
                    case "5": //await BorrarAsync(); 
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
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
                Console.WriteLine(
                "No se pudieron guardar los cambios.");
            }
        }

    }

    static string LeerTexto(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine()?.Trim() ?? "";
    }

}
