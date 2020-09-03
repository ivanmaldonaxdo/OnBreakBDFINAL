using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class Cliente
    {

        #region Campos
        private string _descripActividad;
        private string _descripEmpresa;
        #endregion
        public string RutCliente { get; set; }
        public string RazonSocial { get; set; }
        public string NombreContacto { get; set; }
        public string MailContacto { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int IdActividadEmpresa { get; set; }
        public int IdTipoEmpresa { get; set; }

        //Propiedades Customizadas
        #region Propiedades solo lectura        
        public string DescripActividad { get => _descripActividad;}
        public string DescripEmpresa { get => _descripEmpresa; }
        #endregion

        #region Constructor      
        public Cliente()
        {
            this.Init();
        }
        private void Init()
        {
            RutCliente = string.Empty;
            RazonSocial = string.Empty;
            NombreContacto = string.Empty;
            MailContacto = string.Empty;
            Direccion = string.Empty;
            Telefono = string.Empty;
            IdActividadEmpresa = 0;
            IdTipoEmpresa = 0;
        }
        #endregion
        #region CRUD

            #region CREATE        
        public bool Create()
        {
            //bbdd se conecta al modelo de la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            //empresa se conecta a la tabla " Empresa" de la base de datos
            Datos.Cliente cliente = new Datos.Cliente();// este objeto solo se crea en CREATE

            try
            {
                //commonBC toma el objeto del this y se los pasa a el objeto empresa del BD
                CommonBC.Syncronize(this, cliente);
                //bbdd guarda en la base de datos el objeto empresas de BD
                bbdd.Cliente.Add(cliente);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                bbdd.Cliente.Remove(cliente);
                return false;
            }
        }
        #endregion

            #region READ
        public bool Read()
        {
            //bbdd es conexion a la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                //obtener el primer registro que coincida con el RUT
                Datos.Cliente cliente = bbdd.Cliente.First(c => c.RutCliente == RutCliente);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(cliente, this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion

            #region UPDATE        
        public bool Update()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                //obtener el primer registro que coincida con el RUT
                Datos.Cliente cliente = bbdd.Cliente.First(c => c.RutCliente == RutCliente);
                //se copian las propiedades de negocio hacia la base de datos
                CommonBC.Syncronize(this, cliente);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

            #region DELETE
        public bool Delete()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                //obtener el primer registro que coincida con el RUT
                Datos.Cliente cliente = bbdd.Cliente.First(c => c.RutCliente == RutCliente);
                //elimino el  registro de la EDM
                bbdd.Cliente.Remove(cliente);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

            #region READ ALL        
        public List<Cliente> ReadAll()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                // obtener todos los registros desde la tabla
                List<Datos.Cliente> listaDatos = bbdd.Cliente.ToList();
                //Convertir el listado de datos en Listado de Negocios
                List<Cliente> listaNegocio = GenerarListado(listaDatos);
                //Retorno la lista
                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<Cliente>();

            }

        }
        //convierte una lista de datos en una lita de negocios
        private List<Cliente> GenerarListado(List<Datos.Cliente> listaDatos)
        {
            List<Cliente> lista = new List<Cliente>();
            foreach (Datos.Cliente dato in listaDatos)
            {
                Cliente negocio = new Cliente();
                CommonBC.Syncronize(dato, negocio);
                negocio.LeerActividad();
                negocio.LeerEmpresa();
                lista.Add(negocio);
            }
            return lista;
        }
        #endregion
                
            #region READ ACTIVIDAD        
        private void LeerActividad()
        {
            ActividadEmpresa actividad= new ActividadEmpresa() { IdActividadEmpresa = this.IdActividadEmpresa };
            if (actividad.Read())
            {
                _descripActividad = actividad.Descripcion;
            }
            else
            {
                _descripActividad = string.Empty;
            }
        }
        #endregion

            #region READ EMPRESA

        
        private void LeerEmpresa()
        { 

            TipoEmpresa empresa = new TipoEmpresa() { IdTipoEmpresa = this.IdTipoEmpresa };
            if (empresa.Read())
            {
                _descripEmpresa = empresa.Descripcion;
            }
            else
            {
                _descripEmpresa = string.Empty;
            }
        }
        #endregion

            #region READ ALL POR RUT

        
        public List<Cliente> ReadAllByRut(string rut)
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.Cliente> listaDatos =
                    bbdd.Cliente.Where(r => r.RutCliente == rut).ToList<Datos.Cliente>();

                List<Cliente> listaNegocio = GenerarListado(listaDatos);

                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<Cliente>();
            }
        }
        #endregion

            #region READ ALL POR ACTIVIDAD
        public List<Cliente> ReadAllByActividad(int idActividad)
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.Cliente> listaDatos =
                    bbdd.Cliente.Where(a => a.IdActividadEmpresa == idActividad).ToList<Datos.Cliente>();

                List<Cliente> listaNegocio = GenerarListado(listaDatos);

                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<Cliente>();
            }
        }
        #endregion

            #region READ ALL POR TIPO EMPRESA
        public List<Cliente> ReadAllByTipoEmpresa(int idTipoEmp)
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.Cliente> listaDatos =
                    bbdd.Cliente.Where(e => e.IdTipoEmpresa == idTipoEmp).ToList<Datos.Cliente>();

                List<Cliente> listaNegocio = GenerarListado(listaDatos);

                return listaNegocio;
            }
            catch (Exception )
            {
                return new List<Cliente>();
            }
        }
        #endregion

    }


    #endregion


}
