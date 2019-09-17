using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace CQL_Teacher.compi.controlDOT
{
    class ControlDOT
    {
        /*
         digraph G{
         */
        private static int contador;
        private static String grafo;
        public static String getDOT(ParseTreeNode raiz)
        {
            grafo = "digraph G{";
            grafo += "nodo0[label=\"" + escapar(raiz.ToString()) + "\"];\n";
            contador = 1;
            recorrerAST("nodo0", raiz);
            grafo += "}";
            return grafo;
        }

        private static void recorrerAST(String padre, ParseTreeNode hijos)
        {
            foreach(ParseTreeNode hijo in hijos.ChildNodes)
            {
                String nombrehijo = "nodo" + contador.ToString();
                grafo += nombrehijo + "[label=\"" + escapar(hijo.ToString()) + "\"];\n";
                grafo += padre + "->" + nombrehijo + ";\n";
                contador++;
                recorrerAST(nombrehijo, hijo);
            }
        }

        private static String escapar(String cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\"");
            return cadena;
        }
    }
}
