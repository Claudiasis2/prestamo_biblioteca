using ProyectoConjuntoMaterialPrestamo.Frontend;
using System.Runtime.CompilerServices;

class Program
{
    static void Main(string[] args)
    {
        MenuAdministrador.Menu().GetAwaiter().GetResult();
    }
}
