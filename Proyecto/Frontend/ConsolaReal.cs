
namespace ProyectoConjuntoMaterialPrestamo.Frontend
{
    public class ConsolaReal : IConsola
    {
        public string LeerTexto()
        {
            return Console.ReadLine()?.Trim() ?? "";
        }

        public void Escribir(string texto)
        {
            Console.Write(texto);
        }

        public void Limpiar()
        {
            Console.Clear();
        }

        public void EsperarTecla()
        {
            Console.ReadKey();
        }

    }
}
