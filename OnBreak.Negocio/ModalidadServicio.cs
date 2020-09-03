using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class ModalidadServicio
    {
        #region Campos
        private string _descEvento;
        #endregion
        public string IdModalidad { get; set; }
        public int IdTipoEvento { get; set; }
        public string Nombre { get; set; }
        public double ValorBase { get; set; }
        public int PersonalBase { get; set; }
        public string DescEvento { get => _descEvento; }

        public ModalidadServicio()
        {
            this.Init();
        }
        private void Init()
        {
            IdModalidad = string.Empty;
            IdTipoEvento = 0;
            Nombre = string.Empty;
            ValorBase = 0;
            PersonalBase = 0;
        }

        #region READ
        public bool Read()
        {
            //bbdd es conexion a la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                
                Datos.ModalidadServicio modalidad = bbdd.ModalidadServicio.First(m => m.IdModalidad == IdModalidad);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(modalidad, this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }
        #endregion

        #region MyRegion        
        public List<ModalidadServicio> ReadAll()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                // obtener todos los registros desde la tabla
                List<Datos.ModalidadServicio> listaDatos = bbdd.ModalidadServicio.ToList();
                //Convertir el listado de datos en Listado de Negocios
                List<ModalidadServicio> listaNegocio = GenerarListado(listaDatos);
                //Retorno la lista
                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<ModalidadServicio>();

            }

        }
        #endregion
        //convierte una lista de datos en una lita de negocios
        private List<ModalidadServicio> GenerarListado(List<Datos.ModalidadServicio> listaDatos)
        {
            List<ModalidadServicio> lista = new List<ModalidadServicio>();
            foreach (Datos.ModalidadServicio dato in listaDatos)
            {
                ModalidadServicio negocio = new ModalidadServicio();
                CommonBC.Syncronize(dato, negocio);             
                negocio.LeerEvento();
                lista.Add(negocio);
            }
            return lista;
        }
        private void LeerEvento()
        {
            TipoEvento evento = new TipoEvento() { IdTipoEvento = this.IdTipoEvento };
            if (evento.Read())
            {
                _descEvento = evento.Descripcion;
            }
            else
            {
                _descEvento = string.Empty;
            }
        }
        public List<ModalidadServicio> ReadbyTipoEvento(int IdTipoEvento)
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.ModalidadServicio> listadatos =
                bbdd.ModalidadServicio.Where(c => c.IdTipoEvento == IdTipoEvento).ToList<Datos.ModalidadServicio>();
                List<ModalidadServicio> listanegocio = GenerarListado(listadatos);
                return listanegocio;
            }
            catch (Exception)
            {

                return new List<ModalidadServicio>();
            }
        }

        #region READ ALL TIPO EVENTO
        public List<ModalidadServicio> CargaByTipoEvento(int idTipoEvent)
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.ModalidadServicio> listaDatos =
                    bbdd.ModalidadServicio.Where(m => m.IdTipoEvento == idTipoEvent).ToList<Datos.ModalidadServicio>();

                List<ModalidadServicio> listaNegocio = GenerarListado(listaDatos);

                return listaNegocio;
            }
            catch (Exception )
            {
                return new List<ModalidadServicio>();
            }
        }
        #endregion
    }
}
