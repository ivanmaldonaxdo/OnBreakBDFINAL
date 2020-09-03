using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    class ValorizadorCoffeeBreak 
    {
        //private double _ValorBase;
        //private int _PersonalBase;

        //public double ValorBase { get => _ValorBase; }
        //public int PersonalBase { get => _PersonalBase; }
        //public override double CalcularContrato(Contrato contrato)
        //{


        //    LeerModalidad(contrato);
        //    double valorAsistentes = 0;
        //    double valorPersonalAdicional = 0;

        //    if (contrato.Asistentes >0 && contrato.Asistentes < 21)
        //    {
        //        valorAsistentes = 3;
        //    }else if (contrato.Asistentes>20 && contrato.Asistentes <51)
        //    {
        //        valorAsistentes = 5;
        //    }
        //    else
        //    {
        //        valorAsistentes = (contrato.Asistentes - (contrato.Asistentes % 20)) / 20;
        //    }
        //    if (contrato.PersonalAdicional == 2)
        //    {
        //        valorPersonalAdicional = 2;
        //    }else if (contrato.PersonalAdicional == 3)
        //    {
        //        valorPersonalAdicional = 3;
        //    }else if (contrato.PersonalAdicional == 4)
        //    {
        //        valorPersonalAdicional = 3.5;
        //    }
        //    else if (contrato.PersonalAdicional >4 )
        //    {
        //        valorPersonalAdicional = 3.5 + 0.5*(contrato.PersonalAdicional-4);
        //    }
        //    return valorAsistentes + valorPersonalAdicional + ValorBase;
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
    }
}
