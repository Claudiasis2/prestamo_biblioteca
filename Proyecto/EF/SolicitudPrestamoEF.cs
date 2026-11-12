using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
	public static class SolicitudPrestamoEF
	{
		public static async Task<int> ObtenerCupoUsadoUsuarioAsync(int idUsuario)
		{
			await using var db = ConexionEf.CrearContexto();

			int prestamosActivos = await db.Prestamos
				.CountAsync(p => p.Fkusuario == idUsuario && p.Fkestado == 2);
			int solicitudesVigentes = await db.SolicitudPrestamos
				.CountAsync(s => s.Fkusuario == idUsuario
				&& (s.Fkestado == 4 || s.Fkestado == 5)
				&& !db.Prestamos.Any(p => p.FksolicitudPrestamo == s.IdSolicitud));
			return prestamosActivos + solicitudesVigentes;
		}

		public static async Task<List<int>> VerificarMaterialesSinDisponibilidadAsync(List<int> idsMateriales)
		{
			await using var db = ConexionEf.CrearContexto();

			var sinDisponibilidad = new List<int>();
			foreach (var idMat in idsMateriales)
			{
				bool tieneEjemplarDisponible = await db.Ejemplars
                    .AnyAsync(e => e.FkmaterialBiblioteca == idMat && e.Fkestado == 1);
				if (!tieneEjemplarDisponible)
				{
					sinDisponibilidad.Add(idMat);
				}
			}

			return sinDisponibilidad;
		}
		public static async Task<List<int>> InsertarSolicitudesAsync(int idUsuario, List<int> idsMateriales)
		{
			await using var db = ConexionEf.CrearContexto();
			await using var tx = await db.Database.BeginTransactionAsync();

			try
			{
				var solicitudesNuevas = new List<SolicitudPrestamo>();
				foreach (var idMaterial in idsMateriales)
				{
					var solicitud = new SolicitudPrestamo
					{
						Fkusuario = idUsuario,
						FkmaterialBiblioteca = idMaterial,
						FechaSolicitud = DateTime.Now,
						Fkestado = 4
					};
					solicitudesNuevas.Add(solicitud);
				}

				db.SolicitudPrestamos.AddRange(solicitudesNuevas);
				await db.SaveChangesAsync();
				await tx.CommitAsync();
				return solicitudesNuevas.Select(s => s.IdSolicitud).ToList();
			}
			catch
			{
				await tx.RollbackAsync();
				throw;
			}
		}
		public static async Task<List<SolicitudPrestamo>> ListarPorUsuarioAsync(int idUsuario)
		{
			await using var db = ConexionEf.CrearContexto();
			return await db.SolicitudPrestamos
				.Include(s => s.FkmaterialBibliotecaNavigation)
				.Include(s => s.FkestadoNavigation)
				.Where(s => s.Fkusuario == idUsuario)
				.OrderByDescending(s => s.IdSolicitud)
				.ToListAsync();
		}

        public static async Task<List<SolicitudPrestamo>> ListarPendientesAsync()
        {
            await using var db = ConexionEf.CrearContexto();

            return await db.SolicitudPrestamos
                .Include(s => s.FkusuarioNavigation)
                        .Include(s => s.FkmaterialBibliotecaNavigation)
                        .Include(s => s.FkestadoNavigation)
                        .Where(s => s.Fkestado == 4)
                        .OrderBy(s => s.IdSolicitud)
                        .ToListAsync();
        }

        public static async Task<SolicitudPrestamo?> BuscarPorIdAsync(int idSolicitud)
        {
            await using var db = ConexionEf.CrearContexto();

            return await db.SolicitudPrestamos
                .Include(s => s.FkusuarioNavigation)
                        .Include(s => s.FkmaterialBibliotecaNavigation)
                        .Include(s => s.FkestadoNavigation)
                        .FirstOrDefaultAsync(s => s.IdSolicitud == idSolicitud);
        }

        public static async Task<bool> CambiarEstadoAsync(int idSolicitud, int nuevoEstado)
        {
            await using var db = ConexionEf.CrearContexto();

            var solicitud = await db.SolicitudPrestamos.FindAsync(idSolicitud);
            if (solicitud is null) return false;

            solicitud.Fkestado = nuevoEstado;
            await db.SaveChangesAsync();
            return true;
        }
    }
}






