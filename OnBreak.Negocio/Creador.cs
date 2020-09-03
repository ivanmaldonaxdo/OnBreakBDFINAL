using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class Creador
    {
        public static Valorizar CreadorTipos(int tipo)
        {
            switch (tipo)
            {
                case 10:
                    return new CoffeBreak();
                case 20:
                    return new Cocktail();
                case 30:
                    return new Cenas();
                default: return null;
            }
        }
    }
}
