
using ProyectoConjuntoMaterialPrestamo.Frontend;

public class ConsolaFalsa : IConsola
{
	private readonly Queue<string> entradas;

	public List<string> Salidas { get; } = new();

	public ConsolaFalsa(params string[] entradas)
	{
		this.entradas = new Queue<string>(entradas);
	}

	public string LeerTexto()
	{
		if (entradas.Count == 0)
			throw new InvalidOperationException("No hay más entradas disponibles.");

		return entradas.Dequeue();
	}

	public void Escribir(string texto)
	{
		Salidas.Add(texto);
	}

	public void Limpiar()
	{
		// No necesitamos limpiar una pantalla durante los tests.
	}

	public void EsperarTecla()
	{
		// Simulamos que el usuario pulsa una tecla.
	}

	public string ObtenerTextoMostrado()
	{
		return string.Join("", Salidas);
	}
}
