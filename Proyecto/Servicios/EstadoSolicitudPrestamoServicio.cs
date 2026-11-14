using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class EstadoSolicitudPrestamoServicio
    {
        public static async Task<List<EstadoSolicitudPrestamo>> ListarAsync()
            => await EstadoSolicitudPrestamoEF.ListarAsync();

        public static async Task<EstadoSolicitudPrestamo?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoSolicitudPrestamoEF.BuscarAsync(id);
        }

        private static void ComprobarId(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                "El identificador debe ser positivo.");
        }

        private static void ComprobarTexto(string? texto, int maximo, bool obligatorio)
        {
            if (obligatorio && string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException(
                "Falta un texto obligatorio.");
            if (texto is not null && texto.Length > maximo)
                throw new ArgumentException(
                $"Un texto supera los {maximo} caracteres.");
        }

        static void ComprobarCampos(EstadoSolicitudPrestamo e)
        {
            ComprobarTexto(e.NombreEstado, 100, true);
        }

        public static async Task<int> InsertarAsync(EstadoSolicitudPrestamo e)
        {
            ComprobarCampos(e);

            return await EstadoSolicitudPrestamoEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(EstadoSolicitudPrestamo e)
        {
            EstadoSolicitudPrestamo? mat = await BuscarAsync(e.IdEstadoSolicitudPrestamo);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de EstadoSolicitudPrestamo.");
            }

            ComprobarCampos(e);

            return await EstadoSolicitudPrestamoEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoSolicitudPrestamoEF.BorrarAsync(id);
        }
    }
}