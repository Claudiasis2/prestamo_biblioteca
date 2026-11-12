using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class EjemplarServicio
    {
        public static async Task<List<Ejemplar>> ListarAsync()
            => await EjemplarEF.ListarAsync();

        public static async Task<Ejemplar?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await EjemplarEF.BuscarAsync(id);
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

        static void ComprobarCampos(Ejemplar e)
        {
            ComprobarTexto(e.UbicacionEstanteria, 100, false);
        }

        public static async Task<int> InsertarAsync(Ejemplar e)
        {
            ComprobarCampos(e);

            return await EjemplarEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(Ejemplar e)
        {
            Ejemplar? mat = await BuscarAsync(e.IdEjemplar);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de Ejemplar.");
            }

            ComprobarCampos(e);

            return await EjemplarEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await EjemplarEF.BorrarAsync(id);
        }
    }
}
