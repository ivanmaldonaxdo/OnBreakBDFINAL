using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    class ValorizadorCenas : Valorizar
    {
        public override double CalcularContrato(Contrato contrato)
        {
            
            double valorAsistentes = 0;
            double valorPersonalAdicional = 0;
            int ambientacion = 0;
            double musica = 0;
            double local = 0;

            if (contrato.Asistentes > 0 && contrato.Asistentes < 21)
            {
                valorAsistentes = contrato.Asistentes * 1.5;
            }else if (contrato.Asistentes>20 && contrato.Asistentes <51)
            {
                valorAsistentes = contrato.Asistentes * 1.2;
            }else if (contrato.Asistentes>50 )
            {
                valorAsistentes = 1* contrato.Asistentes;
            }

            if (contrato.PersonalAdicional == 2)
            {
                valorPersonalAdicional = 3;
            }else if (contrato.PersonalAdicional==3)
            {
                valorPersonalAdicional = 4;
            }else if (contrato.PersonalAdicional == 4)
            {
                valorPersonalAdicional = 5;
            }else if (contrato.PersonalAdicional>4)
            {
                valorPersonalAdicional = 5 + 0.5 * (contrato.PersonalAdicional - 4);
            }

            if (contrato.IdAmbientacion == 10)
            {
                ambientacion = 3;
            }
            else if (contrato.IdAmbientacion == 20)
            {
                ambientacion = 5;
            }
            if (contrato.Musica == true)
            {
                musica = 1.5;
            }
            if (contrato.Local == true)
            {
                local = 9;
            }else if(contrato.LocalArriendo == true)
            {
                local = (contrato.Arriendo * 0.05);
            }
            
            return valorAsistentes + valorPersonalAdicional + ambientacion + musica + local;
        }

        public override void LeerModalidad(Contrato contrato)
        {
            throw new NotImplementedException();
        }
    }
}
