using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    public static class PrestamoServicio
    {
        public static async Task<List<SolicitudPrestamo>> ListarSolicitudesAprobadasSinEntregarAsync()
        {
            return await PrestamoEF.ListarSolicitudesAprobadasSinEntregarAsync();
        }

        public static async Task<int> CrearPrestamoAsync(int idSolicitud, int diasPrestamo = 7)
        {
            if (idSolicitud <= 0)
            {
                throw new ArgumentException("El identificador de la solicitud debe ser un entero positivo.");
            }

            if (diasPrestamo <= 0)
            {
                throw new ArgumentException("Los días de préstamo deben ser un entero positivo.");
            }

            var solicitud = await SolicitudPrestamoEF.BuscarPorIdAsync(idSolicitud);
            if (solicitud is null)
            {
                throw new ArgumentException($"No existe ninguna solicitud registrada con el ID {idSolicitud}.");
            }

            if (solicitud.FkestadoSolicitudPrestamo != 5)
            {
                //throw new InvalidOperationException($"La solicitud #{idSolicitud} no se encuentra en estado Aprobado. Estado actual: {solicitud.FkestadoNavigation?.NombreEstado ?? solicitud.Fkestado.ToString()}.");
            }

            return await PrestamoEF.CrearPrestamoTransaccionalAsync(idSolicitud, diasPrestamo);
        }

        public static async Task<List<Prestamo>> ListarPrestamosActivosAsync()
        {
            return await PrestamoEF.ListarPrestamosActivosAsync();
        }

        public static async Task RegistrarDevolucionAsync(int idPrestamo)
        {
            if (idPrestamo <= 0)
            {
                throw new ArgumentException("El identificador del préstamo debe ser un entero positivo.");
            }

            await PrestamoEF.RegistrarDevolucionTransaccionalAsync(idPrestamo);
        }
    }
}
