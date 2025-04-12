using PracticaFinal_ProgramacionAvanzada.Modelo;
using PracticaFinal_ProgramacionAvanzada.Presentador;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaFinal_ProgramacionAvanzada.Vista
{
    public partial class TablaEvoluciones_Eevee_2 : Form
    {
        private readonly int IdSeleccionado;
        private readonly PokemonPresenter presentador;

        public TablaEvoluciones_Eevee_2(int id)
        {
            InitializeComponent();
            IdSeleccionado = id;
            presentador = new PokemonPresenter(null, new DatosAPI());
            this.Load += TablaEvoluciones_Eevee_2_Load;
        }

        private async void TablaEvoluciones_Eevee_2_Load(object sender, EventArgs e)
        {
            string[] nombres = { "leafeon", "umbreon", "glaceon" };

            for (int i = 0; i < nombres.Length; i++)
            {
                await MostrarPokemon(i + 1, nombres[i]);
                var cadena = await presentador.ObtenerCadenaEvolutiva(IdSeleccionado);

                foreach (var evo in cadena.chain.evolves_to)
                {
                    if ((string)evo.species.name == nombres[i])
                    {
                        var detalles = evo.evolution_details[0];
                        if (i == 0) txt_leafeonC.Text = ObtenerCondicion(detalles);
                        else if (i == 1) txt_umbreonC.Text = ObtenerCondicion(detalles);
                        else if (i == 2) txt_glaceonC.Text = ObtenerCondicion(detalles);
                        break;
                    }
                }
            }
        }

        private async Task MostrarPokemon(int posicion, string nombre)
        {
            var datos = await presentador.ObtenerPokemonPorNombre(nombre);
            string gif = presentador.ObtenerGifPreferido(datos);
            string nombreCapitalizado = char.ToUpper(nombre[0]) + nombre.Substring(1);

            if (posicion == 1) { Pic_1.Load(gif); txt_leafeon.Text = nombreCapitalizado; }
            else if (posicion == 2) { Pic_umbreon.Load(gif); txt_umbreon.Text = nombreCapitalizado; }
            else if (posicion == 3) { Pic_2.Load(gif); txt_glaceon.Text = nombreCapitalizado; }
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_eevee1_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Volver al formulario TablaEvoluciones_Eevee con el mismo Pokémon
            TablaEvoluciones_Eevee_ volver = new TablaEvoluciones_Eevee_(IdSeleccionado);
            volver.ShowDialog();

            this.Close();
        }
    }
}