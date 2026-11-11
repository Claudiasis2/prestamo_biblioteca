using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class PrestamoEF
    {
        public static async Task<List<Prestamo>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.Prestamos
                    .OrderBy(f => f.IdPrestamo)
                    .ToListAsync<Prestamo>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<Prestamo>();
            }
        }

        public static async Task<Prestamo?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Prestamos
                .SingleOrDefaultAsync(f => f.IdPrestamo == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(Prestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Prestamo
                {
                    IdPrestamo = e.IdPrestamo,
                    FechaSalida = e.FechaSalida,
                    FechaLimiteDevolucion = e.FechaLimiteDevolucion,
                    FechaDevolucionReal = e.FechaDevolucionReal,
                    FksolicitudPrestamo = e.FksolicitudPrestamo,
                    Fkempleado = e.Fkempleado,
                    Fkusuario = e.Fkusuario,
                    Fkejemplar = e.Fkejemplar,
                    Fkestado = e.Fkestado,
                };
                db.Prestamos.Add(fila);
                await db.SaveChangesAsync();
                return fila.IdPrestamo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(Prestamo e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Prestamo
                {
                    IdPrestamo = e.IdPrestamo,
                    FechaSalida = e.FechaSalida,
                    FechaLimiteDevolucion = e.FechaLimiteDevolucion,
                    FechaDevolucionReal = e.FechaDevolucionReal,
                    FksolicitudPrestamo = e.FksolicitudPrestamo,
                    Fkempleado = e.Fkempleado,
                    Fkusuario = e.Fkusuario,
                    Fkejemplar = e.Fkejemplar,
                    Fkestado = e.Fkestado
                };
                db.Prestamos.Update(fila);
                await db.SaveChangesAsync();
                return fila.IdPrestamo;
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
                var fila = db.Prestamos.SingleOrDefault(
                f => f.IdPrestamo == id);
                if (fila is null) return false;
                db.Prestamos.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Prestamo Convertir(Prestamo fila)
        {
            return new Prestamo
            {
                IdPrestamo = fila.IdPrestamo,
                FechaSalida = fila.FechaSalida,
                FechaLimiteDevolucion = fila.FechaLimiteDevolucion,
                FechaDevolucionReal = fila.FechaDevolucionReal,
                FksolicitudPrestamo = fila.FksolicitudPrestamo,
                Fkempleado = fila.Fkempleado,
                Fkusuario = fila.Fkusuario,
                Fkejemplar = fila.Fkejemplar,
                Fkestado = fila.Fkestado
            };
        }
    }
}
