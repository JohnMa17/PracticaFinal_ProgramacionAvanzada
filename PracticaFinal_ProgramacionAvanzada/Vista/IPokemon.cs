using PracticaFinal_ProgramacionAvanzada.Modelo;

public interface IPokemonView
{
    void MostrarNombre(string nombre);
    void MostrarTipos(string tipos);
    void MostrarImagen(string urlImagen);
}