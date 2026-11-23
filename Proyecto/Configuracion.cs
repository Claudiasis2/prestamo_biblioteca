public static class Configuracion
{
    public static string Cadena =>
        Environment.GetEnvironmentVariable("MATERIAL_PRESTAMO_CONNECTION")
        ?? throw new InvalidOperationException(
            "No se ha configurado la conexión a la base de datos.");
}