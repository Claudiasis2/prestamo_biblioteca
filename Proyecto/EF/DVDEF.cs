using Microsoft.EntityFrameworkCore;
using ProyectoConjuntoMaterialPrestamo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoConjuntoMaterialPrestamo.EF
{
    internal class DVDEF
    {
        public static async Task<List<Dvd>> ListarAsync()
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var filas = await db.Dvds
                    .OrderBy(f => f.FkmaterialBiblioteca)
                    .ToListAsync<Dvd>();
                return filas.Select(fila => Convertir(fila)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return new List<Dvd>();
            }
        }

        public static async Task<Dvd?> BuscarAsync(int id)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = await db.Dvds
                .SingleOrDefaultAsync(f => f.FkmaterialBiblioteca == id);
                return fila is null ? null : Convertir(fila);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return null;
            }
        }

        public static async Task<int> InsertarAsync(Dvd e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Dvd
                {
                    NombreDirector = e.NombreDirector,
                    DuracionMinutos = e.DuracionMinutos,
                    ClasificacionEdad = e.ClasificacionEdad,
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca
                };
                db.Dvds.Add(fila);
                await db.SaveChangesAsync();
                return fila.FkmaterialBiblioteca;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return 0;
            }

        }

        public static async Task<int> ModificarAsync(Dvd e)
        {
            try
            {
                await using var db = ConexionEf.CrearContexto();
                var fila = new Dvd
                {
                    FkmaterialBiblioteca = e.FkmaterialBiblioteca,
                    DuracionMinutos = e.DuracionMinutos,
                    NombreDirector = e.NombreDirector,
                    ClasificacionEdad = e.ClasificacionEdad
                };
                db.Dvds.Update(fila);
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
                var fila = db.Dvds.SingleOrDefault(
                f => f.FkmaterialBiblioteca == id);
                if (fila is null) return false;
                db.Dvds.Remove(fila);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo a DB: {ex.Message}");
                return false;
            }

        }

        public static Dvd Convertir(Dvd fila)
        {
            return new Dvd
            {
                FkmaterialBiblioteca = fila.FkmaterialBiblioteca,
                DuracionMinutos = fila.DuracionMinutos,
                NombreDirector = fila.NombreDirector,
                ClasificacionEdad = fila.ClasificacionEdad
            };
        }

    }
}
