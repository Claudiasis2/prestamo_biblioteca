public static class Configuracion
{
    private static string SERVIDOR_SQL = "79.143.95.126,54321";
    private static string Database = "MaterialPrestamo";
    private static string USUARIO_SQL = "sa";
    private static string CLAVE_SQL = "Sql#123456789";



    public static string Cadena =
    @$"Server={SERVIDOR_SQL};" +
    $"Database={Database};" +
    $"User ID={USUARIO_SQL};" +
    $"Password={CLAVE_SQL};" +
"Encrypt=True;TrustServerCertificate=True;";
}
