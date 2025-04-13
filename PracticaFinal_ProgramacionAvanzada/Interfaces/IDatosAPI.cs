using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaFinal_ProgramacionAvanzada.Modelo
{
    public interface IDatosAPI
    {
        Task<Datos.Pokemon> ObtenerPokemonPorId(int id);
        Task<Datos.Pokemon> ObtenerPokemonPorIdPorNombre(string nombre);
        Task<Datos.Especie> ObtenerDescripcionPorId(int id);
        Task<dynamic> ObtenerCadenaEvolutiva(int id);
    }
}
