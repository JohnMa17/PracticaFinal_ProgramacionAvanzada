using Newtonsoft.Json;
using System.Collections.Generic;

namespace PracticaFinal_ProgramacionAvanzada.Modelo
{
    public class Datos
    {
        public class Pokemon
        {
            public int id { get; set; }
            public string name { get; set; }
            public List<Tipo> types { get; set; }
            public Sprites sprites { get; set; }
            public int height { get; set; }
            public int weight { get; set; }
        }

        public class Tipo { public TipoInfo type { get; set; } }
        public class TipoInfo { public string name { get; set; } }

        public class Sprites
        {
            public string front_default { get; set; }
            public Versiones versions { get; set; }
            public OtherSprites other { get; set; }
        }

        public class Versiones
        {
            [JsonProperty("generation-v")]
            public GenerationV generationV { get; set; }
        }
        public class GenerationV
        {
            [JsonProperty("black-white")]
            public BlackWhite blackWhite { get; set; }
        }
        public class BlackWhite
        {
            public Animated animated { get; set; }
        }
        public class Animated
        {
            public string front_default { get; set; }
        }

        public class OtherSprites
        {
            public Showdown showdown { get; set; }
        }
        public class Showdown
        {
            public string front_default { get; set; }
        }

        public class Especie
        {
            public List<FlavorTextEntry> flavor_text_entries { get; set; }
        }

        public class FlavorTextEntry
        {
            public string flavor_text { get; set; }
            public Language language { get; set; }
        }

        public class Language
        {
            public string name { get; set; }
        }
    }
}
