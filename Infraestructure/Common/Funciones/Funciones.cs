using AccesoDatosGrpcAse.Neg;
using Application.Common.Models;

namespace Infrastructure.Common.Funciones
{
    public static class Funciones
    {
        public static ConjuntoDatos ObtenerDatos(DatosRespuesta resultado)
        {
            ConjuntoDatos cd = new();
            var lst_tablas = new List<Tabla>();
            for (int k = 0; k < resultado.ListaTablas.Count; k++)
            {
                var lst_filas = new List<Application.Common.Models.Fila>();
                for (int i = 0; i < resultado.ListaTablas[k].ListaFilas.Count; i++)
                {
                    Application.Common.Models.Fila fila = new();

                    for (int j = 0; j < resultado.ListaTablas[k].ListaFilas[i].ListaColumnas.Count; j++)
                    {

                        fila.nombre_valor.Add(resultado.ListaTablas[k].ListaFilas[i].ListaColumnas[j].NombreCampo, resultado.ListaTablas[k].ListaFilas[i].ListaColumnas[j].Valor);

                    }
                    lst_filas.Add(new Application.Common.Models.Fila { nombre_valor = fila.nombre_valor });
                }
                lst_tablas.Add(new Tabla { lst_filas = lst_filas });
            }
            cd.lst_tablas = lst_tablas;
            return cd;
        }
    }
}
