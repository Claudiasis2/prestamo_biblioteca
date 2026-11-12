using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.Frontend.Menus
{
    internal class MenuUsuarios
    {
        public static async Task Menu(int idUsuario) 
        { 
            await GestionSolicitudesPrestamos.Menu(idUsuario); 
        }
    }
}
