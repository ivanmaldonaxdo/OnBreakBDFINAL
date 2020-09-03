using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class CoffeBreak : Valorizar
    {
        public string Numero { get; set; }
        public bool Vegetariana { get; set; }

        private double _ValorBase;
        private int _PersonalBase;

        public double ValorBase { get => _ValorBase; }
        public int PersonalBase { get => _PersonalBase; }

        public CoffeBreak()
        {
            this.Init();
        }
        private void Init()
        {
            Numero = string.Empty;
            Vegetariana = false;      
        }
        #region CREATE        
        public bool Create()
        {
            //bbdd se conecta al modelo de la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            //CoffeeBreak se conecta a la tabla " CoffeeBreak" de la base de datos
            Datos.CoffeeBreak coffee = new Datos.CoffeeBreak();// este objeto solo se crea en CREATE

            try
            {
                //commonBC toma el objeto del this y se los pasa a el objeto empresa del BD
                CommonBC.Syncronize(this, coffee);
                //bbdd guarda en la base de datos el objeto coffee de BD
                bbdd.CoffeeBreak.Add(coffee);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                bbdd.CoffeeBreak.Remove(coffee);
                return false;
            }
        }

        public override double CalcularContrato(Contrato contrato)
        {
            LeerModalidad(contrato);
            double valorAsistentes = 0;
            double valorPersonalAdicional = 0;

            if (contrato.Asistentes > 0 && contrato.Asistentes < 21)
            {
                valorAsistentes = 3;
            }
            else if (contrato.Asistentes > 20 && contrato.Asistentes < 51)
            {
                valorAsistentes = 5;
            }
            else
            {
                valorAsistentes = (contrato.Asistentes - (contrato.Asistentes % 20)) / 20;
            }
            if (contrato.PersonalAdicional == 2)
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
            return valorAsistentes + valorPersonalAdicional + ValorBase;
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
        #endregion


        #region READ
        public bool Read()
        {
            //bbdd es conexion a la base de datos
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {

                Datos.CoffeeBreak coffee = bbdd.CoffeeBreak.First(m => m.Numero == Numero);
                //se copian las propiedades de la base datos hacia negocio
                CommonBC.Syncronize(coffee, this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region UPDATE
        public bool Update() //COFFEEBREAK
        {
            Datos.OnBreakEntities bbdd = new Datos.OnBreakEntities();
            try
            {
                //obtener el primer registro que coincida con el RUT
                Datos.CoffeeBreak coffee = bbdd.CoffeeBreak.First(c => c.Numero == Numero);
                //se copian las propiedades de negocio hacia la base de datos
                CommonBC.Syncronize(this, coffee);
                bbdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    } 
        #endregion

    }

