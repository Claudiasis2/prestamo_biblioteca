
namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuEmpleados
    {
        public static async Task Menu(IConsola consola)
        {
            consola.Escribir("MENU DE EMPLEADOS\n");
            consola.Escribir("Pendiente de implementar...\n");

            consola.Escribir("Pulsa una tecla para continuar...\n");
            consola.EsperarTecla();

            await Task.CompletedTask;
        }
    }
}
