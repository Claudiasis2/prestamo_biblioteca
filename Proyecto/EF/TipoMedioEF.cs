
using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    public static class TipoMedioEF
    {
        public static async Task<int> InsertarAsync(TipoMedio tipo)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();

                db.TipoMedios.Add(tipo);
                await db.SaveChangesAsync();

                return tipo.IdTipoMedio;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error insertando tipo de medio: {ex}");
                return 0;
            }
        }

        public static async Task<List<TipoMedio>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();

                return await db.TipoMedios
                    .AsNoTracking()
                    .OrderBy(t => t.IdTipoMedio)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listando tipos de medio: {ex}");
                return new List<TipoMedio>();
            }
        }

        public static async Task<bool> BorrarAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                await using var db = ConexionEf.CrearContexto();

                var tipo = await db.TipoMedios.FindAsync(id);

                if (tipo == null)
                    return false;

                db.TipoMedios.Remove(tipo);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error borrando tipo de medio: {ex}");
                return false;
            }
        }
    }
}
