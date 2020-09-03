using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class TipoEmpresa
    {
        public int IdTipoEmpresa { get; set; }
        public string Descripcion { get; set; }

        public TipoEmpresa()
        {
            this.Init();
        }
        private void Init()
        {
            IdTipoEmpresa = 0;
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
                Datos.TipoEmpresa tipoemp = bbdd.TipoEmpresa.First(t =>t.IdTipoEmpresa  == IdTipoEmpresa);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(tipoemp, this);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        #endregion

        #region ReadAll

        public List<TipoEmpresa> ReadAll()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                // obtener todos los registros desde la tabla
                List<Datos.TipoEmpresa> listaDatos = bbdd.TipoEmpresa.ToList();
                //Convertir el listado de datos en Listado de Negocios
                List<TipoEmpresa> listaNegocio = GenerarListado(listaDatos);
                //Retorno la lista
                return listaNegocio;
            }
            catch (Exception )
            {
                return new List<TipoEmpresa>();

            }

        }
        #endregion

        #region Customer


        //convierte una lista de datos en una lita de negocios
        private List<TipoEmpresa> GenerarListado(List<Datos.TipoEmpresa> listaDatos)
        {
            List<TipoEmpresa> lista = new List<TipoEmpresa>();
            foreach (Datos.TipoEmpresa dato in listaDatos)
            {
                TipoEmpresa negocio = new TipoEmpresa();
                CommonBC.Syncronize(dato, negocio);
                lista.Add(negocio);
            }
            return lista;
        }
        #endregion

    }
}
