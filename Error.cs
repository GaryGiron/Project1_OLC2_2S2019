using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQL_Teacher.compi.analizador.controlCQL
{
    class Error
    {
        String tipo = "";
        int linea = 0;
        int columna = 0;
        String msj = "";

        public void setTipo(String type) {
            this.tipo = type;
        }
        public void setLinea(int line)
        {
            this.linea = line;
        }
        public void setColumna(int col)
        {
            this.columna = col;
        }
        public void setMsj(String mens)
        {
            this.msj = mens;
        }

        public String getTipo()
        {
            return this.tipo;
        }
        public int getLinea()
        {
            return this.linea;
        }
        public int getColumna()
        {
            return this.columna;
        }
        public String getMsj()
        {
            return this.msj;
        }
    }
}
