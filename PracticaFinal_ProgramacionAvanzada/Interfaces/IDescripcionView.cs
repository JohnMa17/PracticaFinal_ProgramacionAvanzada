using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaFinal_ProgramacionAvanzada.Vista
{
    public interface IDescripcionView
    {
        void MostrarImagen(string url);
        void MostrarNombre(string texto);
        void MostrarTipos(string tipos);
        void MostrarDescripcion(string descripcion);
        void MostrarAltura(string altura);
        void MostrarPeso(string peso);
    }
}