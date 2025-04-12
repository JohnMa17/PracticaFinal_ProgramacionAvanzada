using PracticaFinal_ProgramacionAvanzada.Modelo;
using PracticaFinal_ProgramacionAvanzada.Presentador;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaFinal_ProgramacionAvanzada.Vista
{
    public partial class TablaEvoluciones_3_ : Form
    {
        private readonly int IdSeleccionado;
        private readonly PokemonPresenter presentador;

        public TablaEvoluciones_3_(int id)
        {
            InitializeComponent();
            IdSeleccionado = id;
            presentador = new PokemonPresenter(null, new DatosAPI());
            this.Load += TablaEvoluciones3_Load;
        }

        private async void TablaEvoluciones3_Load(object sender, EventArgs e)
        {
            var cadena = await presentador.ObtenerCadenaEvolutiva(IdSeleccionado);

            string nombre1 = cadena.chain.species.name;
            string nombre2 = cadena.chain.evolves_to[0].species.name;
            string nombre3 = cadena.chain.evolves_to[0].evolves_to[0].species.name;

            var detalles1 = cadena.chain.evolves_to[0].evolution_details[0];
            var detalles2 = cadena.chain.evolves_to[0].evolves_to[0].evolution_details[0];

            await MostrarPokemon(1, nombre1);
            await MostrarPokemon(2, nombre2);
            await MostrarPokemon(3, nombre3);

            txt_Condicion1.Text = ObtenerCondicion(detalles1);
            txt_Condicion2.Text = ObtenerCondicion(detalles2);

            lbl_nivel1.Text = ObtenerNivel(detalles1);
            lbl_nivel2.Text = ObtenerNivel(detalles2);
        }

        private string ObtenerNivel(dynamic detalles)
        {
            return detalles.min_level != null ? $"Nivel {detalles.min_level}" : "-";
        }

        private string ObtenerCondicion(dynamic detalles)
        {
            if (detalles.trigger.name == "trade") return "Intercambio";
            if (detalles.item != null) return $"{FormatearTexto((string)detalles.item.name)}";
            if (detalles.held_item != null) return $"Con objeto {FormatearTexto((string)detalles.held_item.name)}";
            if (detalles.min_happiness != null) return "Alegria Alta";
            if (detalles.time_of_day != null && detalles.time_of_day != "") return $"Durante el día: {FormatearTexto((string)detalles.time_of_day)}";
            if (detalles.location != null) return "Lugar Especifico";
            if (detalles.gender != null) return detalles.gender == 1 ? "Hembra" : "Macho";

            return "-";
        }

        private string FormatearTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "-";
            string formateado = texto.Replace("-", " ");
            return char.ToUpper(formateado[0]) + formateado.Substring(1);
        }

        private async Task MostrarPokemon(int pos, string nombre)
        {
            var datos = await presentador.ObtenerPokemonPorNombre(nombre);
            string gif = presentador.ObtenerGifPreferido(datos);
            string capitalizado = char.ToUpper(nombre[0]) + nombre.Substring(1);

            if (pos == 1) { Pic_1.Load(gif); txt_Nombre.Text = capitalizado; }
            else if (pos == 2) { Pic_2.Load(gif); txt_Nombre2.Text = capitalizado; }
            else if (pos == 3) { Pic_3.Load(gif); txt_Nombre3.Text = capitalizado; }
        }

        private void Btn_Volver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}