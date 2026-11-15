using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class EstadoSolicitudPrestamoEF
    {
        public static async Task<List<EstadoSolicitudPrestamo>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.EstadoSolicitudPrestamos
                    .OrderBy(f => f.IdEstadoSolicitudPrestamo)
                    .ToListAsync<EstadoSolicitudPrestamo>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<EstadoSolicitudPrestamo>();
            }
        }

        public static async Task<EstadoSolicitudPrestamo?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.EstadoSolicitudPrestamos
                .SingleOrDefaultAsync(f => f.IdEstadoSolicitudPrestamo == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(EstadoSolicitudPrestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoSolicitudPrestamo
                {
                    IdEstadoSolicitudPrestamo = e.IdEstadoSolicitudPrestamo,
                    NombreEstado = e.NombreEstado,
                };
                db.EstadoSolicitudPrestamos.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoSolicitudPrestamo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(EstadoSolicitudPrestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new EstadoSolicitudPrestamo
                {
                    IdEstadoSolicitudPrestamo = e.IdEstadoSolicitudPrestamo,
                    NombreEstado = e.NombreEstado,
                };
                db.EstadoSolicitudPrestamos.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdEstadoSolicitudPrestamo;
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
                var fila = db.EstadoSolicitudPrestamos.SingleOrDefault(
                f => f.IdEstadoSolicitudPrestamo == id);
                if (fila is null) return false;
                db.EstadoSolicitudPrestamos.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static EstadoSolicitudPrestamo Convertir(EstadoSolicitudPrestamo fila)
        {
            return new EstadoSolicitudPrestamo
            {
                IdEstadoSolicitudPrestamo = fila.IdEstadoSolicitudPrestamo,
                NombreEstado = fila.NombreEstado
            };
        }
    }
}