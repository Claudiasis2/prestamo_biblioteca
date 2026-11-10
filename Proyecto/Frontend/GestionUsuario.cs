using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
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
            
            Console.WriteLine("GESTION DE USUARIOS");
            Console.WriteLine("1 Listar");
            Console.WriteLine("2 Buscar"); 
            Console.WriteLine("3 Alta");
            Console.WriteLine("4 Modificar");
            Console.WriteLine("5 Borrar");
            Console.WriteLine("0 Salir");
            string opcion = LeerTexto("Opción: ");
            if (opcion == "0") break;
            try
            {
                switch (opcion)
                {
                    case "1": await ListarUsuarioAsync();
                        break;
                    case "2": await BuscarUsuarioAsync(); 
                        break;
                    case "3": await InsertarUsuarioAsync();
                        break;
                    case "4": await ModificarUsuarioAsync(); 
                        break;
                    case "5": await BorrarUsuarioAsync(); 
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

    async static Task InsertarUsuarioAsync()
    {


        Console.WriteLine("Los datos con * son obligatorios.\n\n");
        var e = new Usuario
        {

            Email = LeerTexto("Inserta el correo *: "),
            NombreUsuario = LeerTexto("Inserta el nombre *: "),
            PrimerApellido = LeerTexto("Inserta el primer apellido *: "),
            SegundoApellido = LeerTexto("Inserta el segundo apellido: "),
            Password = LeerTexto("Inserta el password *: "),
            Activo = true
        };
        if (e.SegundoApellido == "") e.SegundoApellido = null;


        int id = await UsuarioServicio.InsertarAsync(e);
        Console.WriteLine($"Empleado creado con Id {id}");
    }

    async static Task ListarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("LISTADO DE USUARIOS");
        var lista = await UsuarioServicio.ListarAsync();
        foreach (var fila in lista)
        {
            Console.WriteLine($"{fila.IdUsuario} {fila.NombreUsuario} {fila.PrimerApellido} {fila.SegundoApellido} {fila.Email}");
        }
        while (true)
        {
            Console.WriteLine("\n\nToca una tecla para continuar...");
            Console.ReadLine();
            Console.Clear();
            break;
        }
    }

    async static Task ModificarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("MODIFICAR DE USUARIOS");
        
    }

    async static Task BorrarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("BORRAR DE USUARIOS");
        if (!int.TryParse(LeerTexto("Introduce el Id a buscar: "), out int id))
            throw new ArgumentException("Escribe un entero.");

        

        var e = await UsuarioServicio.BuscarAsync(id);
        /*if (e is null)
        {
            Console.WriteLine("No existe ese Id.");
            return;
        }
        Mostrar(e);*/
        if (LeerTexto("¿Borrar este empleado? (S/N): ").ToUpperInvariant() != "S")
            return;
        bool borrado = await UsuarioServicio.BorrarAsync(id);
        Console.WriteLine(borrado ? "Empleado borrado." : "No se borró: ya no existe o tiene relaciones.");

    }

    async static Task BuscarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("BUSCAR DE USUARIOS");

    }
    


}
