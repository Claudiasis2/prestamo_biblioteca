using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class EstadoEjemplarEF
    {
        public static async Task<List<EstadoEjemplar>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.EstadoEjemplars
                    .OrderBy(f => f.IdEstadoEjemplar)
                    .ToListAsync<EstadoEjemplar>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<EstadoEjemplar>();
            }
        }

        public static async Task<EstadoEjemplar?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.EstadoEjemplars
                .SingleOrDefaultAsync(f => f.IdEstadoEjemplar == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(EstadoEjemplar e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoEjemplar
                {
                    IdEstadoEjemplar = e.IdEstadoEjemplar,
                    NombreEstado = e.NombreEstado,
                };
                db.EstadoEjemplars.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoEjemplar;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(EstadoEjemplar e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoEjemplar
                {
                    IdEstadoEjemplar = e.IdEstadoEjemplar,
                    NombreEstado = e.NombreEstado,
                };
                db.EstadoEjemplars.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoEjemplar;
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
                var fila = db.EstadoEjemplars.SingleOrDefault(
                f => f.IdEstadoEjemplar == id);
                if (fila is null) return false;
                db.EstadoEjemplars.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static EstadoEjemplar Convertir(EstadoEjemplar fila)
        {
            return new EstadoEjemplar
            {
                IdEstadoEjemplar = fila.IdEstadoEjemplar,
                NombreEstado = fila.NombreEstado
            };
        }
    }
}