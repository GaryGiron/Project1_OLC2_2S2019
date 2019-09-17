using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQL_Teacher.compi.analizador.controlCHISON
{
    class TipoDrop
    {
        String nombre;
        Boolean valida;
        String tipo;//bd o tabla

        public void setNombre(String name) {
            this.nombre = name;
        }
        public String getNombre() {
            return this.nombre;
        }

        public void setTipo(String type)
        {
            this.tipo = type;
        }
        public String getTipo()
        {
            return this.tipo;
        }

        public void setValida(Boolean valid)
        {
            this.valida = valid;
        }
        public Boolean getValida()
        {
            return this.valida;
        }
    }
}
