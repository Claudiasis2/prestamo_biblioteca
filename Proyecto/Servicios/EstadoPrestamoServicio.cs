using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class EstadoPrestamoServicio
    {
        public static async Task<List<EstadoPrestamo>> ListarAsync()
            => await EstadoPrestamoEF.ListarAsync();

        public static async Task<EstadoPrestamo?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoPrestamoEF.BuscarAsync(id);
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

        static void ComprobarCampos(EstadoPrestamo e)
        {
            ComprobarTexto(e.NombreEstado, 100, true);
        }

        public static async Task<int> InsertarAsync(EstadoPrestamo e)
        {
            ComprobarCampos(e);

            return await EstadoPrestamoEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(EstadoPrestamo e)
        {
            EstadoPrestamo? mat = await BuscarAsync(e.IdEstadoPrestamo);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de EstadoPrestamo.");
            }

            ComprobarCampos(e);

            return await EstadoPrestamoEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoPrestamoEF.BorrarAsync(id);
        }
    }
}