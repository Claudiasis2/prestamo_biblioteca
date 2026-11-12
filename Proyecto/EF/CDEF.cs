using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class CDEF
    {
        /*public static async Task<List<Cd>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.Cds
                    .OrderBy(f => f.FkmaterialBiblioteca)
                    .ToListAsync<Cd>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<Cd>();
            }
        }

        public static async Task<Cd?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Cds
                .SingleOrDefaultAsync(f => f.FkmaterialBiblioteca == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(Cd e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Cd
                {
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca,
                    DuracionMinutos = e.DuracionMinutos,
                    NombreArtista = e.NombreArtista,
                };
                db.Cds.Add(fila);
                await db.SaveChangesAsync();
                return fila.FkmaterialBiblioteca;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(Cd e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Cd
                {
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca,
                    DuracionMinutos = e.DuracionMinutos,
                    NombreArtista = e.NombreArtista
                };
                db.Cds.Update(fila);
                await db.SaveChangesAsync();
                return fila.FkmaterialBiblioteca;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<bool> BorrarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = db.Cds.SingleOrDefault(
                f => f.FkmaterialBiblioteca == id);
                if (fila is null) return false;
                db.Cds.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Cd Convertir(Cd fila)
        {
            return new Cd
            {
                FkmaterialBiblioteca = fila.FkmaterialBiblioteca,
                DuracionMinutos = fila.DuracionMinutos,
                NombreArtista = fila.NombreArtista
            };
        }*/

    }
}
