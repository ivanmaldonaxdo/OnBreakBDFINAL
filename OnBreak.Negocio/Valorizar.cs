using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public abstract class Valorizar
    {
        public abstract double CalcularContrato(Contrato contrato);
        public abstract void LeerModalidad(Contrato contrato);
    }
}
