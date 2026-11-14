using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class EstadoPrestamoEF
    {
        public static async Task<List<EstadoPrestamo>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.EstadoPrestamos
                    .OrderBy(f => f.IdEstadoPrestamo)
                    .ToListAsync<EstadoPrestamo>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<EstadoPrestamo>();
            }
        }

        public static async Task<EstadoPrestamo?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.EstadoPrestamos
                .SingleOrDefaultAsync(f => f.IdEstadoPrestamo == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(EstadoPrestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoPrestamo
                {
                    IdEstadoPrestamo = e.IdEstadoPrestamo,
                    NombreEstado = e.NombreEstado,
                };
                db.EstadoPrestamos.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoPrestamo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(EstadoPrestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoPrestamo
                {
                    IdEstadoPrestamo = e.IdEstadoPrestamo,
                    NombreEstado = e.NombreEstado,
                };
                db.EstadoPrestamos.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoPrestamo;
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
                var fila = db.EstadoPrestamos.SingleOrDefault(
                f => f.IdEstadoPrestamo == id);
                if (fila is null) return false;
                db.EstadoPrestamos.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static EstadoPrestamo Convertir(EstadoPrestamo fila)
        {
            return new EstadoPrestamo
            {
                IdEstadoPrestamo = fila.IdEstadoPrestamo,
                NombreEstado = fila.NombreEstado
            };
        }
    }
}