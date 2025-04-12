using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PracticaFinal_ProgramacionAvanzada.Modelo
{
    public class DatosAPI : IDatosAPI
    {
        private readonly HttpClient http;

        public DatosAPI()
        {
            http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        }

        public async Task<Datos.Pokemon> ObtenerPokemonPorId(int id)
        {
            try
            {
                string url = $"https://pokeapi.co/api/v2/pokemon/{id}/";
                var respuesta = await http.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Datos.Pokemon>(respuesta);
            }
            catch { return null; }
        }

        public async Task<Datos.Pokemon> ObtenerPokemonPorIdPorNombre(string nombre)
        {
            string url = $"https://pokeapi.co/api/v2/pokemon/{nombre.ToLower()}/";
            var respuesta = await http.GetStringAsync(url);
            return JsonConvert.DeserializeObject<Datos.Pokemon>(respuesta);
        }

        public async Task<Datos.Especie> ObtenerDescripcionPorId(int id)
        {
            try
            {
                string url = $"https://pokeapi.co/api/v2/pokemon-species/{id}/";
                var respuesta = await http.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Datos.Especie>(respuesta);
            }
            catch { return null; }
        }

        public async Task<dynamic> ObtenerCadenaEvolutiva(int id)
        {
            string especieUrl = $"https://pokeapi.co/api/v2/pokemon-species/{id}/";
            var respuesta = await http.GetStringAsync(especieUrl);
            dynamic especie = JsonConvert.DeserializeObject(respuesta);

            string urlEvolucion = especie.evolution_chain.url;
            string respuestaEvo = await http.GetStringAsync(urlEvolucion);
            return JsonConvert.DeserializeObject(respuestaEvo);
        }
    }
}
