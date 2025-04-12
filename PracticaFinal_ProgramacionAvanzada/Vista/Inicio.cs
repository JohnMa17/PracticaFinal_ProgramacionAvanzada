using PracticaFinal_ProgramacionAvanzada.Modelo;
using PracticaFinal_ProgramacionAvanzada.Presentador;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaFinal_ProgramacionAvanzada.Vista
{
    public partial class Inicio : Form, IPokemonView
    {
        private readonly PokemonPresenter presentador;

        public Inicio()
        {
            InitializeComponent();
            presentador = new PokemonPresenter(this, new DatosAPI());
            _ = CargarPokemones();
            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private async Task CargarPokemones()
        {
            dataGridView1.SuspendLayout();

            for (int i = 1; i <= 905; i++)
            {
                var pokemon = await presentador.ObtenerPokemonSimple(i);
                if (pokemon != null)
                {
                    string nombreCapitalizado = char.ToUpper(pokemon.name[0]) + pokemon.name.Substring(1);
                    dataGridView1.Rows.Add(pokemon.id, nombreCapitalizado);
                }

                if (i % 50 == 0)
                    await Task.Delay(100);
            }

            dataGridView1.ResumeLayout();
        }

        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                await presentador.CargarDatosPokemon(id);
                dataGridView1.Rows[e.RowIndex].Cells[1].Value = txtNombre.Text;
            }
        }

        public void MostrarNombre(string nombre) => txtNombre.Text = nombre.ToUpper();
        public void MostrarTipos(string tipos) => txtTipos.Text = tipos.ToUpper();
        public void MostrarImagen(string urlImagen) => pictureBox1.Load(urlImagen);

        private async void Btn_Ver_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un Pokémon para ver detalles.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var pokemon = await presentador.ObtenerDetallesPokemon(id);
            string nombre = pokemon.name.ToLower();

            string[] eeveeEvoluciones1 = { "eevee", "vaporeon", "jolteon", "flareon" };
            string[] eeveeEvoluciones2 = { "leafeon", "glaceon" };
            string[] excepciones = { "espeon", "umbreon", "sylveon" };

            Form form;
            if (excepciones.Contains(nombre)) form = new Descripciones(id);
            else if (eeveeEvoluciones1.Contains(nombre)) form = new Descripciones(id);
            else if (eeveeEvoluciones2.Contains(nombre)) form = new Descripciones(id);
            else
            {
                int cantidad = await presentador.ObtenerCantidadEvoluciones(id);
                if (cantidad == 1)
                    form = new Descripciones_No_tiene_evolucion_(id);
                else
                    form = new Descripciones(id);
            }
            form.ShowDialog();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.ToLower();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    string nombre = row.Cells[1].Value.ToString().ToLower();
                    row.Visible = nombre.Contains(filtro);
                }
            }
        }
    }
}