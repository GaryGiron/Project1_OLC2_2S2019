using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace CQL_Teacher.compi.analizador.controlCQL
{
    class TablaSimbolos
    {
        public static List<Symbol> symbolList = new List<Symbol>();

        public static List<Error> errorList = new List<Error>();

        public static Symbol buscaDato(String id, String ambito) { 
            foreach(Symbol dato in symbolList){
                if(dato.getNombre().Equals(id) && dato.getAmbito().Equals(ambito)){
                    return dato;
                }
            }
            return null;
        }

        public static int getDato(String id, String ambito, int rol, String tipo) {
            int cont = 0;
            foreach (Symbol dato in symbolList)
            {
                if (dato.getNombre().Equals(id) && dato.getAmbito().Equals(ambito) && dato.getTipo().Equals(tipo) && dato.getRol().Equals(rol))
                {
                    return cont;
                }
                cont++;
            }
            return -1;
        }

        public static int getDato(String id, String ambito)
        {
            int cont = 0;
            foreach (Symbol dato in symbolList)
            {
                if (dato.getNombre().Equals(id) && dato.getAmbito().Equals(ambito))
                {
                    return cont;
                }
                cont++;
            }
            return -1;
        }

        public static List<int> getByAmbit(String ambito) {
            List<int> result = new List<int>();
            int cont = 0;
            foreach (Symbol dato in symbolList)
            {
                if (dato.getAmbito().Equals(ambito))
                {
                    result.Add(cont);
                }
                cont++;
            }
            return result;
        }

        public static int getFuncion(String id, String ambito, int rol, String tipo, int numparms, List<String> tipos)
        {
            int cont = 0;
            foreach (Symbol dato in symbolList)
            {
                if (dato.getNombre().Equals(id) && dato.getAmbito().Equals(ambito) && dato.getTipo().Equals(tipo) && dato.getRol().Equals(rol) && dato.parametros.Count==numparms)
                {
                    Boolean valida = true;
                    int num = 0;
                    foreach(String type in tipos){
                        if(type!=dato.parametros[num].getTipo()){
                            valida = false;
                        }
                        num++;
                    }
                    if(valida){
                        return cont;
                    }
                    
                }
                cont++;
            }
            return -1;
        }

        public static int getProcedure(String id, String ambito, int rol, int numparms, List<String> tipos, int numret, List<String> tipoRet)
        {
            int cont = 0;
            foreach (Symbol dato in symbolList)
            {
                if (dato.getNombre().Equals(id) && dato.getAmbito().Equals(ambito) && dato.getRol().Equals(rol) && dato.parametros.Count == numparms && dato.retornos.Count==numret)
                {
                    Boolean valida = true;
                    int num = 0;
                    foreach (String type in tipos)
                    {
                        if (type != dato.parametros[num].getTipo())
                        {
                            valida = false;
                        }
                        num++;
                    }
                    num = 0;
                    foreach (String type in tipoRet)
                    {
                        if (type != dato.retornos[num].getTipo())
                        {
                            valida = false;
                        }
                        num++;
                    }
                    if (valida)
                    {
                        return cont;
                    }

                }
                cont++;
            }
            return -1;
        }

        public static void insertDato(Symbol s) {
            Boolean datoEnc = false;
            int cont = 0;
            foreach (Symbol dato in symbolList)
            {
                if (s.getAmbito()==dato.getAmbito() && s.getNombre()==dato.getNombre() && s.getRol()==dato.getRol() && s.getTipo()==dato.getTipo())
                {
                    datoEnc = true;
                    symbolList[cont].setTipo(s.getTipo());
                    symbolList[cont].setValor(s.getValor());
                }
                cont++;
            }
            if (!datoEnc) {
                symbolList.Add(s);
            }
        }


    }
}
