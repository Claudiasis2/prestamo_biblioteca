using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class EstadoSolitudPrestamoEF
    {
        public static async Task<List<EstadoSolitudPrestamo>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.EstadoSolitudPrestamos
                    .OrderBy(f => f.IdEstadoSolitudPrestamo)
                    .ToListAsync<EstadoSolitudPrestamo>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<EstadoSolitudPrestamo>();
            }
        }

        public static async Task<EstadoSolitudPrestamo?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.EstadoSolitudPrestamos
                .SingleOrDefaultAsync(f => f.IdEstadoSolitudPrestamo == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(EstadoSolitudPrestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoSolitudPrestamo
                {
                    IdEstadoSolitudPrestamo = e.IdEstadoSolitudPrestamo,
                    NombreEstadoSolitudPrestamo = e.NombreEstadoSolitudPrestamo,
                };
                db.EstadoSolitudPrestamos.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoSolitudPrestamo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(EstadoSolitudPrestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoSolitudPrestamo
                {
                    IdEstadoSolitudPrestamo = e.IdEstadoSolitudPrestamo,
                    NombreEstadoSolitudPrestamo = e.NombreEstadoSolitudPrestamo,
                };
                db.EstadoSolitudPrestamos.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoSolitudPrestamo;
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
                var fila = db.EstadoSolitudPrestamos.SingleOrDefault(
                f => f.IdEstadoSolitudPrestamo == id);
                if (fila is null) return false;
                db.EstadoSolitudPrestamos.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static EstadoSolitudPrestamo Convertir(EstadoSolitudPrestamo fila)
        {
            return new EstadoSolitudPrestamo
            {
                IdEstadoSolitudPrestamo = fila.IdEstadoSolitudPrestamo,
                NombreEstadoSolitudPrestamo = fila.NombreEstadoSolitudPrestamo
            };
        }
    }
}