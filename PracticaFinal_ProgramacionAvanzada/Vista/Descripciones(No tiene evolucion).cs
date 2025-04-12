using PracticaFinal_ProgramacionAvanzada.Modelo;
using PracticaFinal_ProgramacionAvanzada.Presentador;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PracticaFinal_ProgramacionAvanzada.Vista
{
    public partial class Descripciones_No_tiene_evolucion_ : Form
    {
        private readonly int IdSeleccionado;
        private readonly PokemonPresenter presentador;

        public Descripciones_No_tiene_evolucion_(int id)
        {
            InitializeComponent();
            IdSeleccionado = id;
            presentador = new PokemonPresenter(null, new DatosAPI());
            this.Load += Descripciones_No_tiene_evolucion_Load;
        }

        private async void Descripciones_No_tiene_evolucion_Load(object sender, EventArgs e)
        {
            var pokemon = await presentador.ObtenerDetallesPokemon(IdSeleccionado);
            string descripcion = await presentador.ObtenerDescripcionEnEspanol(IdSeleccionado);

            string gifUrl = presentador.ObtenerGifPreferido(pokemon);
            pictureBox1.Load(string.IsNullOrEmpty(gifUrl) ? pokemon.sprites.front_default : gifUrl);

            txt_Descripcion.Text = $"N.{pokemon.id} {char.ToUpper(pokemon.name[0]) + pokemon.name.Substring(1)}";
            txt_TiposDesc.Text = string.Join(", ", pokemon.types.Select(t => t.type.name.ToUpper()));
            txt_DescripcionGrande.Text = descripcion;
            lbl_Altura.Text = $"{pokemon.height / 10.0} m";
            lbl_Peso.Text = $"{pokemon.weight / 10.0} kg";
        }

        private void Btn_Volver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Descripciones(IdSeleccionado).ShowDialog();
            this.Close();
        }
    }
}