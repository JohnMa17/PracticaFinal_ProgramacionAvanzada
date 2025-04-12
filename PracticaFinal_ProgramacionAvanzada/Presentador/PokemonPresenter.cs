using System.Linq;
using System.Threading.Tasks;
using PracticaFinal_ProgramacionAvanzada.Modelo;

namespace PracticaFinal_ProgramacionAvanzada.Presentador
{
    public class PokemonPresenter
    {
        private readonly IPokemonView vista;
        private readonly DatosAPI api;

        public PokemonPresenter(IPokemonView vista, DatosAPI api)
        {
            this.vista = vista;
            this.api = api;
        }

        public async Task CargarDatosPokemon(int id)
        {
            var pokemon = await api.ObtenerPokemonPorId(id);
            if (pokemon != null)
            {
                vista?.MostrarNombre(pokemon.name);
                string tipos = string.Join(", ", pokemon.types.ConvertAll(t => t.type.name));
                vista?.MostrarTipos(tipos);

                if (vista is Vista.Inicio inicioForm)
                {
                    string gifAntiguo = pokemon.sprites.versions.generationV.blackWhite.animated.front_default;
                    inicioForm.MostrarImagen(gifAntiguo ?? pokemon.sprites.front_default);
                }
            }
        }

        public async Task<Datos.Pokemon> ObtenerPokemonSimple(int id) => await api.ObtenerPokemonPorId(id);
        public async Task<Datos.Pokemon> ObtenerDetallesPokemon(int id) => await api.ObtenerPokemonPorId(id);
        public async Task<Datos.Pokemon> ObtenerPokemonPorNombre(string nombre) => await api.ObtenerPokemonPorIdPorNombre(nombre);
        public async Task<dynamic> ObtenerCadenaEvolutiva(int id) => await api.ObtenerCadenaEvolutiva(id);

        public async Task<int> ObtenerCantidadEvoluciones(int id)
        {
            var cadena = await api.ObtenerCadenaEvolutiva(id);
            int contador = 1;
            var actual = cadena.chain;
            while (actual.evolves_to.Count > 0)
            {
                contador++;
                actual = actual.evolves_to[0];
            }
            return contador;
        }

        public async Task<string> ObtenerDescripcionEnEspanol(int id)
        {
            var especie = await api.ObtenerDescripcionPorId(id);
            var entrada = especie?.flavor_text_entries.FirstOrDefault(x => x.language.name == "es");
            return entrada != null ? entrada.flavor_text.Replace("\n", " ").Replace("\f", " ") : "Descripción no disponible";
        }

        public string ObtenerGifPreferido(Datos.Pokemon pokemon)
        {
            string nombre = pokemon.name.ToLower().Replace("-", "");
            string showdown = pokemon.sprites.other?.showdown?.front_default;
            string gifAntiguo = $"https://projectpokemon.org/images/normal-sprite/{nombre}.gif";
            string gifGen5 = pokemon.sprites.versions?.generationV?.blackWhite?.animated?.front_default;
            string imagenNormal = pokemon.sprites.front_default;

            if (!string.IsNullOrEmpty(showdown)) return showdown;
            if (!string.IsNullOrEmpty(gifAntiguo)) return gifAntiguo;
            if (!string.IsNullOrEmpty(gifGen5)) return gifGen5;

            return imagenNormal ?? "";
        }
    }
}