using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using ProyectoConjuntoMaterialPrestamo.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


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
                    case "1":
                        await ListarUsuarioAsync();
                        break;
                    case "2":
                        await BuscarUsuarioAsync();
                        break;
                    case "3":
                        await InsertarUsuarioAsync();
                        break;
                    case "4":
                        await ModificarUsuarioAsync();
                        break;
                    case "5":
                        await BorrarUsuarioAsync();
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

        Console.Clear();
        Console.WriteLine("ALTA DE USUARIOS");
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
        Console.WriteLine("Usuario creado con éxito.\n");
        var u = await UsuarioEF.BuscarAsync(id);
        //Mostrar(u);
        SalirPantalla();
    }

    static async Task ListarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("LISTADO DE USUARIOS");
        var lista = await UsuarioServicio.ListarAsync();
        foreach (var u in lista)
            Mostrar(u);

        SalirPantalla();
    }

    async static Task ModificarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("MODIFICAR DE USUARIOS");
        var (email, nombre) = FormularioBusqueda();
        var listaUsuarios = await UsuarioServicio.BuscarAsync(0, email, nombre);
        if (listaUsuarios is null || listaUsuarios.Count == 0)
        {
            Console.WriteLine("No se han encontrado coincidencias.");
            SalirPantalla();
            return;
        }
        if (listaUsuarios.Count > 1)
        {
            foreach (var u in listaUsuarios)
                Mostrar(u);
            Console.WriteLine("Mas de una coincidencia.");
            SalirPantalla();
            return;
        }

        if (listaUsuarios.Count == 1)
        {
            var ok = await UsuarioServicio.ModificarAsync(FormularioCambios(listaUsuarios[0]));
            return;
        }


    }

    async static Task BorrarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("BORRAR DE USUARIOS");
        var lista = await UsuarioServicio.ListarAsync();
        foreach (var l in lista)
            Mostrar(l);

        if (!int.TryParse(LeerTexto("Introduce el Id a eliminar: "), out int id))
            throw new ArgumentException("Escribe un entero.");
        var usuario = await UsuarioServicio.BuscarAsync(id);
        if (usuario is null || usuario.Count > 1)
        {
            Console.WriteLine("No existe ese Id.");
            return;
        }

        foreach (var l in usuario)
            Mostrar(l);
        if (LeerTexto("\n¿Borrar este empleado? (S/N): ").ToUpperInvariant() != "S")
            return;
        bool borrado = await UsuarioServicio.BorrarAsync(id);
        Console.WriteLine(borrado ? "Empleado borrado." : "No se borró: ya no existe o tiene relaciones.");
        SalirPantalla();
    }

    public static async Task BuscarUsuarioAsync()
    {
        Console.Clear();
        Console.WriteLine("BUSCAR USUARIO");
        var (email, nombre) = FormularioBusqueda();
        var listaUsuarios = await UsuarioServicio.BuscarAsync(0, email, nombre);

        if (listaUsuarios is null)
            Console.WriteLine("No se han encontrado coincidencias.");
        else
            foreach (var u in listaUsuarios)
                Mostrar(u);

        SalirPantalla();
    }

    public static void Mostrar(Usuario u)
    {
        var estado = u.Activo ? "Activo" : "Desactivado";
        Console.WriteLine($"{u.IdUsuario} | {u.Email} {u.NombreUsuario} {u.PrimerApellido} {u.SegundoApellido} {estado}");

    }

    public static void SalirPantalla()
    {
        Console.WriteLine("\nPulsa una tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }

    static (string bEmail, string bNombre) FormularioBusqueda()
    {
        Console.WriteLine("FORMULARIO DE BÚSQUEDA");
        Console.WriteLine("Introduce los datos que buscas. Deja en blanco los valores que desconozcas.\n");
        string bEmail = LeerTexto("Introduce el email: ");
        string bNombre = LeerTexto("Introduce el Nombre: ");

        return (bEmail, bNombre);
    }

    public static Usuario FormularioCambios(Usuario u)
    {
        Console.Clear();
        Console.WriteLine("FORMULARIO DE CAMBIOS");
        Console.WriteLine("Los datos con * son obligatorios.\n\n");
        var e = new Usuario
        {
            IdUsuario = u.IdUsuario,
            Email = LeerTexto($"Inserta el correo * ({u.Email}): "),
            NombreUsuario = LeerTexto($"Inserta el nombre * ({u.NombreUsuario}): "),
            PrimerApellido = LeerTexto($"Inserta el primer apellido * ({u.PrimerApellido}): "),
            SegundoApellido = LeerTexto($"Inserta el segundo apellido ({u.SegundoApellido}): "),
            Password = u.Password,
            Activo = u.Activo
        };
        if (e.SegundoApellido == "") e.SegundoApellido = null;

        return e;
    }

}
