using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    internal class LibroServicio
    {
        public static async Task<List<Libro>> ListarAsync()
            => await LibroEF.ListarAsync();

        public static async Task<Libro?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await LibroEF.BuscarAsync(id);
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

        static void ComprobarCampos(Libro e)
        {
            //TODO: SERIA BUENO COMPROBAR QUE EL FORMATO ISBN ES CORRECTO
            ComprobarTexto(e.Isbn, 50, false);
            ComprobarTexto(e.Autor, 50, false);
        }

        public static async Task<int> InsertarAsync(Libro e)
        {
            ComprobarCampos(e);

            return await LibroEF.InsertarAsync(e);
        }

        public static async Task<int> ModificarAsync(Libro e)
        {
            Libro? mat = await BuscarAsync(e.FkmaterialBiblioteca);

            if (mat is null)
            {
                throw new ArgumentException(
                "No existe ese Id de Libro.");
            }

            ComprobarCampos(e);

            return await LibroEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            ComprobarId(id);
            return await LibroEF.BorrarAsync(id);
        }
    }
}
