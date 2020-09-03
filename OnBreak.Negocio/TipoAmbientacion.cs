using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class TipoAmbientacion
    {
        public int IdTipoAmbientacion { get; set; }
        public string Descripcion { get; set; }

        public TipoAmbientacion()
        {
            this.Init();
        }
        private void Init()
        {
            IdTipoAmbientacion = 0;
            Descripcion = string.Empty;
        }

        #region ReadAll

        public List<TipoAmbientacion> ReadAll()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                // obtener todos los registros desde la tabla
                List<Datos.TipoAmbientacion> listaDatos = bbdd.TipoAmbientacion.ToList();
                //Convertir el listado de datos en Listado de Negocios
                List<TipoAmbientacion> listaNegocio = GenerarListado(listaDatos);
                //Retorno la lista
                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<TipoAmbientacion>();
            }
        }
        #endregion

        #region Customer
        //convierte una lista de datos en una lita de negocios
        private List<TipoAmbientacion> GenerarListado(List<Datos.TipoAmbientacion> listaDatos)
        {
            List<TipoAmbientacion> lista = new List<TipoAmbientacion>();
            foreach (Datos.TipoAmbientacion dato in listaDatos)
            {
                TipoAmbientacion negocio = new TipoAmbientacion();
                CommonBC.Syncronize(dato, negocio);
                lista.Add(negocio);
            }
            return lista;
        }
        #endregion
    }

}
