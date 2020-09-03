using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    class ValorizadorCocktail 
    {
        //private double _ValorBase;
        //private int _PersonalBase;

        //public double ValorBase { get => _ValorBase; }
        //public int PersonalBase { get => _PersonalBase; }
        //public override double CalcularContrato(Contrato contrato)
        //{
        //        LeerModalidad(contrato);
        //        contrato.LeerIdAmbientacion();
        //        int valorAsistentes = 0;
        //        double valorPersonalAdicional = 0;
        //        int ambientacion = 0;
        //        int musica = 0;
                
                

        //        //AWA
        //        if (contrato.Asistentes > 1 && contrato.Asistentes <= 20)
        //        {
        //            valorAsistentes = 4 * contrato.Asistentes;
        //        }
        //        else if (contrato.Asistentes > 20 && contrato.Asistentes < 51)
        //        {
        //            valorAsistentes = 6 * contrato.Asistentes;
        //        }
        //        else
        //        {
        //            valorAsistentes = (contrato.Asistentes - (contrato.Asistentes % 20)) / 20;
        //        }

        //        //OWO
        //        if (contrato.PersonalAdicional < 2)
        //        {
        //            valorPersonalAdicional = contrato.PersonalAdicional;
        //        }
        //        else if (contrato.PersonalAdicional == 2)
        //        {
        //            valorPersonalAdicional = 2;
        //        }
        //        else if (contrato.PersonalAdicional == 3)
        //        {
        //            valorPersonalAdicional = 3;
        //        }
        //        else if (contrato.PersonalAdicional == 4)
        //        {
        //            valorPersonalAdicional = 3.5;
        //        }
        //        else if (contrato.PersonalAdicional > 4)
        //        {
        //            valorPersonalAdicional = 3.5 + 0.5 * (contrato.PersonalAdicional - 4);
        //        }

        //        //UWU
        //        if (contrato.IdAmbientacion == 10)
        //        {
        //            ambientacion = 2;
        //        }
        //        else if (contrato.IdAmbientacion == 20)
        //        {
        //            ambientacion = 5;
        //        }

        //        //IWI
        //        if (contrato.Musica == true)
        //        {
        //            musica = 1;
        //        }
        //        return valorAsistentes + valorPersonalAdicional + ambientacion + musica + ValorBase;

        //}

        //public override void LeerModalidad(Contrato contrato)
        //{
         
        //    ModalidadServicio modalidad = new ModalidadServicio()
        //    { IdModalidad = contrato.IdModalidad };
        //    if (modalidad.Read())
        //    {
        //        _PersonalBase = modalidad.PersonalBase;
        //        _ValorBase = modalidad.ValorBase;
              
        //    }
        //    else
        //    {
        //        _PersonalBase = 0;
        //        _ValorBase = 0;
                
        //    }
        //}
        //public void LeerAmbientacion(Contrato contrato)
        //{
        //    contrato.IdAmbientacion
        //}
    }
}
