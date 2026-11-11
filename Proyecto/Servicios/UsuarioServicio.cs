using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    public static class UsuarioServicio
    {

        public static async Task<Usuario?> BuscarAsync(int id)
        {
            //ComprobarId(id);
            return UsuarioEF.BuscarAsync(id);
        }

        
        public static async Task<List<Usuario>> ListarAsync()
            => await UsuarioEF.ListarAsync();


        public static async Task<int> InsertarAsync(Usuario u)
        {
            ComprobarTexto(u.NombreUsuario, 20, true);
            ComprobarTexto(u.PrimerApellido, 20, true);
            ComprobarTexto(u.SegundoApellido, 20, false);
            ComprobarTexto(u.Email, 255, true);
            ComprobarTexto(u.Password, 50, true);

            return await UsuarioEF.InsertarAsync(u);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            return await UsuarioEF.BorrarAsync(id);
        }

        private static void ComprobarTexto(string? texto, int maximo, bool obligatorio)
        {
            if (obligatorio && string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Falta un texto obligatorio.");
            if (texto is not null && texto.Length > maximo)
                throw new ArgumentException($"Un texto supera los {maximo} caracteres.");
        }

        
       

        /*




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

        
        */
    }
}
