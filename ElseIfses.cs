using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace CQL_Teacher.compi.analizador.controlCQL
{
    class ElseIfses
    {
        ParseTreeNode condicion;
        ParseTreeNode acciones;

        public void setAcciones(ParseTreeNode acc) {
            this.acciones = acc;
        }

        public ParseTreeNode getAcciones() {
            return this.acciones;
        }

        public void setCondicion(ParseTreeNode cond)
        {
            this.condicion = cond;
        }

        public ParseTreeNode getCondicion()
        {
            return this.condicion;
        }
    }
}
