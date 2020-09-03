using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    [Serializable]
    public class Contrato
    {
        
        #region CAMPOS
        private String _descModalidad;
        private String _descTipoEvento;
        private double _valorBaseTipo;
        private int _personalBaseTipo;
        private int _idAmbientacion;
        private bool _musica;
        private bool _localOnBreak;
        private double _valorArriendo;
        private bool _localArriendo;
        #endregion
        public string Numero { get; set; }
        public System.DateTime Creacion { get; set; }
        public System.DateTime Termino { get; set; }
        public string RutCliente { get; set; }
        public string IdModalidad { get; set; }
        public int IdTipoEvento { get; set; }
        public System.DateTime FechaHoraInicio { get; set; }
        public System.DateTime FechaHoraTermino { get; set; }
        public int Asistentes { get; set; }
        public int PersonalAdicional { get; set; }
        public bool Realizado { get; set; }
        public double ValorTotalContrato { get; set; }
        public string Observaciones { get; set; }

        //PROPERTY CUSTOMIZADAS
        public string DescModalidad { get => _descModalidad; }
        public string DescTipoEvento { get => _descTipoEvento;}
        public double ValorBaseTipo { get => _valorBaseTipo; }
        public int PersonalBaseTipo { get => _personalBaseTipo;}
        
        public int IdAmbientacion { get => _idAmbientacion; }

        public bool Musica { get => _musica; }

        public bool Local { get => _localOnBreak; }
        public double Arriendo { get => _valorArriendo; }

        public bool LocalArriendo { get => _localArriendo; }
        #region CONSTRUCTOR
        public Contrato()
        {
            this.Init();
        }

        private void Init()
        {
            Numero = string.Empty;
            RutCliente = string.Empty;
            IdModalidad = string.Empty;
            IdTipoEvento = 0;
            Asistentes = 0;
            PersonalAdicional = 0;
            Realizado = false;
            ValorTotalContrato = 0;
            Observaciones = string.Empty;
        }

        #endregion



        #region SET MEMENTO
        public void SetMemento(Memento m)
        {
            //Numero = m.Numero;
            //Creacion = m.Creacion;
            //Termino = m.Termino;
            RutCliente = m.RutCliente;
            IdModalidad = m.IdModalidad;
            IdTipoEvento = m.IdTipoEvento;
            Asistentes = m.Asistentes;
            FechaHoraInicio = m.FechaHoraInicio;
            FechaHoraTermino = m.FechaHoraTermino;
            PersonalAdicional = m.PersonalAdicional;
            Realizado = m.Realizado;
            ValorTotalContrato = m.ValorTotalContrato;
            Observaciones = m.Observaciones;
        }
        #endregion

        #region CREADOR DE MEMENTO
        public Memento CreateMemento()
        {
            return new Memento(Creacion,Termino,RutCliente, IdModalidad, IdTipoEvento,
                        Asistentes, FechaHoraInicio, FechaHoraTermino,
                        PersonalAdicional, Realizado, ValorTotalContrato, Observaciones);
        }
        #endregion

        #region CREATE        
        public bool Create()
        {
            //bbdd se conecta al modelo de la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            //empresa se conecta a la tabla " Empresa" de la base de datos
            Datos.Contrato contrato = new Datos.Contrato();// este objeto solo se crea en CREATE

            try
            {
                //commonBC toma el objeto del this y se los pasa a el objeto empresa del BD
                CommonBC.Syncronize(this, contrato);
                //bbdd guarda en la base de datos el objeto empresas de BD
                bbdd.Contrato.Add(contrato);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                bbdd.Contrato.Remove(contrato);
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
                Datos.Contrato contrato = bbdd.Contrato.First(c => c.Numero == Numero);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(contrato, this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion

        #region READ
        public bool ReadRut()
        {
            //bbdd es conexion a la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                //obtener el primer registro que coincida con el RUT
                Datos.Contrato contrato = bbdd.Contrato.First(r => r.RutCliente == RutCliente);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(contrato, this);
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
                Datos.Contrato contrato = bbdd.Contrato.First(c => c.Numero == Numero);
                //se copian las propiedades de negocio hacia la base de datos
                CommonBC.Syncronize(this, contrato);
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
        public List<Contrato> ReadAll()
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                // obtener todos los registros desde la tabla
                List<Datos.Contrato> listaDatos = bbdd.Contrato.ToList();
                //Convertir el listado de datos en Listado de Negocios
                List<Contrato> listaNegocio = GenerarListado(listaDatos);
                //Retorno la lista
                

                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<Contrato>();                
            }

        }
        //convierte una lista de datos en una lita de negocios
        private List<Contrato> GenerarListado(List<Datos.Contrato> listaDatos)
        {
            List<Contrato> lista = new List<Contrato>();
            foreach (Datos.Contrato dato in listaDatos)
            {
                Contrato negocio = new Contrato();
                CommonBC.Syncronize(dato, negocio);
                negocio.LeerTipoEvento();
                negocio.LeerModalidadEvento();                
                lista.Add(negocio);
            }
            return lista;
        }
        #endregion

        #region READ MODALIDAD EVENTO        {
        public void LeerModalidadEvento()
        {
            ModalidadServicio modalidad = new ModalidadServicio() { IdModalidad = this.IdModalidad };
            if (modalidad.Read())
            {
                _personalBaseTipo = modalidad.PersonalBase;
                _valorBaseTipo = modalidad.ValorBase;
                _descModalidad = modalidad.Nombre;
            }
            else
            {
                _personalBaseTipo = 0;
                _valorBaseTipo = 0;
                _descModalidad = string.Empty;
            }
        }
        #endregion
        public void LeerIdAmbientacion()
        {
            Cocktail cocktail = new Cocktail() { Numero = this.Numero};
            if (cocktail.Read())
            {
                _idAmbientacion = cocktail.IdTipoAmbientacion;
                _musica = cocktail.MusicaAmbiental;
               
            }
        }
        public void LeerLocal()
        {
            Cenas cenas = new Cenas() { Numero = this.Numero };
            if (cenas.Read())
            {
                _localOnBreak = cenas.LocalOnBreak;
                _valorArriendo = cenas.ValorArriendo;
                _localArriendo = cenas.OtroLocalOnBreak;
            }
        }


        #region READ TIPO EVENTO
        private void LeerTipoEvento()
        {
            TipoEvento evento = new TipoEvento() { IdTipoEvento = this.IdTipoEvento };
            if (evento.Read())
            {
                _descTipoEvento = evento.Descripcion;
            }
            else
            {
                _descModalidad = string.Empty;
            }
        } 
        #endregion

        #region READ ALL POR RUT
        public List<Contrato> ReadAllByRut(string rut)
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.Contrato> listaDatos =
                    bbdd.Contrato.Where(r => r.RutCliente == rut).ToList<Datos.Contrato>();

                List<Contrato> listaNegocio = GenerarListado(listaDatos);

                return listaNegocio;
            }
            catch (Exception )
            {
                return new List<Contrato>();
            }
        }
        #endregion

        #region READ ALL POR TIPO EVENTO
        public List<Contrato> ReadAllByTipoEvento(int idTipEvent)
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.Contrato> listaDatos =
                    bbdd.Contrato.Where(a => a.IdTipoEvento == idTipEvent).ToList<Datos.Contrato>();

                List<Contrato> listaNegocio = GenerarListado(listaDatos);

                return listaNegocio;
            }
            catch (Exception )
            {
                return new List<Contrato>();
            }
        }
        #endregion

        #region READ ALL POR NUM CONTRATO
        public List<Contrato> ReadAllByNumero(string  numero) 
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                List<Datos.Contrato> listaDatos =
                    bbdd.Contrato.Where(n => n.Numero == numero).ToList<Datos.Contrato>();

                List<Contrato> listaNegocio = GenerarListado(listaDatos);

                return listaNegocio;
            }
            catch (Exception)
            {
                return new List<Contrato>();
            }
        }
        #endregion


        #region CALCULO DE CONTRATO
        //public double CalculoContrato()
        //{
        //    LeerModalidadEvento();
        //    LeerIdAmbientacion();
        //    double resultado;
        //    if (IdTipoEvento == 10)
        //    {
        //       valor = new ValorizadorCoffeeBreak();
        //    }else if(IdTipoEvento == 20)
        //    {
        //        valor = new ValorizadorCocktail();
        //    } else if (IdTipoEvento == 30)
        //    {
        //        valor = new ValorizadorCenas();
        //    }
            
        //    resultado = valor.CalculoContrato();
        //    return resultado;
        //}
        

       


        #endregion

    }
}
