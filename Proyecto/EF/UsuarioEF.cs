using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    public static class UsuarioEF
    {
        public static async Task<List<Usuario>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.Usuarios
                    .OrderBy(f => f.IdUsuario)
                    .ToListAsync<Usuario>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<Usuario>();
            }
        }

        public static async Task<Usuario?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Usuarios
                .SingleOrDefaultAsync(f => f.IdUsuario == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(Usuario e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Usuario
                {
                    IdUsuario = e.IdUsuario,
                    NombreUsuario = e.NombreUsuario,
                    PrimerApellido = e.PrimerApellido,
                    SegundoApellido = e.SegundoApellido,
                    Email = e.Email,
                    Activo = e.Activo,
                };
                db.Usuarios.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdUsuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<bool> BorrarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = db.Usuarios.SingleOrDefault(
                f => f.IdUsuario == id);
                if (fila is null) return false;
                db.Usuarios.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Usuario Convertir(Usuario fila)
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

    }
}
