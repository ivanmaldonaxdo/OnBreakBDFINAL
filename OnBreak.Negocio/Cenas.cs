using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class Cenas : Valorizar
    {
        public string Numero { get; set; }
        public int IdTipoAmbientacion { get; set; }
        public bool MusicaAmbiental { get; set; }
        public bool LocalOnBreak { get; set; }
        public bool OtroLocalOnBreak { get; set; }
        public double ValorArriendo { get; set; }

        private double _ValorBase;
        private int _PersonalBase;
        private int _idAmbientacion;
        private bool _musica;
        private bool _local;
        private double _arriendo;
        private bool _localarriendo;
        public double ValorBase { get => _ValorBase; }
        public int PersonalBase { get => _PersonalBase; }
        public int IdAmbientacion { get => _idAmbientacion; }
        public bool Musica { get => _musica; }
        public bool Local { get => _local; }
        public double Arriendo { get => _arriendo; }

        private bool LocalArriendo { get => _localarriendo; }


        public Cenas()
        {
            this.Init();
        }
        private void Init()
        {
            Numero = string.Empty;
            IdTipoAmbientacion = 0;
            MusicaAmbiental = false;
            LocalOnBreak  = false;
            OtroLocalOnBreak = false;
            ValorArriendo = 0;
        }
        #region READ
        public bool Read()
        {
            //bbdd es conexion a la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {

                Datos.Cenas cenas = bbdd.Cenas.First(m => m.Numero == Numero);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(cenas, this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region UPDATE
        public bool Update() //CENAS
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                //obtener el primer registro que coincida con el RUT
                Datos.Cenas cenas = bbdd.Cenas.First(c => c.Numero == Numero);
                //se copian las propiedades de negocio hacia la base de datos
                CommonBC.Syncronize(this, cenas);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        } 
        #endregion

        public override double CalcularContrato(Contrato contrato)
        {
            LeerModalidad(contrato);
            LeerCenas(contrato);
            double valorAsistentes = 0;
            double valorPersonalAdicional = 0;
            int ambientacion = 0;
            double musica = 0;
            double local = 0;

            if (contrato.Asistentes > 0 && contrato.Asistentes < 21)
            {
                valorAsistentes = contrato.Asistentes * 1.5;
            }
            else if (contrato.Asistentes > 20 && contrato.Asistentes < 51)
            {
                valorAsistentes = contrato.Asistentes * 1.2;
            }
            else if (contrato.Asistentes > 50)
            {
                valorAsistentes = 1 * contrato.Asistentes;
            }

            if (contrato.PersonalAdicional == 2)
            {
                valorPersonalAdicional = 3;
            }
            else if (contrato.PersonalAdicional == 3)
            {
                valorPersonalAdicional = 4;
            }
            else if (contrato.PersonalAdicional == 4)
            {
                valorPersonalAdicional = 5;
            }
            else if (contrato.PersonalAdicional > 4)
            {
                valorPersonalAdicional = 5 + 0.5 * (contrato.PersonalAdicional - 4);
            }

            if (IdAmbientacion == 10)
            {
                ambientacion = 3;
            }
            else if (IdAmbientacion == 20)
            {
                ambientacion = 5;
            }
            if (Musica == true)
            {
                musica = 1.5;
            }
            if (Local == true)
            {
                local = 9;
            }
            else if (LocalArriendo == true)
            {
                local = (Arriendo * 0.05);
            }

            return valorAsistentes + valorPersonalAdicional + ambientacion + musica + local + ValorBase;
        }

        public override void LeerModalidad(Contrato contrato)
        {
            ModalidadServicio modalidad = new ModalidadServicio()
            { IdModalidad = contrato.IdModalidad };
            if (modalidad.Read())
            {
                _PersonalBase = modalidad.PersonalBase;
                _ValorBase = modalidad.ValorBase;

            }
            else
            {
                _PersonalBase = 0;
                _ValorBase = 0;

            }
        }
        public void LeerCenas(Contrato contrato)
        {
            Cenas cenas = new Cenas()
            {
                Numero = contrato.Numero
            };
            if (cenas.Read())
            {
                _idAmbientacion = cenas.IdTipoAmbientacion;
                _musica = cenas.MusicaAmbiental;
                _local = cenas.LocalOnBreak;
                _arriendo = cenas.ValorArriendo;
                _localarriendo = cenas.OtroLocalOnBreak;
            }
        }
    } 
}
