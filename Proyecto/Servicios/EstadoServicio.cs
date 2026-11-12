using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class EstadoServicio
    {
        public static async Task<List<Estado>> ListarAsync()
            => await EstadoEF.ListarAsync();

        public static async Task<Estado?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoEF.BuscarAsync(id);
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

        static void ComprobarCampos(Estado e)
        {
            ComprobarTexto(e.NombreEstado, 100, true);
        }

        public static async Task<int> InsertarAsync(Estado e)
        {
            ComprobarCampos(e);

            return await EstadoEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(Estado e)
        {
            Estado? mat = await BuscarAsync(e.IdEstado);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de Estado.");
            }

            ComprobarCampos(e);

            return await EstadoEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await EstadoEF.BorrarAsync(id);
        }
    }
}
