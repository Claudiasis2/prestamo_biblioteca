using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class PrestamoEF
    {
        public static async Task<List<SolicitudPrestamo>> ListarSolicitudesAprobadasSinEntregarAsync()
        {
            await using var db = ConexionEf.CrearContexto();

            return await db.SolicitudPrestamos
                .Include(s => s.FkusuarioNavigation)
                .Include(s => s.FkmaterialBibliotecaNavigation)
                .Where(s => s.Fkestado == 5
                         && !db.Prestamos.Any(p => p.FksolicitudPrestamo == s.IdSolicitud))
                .OrderBy(s => s.IdSolicitud)
                .ToListAsync();
        }

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

        public static async Task<int> CrearPrestamoTransaccionalAsync(int idSolicitud, int diasPrestamo, int idEmpleado = 1)
        {
            await using var db = ConexionEf.CrearContexto();
            await using var tx = await db.Database.BeginTransactionAsync();

            try
            {
                var solicitud = await db.SolicitudPrestamos
                    .Include(s => s.FkmaterialBibliotecaNavigation)
                    .FirstOrDefaultAsync(s => s.IdSolicitud == idSolicitud);

                if (solicitud is null)
                    throw new ArgumentException($"No existe la solicitud #{idSolicitud}.");

                var ejemplar = await db.Ejemplars
                    .FirstOrDefaultAsync(e => e.FkmaterialBiblioteca == solicitud.FkmaterialBiblioteca && e.Fkestado == 1);

                if (ejemplar is null)
                    throw new InvalidOperationException($"No hay ejemplares físicos disponibles en estantería para el material '{solicitud.FkmaterialBibliotecaNavigation?.Titulo}'.");

                var nuevoPrestamo = new Prestamo
                {
                    Fkusuario = solicitud.Fkusuario,
                    Fkejemplar = ejemplar.IdEjemplar,
                    FksolicitudPrestamo = solicitud.IdSolicitud,
                    Fkempleado = idEmpleado,
                    FechaSalida = DateTime.Now,
                    FechaLimiteDevolucion = DateOnly.FromDateTime(DateTime.Now.AddDays(diasPrestamo)),
                    Fkestado = 2
                };

                db.Prestamos.Add(nuevoPrestamo);

                ejemplar.Fkestado = 2;

                await db.SaveChangesAsync();
                await tx.CommitAsync();

                return nuevoPrestamo.IdPrestamo;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public static async Task<List<Prestamo>> ListarPrestamosActivosAsync()
        {
            await using var db = ConexionEf.CrearContexto();

            return await db.Prestamos
                .Include(p => p.FkusuarioNavigation)
                .Include(p => p.FkejemplarNavigation)
                    .ThenInclude(e => e.FkmaterialBibliotecaNavigation)
                .Include(p => p.FkestadoNavigation)
                .Where(p => p.Fkestado == 2)
                .OrderBy(p => p.IdPrestamo)
                .ToListAsync();
        }

        public static async Task<bool> RegistrarDevolucionTransaccionalAsync(int idPrestamo)
        {
            await using var db = ConexionEf.CrearContexto();
            await using var tx = await db.Database.BeginTransactionAsync();

            try
            {
                var prestamo = await db.Prestamos
                    .Include(p => p.FkejemplarNavigation)
                    .FirstOrDefaultAsync(p => p.IdPrestamo == idPrestamo);

                if (prestamo is null)
                    throw new ArgumentException($"No existe el préstamo #{idPrestamo}.");

                if (prestamo.Fkestado != 2)
                    throw new InvalidOperationException($"El préstamo #{idPrestamo} ya se encuentra devuelto o finalizado.");

                prestamo.FechaDevolucionReal = DateTime.Now;
                prestamo.Fkestado = 3;

                if (prestamo.FkejemplarNavigation != null)
                {
                    prestamo.FkejemplarNavigation.Fkestado = 1;
                }

                await db.SaveChangesAsync();
                await tx.CommitAsync();

                return true;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}