using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQL_Teacher.compi.analizador.controlCQL
{
    class Retorno
    {
        String valor;
        String tipo;

        public void setValor(String val) {
            this.valor = val;
        }

        public String getValor() {
            return this.valor;
        }

        public void setTipo(String type) {
            this.tipo = type;
        }

        public String getTipo() {
            return this.tipo;
        }
    }
}
