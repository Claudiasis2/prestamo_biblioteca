namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuUsuarios
    {
        public static async Task Menu(int idUsuario, IConsola consola)
        {
            await GestionSolicitudesPrestamos.Menu(idUsuario, consola);
        }
    }
}