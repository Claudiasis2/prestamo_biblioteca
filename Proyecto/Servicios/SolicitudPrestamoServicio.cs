using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    public static class SolicitudPrestamoServicio
    {
        private const int LIMITE_MAXIMO_CUPOS = 3;

        public static async Task<int> ObtenerCupoDisponibleAsync(int idUsuario)
        {
            ComprobarIdUsuario(idUsuario);

            int cupoConsumido = await SolicitudPrestamoEF.ObtenerCupoUsadoUsuarioAsync(idUsuario);
            int disponible = LIMITE_MAXIMO_CUPOS - cupoConsumido;

            return disponible < 0 ? 0 : disponible;
        }

        public static async Task<List<int>> CrearSolicitudAsync(int idUsuario, List<int> idsMateriales)
        {
            ComprobarIdUsuario(idUsuario);

            var usuarios = await UsuarioEF.BuscarAsync(id: idUsuario);
            var usuario = usuarios.FirstOrDefault();
            if (usuario is null)
            {
                throw new ArgumentException($"No existe ningún usuario registrado con el ID {idUsuario}.");
            }
            if (!usuario.Activo)
            {
                throw new ArgumentException($"El usuario con ID {idUsuario} se encuentra inactivo y no puede realizar solicitudes.");
            }

            if (idsMateriales is null || !idsMateriales.Any())
            {
                throw new ArgumentException("Debe seleccionar al menos un material para realizar la solicitud.");
            }
            if (idsMateriales.Count > LIMITE_MAXIMO_CUPOS)
            {
                throw new ArgumentException($"No puede solicitar más de {LIMITE_MAXIMO_CUPOS} materiales en una misma transacción.");
            }
            if (idsMateriales.Any(id => id <= 0))
            {
                throw new ArgumentException("Uno o más identificadores de material no son válidos.");
            }
            if (idsMateriales.Distinct().Count() != idsMateriales.Count)
            {
                throw new ArgumentException("No puede incluir el mismo material más de una vez en la misma solicitud.");
            }

            foreach (var idMat in idsMateriales)
            {
                var mat = await MaterialBibliotecaEF.BuscarAsync(idMat);
                if (mat is null)
                {
                    throw new ArgumentException($"No existe el material con ID {idMat} en el catálogo de la biblioteca.");
                }
            }

            int cupoConsumido = await SolicitudPrestamoEF.ObtenerCupoUsadoUsuarioAsync(idUsuario);
            int cupoDisponible = LIMITE_MAXIMO_CUPOS - cupoConsumido;

            if (cupoDisponible <= 0)
            {
                throw new ArgumentException("Ha alcanzado el límite máximo de 3 materiales pendientes de devolución (entre préstamos activos y solicitudes en trámite). No puede realizar más solicitudes hasta devolver material o cancelar solicitudes previas.");
            }

            if (idsMateriales.Count > cupoDisponible)
            {
                throw new ArgumentException($"Actualmente solo dispone de {cupoDisponible} cupo(s) libre(s). No puede solicitar {idsMateriales.Count} materiales.");
            }

            var sinStock = await SolicitudPrestamoEF.VerificarMaterialesSinDisponibilidadAsync(idsMateriales);
            if (sinStock.Any())
            {
                string idsSinStock = string.Join(", ", sinStock);
                throw new ArgumentException($"Los siguientes materiales no cuentan con ejemplares disponibles en estantería (ID Material: {idsSinStock}).");
            }

            return await SolicitudPrestamoEF.InsertarSolicitudesAsync(idUsuario, idsMateriales);
        }

        public static async Task<List<SolicitudPrestamo>> ListarPorUsuarioAsync(int idUsuario)
        {
            ComprobarIdUsuario(idUsuario);
            return await SolicitudPrestamoEF.ListarPorUsuarioAsync(idUsuario);
        }

        private static void ComprobarIdUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El identificador del usuario debe ser un entero positivo.");
            }
        }

        public static async Task<List<SolicitudPrestamo>> ListarPendientesAsync()
        {
            return await SolicitudPrestamoEF.ListarPendientesAsync();
        }

        /*public static async Task AprobarSolicitudAsync(int idSolicitud)
        {
            if (idSolicitud <= 0)
            {
                throw new ArgumentException("El identificador de la solicitud debe ser un entero positivo.");
            }

            var solicitud = await SolicitudPrestamoEF.BuscarPorIdAsync(idSolicitud);
            if (solicitud is null)
            {
                throw new ArgumentException($"No existe ninguna solicitud registrada con el ID {idSolicitud}.");
            }

            if (solicitud.Fkestado != 4)
            {
                throw new ArgumentException($"La solicitud #{idSolicitud} no se encuentra en estado Pendiente. Estado actual: {solicitud.FkestadoNavigation?.NombreEstado ?? solicitud.Fkestado.ToString()}.");
            }

            await SolicitudPrestamoEF.CambiarEstadoAsync(idSolicitud, 5);
        }*/

        /*public static async Task RechazarSolicitudAsync(int idSolicitud)
        {
            if (idSolicitud <= 0)
            {
                throw new ArgumentException("El identificador de la solicitud debe ser un entero positivo.");
            }

            var solicitud = await SolicitudPrestamoEF.BuscarPorIdAsync(idSolicitud);
            if (solicitud is null)
            {
                throw new ArgumentException($"No existe ninguna solicitud registrada con el ID {idSolicitud}.");
            }

            if (solicitud.Fkestado != 4)
            {
                throw new ArgumentException($"La solicitud #{idSolicitud} no se encuentra en estado Pendiente. Estado actual: {solicitud.FkestadoNavigation?.NombreEstado ?? solicitud.Fkestado.ToString()}.");
            }

            await SolicitudPrestamoEF.CambiarEstadoAsync(idSolicitud, 6);
        }*/
    }
}




