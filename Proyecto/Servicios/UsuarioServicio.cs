using ProyectoConjuntoMaterialPrestamo.EF;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    public static class UsuarioServicio
    {
        public static async Task<List<Usuario>> ListarAsync()
            => UsuarioEF.ListarAsync();

        public static async Task<Usuario?> BuscarAsync(int id)
        {
            ComprobarId(id);
            return await UsuarioEF.BuscarAsync(id);
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

        public static async Task<int> InsertarAsync(Usuario e)
        {
            ComprobarTexto(e.NombreUsuario, 20, true);
            ComprobarTexto(e.PrimerApellido, 20, true);
            ComprobarTexto(e.SegundoApellido, 20, false);
            ComprobarTexto(e.Email, 255, true);
            ComprobarTexto(e.Password, 50, true);

            return await UsuarioEF.InsertarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            //ComprobarId(id);
            return await UsuarioEF.BorrarAsync(id);
        }
    }
}
