using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnBreak.Datos;
namespace OnBreak.Negocio
{
    public class ActividadEmpresa
    {
        public int IdActividadEmpresa { get; set; }
        public string Descripcion { get; set; }

        public ActividadEmpresa()
        {
            this.Init();
        }
        private void Init()
        {
            IdActividadEmpresa = 0;
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
                Datos.ActividadEmpresa actividad = bbdd.ActividadEmpresa.First(a => a.IdActividadEmpresa == IdActividadEmpresa);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(actividad, this);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        #endregion

        #region ReadAll

        public List<ActividadEmpresa> ReadAll()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                // obtener todos los registros desde la tabla
                List<Datos.ActividadEmpresa> listaDatos = bbdd.ActividadEmpresa.ToList();
                //Convertir el listado de datos en Listado de Negocios
                List<ActividadEmpresa> listaNegocio = GenerarListado(listaDatos);
                //Retorno la lista
                return listaNegocio;
            }
            catch (Exception )
            {
                return new List<ActividadEmpresa>();

            }

        }
        #endregion

        #region Customer

        
        //convierte una lista de datos en una lita de negocios
        private List<ActividadEmpresa> GenerarListado(List<Datos.ActividadEmpresa> listaDatos)
        {
            List<ActividadEmpresa> lista = new List<ActividadEmpresa>();
            foreach (Datos.ActividadEmpresa dato in listaDatos)
            {
                ActividadEmpresa negocio = new ActividadEmpresa();
                CommonBC.Syncronize(dato, negocio);
                lista.Add(negocio);
            }
            return lista;
        }
        #endregion

    }
}
