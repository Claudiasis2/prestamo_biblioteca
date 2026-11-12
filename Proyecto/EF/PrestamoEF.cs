using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    public static class PrestamoEF
    {
        public static async Task<List<SolicitudPrestamo>> ListarSolicitudesAprobadasSinEntregarAsync()
        {
            await using var db = ConexionEf.CrearContexto();

            return await db.SolicitudPrestamos
                .Include(s => s.FkusuarioNavigation)
                .Include(s => s.FkmaterialBibliotecaNavigation)
                .Where(s => s.FkestadoSolicitudPrestamo == 5
                         && !db.Prestamos.Any(p => p.FksolicitudPrestamo == s.IdSolicitud))
                .OrderBy(s => s.IdSolicitud)
                .ToListAsync();
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
                    FkestadoPrestamo = 2
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
                //.Include(p => p.FkestadoNavigation)
                .Include(p => p.FkejemplarNavigation)
                .Where(p => p.FkestadoPrestamo == 2)
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

                if (prestamo.FkestadoPrestamo != 2)
                    throw new InvalidOperationException($"El préstamo #{idPrestamo} ya se encuentra devuelto o finalizado.");

                prestamo.FechaDevolucionReal = DateTime.Now;
                prestamo.FkestadoPrestamo = 3; 

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
