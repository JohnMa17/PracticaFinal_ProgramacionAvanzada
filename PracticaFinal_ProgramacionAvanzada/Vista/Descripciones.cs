using PracticaFinal_ProgramacionAvanzada.Modelo;
using PracticaFinal_ProgramacionAvanzada.Presentador;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PracticaFinal_ProgramacionAvanzada.Vista
{
    public partial class Descripciones : Form, IDescripcionView
    {
        private readonly PokemonPresenter presentador;
        private readonly int IdSeleccionado;

        public Descripciones(int id)
        {
            InitializeComponent();
            IdSeleccionado = id;
            presentador = new PokemonPresenter(null, new DatosAPI());
            this.Load += Descripciones_Load;
        }

        private async void Descripciones_Load(object sender, EventArgs e)
        {
            var pokemon = await presentador.ObtenerDetallesPokemon(IdSeleccionado);
            string descripcion = await presentador.ObtenerDescripcionEnEspanol(IdSeleccionado);

            string gifUrl = presentador.ObtenerGifPreferido(pokemon);
            MostrarImagen(string.IsNullOrEmpty(gifUrl) ? pokemon.sprites.front_default : gifUrl);

            MostrarNombre($"N.{pokemon.id} {char.ToUpper(pokemon.name[0]) + pokemon.name.Substring(1)}");
            MostrarTipos(string.Join(", ", pokemon.types.Select(t => t.type.name.ToUpper())));
            MostrarDescripcion(descripcion);
            MostrarAltura($"{pokemon.height / 10.0} m");
            MostrarPeso($"{pokemon.weight / 10.0} kg");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void pictureBox4_Click(object sender, EventArgs e)
        {
            var pokemon = await presentador.ObtenerDetallesPokemon(IdSeleccionado);
            string nombre = pokemon.name.ToLower();

            string[] eeveeEvoluciones1 = { "eevee", "vaporeon", "jolteon", "flareon" };
            string[] eeveeEvoluciones2 = { "leafeon", "glaceon" };
            string[] excepciones = { "espeon", "sylveon" };

            if (excepciones.Contains(nombre))
            {
                MessageBox.Show("Este Pokémon no tiene formulario de evolución.");
                return;
            }

            if (eeveeEvoluciones1.Contains(nombre))
            {
                new TablaEvoluciones_Eevee_(IdSeleccionado).ShowDialog();
                return;
            }

            if (eeveeEvoluciones2.Contains(nombre) || nombre == "umbreon")
            {
                new TablaEvoluciones_Eevee_2(IdSeleccionado).ShowDialog();
                return;
            }

            int cantidad = await presentador.ObtenerCantidadEvoluciones(IdSeleccionado);

            if (cantidad == 2)
                new TablaEvoluciones_2(IdSeleccionado).ShowDialog();
            else if (cantidad == 3)
                new TablaEvoluciones_3_(IdSeleccionado).ShowDialog();
            else
                MessageBox.Show("Este Pokémon no tiene evoluciones registradas.");
        }

        public void MostrarImagen(string url)
        {
            if (!string.IsNullOrEmpty(url)) pictureBox1.Load(url);
        }

        public void MostrarNombre(string texto) => txt_Descripcion.Text = texto;
        public void MostrarTipos(string tipos) => txt_TiposDesc.Text = tipos;
        public void MostrarDescripcion(string descripcion) => txt_DescripcionGrande.Text = descripcion;
        public void MostrarAltura(string altura) => lbl_Altura.Text = altura;
        public void MostrarPeso(string peso) => lbl_Peso.Text = peso;
    }
}