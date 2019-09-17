using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQL_Teacher.compi.analizador
{
    class ManejoCHISON
    {
        String inicio;
        String fin;
        public List<ManejoCHISON> contenido;
        String tipo;
        String nombre;

        public void setTipo(String type)
        {
            this.tipo = type;
        }
        public String getTipo()
        {
            return this.tipo;
        }

        public void setNombre(String name)
        {
            this.nombre = name;
        }
        public String getNombre()
        {
            return this.nombre;
        }

        public void setInicio(String ini) {
            this.inicio = ini;
        }
        public String getInicio() {
            return this.inicio;
        }

        public void setFin(String end)
        {
            this.fin = end;
        }
        public String getFin()
        {
            return this.fin;
        }

    }
}
