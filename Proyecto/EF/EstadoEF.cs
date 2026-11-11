using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialEstado.EF
{
    internal class EstadoEF
    {
        public static async Task<List<Estado>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.Estados
                    .OrderBy(f => f.IdEstado)
                    .ToListAsync<Estado>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<Estado>();
            }
        }

        public static async Task<Estado?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Estados
                .SingleOrDefaultAsync(f => f.IdEstado == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(Estado e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Estado
                {
                    IdEstado = e.IdEstado,
                    NombreEstado = e.NombreEstado,
                };
                db.Estados.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdEstado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(Estado e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Estado
                {
                    IdEstado = e.IdEstado,
                    NombreEstado = e.NombreEstado,
                };
                db.Estados.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdEstado;
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
                var fila = db.Estados.SingleOrDefault(
                f => f.IdEstado == id);
                if (fila is null) return false;
                db.Estados.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Estado Convertir(Estado fila)
        {
            return new Estado
            {
                IdEstado = fila.IdEstado,
                NombreEstado = fila.NombreEstado
            };
        }
    }
}
