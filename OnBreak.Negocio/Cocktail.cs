using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class Cocktail : Valorizar
    {
        public string Numero { get; set; }
        public int IdTipoAmbientacion { get; set; }
        public bool Ambientacion { get; set; }
        public bool MusicaAmbiental { get; set; }
        public bool MusicaCliente { get; set; }

        private double _ValorBase;
        private int _PersonalBase;
        private int _idAmbientacion;
        private bool _musica;
        public double ValorBase { get => _ValorBase; }
        public int PersonalBase { get => _PersonalBase; }

        public int IdAmbientacion { get => _idAmbientacion; }

        public bool Musica { get => _musica; }
        public Cocktail()
        {
            this.Init();
        }
        private void Init()
        {
            Numero = string.Empty;
            IdTipoAmbientacion = 0;
            Ambientacion = false;
            MusicaAmbiental = false;
            MusicaCliente = false;            
        }
        public bool Read()
        {
            //bbdd es conexion a la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {

                Datos.Cocktail cocktail = bbdd.Cocktail.First(m => m.Numero == Numero);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(cocktail, this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region CREATE        
        public bool Create()
        {
            //bbdd se conecta al modelo de la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            //empresa se conecta a la tabla " Empresa" de la base de datos
            Datos.Cocktail cocktail = new Datos.Cocktail();// este objeto solo se crea en CREATE

            try
            {
                //commonBC toma el objeto del this y se los pasa a el objeto empresa del BD
                CommonBC.Syncronize(this, cocktail);
                //bbdd guarda en la base de datos el objeto empresas de BD
                bbdd.Cocktail.Add(cocktail);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                bbdd.Cocktail.Remove(cocktail);
                return false;
            }
        }

        public override double CalcularContrato(Contrato contrato)
        {
            LeerCocktail(contrato);
            LeerModalidad(contrato);
            int valorAsistentes = 0;
            double valorPersonalAdicional = 0;
            int ambientacion = 0;
            int musica = 0;



            //AWA
            if (contrato.Asistentes > 1 && contrato.Asistentes <= 20)
            {
                valorAsistentes = 4 * contrato.Asistentes;
            }
            else if (contrato.Asistentes > 20 && contrato.Asistentes < 51)
            {
                valorAsistentes = 6 * contrato.Asistentes;
            }
            else
            {
                valorAsistentes = (contrato.Asistentes - (contrato.Asistentes % 20)) / 20;
            }

            //OWO
            if (contrato.PersonalAdicional < 2)
            {
                valorPersonalAdicional = contrato.PersonalAdicional;
            }
            else if (contrato.PersonalAdicional == 2)
            {
                valorPersonalAdicional = 2;
            }
            else if (contrato.PersonalAdicional == 3)
            {
                valorPersonalAdicional = 3;
            }
            else if (contrato.PersonalAdicional == 4)
            {
                valorPersonalAdicional = 3.5;
            }
            else if (contrato.PersonalAdicional > 4)
            {
                valorPersonalAdicional = 3.5 + 0.5 * (contrato.PersonalAdicional - 4);
            }

            //UWU
            if (IdAmbientacion == 10)
            {
                ambientacion = 2;
            }
            else if (IdAmbientacion == 20)
            {
                ambientacion = 5;
            }

            
            if (Musica == true)
            {
                musica = 1;
            }
            return valorAsistentes + valorPersonalAdicional + ambientacion + musica + ValorBase;
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
        public void LeerCocktail(Contrato contrato)
        {
            Cocktail cocktail = new Cocktail() 
            { Numero = contrato.Numero
            };
            if (cocktail.Read())
            {
                _idAmbientacion = cocktail.IdTipoAmbientacion;
                _musica = cocktail.MusicaAmbiental;
                

            }
        }


        #endregion

        #region UPDATE
        public bool Update() //COCKTAIL
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                //obtener el primer registro que coincida con el RUT
                Datos.Cocktail cocktail = bbdd.Cocktail.First(c => c.Numero == Numero);
                //se copian las propiedades de negocio hacia la base de datos
                CommonBC.Syncronize(this, cocktail);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        } 
        #endregion
    }


}
