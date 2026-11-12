using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class PrestamoServicio
    {
        public static async Task<List<Prestamo>> ListarAsync()
            => await PrestamoEF.ListarAsync();

        public static async Task<Prestamo?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await PrestamoEF.BuscarAsync(id);
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

        static void ComprobarCampos(Prestamo e)
        {
            if (e.FechaSalida == DateTime.MinValue)
                throw new ArgumentException(
                "Falta la fecha de salida.");
            if (e.FechaLimiteDevolucion == DateOnly.MinValue)
                throw new ArgumentException(
                "Falta la fecha de devolución.");
        }

        public static async Task<int> InsertarAsync(Prestamo e)
        {
            ComprobarCampos(e);

            return await PrestamoEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(Prestamo e)
        {
            Prestamo? mat = await BuscarAsync(e.IdPrestamo);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de Prestamo.");
            }

            ComprobarCampos(e);

            return await PrestamoEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await PrestamoEF.BorrarAsync(id);
        }
    }
}
