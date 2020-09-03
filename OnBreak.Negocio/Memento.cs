using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class Memento
    {
       
        #region PROPERTIES SHORT
        //public string Numero { get; set; }
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
        #endregion

        #region CTOR DEL MEMENTO
        public Memento(System.DateTime creacion, System.DateTime termino, string rutCliente, string idModalidad, int idTipoEvento, int asistentes, DateTime fechaHoraInicio, DateTime fechaHoraTermino, int personalAdicional, bool realizado, double valorTotalContrato, string observaciones)
        {
            Creacion = creacion;
            Termino = termino;
            RutCliente = rutCliente;
            IdModalidad = idModalidad;
            IdTipoEvento = idTipoEvento;
            Asistentes = asistentes;
            FechaHoraInicio = fechaHoraInicio;
            FechaHoraTermino = fechaHoraTermino;
            PersonalAdicional = personalAdicional;
            Realizado = realizado;
            ValorTotalContrato = valorTotalContrato;
            Observaciones = observaciones;
        } 
        #endregion
    }
}
