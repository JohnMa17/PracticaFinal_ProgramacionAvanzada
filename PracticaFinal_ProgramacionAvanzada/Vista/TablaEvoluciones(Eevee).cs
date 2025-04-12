using PracticaFinal_ProgramacionAvanzada.Modelo;
using PracticaFinal_ProgramacionAvanzada.Presentador;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaFinal_ProgramacionAvanzada.Vista
{
    public partial class TablaEvoluciones_Eevee_ : Form
    {
        private readonly int IdSeleccionado;
        private readonly PokemonPresenter presentador;

        public TablaEvoluciones_Eevee_(int id)
        {
            InitializeComponent();
            IdSeleccionado = id;
            presentador = new PokemonPresenter(null, new DatosAPI());
            this.Load += TablaEvoluciones_Eevee_Load;
        }

        private async void TablaEvoluciones_Eevee_Load(object sender, EventArgs e)
        {
            string[] nombres = { "eevee", "vaporeon", "jolteon", "flareon" };

            for (int i = 0; i < nombres.Length; i++)
            {
                await MostrarPokemon(i + 1, nombres[i]);

                if (i > 0) // Saltamos Eevee (posición 0)
                {
                    var cadena = await presentador.ObtenerCadenaEvolutiva(IdSeleccionado);

                    // Buscar la evolución correspondiente
                    foreach (var evo in cadena.chain.evolves_to)
                    {
                        if ((string)evo.species.name == nombres[i])
                        {
                            var detalles = evo.evolution_details[0];

                            switch (nombres[i])
                            {
                                case "vaporeon":
                                    txt_vaporeonC.Text = ObtenerCondicion(detalles);
                                    break;
                                case "jolteon":
                                    txt_jolteonC.Text = ObtenerCondicion(detalles);
                                    break;
                                case "flareon":
                                    txt_flameonC.Text = ObtenerCondicion(detalles);
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
        }


        private string ObtenerCondicion(dynamic detalles)
        {
            if (detalles.trigger.name == "trade")
                return "Intercambio";
            if (detalles.item != null)
                return $"{FormatearTexto((string)detalles.item.name)}";
            if (detalles.held_item != null)
                return $"Con objeto {FormatearTexto((string)detalles.held_item.name)}";
            if (detalles.min_happiness != null)
                return "Alegria Alta";
            if (detalles.time_of_day != null && detalles.time_of_day != "")
                return $"Durante el día: {FormatearTexto((string)detalles.time_of_day)}";
            if (detalles.location != null)
                return $"Lugar Especifico";
            if (detalles.gender != null)
                return detalles.gender == 1 ? "Hembra" : "Macho";

            return "-";
        }

        private string FormatearTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "-";
            string formateado = texto.Replace("-", " ");
            return char.ToUpper(formateado[0]) + formateado.Substring(1);
        }


        private async Task MostrarPokemon(int posicion, string nombre)
        {
            var datos = await presentador.ObtenerPokemonPorNombre(nombre);
            string gif = presentador.ObtenerGifPreferido(datos);
            string capitalizado = char.ToUpper(nombre[0]) + nombre.Substring(1);

            switch (posicion)
            {
                case 1:
                    Pic_1.Load(gif);
                    txt_eevee.Text = capitalizado;
                    break;
                case 2:
                    Pic_2.Load(gif);
                    txt_vaporeon.Text = capitalizado;
                    break;
                case 3:
                    Pic_3.Load(gif);
                    txt_jolteon.Text = capitalizado;
                    break;
                case 4:
                    Pic_4.Load(gif);
                    txt_flameon.Text = capitalizado;
                    break;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_Siguiente_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TablaEvoluciones_Eevee_2(IdSeleccionado).ShowDialog();
            this.Close();
        }
    }
}