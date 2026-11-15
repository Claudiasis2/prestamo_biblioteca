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
        public const int ESTADO_SOLICITUD_PENDIENTE = 1; 
		public const int ESTADO_SOLICITUD_APROBADA = 2;
		public const int ESTADO_SOLICITUD_RECHAZADA = 3; 
		public const int ESTADO_SOLICITUD_CANCELADA = 4;
		
		public static async Task<int> ObtenerCupoUsadoUsuarioAsync(int idUsuario)
		{
			await using var db = ConexionEf.CrearContexto();

			int prestamosActivos = await db.Prestamos
				.CountAsync(p => p.Fkusuario == idUsuario && p.FkestadoPrestamo == 2);
			int solicitudesVigentes = await db.SolicitudPrestamos
				.CountAsync(s => s.Fkusuario == idUsuario
				&& (s.FkestadoSolicitudPrestamo == ESTADO_SOLICITUD_PENDIENTE || s.FkestadoSolicitudPrestamo == ESTADO_SOLICITUD_APROBADA)
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
                        FkestadoSolicitudPrestamo = ESTADO_SOLICITUD_PENDIENTE
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
				.Include(s => s.FkestadoSolicitudPrestamoNavigation)
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
                        .Include(s => s.FkestadoSolicitudPrestamoNavigation)
                        .Where(s => s.FkestadoSolicitudPrestamo == ESTADO_SOLICITUD_PENDIENTE)
                        .OrderBy(s => s.IdSolicitud)
                        .ToListAsync();
        }

		public static async Task<SolicitudPrestamo?> BuscarPorIdAsync(int idSolicitud)
		{
			await using var db = ConexionEf.CrearContexto();

			return await db.SolicitudPrestamos
				.Include(s => s.FkusuarioNavigation)
						.Include(s => s.FkmaterialBibliotecaNavigation)
						.Include(s => s.FkestadoSolicitudPrestamoNavigation)
						.FirstOrDefaultAsync(s => s.IdSolicitud == idSolicitud);
		}

		public static async Task<bool> CambiarEstadoAsync(int idSolicitud, int nuevoEstado)
        {
            await using var db = ConexionEf.CrearContexto();

            var solicitud = await db.SolicitudPrestamos.FindAsync(idSolicitud);
            if (solicitud is null) return false;

            solicitud.FkestadoSolicitudPrestamo = nuevoEstado;
            await db.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> CancelarSolicitudUsuarioAsync(int idSolicitud, int idUsuario)
        {
            await using var db = ConexionEf.CrearContexto();

            var solicitud = await db.SolicitudPrestamos.FirstOrDefaultAsync(s => s.IdSolicitud == idSolicitud && s.Fkusuario == idUsuario);
            if (solicitud is null) return false;

            if (solicitud.FkestadoSolicitudPrestamo != 1)
			{ 
				throw new InvalidOperationException($"La solicitud #{idSolicitud} no se puede cancelar porque no está en estado Pendiente."); 
			}
            solicitud.FkestadoSolicitudPrestamo = 4;
			await db.SaveChangesAsync(); 
			return true;
        }
    }
}






