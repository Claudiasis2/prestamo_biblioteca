using ProyectoConjuntoMaterialPrestamo.Models;


namespace ProyectoConjuntoMaterialPrestamo.Servicios
{
    public static class EmpleadoServicio
    {
        public static async Task<int> InsertarAsync(Empleado e)
        {
            ComprobarTexto(e.NombreEmpleado, 20, true);
            ComprobarTexto(e.PrimerApellido, 20, true);
            ComprobarTexto(e.SegundoApellido, 20, false);
            ComprobarTexto(e.Email, 255, true);

            return await EmpleadoEF.InsertarAsync(e);
        }

        public static async Task<List<Empleado>> ListarAsync()
            => await EmpleadoEF.ListarAsync();

        private static void ComprobarTexto(string? texto, int maximo, bool obligatorio)
        {
            if (obligatorio && string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Falta un texto obligatorio.");
            if (texto is not null && texto.Length > maximo)
                throw new ArgumentException($"Un texto supera los {maximo} caracteres.");
        }

        public static async Task<List<Empleado>> BuscarAsync(int id = 0, string email = "", string nombre = "")
        {
            return await EmpleadoEF.BuscarAsync(id, email, nombre);
        }

        public static async Task<bool> ModificarAsync(Empleado e)
        {
            return await EmpleadoEF.ModificarAsync(e);
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            return await EmpleadoEF.BorrarAsync(id);
        }
    }
}