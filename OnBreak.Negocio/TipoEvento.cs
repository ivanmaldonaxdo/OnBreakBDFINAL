using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class TipoEvento
    {
        public int IdTipoEvento { get; set; }
        public string Descripcion { get; set; }
        public TipoEvento()
        {
            this.Init();

        }
        private void Init()
        {
            IdTipoEvento = 0;
            Descripcion = string.Empty;
          
        }
        #region READ
        public bool Read()
        {
            //bbdd es conexion a la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();

            try
            {
                //obtener el primer registro que coincida con el id actividad empresa
                Datos.TipoEvento tipoevent = bbdd.TipoEvento.First(t => t.IdTipoEvento == IdTipoEvento);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(tipoevent, this);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        #endregion

        #region ReadAll

        public List<TipoEvento> ReadAll()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                // obtener todos los registros desde la tabla
                List<Datos.TipoEvento> listaDatos = bbdd.TipoEvento.ToList();
                //Convertir el listado de datos en Listado de Negocios
                List<TipoEvento> listaNegocio = GenerarListado(listaDatos);
                //Retorno la lista
                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<TipoEvento>();

            }

        }
        #endregion

        #region Customer


        //convierte una lista de datos en una lita de negocios
        private List<TipoEvento> GenerarListado(List<Datos.TipoEvento> listaDatos)
        {
            List<TipoEvento> lista = new List<TipoEvento>();
            foreach (Datos.TipoEvento dato in listaDatos)
            {
                TipoEvento negocio = new TipoEvento();
                CommonBC.Syncronize(dato, negocio);
                lista.Add(negocio);
            }
            return lista;
        }
        #endregion
    }
}
