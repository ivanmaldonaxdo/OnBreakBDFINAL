using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBreak.Negocio
{
    public class Caretaker
    {
        private Memento _memento;
        public Memento Memento { get => _memento; set => _memento = value; }
    }
}
