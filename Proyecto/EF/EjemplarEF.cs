using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class EjemplarEF
    {
        public static async Task<List<Ejemplar>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.Ejemplars
                    .OrderBy(f => f.IdEjemplar)
                    .ToListAsync<Ejemplar>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<Ejemplar>();
            }
        }

        public static async Task<Ejemplar?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Ejemplars
                .SingleOrDefaultAsync(f => f.IdEjemplar == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(Ejemplar e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Ejemplar
                {
                    IdEjemplar = e.IdEjemplar,
                    UbicacionEstanteria = e.UbicacionEstanteria,
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca,
                    FKEempleado = e.FKEempleado,
                    FkestadoEjemplar = e.FKEstadoEjemplar
                };
                db.Ejemplars.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdEjemplar;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(Ejemplar e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Ejemplar
                {
                    IdEjemplar = e.IdEjemplar,
                    UbicacionEstanteria = e.UbicacionEstanteria,
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca,
                    FKEempleado = e.FKEempleado,
                    FkestadoEjemplar = e.FKEstadoEjemplar
                };
                db.Ejemplars.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdEjemplar;
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
                var fila = db.Ejemplars.SingleOrDefault(
                f => f.IdEjemplar == id);
                if (fila is null) return false;
                db.Ejemplars.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Ejemplar Convertir(Ejemplar fila)
        {
            return new Ejemplar
            {
                IdEjemplar = fila.IdEjemplar,
                UbicacionEstanteria = fila.UbicacionEstanteria,
                FkmaterialBiblioteca = fila.FkmaterialBiblioteca,
                FKEempleado = fila.FKEempleado,
                FkestadoEjemplar = fila.FkestadoEjemplar    
            };
        }
    }
}