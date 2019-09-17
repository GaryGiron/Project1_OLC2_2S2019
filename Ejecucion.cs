using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Data;
using CQL_Teacher.compi.analizador.controlCHISON;

namespace CQL_Teacher.compi.analizador.controlCQL
{
    class Ejecucion
    {
        String tipoVar = "";
        List<String> ambito = new List<String>();
        String ambitoVals = "";
        List<String> errores= new List<String>();
        List<Resultados> valoresRet = new List<Resultados>();
        List<Boolean> breaks = new List<Boolean>();
        List<Boolean> suitches = new List<Boolean>();
        List<Boolean> continuar = new List<Boolean>();
        List<String> trys = new List<String>();
        Boolean flagExist = false;
        String dbActual = "";
        int fors = 0;
        Boolean inBatch = false;
        int whiles = 0;
        int ifsesx = 0;
        int switches = 0;
        int does = 0;
        Boolean isWhere = false;
        String tablaActual = "";
        /*
         * s-> LISTLAN;
         */
        public void recorreEntrada(ParseTreeNode raiz)
        {
            //S
            ambito.Add("global");
            recorreLISTLAN(raiz.ChildNodes.ElementAt(0));
        }

        /*
         * LISTLAN.Rule =      MakePlusRule(LISTLAN,LANGUAGE);
          */
        private void recorreLISTLAN(ParseTreeNode listlan)
        {
            int num = listlan.ChildNodes.Count;
            for (int i = 0; i < num; i++)
            {
                recorreLanguage(listlan.ChildNodes.ElementAt(i));
            }
        }

        /*
         * LANGUAGE.Rule =     procedure + id + "(" + PARAMETERS + "," + "(" + PARAMETERS + "{" + CQL + "}"
                                | TIPO + id + "(" + PARAMETERS + "{" + LCQL + "}"
                                | CQL;
        */
        private void recorreLanguage(ParseTreeNode language)
        {
            if (language.ChildNodes.Count == 5)
            {
                /*
                 * ya se realizo en la recoleccion
                 */
            }
            else if (language.ChildNodes.Count == 4)
            {
                //verificar si se requiere que se vuelva a hacer algo en estos casos
            }
            else
            {
                recorreCQL(language.ChildNodes.ElementAt(0));
            }
        }

        /*
         * CQL.Rule =          create + INSTR --typeuser - todo
                                | TIPO + LVARIABLES + ASIGNA + ";" -asigna y decla -todo
                                | alter + ALTERDATA
                                | use + id + ";"
                                | drop + DROPDATA
                                | truncate + table + id + ";"
                                | commit + ";"
                                | rollback + ";"
                                | grant + id + on + id + ";"
                                | revoke + id + on + id + ";"
                                | SENTDML
                                | select + DATASELECT + froms + id + SELECTWHERE
                                | begin + batch + LSENTDML + apply + batch + ";"
                                | FUNAGREG + "(" + "<" + select + DATASELECT + froms + id + SELECTWHEREVAL + ">" + ")" + ";"
                                | FUNAGREG + "(" + "<" + select + DATASELECT + froms + id + ">" + ")" + ";"
                                | SENTENCIA --todas -ya
                                | PUNTOID + "(" + PARMVALS + ";" --ll amadas -todo
                                | call + PUNTOID + "(" + PARMVALS + ";" --llamadas -todo
                                | returns + RETURNVAL  -retornos -todo
                                | cursor + "@" + id + iss + select + DATASELECT + froms + id + SELECTWHERE
                                | open + "@" + id + ";"
                                | close + "@" + id + ";"
                                | log + "(" + EXP + ")" + ";" --log - ya
                                | throws + news + TYPEEXCEPTION + ";" -exceptions -todo
                                | trys + "{" + LCQL + "}" + catchs + "(" + TYPEEXCEPTION + id + ")" + "{" + LCQL + "}" -intenta -todo
         *                      | PUNTOID + "." + get + "(" + VAL + ")"
                                | PUNTOID + "." + insert + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + set + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + remove + "(" + VAL + ")"
                                | PUNTOID + "." + clear + "(" + ")"
                                | PUNTOID + aumenta --ya
                                | PUNTOID + disminuye --ya
         *                      | PUNTOID + SIGNO + EXP + ";"
                               ;
        */
        private void recorreCQL(ParseTreeNode cql)
        {
            String[] token1 = cql.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "create")
            {
                recorreinstr(cql.ChildNodes.ElementAt(1));
            }
            else if (token1[0].ToLower() == "tipo")
            {
                Symbol sim = new Symbol();
                sim.setAmbito(ambito.ElementAt(ambito.Count-1));
                sim.setColumna(0);
                sim.setLinea(0);
                sim.setTipo(recorretipo(cql.ChildNodes.ElementAt(0)));
                List<ParseTreeNode> listado = recorreasigna(cql.ChildNodes.ElementAt(2));
                if(listado!=null){
                    sim.setCuerpo(listado.ElementAt(1));
                    String signo = listado.ElementAt(0).ChildNodes.ElementAt(0).Token.Value.ToString();
                    Resultados val = recorreexp(listado.ElementAt(1));
                    if(val.getIsArray()==0){
                        sim.setSigno(signo);
                        if (signo == "+=")
                        {
                            if (val.getTipo() == "string" && sim.getTipo() == "string")
                            {
                                sim.setValor(sim.getValor() + val.getValor());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) + Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) + Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else if (signo == "-=")
                        {
                            if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) - Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) - Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else if (signo == "*=")
                        {
                            if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) * Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) * Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else if (signo == "/=")
                        {
                            if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) / Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) / Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else
                        {
                            if (sim.getTipo() == val.getTipo() || sim.getTipo() == "int" && val.getTipo() == "double" || sim.getTipo() == "double" && val.getTipo() == "int")
                            {
                                sim.setValor(val.getValor());
                                sim.setTipo(val.getTipo());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(0);
                                e.setColumna(0);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                    }
                    else if (val.getIsArray() == 3)
                    {
                        //map
                        if(sim.getTipo()=="map"){
                            sim.setValor(val.getValor());
                            sim.collection = val.getContenido();
                            sim.count = val.getContenido().Count;
                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(0);
                            e.setColumna(0);
                            e.setTipo("Semantico");
                            e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                    else {
                        if (sim.getTipo() == "list")
                        {
                            //list
                            sim.setValor(val.getValor());
                            sim.collection = val.getContenido();
                            sim.count = val.getContenido().Count;
                        }
                        else
                        {
                            //set
                            sim.setValor(val.getValor());
                            sim.count = val.getContenido().Count;
                            foreach (Resultados dato in val.getContenido())
                            {
                                Boolean existe = false;
                                foreach(Resultados infoVar in sim.collection){
                                    if(dato.getValor() == infoVar.getValor() && dato.getTipo()==infoVar.getTipo()){
                                        existe = true;
                                        return;
                                    }
                                }
                                if (!existe)
                                {
                                    sim.collection.Add(dato);
                                }
                                else {
                                    Error e = new Error();
                                    e.setLinea(0);
                                    e.setColumna(0);
                                    e.setTipo("Semantico");
                                    e.setMsj("Se intento ingresar el valor: " + dato.getValor() + " en una variable tipo Set");
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                            sim.collection.Sort();
                        }
                    }
                    
                }
                sim.setRol(1);
                List<String> lnom = recorrelvariable(cql.ChildNodes.ElementAt(1));
                foreach(String nombre in lnom){
                    sim.setNombre(nombre);
                    TablaSimbolos.insertDato(sim);
                }
            }
            else if (token1[0].ToLower() == "alter")
            {
                recorrealterdata(cql.ChildNodes.ElementAt(1));
            }
            else if (token1[0].ToLower() == "use")
            {
                Boolean acceso = false;
                foreach (DataSet db in Sintactico.basesDatos)
                {
                    if (db.DataSetName == "System")
                    {
                        DataRow[] filas = db.Tables["Rol"].Select();
                        foreach (DataRow data in filas)
                        {
                            if (data["user"].ToString().Equals(Sintactico.userLogged) && data["db"].ToString().Equals(cql.ChildNodes.ElementAt(1).Token.Value.ToString()))
                            {
                                acceso = true;
                                dbActual = cql.ChildNodes.ElementAt(1).Token.Value.ToString();
                            }
                        }
                    }
                }
                if (!acceso) {
                    Error e = new Error();
                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No tiene permisos para utilizar esta base de datos");
                    TablaSimbolos.errorList.Add(e);
                }
                
            }
            else if (token1[0].ToLower() == "drop")
            {
                if (dbActual != "")
                {
                    TipoDrop datos = recorredropdata(cql.ChildNodes.ElementAt(1));
                    Boolean ingresa = false;
                    if (datos.getTipo() == "db")
                    {
                        for (int i = 0; i < Sintactico.basesDatos.Count; i++) {
                            ingresa=true;
                            if(Sintactico.basesDatos[i].DataSetName==datos.getNombre()){
                                Sintactico.basesDatos.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else {
                        for (int i = 0; i < Sintactico.basesDatos.Count; i++)
                        {
                            if (Sintactico.basesDatos[i].DataSetName == dbActual)
                            {
                                ingresa = true;
                                if (Sintactico.basesDatos[i].Tables.Contains(datos.getNombre()))
                                {
                                    Sintactico.basesDatos[i].Tables.Remove(datos.getNombre());
                                    break;
                                }
                                else if(!datos.getValida())
                                {
                                    Error e = new Error();
                                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                                    e.setTipo("Semantico");
                                    e.setMsj("No existe la tabla dentro de la base de datos que se require eliminar");
                                    TablaSimbolos.errorList.Add(e);
                                    break;
                                }
                            }
                        }
                    }
                    if (!ingresa && !datos.getValida())
                    {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se encontro la base de datos que se desea eliminar");
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se ha realizado la accion USE sobre una base de datos");
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else if (token1[0].ToLower() == "truncate")
            {
                if(dbActual!=""){
                    String id = cql.ChildNodes.ElementAt(2).Token.Value.ToString();
                    Boolean ingresa = false;
                    for(int i=0; i<Sintactico.basesDatos.Count; i++){
                        if(Sintactico.basesDatos[i].DataSetName==dbActual){
                            ingresa = true;
                            if (Sintactico.basesDatos[i].Tables.Contains(id))
                            {
                                Sintactico.basesDatos[i].Tables[id].Clear();
                                break;
                            }
                            else {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No existe la tabla dentro de la base de datos");
                                TablaSimbolos.errorList.Add(e);
                                break;
                            }
                        }
                    }
                    if(!ingresa){
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se encontro la base de datos actual");
                        TablaSimbolos.errorList.Add(e);
                    }
                }else
                {
                    Error e = new Error();
                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se ha realizado la accion USE sobre una base de datos");
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else if (token1[0].ToLower() == "commit")
            {

            }
            else if (token1[0].ToLower() == "rollback")
            {

            }
            else if (token1[0].ToLower() == "grant")
            {
                if (dbActual != "") {
                    Boolean ingresa = false;
                    for (int i = 0; i < Sintactico.basesDatos.Count; i++ )
                    {
                        if (Sintactico.basesDatos[i].DataSetName == "System")
                        {
                            ingresa = true;
                            DataRow[] rolesAct = Sintactico.basesDatos[i].Tables["Rol"].Select();
                            Boolean permiso = false;
                            foreach(DataRow nuevoRol in rolesAct){
                                if (nuevoRol["user"].ToString().Equals(Sintactico.userLogged) && nuevoRol["db"].ToString().Equals("System"))
                                {
                                    permiso = true;
                                    DataRow rolGrant = Sintactico.basesDatos[i].Tables["Rol"].NewRow();
                                    rolGrant["user"] = cql.ChildNodes.ElementAt(1).Token.Value.ToString();
                                    rolGrant["db"] = cql.ChildNodes.ElementAt(3).Token.Value.ToString();
                                    Sintactico.basesDatos[i].Tables["Rol"].Rows.Add(rolGrant);
                                    break;
                                }
                            }
                            if (!permiso)
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No tiene permisos para dar GRANTT");
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        
                    }
                    if (!ingresa)
                    {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se encontro la base de datos System");
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se ha realizado la accion USE sobre una base de datos");
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else if (token1[0].ToLower() == "revoke")
            {
                if (dbActual != "")
                {
                    Boolean ingresa = false;
                    for (int i = 0; i < Sintactico.basesDatos.Count; i++)
                    {
                        if (Sintactico.basesDatos[i].DataSetName == "System")
                        {
                            ingresa = true;
                            DataRow[] rolesAct = Sintactico.basesDatos[i].Tables["Rol"].Select();
                            Boolean permiso = false;
                            foreach (DataRow nuevoRol in rolesAct)
                            {
                                if (nuevoRol["user"].ToString().Equals(Sintactico.userLogged) && nuevoRol["db"].ToString().Equals("System"))
                                {
                                    permiso = true;
                                    String id = cql.ChildNodes.ElementAt(1).Token.Value.ToString();
                                    String bdt = cql.ChildNodes.ElementAt(3).Token.Value.ToString();
                                    foreach (DataRow rolDel in rolesAct) {
                                        if (rolDel["user"].ToString().Equals(id) && rolDel["db"].ToString().Equals(bdt))
                                        {
                                            rolDel.Delete();
                                        }
                                    }
                                    break;
                                }
                            }
                            if (!permiso)
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No tiene permisos para dar GRANTT");
                                TablaSimbolos.errorList.Add(e);
                            }
                        }

                    }
                    if (!ingresa)
                    {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se encontro la base de datos System");
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se ha realizado la accion USE sobre una base de datos");
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else if (token1[0].ToLower() == "sentdml")
            {
                recorresentdml(cql.ChildNodes.ElementAt(0));
            }
            else if (token1[0].ToLower() == "select")
            {

            }
            else if (token1[0].ToLower() == "begin")
            {
                inBatch = true;
                recorrelsentdml(cql.ChildNodes.ElementAt(2));
                inBatch = false;
            }
            else if (token1[0].ToLower() == "funagreg")
            {

            }
            else if (token1[0].ToLower() == "ifs" || token1[0].ToLower() == "switch" || token1[0].ToLower() == "while" ||
                token1[0].ToLower() == "do" || token1[0].ToLower() == "for")
            {
                recorreSentencia(cql);
            }
           /* else if (token1[0].ToLower() == "puntoid")
            {
                //llamada a metodos
                Symbol ident = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                int index = TablaSimbolos.getFuncion(ident.getNombre(), ident.getAmbito(), ident.getRol(), ident.getTipo(), );

            }*/
            else if (token1[0].ToLower() == "call")
            {
                List<Resultados> parms = recorreParmvals(cql.ChildNodes.ElementAt(2));
                List<String> tipos = new List<String>();
                foreach (Resultados parm in parms)
                {
                    tipos.Add(parm.getTipo());
                }
                Symbol dato = recorrepuntoid(cql.ChildNodes.ElementAt(1));
                List<String> retList = new List<String>();
                foreach (Retorno ret in dato.getRetornos())
                {
                    retList.Add(ret.getTipo());
                }
                if (dato != null)
                {
                    int index = TablaSimbolos.getProcedure(dato.getNombre(), dato.getAmbito(), dato.getRol(), parms.Count, tipos, dato.getRetornos().Count, retList);
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se encuentra declarado el procedimiento que se intenta llamar: ");
                    TablaSimbolos.errorList.Add(e);
                }

                for (int i = 0; i < parms.Count; i++)
                {
                    TablaSimbolos.symbolList[dato.parametros.ElementAt(i).getIndex()].setValor(parms[i].getValor());
                }
                int numret = valoresRet.Count;
                recorrelcql(dato.getCuerpo());
                if (dato.getRetornos().Count > 0 && valoresRet.Count <= numret)
                {
                    Error e = new Error();
                    e.setLinea(cql.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cql.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("El procedimiento no retornaba valores aunque se habia declarado que si: ");
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else if (token1[0].ToLower() == "returns")
            {
                //returns + RETURNVAL  -retornos -todo
                Resultados res = new Resultados();
                res.retornos=recorrereturnval(cql.ChildNodes.ElementAt(1));
                if(res.retornos!=null){
                    res.retornoInf=true;
                }else{
                    res.retornoInf=false;
                }
                valoresRet.Add(res);
            }
            else if (token1[0].ToLower() == "cursor")
            {

            }
            else if (token1[0].ToLower() == "open")
            {

            }
            else if (token1[0].ToLower() == "close")
            {

            }
            else if (token1[0].ToLower() == "log")
            {
                Resultados res = recorreexp(cql.ChildNodes.ElementAt(1));
                Console.WriteLine(res.getValor());
                Sintactico.respuestaCLI += res.getValor() + "\n";
            }
            else if (token1[0].ToLower() == "throws")
            {

            }
            else if (token1[0].ToLower() == "trys")
            {

            }
            else
            {
                //funciones de colecciones y aumenta y disminye
                String[] tokenexp = cql.ChildNodes.ElementAt(1).ToString().Split(' ');
                if(tokenexp[0].ToLower()=="++"){
                    Symbol s = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    if(s.getTipo()=="int"){
                        int index = TablaSimbolos.getDato(s.getNombre(), s.getAmbito(), s.getRol(), s.getTipo());
                        s.setValor((Int32.Parse(s.getValor()) + 1).ToString());
                        TablaSimbolos.symbolList[index] = s;
                    }
                    else if (s.getTipo() == "boolean")
                    {
                        int index = TablaSimbolos.getDato(s.getNombre(), s.getAmbito(), s.getRol(), s.getTipo());
                        s.setValor((Convert.ToDouble(s.getValor()) + 1).ToString());
                        TablaSimbolos.symbolList[index] = s;
                    }
                    else {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se permite esta operacion mas que para datos de tipo numerico");
                        TablaSimbolos.errorList.Add(e);
                    }
                    
                }
                else if (tokenexp[0].ToLower() == "--")
                {
                    Symbol s = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    if (s.getTipo() == "int")
                    {
                        int index = TablaSimbolos.getDato(s.getNombre(), s.getAmbito(), s.getRol(), s.getTipo());
                        s.setValor((Int32.Parse(s.getValor()) - 1).ToString());
                        if(index!=-1){
                            TablaSimbolos.symbolList[index] = s;
                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro encontrar el dato al que se desea aplicar la operacion");
                            TablaSimbolos.errorList.Add(e);
                        }
                        
                    }
                    else if (s.getTipo() == "boolean")
                    {
                        int index = TablaSimbolos.getDato(s.getNombre(), s.getAmbito(), s.getRol(), s.getTipo());
                        s.setValor((Convert.ToDouble(s.getValor()) - 1).ToString());
                        TablaSimbolos.symbolList[index] = s;
                    }
                    else
                    {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se permite esta operacion mas que para datos de tipo numerico");
                        TablaSimbolos.errorList.Add(e);
                    }
                }else if(tokenexp[0].ToLower()=="signo"){
                    Resultados val = recorreexp(cql.ChildNodes.ElementAt(2));
                    String signo = cql.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Token.Value.ToString();
                    Symbol sim = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    String typeAnt = sim.getTipo();
                    sim.setCuerpo(cql.ChildNodes.ElementAt(2));
                    if(val.getIsArray()==0){
                        if (signo == "+=")
                        {
                            if (val.getTipo() == "string" && sim.getTipo() == "string")
                            {
                                sim.setValor(sim.getValor() + val.getValor());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) + Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) + Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else if (signo == "-=")
                        {
                            if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) - Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) - Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else if (signo == "*=")
                        {
                            if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) * Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) * Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else if (signo == "/=")
                        {
                            if (val.getTipo() == "double" && sim.getTipo() == "int" || val.getTipo() == "int" && sim.getTipo() == "int")
                            {
                                sim.setValor((Int32.Parse(sim.getValor()) / Int32.Parse(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (val.getTipo() == "int" && sim.getTipo() == "double" || val.getTipo() == "double" && sim.getTipo() == "double")
                            {
                                sim.setValor((Convert.ToDouble(sim.getValor()) / Convert.ToDouble(val.getValor())).ToString());
                                sim.setTipo(val.getTipo());
                            }
                            else if (sim.getTipo() == val.getTipo())
                            {
                                sim.setValor(val.getValor());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else
                        {
                            if (sim.getTipo() == val.getTipo() || sim.getTipo() == "int" && val.getTipo() == "double" || sim.getTipo() == "double" && val.getTipo() == "int")
                            {
                                sim.setValor(val.getValor());
                                sim.setTipo(val.getTipo());
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                    }
                    else if (val.getIsArray() == 3)
                    {
                        //map
                        if (sim.getTipo() == "map")
                        {
                            sim.setValor(val.getValor());
                            sim.collection = val.getContenido();
                            sim.count = val.getContenido().Count;
                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(0);
                            e.setColumna(0);
                            e.setTipo("Semantico");
                            e.setMsj("La variable: " + sim.getNombre() + " tiene el tipo: " + sim.getTipo() + " y su valor tiene un tipo: " + val.getTipo());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                    else
                    {
                        if (sim.getTipo() == "list")
                        {
                            //list
                            sim.setValor(val.getValor());
                            sim.collection = val.getContenido();
                            sim.count = val.getContenido().Count;
                        }
                        else
                        {
                            //set
                            sim.setValor(val.getValor());
                            sim.count = val.getContenido().Count;
                            foreach (Resultados dato in val.getContenido())
                            {
                                Boolean existe = false;
                                foreach (Resultados infoVar in sim.collection)
                                {
                                    if (dato.getValor() == infoVar.getValor() && dato.getTipo() == infoVar.getTipo())
                                    {
                                        existe = true;
                                        return;
                                    }
                                }
                                if (!existe)
                                {
                                    sim.collection.Add(dato);
                                }
                                else
                                {
                                    Error e = new Error();
                                    e.setLinea(0);
                                    e.setColumna(0);
                                    e.setTipo("Semantico");
                                    e.setMsj("Se intento ingresar el valor: " + dato.getValor() + " en una variable tipo Set");
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                            sim.collection.Sort();
                        }
                    }
                    
                    int index=TablaSimbolos.getDato(sim.getNombre(),sim.getAmbito(),sim.getRol(),typeAnt);
                    TablaSimbolos.symbolList[index].setCuerpo(sim.getCuerpo());
                    TablaSimbolos.symbolList[index].setValor(sim.getValor());
                    TablaSimbolos.symbolList[index].setTipo(sim.getTipo());
                }
                else if(tokenexp[0].ToLower()=="parmvals"){
                    List<Resultados> parms = recorreParmvals(cql.ChildNodes.ElementAt(1));
                    List<String> tipos = new List<String>();
                    foreach(Resultados parm in parms){
                        tipos.Add(parm.getTipo());
                    }
                    Symbol ident = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    int index = TablaSimbolos.getFuncion(ident.getNombre(), ident.getAmbito(), ident.getRol(), ident.getTipo(), parms.Count, tipos);
                    for(int i=0; i<parms.Count; i++){
                        TablaSimbolos.symbolList[ident.parametros.ElementAt(i).getIndex()].setValor(parms[i].getValor());
                    }
                    recorrelcql(ident.getCuerpo());
                }
                else if (tokenexp[0].ToLower() == "exp") {
                    Symbol ident = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    Resultados val = recorreexp(cql.ChildNodes.ElementAt(1));
                    int num1 = -1;
                    try {
                        num1 = Int32.Parse(val.getValor());
                        if (ident.collection.Count>num1)
                        {
                            String signo = cql.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).Token.Value.ToString();
                            Resultados result = recorreexp(cql.ChildNodes.ElementAt(3));
                            int index = TablaSimbolos.getDato(ident.getNombre(), ident.getAmbito(), ident.getRol(), ident.getTipo());
                            if (signo == "+=")
                            {
                                if (result.getTipo() == "string" && ident.getTipo() == "string")
                                {
                                    result.setValor(ident.getValor()+result.getValor());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (result.getTipo() == "double" && ident.getTipo() == "int" || result.getTipo() == "int" && ident.getTipo() == "int")
                                {
                                    result.setValor((Int32.Parse(result.getValor()) + Int32.Parse(ident.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (result.getTipo() == "int" && ident.getTipo() == "double" || result.getTipo() == "double" && ident.getTipo() == "double")
                                {
                                    result.setValor((Convert.ToDouble(ident.getValor()) + Convert.ToDouble(result.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (ident.getTipo() == result.getTipo())
                                {
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else
                                {
                                    Error e = new Error();
                                    e.setLinea(0);
                                    e.setColumna(0);
                                    e.setTipo("Semantico");
                                    e.setMsj("La variable: " + ident.getNombre() + " tiene el tipo: " + ident.getTipo() + " y su valor tiene un tipo: " + result.getTipo());
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                            else if (signo == "-=")
                            {
                                if (result.getTipo() == "double" && ident.getTipo() == "int" || result.getTipo() == "int" && ident.getTipo() == "int")
                                {
                                    result.setValor((Int32.Parse(ident.getValor()) - Int32.Parse(result.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (result.getTipo() == "int" && ident.getTipo() == "double" || result.getTipo() == "double" && ident.getTipo() == "double")
                                {
                                    result.setValor((Convert.ToDouble(ident.getValor()) - Convert.ToDouble(result.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                }
                                else if (ident.getTipo() == result.getTipo())
                                {
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else
                                {
                                    Error e = new Error();
                                    e.setLinea(0);
                                    e.setColumna(0);
                                    e.setTipo("Semantico");
                                    e.setMsj("La variable: " + ident.getNombre() + " tiene el tipo: " + ident.getTipo() + " y su valor tiene un tipo: " + result.getTipo());
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                            else if (signo == "*=")
                            {
                                if (result.getTipo() == "double" && ident.getTipo() == "int" || result.getTipo() == "int" && ident.getTipo() == "int")
                                {
                                    result.setValor((Int32.Parse(ident.getValor()) * Int32.Parse(result.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (result.getTipo() == "int" && ident.getTipo() == "double" || result.getTipo() == "double" && ident.getTipo() == "double")
                                {
                                    result.setValor((Convert.ToDouble(ident.getValor()) * Convert.ToDouble(result.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (ident.getTipo() == result.getTipo())
                                {
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else
                                {
                                    Error e = new Error();
                                    e.setLinea(0);
                                    e.setColumna(0);
                                    e.setTipo("Semantico");
                                    e.setMsj("La variable: " + ident.getNombre() + " tiene el tipo: " + ident.getTipo() + " y su valor tiene un tipo: " + result.getTipo());
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                            else if (signo == "/=")
                            {
                                if (result.getTipo() == "double" && ident.getTipo() == "int" || result.getTipo() == "int" && ident.getTipo() == "int")
                                {
                                    result.setValor((Int32.Parse(ident.getValor()) / Int32.Parse(result.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (result.getTipo() == "int" && ident.getTipo() == "double" || result.getTipo() == "double" && ident.getTipo() == "double")
                                {
                                    result.setValor((Convert.ToDouble(ident.getValor()) / Convert.ToDouble(result.getValor())).ToString());
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else if (ident.getTipo() == val.getTipo())
                                {
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else
                                {
                                    Error e = new Error();
                                    e.setLinea(0);
                                    e.setColumna(0);
                                    e.setTipo("Semantico");
                                    e.setMsj("La variable: " + ident.getNombre() + " tiene el tipo: " + ident.getTipo() + " y su valor tiene un tipo: " + result.getTipo());
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                            else
                            {
                                if (ident.getTipo() == result.getTipo() || ident.getTipo() == "int" && result.getTipo() == "double" || ident.getTipo() == "double" && result.getTipo() == "int")
                                {
                                    result.setTipo(ident.getTipo());
                                    ident.collection[num1] = result;
                                    TablaSimbolos.symbolList[index] = ident;
                                }
                                else
                                {
                                    Error e = new Error();
                                    e.setLinea(0);
                                    e.setColumna(0);
                                    e.setTipo("Semantico");
                                    e.setMsj("La variable: " + ident.getNombre() + " tiene el tipo: " + ident.getTipo() + " y su valor tiene un tipo: " + result.getTipo());
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                        }
                    }
                    catch(FormatException excep)
                    {
                        Error e = new Error();
                        e.setLinea(0);
                        e.setColumna(0);
                        e.setTipo("Semantico");
                        e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + val.getValor() +", Error: " + excep.ToString());
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else if (tokenexp[0].ToLower() == "insert")
                {
                    Symbol id = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    if(cql.ChildNodes.Count==4){
                        //map
                        Resultados clave = recorrevals(cql.ChildNodes.ElementAt(2));
                        Resultados valor = recorreexp(cql.ChildNodes.ElementAt(3));
                        valor.settipoClave(clave.getTipo());
                        valor.setClave(clave.getValor());
                        valor.setTipo("map");
                        id.collection.Add(valor);
                        int index = TablaSimbolos.getDato(id.getNombre(), id.getAmbito(), id.getRol(), id.getTipo());
                        TablaSimbolos.symbolList[index] = id;
                    }else{
                        if (id.getTipo() == "list")
                        {
                            //list
                            id.collection.Add(recorreexp(cql.ChildNodes.ElementAt(2)));
                            int index =TablaSimbolos.getDato(id.getNombre(), id.getAmbito(), id.getRol(), id.getTipo());
                            TablaSimbolos.symbolList[index] = id;
                        }
                        else if (id.getTipo() == "set")
                        {
                            //set
                            id.collection.Add(recorreexp(cql.ChildNodes.ElementAt(2)));
                            id.collection.Sort();
                            int index = TablaSimbolos.getDato(id.getNombre(), id.getAmbito(), id.getRol(), id.getTipo());
                            TablaSimbolos.symbolList[index] = id;
                        }
                        else {
                            Error e = new Error();
                            e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("El tipo de dato del id no coincide con un collection: " + id.getNombre() + ", Tipo: " + id.getTipo());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                }
                else if (tokenexp[0].ToLower() == "set")
                {
                    Symbol id = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    Resultados indice = recorrevals(cql.ChildNodes.ElementAt(2));
                    int num1;
                    try 
                    {
                        num1 = Int32.Parse(indice.getValor());
                        if (id.getIsArray() != 1)
                        {
                            if(num1<id.collection.Count){
                                id.collection[num1] = recorreexp(cql.ChildNodes.ElementAt(3));
                                int index = TablaSimbolos.getDato(id.getNombre(),id.getAmbito(), id.getRol(), id.getTipo());
                                TablaSimbolos.symbolList[index] = id;
                            }else{
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("El valor de indice de la sentencia set es mayor que el tamaño de la collection : " + id.getValor() + ", Tamaño de la collection: " + id.collection.Count+", Indice enviado: "+num1);
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else {
                            Error e = new Error();
                            e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("El identificador no muestra ser de tipo Collection : " + id.getValor() + ", Error: " + id.getIsArray());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                    catch (FormatException excep)
                    {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + id.getValor() + ", Error: " + excep.ToString());
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else if (tokenexp[0].ToLower() == "remove") {
                    Symbol id = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    Resultados indice = recorrevals(cql.ChildNodes.ElementAt(2));
                    int num1;
                    try
                    {
                        num1 = Int32.Parse(indice.getValor());
                        if (id.getIsArray() != 1)
                        {
                            if (num1 < id.collection.Count)
                            {
                                id.collection.RemoveAt(num1);
                                int index = TablaSimbolos.getDato(id.getNombre(), id.getAmbito(), id.getRol(), id.getTipo());
                                TablaSimbolos.symbolList[index] = id;
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("El valor de indice de la sentencia remove es mayor que el tamaño de la collection : " + id.getValor() + ", Tamaño de la collection: " + id.collection.Count + ", Indice enviado: " + num1);
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("El identificador no muestra ser de tipo Collection : " + id.getValor() + ", Error: " + id.getIsArray());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                    catch (FormatException excep)
                    {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + id.getValor() + ", Error: " + excep.ToString());
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else if (tokenexp[0].ToLower() == "clear") {
                    Symbol id = recorrepuntoid(cql.ChildNodes.ElementAt(0));
                    if (id.getIsArray() != 1)
                    {
                        id.collection.Clear();
                        int index = TablaSimbolos.getDato(id.getNombre(), id.getAmbito(), id.getRol(), id.getTipo());
                        TablaSimbolos.symbolList[index] = id;
                    }
                    else
                    {
                        Error e = new Error();
                        e.setLinea(cql.ChildNodes.ElementAt(1).Token.Location.Line);
                        e.setColumna(cql.ChildNodes.ElementAt(1).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("El identificador no muestra ser de tipo Collection : " + id.getValor() + ", Error: " + id.getIsArray());
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else
                {

                }
            }
        }

        /*
         * LCQL.Rule =         MakePlusRule(LCQL, CQL);      
         */

        private void recorrelcql(ParseTreeNode lcql)
        {
            int num = lcql.ChildNodes.Count;
            for (int i = 0; i < num; i++)
            {
                recorreCQL(lcql.ChildNodes.ElementAt(i));
            }
        }

        /*
         * TYPEEXCEPTION.Rule = TypeAlreadyExists
                                | TypeDontExists
                                | BDAlreadyExists
                                | BDDontExists
                                | UseBDException
                                | TableAlreadyExists
                                | TableDontExists
                                | CounterTypeException
                                | UserAlreadyExists
                                | UserDontExists
                                | ValuesException
                                | ColumnException
                                | BatchException
                                | IndexOutException
                                | ArithmeticException
                                | NullPointerException
                                | NumberReturnsException
                                | FunctionAlreadyExists
                                | ProcedureAlreadyExists
                                | ObjectAlreadyExists;*/
        private String recorreexception(ParseTreeNode exception)
        {
            String[] token1 = exception.ChildNodes.ElementAt(0).ToString().Split(' ');
            /*if (token1[0].ToLower() == "typealreadyexists")
            {

            }
            else if (token1[0].ToLower() == "typedontexists")
            {

            }
            else if (token1[0].ToLower() == "bdalreadyexists")
            {

            }
            else if (token1[0].ToLower() == "bddontexists")
            {

            }
            else if (token1[0].ToLower() == "usebdexception")
            {

            }
            else if (token1[0].ToLower() == "tablealreadyexists")
            {

            }
            else if (token1[0].ToLower() == "tabledontexists")
            {

            }
            else if (token1[0].ToLower() == "countertypeexception")
            {

            }
            else if (token1[0].ToLower() == "useralreadyexists")
            {

            }
            else if (token1[0].ToLower() == "userdontexists")
            {

            }
            else if (token1[0].ToLower() == "valuesexception")
            {

            }
            else if (token1[0].ToLower() == "columnexception")
            {

            }
            else if (token1[0].ToLower() == "batchexception")
            {

            }
            else if (token1[0].ToLower() == "indexoutexception")
            {

            }
            else if (token1[0].ToLower() == "arithmeticexception")
            {

            }
            else if (token1[0].ToLower() == "nullpointerexception")
            {

            }
            else if (token1[0].ToLower() == "batchexception")
            {

            }

            else if (token1[0].ToLower() == "numberreturnsexception")
            {

            }
            else if (token1[0].ToLower() == "functionalreadyexists")
            {

            }
            else if (token1[0].ToLower() == "procedurealreadyexists")
            {

            }
            else if (token1[0].ToLower() == "objectalreadyexists")
            {

            }*/
            return token1[0].ToLower();
        }

        /*
         * RETURNVAL.Rule =    LVAL + ";"
                                | ";";
         */
        private List<Retorno> recorrereturnval(ParseTreeNode returnval)
        {
            if (returnval.ChildNodes.ElementAt(0).ToString() == "LVAL")
            {
                List<Retorno> ret = new List<Retorno>();
                List<Resultados> lval = recorreLval(returnval.ChildNodes.ElementAt(0));
                foreach(Resultados exp in lval){
                    Retorno r = new Retorno();
                    r.setTipo(exp.getTipo());
                    r.setValor(exp.getValor());
                    ret.Add(r);
                }
                return ret;
            }
            else
            {
                return null;
            }
        }

        /*
         * CQLBREAK.Rule =     CQL
                                | breaks + ";";
         */
        private void recorrecqlbreak(ParseTreeNode cqlbreak)
        {
            if (cqlbreak.ChildNodes.ElementAt(0).ToString() == "CQL")
            {
                recorreCQL(cqlbreak.ChildNodes.ElementAt(0));
            }
            else
            {
                if (!breaks.ElementAt(breaks.Count - 1))
                {
                    breaks[breaks.Count - 1] = true;
                }
                else {
                    Error e = new Error();
                    e.setLinea(cqlbreak.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cqlbreak.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("Existe una sentencia break fuera del contexto permitido por el lenguaje");
                    TablaSimbolos.errorList.Add(e);
                }
            }
        }

        /*
         * CQLCYCLE.Rule =     CQLBREAK
                                | continues + ";";
         */

        private void recorrecqlCycle(ParseTreeNode cqlcycle)
        {
            if (cqlcycle.ChildNodes.ElementAt(0).ToString() == "CQLBREAK")
            {
                recorrecqlbreak(cqlcycle.ChildNodes.ElementAt(0));
            }
            else
            {
                if (!continuar.ElementAt(continuar.Count - 1))
                {
                    continuar[continuar.Count - 1] = true;
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(cqlcycle.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(cqlcycle.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("Existe una sentencia continue fuera del contexto permitido por el lenguaje");
                    TablaSimbolos.errorList.Add(e);
                }
            }
        }

        /*LVAL.Rule =         MakePlusRule(LVAL, EXP);*/
        private List<Resultados> recorreLval(ParseTreeNode lval)
        {
            List<Resultados> results = new List<Resultados>();
            foreach (ParseTreeNode exp in lval.ChildNodes)
            {
                results.Add(recorreexp(exp));
            }
            return results;
        }

        /*LVARIABLES.Rule = MakePlusRule(LVARIABLES, ToTerm(","), IDENT);*/
        private List<String> recorrelvariable(ParseTreeNode lvariable)
        {
            List<String> lnombres = new List<String>();
            foreach (ParseTreeNode variable in lvariable.ChildNodes)
            {
                lnombres.Add(recorreident(variable));
            }
            return lnombres;
        }

        /*
         * IDENT.Rule =        "@" + id
                            | id;
         */
        private String recorreident(ParseTreeNode ident)
        {
            return ident.ChildNodes.ElementAt(0).Token.Value.ToString();
        }

        /*  PARMVALS.Rule =     LVAL + ")" 
                                | ")";
         */
        private List<Resultados> recorreParmvals(ParseTreeNode parmVals)
        {
            if (parmVals.ChildNodes.Count > 0)
            {
                return recorreLval(parmVals.ChildNodes.ElementAt(0));
            }
            else
            {
                return null;
            }
        }
        /*
         * PARAMETERS.Rule =   LPARMS + ")"
                                | ")";
         */
        private void recorreParameters(ParseTreeNode parameters)
        {
            if (parameters.ChildNodes.Count > 0)
            {

            }
            else
            {

            }
        }

        /*
         * LPARMS.Rule =       MakePlusRule(LPARMS, ToTerm(","), IDENTIFICADOR);
         */
        private void recorreLParms(ParseTreeNode lparms)
        {
            foreach (ParseTreeNode parms in lparms.ChildNodes)
            {

            }
        }

        /*
         * IDENTIFICADOR.Rule = TIPO + "@" + id;
         */
        private void recorreIdentificador(ParseTreeNode identificador)
        {

        }

        /*
         * SENTENCIA.Rule =    IFS
                                | switchs + "(" + EXP + ")" + "{" + LCASES + DEFAULTS
                                | whiles + "(" + EXP + ")" + "{" + LCYCLE + "}"
                                | does + "{" + LCYCLE + "}" + whiles + "(" + EXP + ")" + ";"
                                | fors + FORES;
         */
        private void recorreSentencia(ParseTreeNode sentencia)
        {
            String[] token1 = sentencia.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "ifs")
            {
                recorreIfs(sentencia.ChildNodes.ElementAt(0));
            }
            else if (token1[0].ToLower() == "switch")
            {
                switches++;
                ambito.Add("switch"+switches);
                suitches.Add(false);
                breaks.Add(false);
                Resultados res = recorreexp(sentencia.ChildNodes.ElementAt(1));
                recorrelcases(sentencia.ChildNodes.ElementAt(2), res.getValor());
                recorreDefaults(sentencia.ChildNodes.ElementAt(3));
                breaks.RemoveAt(breaks.Count-1);
                suitches.RemoveAt(suitches.Count-1);
                ambito.RemoveAt(ambito.Count - 1);
            }
            else if (token1[0].ToLower() == "while")
            {
                Resultados res = recorreexp(sentencia.ChildNodes.ElementAt(1));
                whiles++;
                ambito.Add("while"+whiles);
                breaks.Add(false);
                if(res.getTipo()=="boolean"){
                    while (Convert.ToBoolean(res.getValor()))
                    {
                        if (breaks.ElementAt(breaks.Count - 1)) {
                            break;
                        }
                        recorrelcycle(sentencia.ChildNodes.ElementAt(2));
                        res = recorreexp(sentencia.ChildNodes.ElementAt(1));
                    }
                }else{
                    Error e = new Error();
                    e.setLinea(sentencia.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(sentencia.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("La expresion condicional del while debe ser de tipo booleana: " + res.getValor());
                    TablaSimbolos.errorList.Add(e);
                }
                breaks.RemoveAt(breaks.Count - 1);
                ambito.RemoveAt(ambito.Count-1);
            }
            else if (token1[0].ToLower() == "do")
            {
                Resultados res = recorreexp(sentencia.ChildNodes.ElementAt(3));
                does++;
                ambito.Add("do" + does);
                breaks.Add(false);
                if (res.getTipo() == "boolean")
                {
                    do
                    {
                        if (breaks.ElementAt(breaks.Count - 1))
                        {
                            break;
                        }
                        res = recorreexp(sentencia.ChildNodes.ElementAt(3));
                        recorrelcycle(sentencia.ChildNodes.ElementAt(1));
                    } while (Convert.ToBoolean(res.getValor()));
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(sentencia.ChildNodes.ElementAt(2).Token.Location.Line);
                    e.setColumna(sentencia.ChildNodes.ElementAt(2).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("La expresion condicional del while debe ser de tipo booleana: " + res.getValor());
                    TablaSimbolos.errorList.Add(e);
                }
                breaks.RemoveAt(breaks.Count - 1);
                ambito.RemoveAt(ambito.Count - 1);
            }
            else if (token1[0].ToLower() == "fors")
            {
                recorreFores(sentencia.ChildNodes.ElementAt(1));
            }
        }

        /*
         * LCYCLE.Rule =       MakePlusRule(LCYCLE, CQLCYCLE);
         */
        private void recorrelcycle(ParseTreeNode lcycle)
        {
            continuar.Add(false);
            foreach (ParseTreeNode cycle in lcycle.ChildNodes)
            {
                if(breaks.ElementAt(breaks.Count-1)){
                    break;
                }
                if (continuar.ElementAt(continuar.Count - 1)) {
                    break;
                }
                recorrecqlCycle(cycle);
            }
            continuar.RemoveAt(continuar.Count - 1);
        }

        /*
         * LBREAKCQL.Rule =    MakePlusRule(LBREAKCQL, CQLBREAK);
         */
        private void recorrelbreakcql(ParseTreeNode lbreakcql)
        {
            foreach (ParseTreeNode breakcql in lbreakcql.ChildNodes)
            {
                if(breaks.ElementAt(breaks.Count-1)){
                    break;
                }
                recorrecqlbreak(breakcql);
            }
        }

        /*
         * FORES.Rule =        "(" + TIPO + IDENT + SIGNO + EXP + ";" + EXP + ";" + EXP + ")" + "{" + LCYCLE + "}"
                                | each + "(" + LPARMS + ins + "@" + id + "{" + LCYCLE + "}";
         */
        private void recorreFores(ParseTreeNode fores)
        {
            if (fores.ChildNodes.ElementAt(0).ToString() == "TIPO")
            {//revisar
                fors++;
                ambito.Add("for"+fores);
                Resultados dato = recorreexp(fores.ChildNodes.ElementAt(3));
                
                Symbol s = new Symbol();
                s.setAmbito(ambito.ElementAt(ambito.Count - 1));
                s.setNombre(recorreident(fores.ChildNodes.ElementAt(1)));
                s.setValor(dato.getValor());
                s.setTipo(dato.getTipo());
                s.setRol(1);
                TablaSimbolos.symbolList.Add(s);
                Resultados cond = recorreexp(fores.ChildNodes.ElementAt(4));
                if (cond.getTipo() == "boolean")
                {
                    while (Convert.ToBoolean(cond.getValor()))
                    {
                        recorrelcycle(fores.ChildNodes.ElementAt(6));
                        Resultados inc = recorreexp(fores.ChildNodes.ElementAt(5));
                        int index = TablaSimbolos.getDato(s.getNombre(), s.getAmbito(), s.getRol(), s.getTipo());
                        Symbol cambia = TablaSimbolos.symbolList.ElementAt(index);
                        cambia.setValor(inc.getValor());
                        TablaSimbolos.symbolList[index] = cambia;
                        cond = recorreexp(fores.ChildNodes.ElementAt(4));
                    }
                }
                else {
                    Error e = new Error();
                    e.setLinea(fores.ChildNodes.ElementAt(4).Token.Location.Line);
                    e.setColumna(fores.ChildNodes.ElementAt(4).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("La expresion condicional del for debe ser de tipo booleana: " + cond.getValor());
                    TablaSimbolos.errorList.Add(e);
                }
                ambito.RemoveAt(ambito.Count - 1);
            }
            else
            {
                //manejo de cursores
            }
        }

        /*
         * LCASES.Rule =       MakePlusRule(LCASES, CASO); 
         */
        private void recorrelcases(ParseTreeNode lcases, String valor)
        {
            foreach (ParseTreeNode cases in lcases.ChildNodes)
            {
                if(!breaks.ElementAt(breaks.Count-1)){
                    recorreCaso(cases, valor);
                }
            }
        }

        /* 
         * CASO.Rule =         cases + EXP + ":" + LBREAKCQL; ;
         */
        private void recorreCaso(ParseTreeNode caso, String val)
        {
            Resultados res = recorreexp(caso.ChildNodes.ElementAt(1));
            if(res.getValor()==val || suitches.ElementAt(suitches.Count-1)){
                suitches[suitches.Count-1]=true;
                recorrelbreakcql(caso.ChildNodes.ElementAt(2));
            }
        }

        /*
         * DEFAULTS.Rule =     defaults + ":" + LBREAKCQL + "}"
                                | "}";
         */
        private void recorreDefaults(ParseTreeNode defaults)
        {
            if (defaults.ChildNodes.Count() > 0)
            {
                if(!breaks.ElementAt(breaks.Count-1)){
                    recorrelbreakcql(defaults.ChildNodes.ElementAt(1));
                }
            }
        }

        /*
         * IFS.Rule =          ifs + "(" + EXP + ")" + "{" + LCQL + "}" + ELSEIFS + ELSE
                                | ifs + "(" + EXP + ")" + "{" + LCQL + "}" + ELSEIFS
                                | ifs + "(" + EXP + ")" + "{" + LCQL + "}" + ELSE
                                | ifs + "(" + EXP + ")" + "{" + LCQL + "}";
         */
        private void recorreIfs(ParseTreeNode ifs)
        {
            if (ifs.ChildNodes.Count == 5)
            {
                Resultados cont = recorreexp(ifs.ChildNodes.ElementAt(1));
                if (cont.getTipo() == "boolean")
                {
                    if (Convert.ToBoolean(cont.getValor()))
                    {
                        ifsesx++;
                        ambito.Add("if" + ifsesx);
                        recorrelcql(ifs.ChildNodes.ElementAt(2));
                        ambito.RemoveAt(ambito.Count - 1);
                    }
                    else
                    {
                        List<ElseIfses> elseifs = recorreelseifs(ifs.ChildNodes.ElementAt(3));
                        Boolean entro = false;
                        foreach (ElseIfses elseif in elseifs)
                        {
                            Resultados result = recorreexp(elseif.getCondicion());
                            if (result.getTipo() == "boolean")
                            {
                                if (Convert.ToBoolean(result.getValor()))
                                {
                                    ifsesx++;
                                    ambito.Add("elseif" + ifsesx);
                                    recorrelcql(elseif.getAcciones());
                                    ambito.RemoveAt(ambito.Count - 1);
                                    entro = true;
                                    break;
                                }
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(ifs.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(ifs.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("La expresion condicional del elseif debe ser de tipo booleana: " + result.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        if (!entro)
                        {
                            ifsesx++;
                            ambito.Add("else" + ifsesx);
                            recorreelse(ifs.ChildNodes.ElementAt(4));
                            ambito.RemoveAt(ambito.Count - 1);
                        }
                    }
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(ifs.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(ifs.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("La expresion condicional del if debe ser de tipo booleana: " + cont.getValor());
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else if (ifs.ChildNodes.Count == 4)
            {
                if (ifs.ChildNodes.ElementAt(3).ToString() == "ELSEIFS")
                {
                    Resultados cont = recorreexp(ifs.ChildNodes.ElementAt(1));
                    if (cont.getTipo() == "boolean")
                    {
                        if (Convert.ToBoolean(cont.getValor()))
                        {
                            ifsesx++;
                            ambito.Add("if" + ifsesx);
                            recorrelcql(ifs.ChildNodes.ElementAt(2));
                            ambito.RemoveAt(ambito.Count - 1);
                        }
                        else
                        {
                            List<ElseIfses> elseifs = recorreelseifs(ifs.ChildNodes.ElementAt(3));
                            foreach(ElseIfses elseif in elseifs){
                                Resultados result = recorreexp(elseif.getCondicion());
                                if (result.getTipo() == "boolean")
                                {
                                    if (Convert.ToBoolean(result.getValor()))
                                    {
                                        ifsesx++;
                                        ambito.Add("elseif" + ifsesx);
                                        recorrelcql(elseif.getAcciones());
                                        ambito.RemoveAt(ambito.Count - 1);
                                        break;
                                    }
                                }
                                else {
                                    Error e = new Error();
                                    e.setLinea(ifs.ChildNodes.ElementAt(0).Token.Location.Line);
                                    e.setColumna(ifs.ChildNodes.ElementAt(0).Token.Location.Column);
                                    e.setTipo("Semantico");
                                    e.setMsj("La expresion condicional del elseif debe ser de tipo booleana: " + result.getValor());
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }
                        }
                    }
                    else
                    {
                        Error e = new Error();
                        e.setLinea(ifs.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(ifs.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("La expresion condicional del if debe ser de tipo booleana: " + cont.getValor());
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else
                {
                    Resultados cont = recorreexp(ifs.ChildNodes.ElementAt(1));
                    if (cont.getTipo() == "boolean")
                    {
                        if (Convert.ToBoolean(cont.getValor()))
                        {
                            ifsesx++;
                            ambito.Add("if" + ifsesx);
                            recorrelcql(ifs.ChildNodes.ElementAt(2));
                            ambito.RemoveAt(ambito.Count - 1);
                        }
                        else {
                            ifsesx++;
                            ambito.Add("else" + ifsesx);
                            recorreelse(ifs.ChildNodes.ElementAt(3));
                            ambito.RemoveAt(ambito.Count - 1);
                        }
                    }
                    else
                    {
                        Error e = new Error();
                        e.setLinea(ifs.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(ifs.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("La expresion condicional del if debe ser de tipo booleana: " + cont.getValor());
                        TablaSimbolos.errorList.Add(e);
                    }
                }
            }
            else if (ifs.ChildNodes.Count == 3)
            {
                Resultados cont = recorreexp(ifs.ChildNodes.ElementAt(1));
                if (cont.getTipo() == "boolean")
                {
                    if (Convert.ToBoolean(cont.getValor()))
                    {
                        ifsesx++;
                        ambito.Add("if" + ifsesx);
                        recorrelcql(ifs.ChildNodes.ElementAt(2));
                        ambito.RemoveAt(ambito.Count - 1);
                    }
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(ifs.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(ifs.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("La expresion condicional del if debe ser de tipo booleana: " + cont.getValor());
                    TablaSimbolos.errorList.Add(e);
                }
            }
        }

        /*
         * ELSEIFS.Rule =      MakePlusRule(ELSEIFS, ELSEIFSES); 
         */
        private List<ElseIfses> recorreelseifs(ParseTreeNode elseifs)
        {
            List<ElseIfses> eifs = new List<ElseIfses>();
            foreach (ParseTreeNode elseif in elseifs.ChildNodes)
            {
                eifs.Add(recorreelseifses(elseif));
            }

            return eifs;
        }

        /*
         * ELSEIFSES.Rule =    elses + ifs + "(" + EXP + ")" + "{" + LCQL + "}";
         */
        private ElseIfses recorreelseifses(ParseTreeNode elseifses)
        {
            ElseIfses elseifs = new ElseIfses();
            elseifs.setAcciones(elseifses.ChildNodes.ElementAt(3));
            elseifs.setCondicion(elseifses.ChildNodes.ElementAt(2));
            return elseifs;
        }

        /*
         * ELSE.Rule =         elses + "{" + LCQL + "}";
         */
        private void recorreelse(ParseTreeNode elses)
        {
            recorrelcql(elses.ChildNodes.ElementAt(1));
        }

        /*
         * FUNAGREG.Rule =     count
                                | min
                                | max
                                | sum
                                | avg;
         */
        private void recorrefunagreg(ParseTreeNode funagreg)
        {
            String[] token1 = funagreg.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "count")
            {

            }
            else if (token1[0].ToLower() == "min")
            {

            }
            else if (token1[0].ToLower() == "max")
            {

            }
            else if (token1[0].ToLower() == "sum")
            {

            }
            else if (token1[0].ToLower() == "avg")
            {

            }
        }

        /*
         * LSENTDML.Rule =     MakePlusRule(LSENTDML, SENTDML);
         */
        private void recorrelsentdml(ParseTreeNode lsentdml)
        {
            foreach (ParseTreeNode sentdml in lsentdml.ChildNodes)
            {
                recorresentdml(sentdml);
            }
        }

        /*
         * SENTDML.Rule =      insert + into + id + INSERTDATA
                                | update + id + set + LASIGNACION + WHEREEXP
                                | delete + DELETEDATA;
         */
        private void recorresentdml(ParseTreeNode sentdml)
        {
            String[] token1 = sentdml.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "insert")
            {
                for (int i = 0; i < Sintactico.basesDatos.Count; i++ )
                {
                    if(Sintactico.basesDatos[i].DataSetName==dbActual){
                        List<List<Resultados>> valuesInsert = recorreinsertdata(sentdml.ChildNodes.ElementAt(3));
                        if (valuesInsert.Count == 2)
                        {
                            List<Resultados> info = valuesInsert.ElementAt(0);
                            List<Resultados> values = valuesInsert.ElementAt(1);
                            if (info.Count == values.Count)
                            {
                                String name = sentdml.ChildNodes.ElementAt(2).Token.Value.ToString();
                                if (Sintactico.basesDatos[i].Tables.Contains(name))
                                {
                                    DataRow datanew = Sintactico.basesDatos[i].Tables[name].NewRow();
                                    for (int j = 0; j < values.Count; j++)
                                    {
                                        datanew[info[j].getValor()] = values[j].getValor();
                                    }
                                    Sintactico.basesDatos[i].Tables[name].Rows.Add(datanew);
                                }
                                
                            }
                            else {
                                Error e = new Error();
                                e.setLinea(sentdml.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(sentdml.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("Se estan intentando ingresar diferente cantidad de datos a los que se esta indicando");
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else {
                            List<Resultados> values = valuesInsert.ElementAt(0);
                            String name = sentdml.ChildNodes.ElementAt(2).Token.Value.ToString();
                            if (Sintactico.basesDatos[i].Tables.Contains(name))
                            {
                                if (Sintactico.basesDatos[i].Tables[name].Rows.Count == values.Count)
                                {
                                    DataRow datanew = Sintactico.basesDatos[i].Tables[name].NewRow();
                                    for (int j = 0; j < values.Count; j++)
                                    {
                                        datanew[j] = values[j].getValor();
                                    }
                                    Sintactico.basesDatos[i].Tables[name].Rows.Add(datanew);
                                }
                                else
                                {
                                    Error e = new Error();
                                    e.setLinea(sentdml.ChildNodes.ElementAt(0).Token.Location.Line);
                                    e.setColumna(sentdml.ChildNodes.ElementAt(0).Token.Location.Column);
                                    e.setTipo("Semantico");
                                    e.setMsj("Se estan intentando ingresar diferente cantidad de datos a los que se esta indicando");
                                    TablaSimbolos.errorList.Add(e);
                                }
                            }   
                        }
                        
                    }
                }
            }
            else if (token1[0].ToLower() == "update")
            {
                if (dbActual != "")
                {
                    Boolean ingresa = false;
                    for (int i = 0; i < Sintactico.basesDatos.Count; i++)
                    {
                        if (Sintactico.basesDatos[i].DataSetName == dbActual)
                        {
                            ingresa = true;
                            String nomTable = sentdml.ChildNodes.ElementAt(1).Token.Value.ToString();
                            if (Sintactico.basesDatos[i].Tables.Contains(nomTable))
                            {
                                String where = devCadExp(sentdml.ChildNodes.ElementAt(4));
                                foreach (DataRow row in Sintactico.basesDatos[i].Tables[nomTable].Select(where))
                                {
                                    List<Symbol> lasig = recorrelasignacion(sentdml.ChildNodes.ElementAt(3));
                                    foreach (Symbol asig in lasig)
                                    {
                                        row[asig.getNombre()] = asig.getValor();
                                    }
                                }
                                Sintactico.basesDatos[i].Tables[nomTable].AcceptChanges();
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(sentdml.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(sentdml.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se encuentra la tabla " + nomTable + " dentro de la base de datos");
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                    }
                    if(!ingresa){
                        Error e = new Error();
                        e.setLinea(sentdml.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(sentdml.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se encontro la base de datos actual");
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else {
                    Error e = new Error();
                    e.setLinea(sentdml.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(sentdml.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se esta utilizando ninguna base de datos, utilice el cmando USE");
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else if (token1[0].ToLower() == "delete")
            {
                recorredeletedata(sentdml.ChildNodes.ElementAt(1));
            }
        }

        /*
         * SELECTWHERE.Rule =  where + EXP + EXPSELECT;
         */
        private void recorreselectwhere(ParseTreeNode selectwhere)
        {

        }

        /*
         * SELECTWHEREVAL.Rule = where + EXP + EXPSELECTVAL;
         */
        private void recorredselectwhereval(ParseTreeNode selectwhereval)
        {

        }

        /*
         * ORDERLIMIT.Rule =   order + by + LORDER + LIMITDATA
                                | LIMITDATA;
         */
        private void recorreorderlimit(ParseTreeNode orderlimit)
        {
            if (orderlimit.ChildNodes.ElementAt(0).ToString() == "LIMITDATA")
            {

            }
            else
            {

            }
        }

        /*
         * ORDERLIMITVAL.Rule = order + by + LORDER + limit + EXP
                                | order + by + LORDER
                                | limit + EXP;
         */
        private void recorreorderlimitval(ParseTreeNode orderlimitval)
        {
            String[] token1 = orderlimitval.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "limit")
            {

            }
            else
            {
                if (orderlimitval.ChildNodes.Count == 5)
                {

                }
                else
                {

                }
            }
        }

        /*
         * LORDER.Rule =       MakePlusRule(LORDER, ToTerm(","), ORDENA); 
         */
        private void recorrelorder(ParseTreeNode lorder)
        {
            foreach (ParseTreeNode order in lorder.ChildNodes)
            {

            }
        }

        /*
         * ORDENA.Rule =       id + ASCDESC;
         */
        private void recorreordena(ParseTreeNode ordena)
        {

        }

        /*
         * ASCDESC.Rule =      asc
                                | desc;
         */
        private void recorreascdesc(ParseTreeNode ascdesc)
        {
            String[] token1 = ascdesc.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "asc")
            {

            }
            else if (token1[0].ToLower() == "desc")
            {

            }
        }

        /*
         * LIMITDATA.Rule =    limit + EXP + ";"
                               | ";";
         */
        private void recorrelimitdata(ParseTreeNode limitdata)
        {
            if (limitdata.ChildNodes.Count > 0)
            {

            }
            else
            {

            }
        }

        /*
         * DATASELECT.Rule =   LVAL
                                | ToTerm("*");
         */
        private void recorredataselect(ParseTreeNode dataselect)
        {
            if (dataselect.ChildNodes.ElementAt(0).ToString().ToLower() == "lval")
            {

            }
            else
            {

            }
        }

        /*
         * LASIGNACION.Rule = MakePlusRule(LASIGNACION, ToTerm(","), TIPOASIG);
         */
        private List<Symbol> recorrelasignacion(ParseTreeNode lasignacion)
        {
            List<Symbol> lsets = new List<Symbol>();
            foreach (ParseTreeNode asignacion in lasignacion.ChildNodes)
            {
                lsets.Add(recorretipoasig(asignacion));
            }
            return lsets;
        }

        /*
         * TIPOASIG.Rule =     id + "=" + VAL;
         */
        private Symbol recorretipoasig(ParseTreeNode tipoasig)
        {
            Symbol res = new Symbol();
            res.setNombre(tipoasig.ChildNodes.ElementAt(0).Token.Value.ToString());
            res.setValor(recorrevals(tipoasig.ChildNodes.ElementAt(2)).getValor());
            return res;
        }

        /* 
         * DELETEDATA.Rule =   type + id + ";"
                                | froms + id + WHEREEXP;
         */
        private void recorredeletedata(ParseTreeNode deletedata)
        {
            String[] token1 = deletedata.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "type")
            {
                int index=-1;
                int cont=0;
                String nombre = deletedata.ChildNodes.ElementAt(1).Token.Value.ToString();
                while(index==-1 && cont<ambito.Count){
                    index = TablaSimbolos.getDato(nombre,ambito[cont]);
                }
                if (index != -1)
                {
                    TablaSimbolos.symbolList.RemoveAt(index);
                    List<int> lindices = TablaSimbolos.getByAmbit(nombre);
                    foreach(int indices in lindices){
                        TablaSimbolos.symbolList.RemoveAt(indices);
                    }
                }
                else {
                    Error e = new Error();
                    e.setLinea(deletedata.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(deletedata.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se encuentra declarado el user type " + nombre );
                    TablaSimbolos.errorList.Add(e);
                }
            }
            else
            {
                if (dbActual != "")
                {
                    Boolean ingresa = false;
                    for (int i = 0; i < Sintactico.basesDatos.Count; i++)
                    {
                        if (Sintactico.basesDatos[i].DataSetName == dbActual)
                        {
                            ingresa = true;
                            String nomTable = deletedata.ChildNodes.ElementAt(1).Token.Value.ToString();
                            if (Sintactico.basesDatos[i].Tables.Contains(nomTable))
                            {
                                String where = devCadExp(deletedata.ChildNodes.ElementAt(2));
                                foreach (DataRow row in Sintactico.basesDatos[i].Tables[nomTable].Select(where))
                                {
                                    row.Delete();
                                }
                                Sintactico.basesDatos[i].Tables[nomTable].AcceptChanges();
                            }
                            else
                            {
                                Error e = new Error();
                                e.setLinea(deletedata.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(deletedata.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se encuentra la tabla " + nomTable + " dentro de la base de datos");
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                    }
                    if (!ingresa)
                    {
                        Error e = new Error();
                        e.setLinea(deletedata.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(deletedata.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se encontro la base de datos actual");
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else
                {
                    Error e = new Error();
                    e.setLinea(deletedata.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(deletedata.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se esta utilizando ninguna base de datos, utilice el cmando USE");
                    TablaSimbolos.errorList.Add(e);
                }
            }
        }

        /*
         * wHEREEXP.Rule =     where + EXP + ";"
                            | ";";
         */
        private String recorredwhereexp(ParseTreeNode whereexp)
        {
            if (whereexp.ChildNodes.Count > 0)
            {
                return devCadExp(whereexp.ChildNodes.ElementAt(1));
            }
            else
            {
                return "";
            }
        }

        private String devCadExp(ParseTreeNode nodo) {
            String val = "";
            if (nodo.ChildNodes.Count > 0)
            {
                foreach (ParseTreeNode hoja in nodo.ChildNodes)
                {
                    val += devCadExp(hoja);
                }
            }
            else {
                val += nodo.Token.Value.ToString();
            }
            return val;
        }

        /*
         * INSERTDATA.Rule =   values + "(" + LVAL + ")"
                            | "(" + LVAL + ")" + values + "(" + LVAL + ")";
         */
        private List<List<Resultados>> recorreinsertdata(ParseTreeNode insertdata)
        {
            List <List< Resultados >> res = new List<List<Resultados>>();
            
            if (insertdata.ChildNodes.Count==2)
            {
                List<Resultados> values = recorreLval(insertdata.ChildNodes.ElementAt(1));
                res.Add(values);
            }
            else
            {
                List<Resultados> info = recorreLval(insertdata.ChildNodes.ElementAt(0));
                res.Add(info);
                List<Resultados> values = recorreLval(insertdata.ChildNodes.ElementAt(2));
                res.Add(values);
            }
            return res;
        }

        /*
         * LVALUSER.Rule =     MakePlusRule(LVALUSER, TIPOUSER);
         */
        private List<Symbol> recorrelvaluser(ParseTreeNode lvaluser)
        {
            List<Symbol> lrets = new List<Symbol>();
            foreach (ParseTreeNode valuser in lvaluser.ChildNodes)
            {
                lrets.Add(recorretipouser(valuser));
            }
            return lrets;
        }

        /*
         * TIPOUSER.Rule =     id + TIPO;
         */
        private Symbol recorretipouser(ParseTreeNode tipouser)
        {
            Symbol ret = new Symbol();
            ret.setTipo(recorretipo(tipouser.ChildNodes.ElementAt(1)));
            ret.setNombre(tipouser.ChildNodes.ElementAt(0).Token.Value.ToString());
            return ret;
        }

        /*
         * ASIGNA.Rule =       ";"
                            | SIGNO + EXP + ";";
         */
        private List<ParseTreeNode> recorreasigna(ParseTreeNode asigna)
        {
            if (asigna.ChildNodes.Count > 0)
            {
                List<ParseTreeNode> lista = new List<ParseTreeNode>();
                lista.Add(asigna.ChildNodes.ElementAt(0));
                lista.Add(asigna.ChildNodes.ElementAt(1));
                return lista;
            }
            else
            {
                return null;
            }
        }

        /*
         * SIGNO.Rule =        ToTerm("=")
                             | ToTerm("+=")
                             | ToTerm("-=")
                             | ToTerm("*=")
                             | ToTerm("/=");
         */

        private void recorresigno(ParseTreeNode signo)
        {
            String[] token1 = signo.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "=")
            {

            }
            else if (token1[0].ToLower() == "+=")
            {

            }
            else if (token1[0].ToLower() == "-=")
            {

            }
            else if (token1[0].ToLower() == "*=")
            {

            }
            else if (token1[0].ToLower() == "/=")
            {

            }
        }


        /*
         * CAMBIA.Rule =       add + "(" + LVALUSER + ")" + ";"
                                | delete + "(" + LIDS + ")" + ";";
         */
        private List<Symbol> recorrecambia(ParseTreeNode cambia)
        {
            List<Symbol> result = new List<Symbol>();
            String[] token1 = cambia.ChildNodes.ElementAt(0).ToString().Split(' ');
            Symbol rsign = new Symbol();
            rsign.setTipo("signo");
            if (token1[0].ToLower() == "add")
            {
                rsign.setValor("+");
                result.Add(rsign);
                result.AddRange(recorrelvaluser(cambia.ChildNodes.ElementAt(1)));
            }
            else //if (token1[0].ToLower() == "delete")
            {
                rsign.setValor("-");
                result.Add(rsign);
                foreach(String id in recorrelids(cambia.ChildNodes.ElementAt(1))){
                    Symbol r = new Symbol();
                    r.setNombre(id);
                    result.Add(r);
                }
            }
            return result;
        }

        /*
         * LIDS.Rule =        MakePlusRule(LIDS, ToTerm(","), id);
         */
        private List<String> recorrelids(ParseTreeNode lids)
        {
            List<String> nombres = new List<String>();
            foreach (ParseTreeNode id in lids.ChildNodes)
            {
                nombres.Add(id.Token.Value.ToString());
            }
            return nombres;
        }

        /*
         * INSTR.Rule =        type + id + "(" + LVALUSER + ")" + ";"
                                | db + INFODB
                                | table + INFODB + "(" + LCOLUMS + ")" + ";"
                                | user + VAL + with + pass + VAL + ";";
         */
        private void recorreinstr(ParseTreeNode instr)
        {
            String[] token1 = instr.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "type")
            {
                
                String nomtype = recorreInfodb(instr.ChildNodes.ElementAt(1));
                Symbol s = TablaSimbolos.buscaDato(nomtype, ambito.ElementAt(ambito.Count-1));
                if( s==null){
                    Symbol sim = new Symbol();
                    sim.setAmbito("global");
                    sim.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                    sim.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                    sim.setTipo("userType");
                    //sim.setCuerpo(language.ChildNodes.ElementAt(4));
                    sim.setNombre(nomtype);
                    sim.setRol(4);
                    TablaSimbolos.symbolList.Add(sim);
                    List<Symbol> listavar = recorrelvaluser(instr.ChildNodes.ElementAt(2));
                    foreach(Symbol simbol in listavar){
                        simbol.setAmbito(sim.getNombre());
                        simbol.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                        simbol.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                        simbol.setRol(1);
                        TablaSimbolos.symbolList.Add(simbol);
                    }
                }
            }
            else if (token1[0].ToLower() == "db")
            {
                String id = recorreInfodb(instr.ChildNodes.ElementAt(1));
                int index = -1;
                int cont = 0;
                foreach(DataSet db in Sintactico.basesDatos){
                    if(db.DataSetName==id){
                        index = cont;
                    }
                    cont++;
                }
                if (index == -1)
                {
                    DataSet newDb = new DataSet(id);
                    Sintactico.basesDatos.Add(newDb);
                }
                else {
                    if (!flagExist)
                    {
                        Error e = new Error();
                        e.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("Ya existe una base de datos con ese nombre");
                        TablaSimbolos.errorList.Add(e);
                    }
                }
            }
            else if (token1[0].ToLower() == "table")
            {
                String id = recorreInfodb(instr.ChildNodes.ElementAt(1));
                int index = -1;
                int cont = 0;
                if(dbActual!=""){
                    foreach (DataSet db in Sintactico.basesDatos)
                    {
                        if (db.DataSetName == dbActual)
                        {
                            index = cont;
                        }
                        cont++;
                    }
                    if (index != -1)
                    {
                        DataSet db = Sintactico.basesDatos.ElementAt(index);
                        cont = 0;
                        int index2 = -1;
                        foreach(DataTable tabla in db.Tables){
                            if(tabla.TableName == id){
                                index2 = cont; 
                            }
                            cont++;
                        }
                        if (index2 == -1)
                        {
                            List<String> primarias = new List<String>();
                            DataTable newtable = new DataTable(id);
                            List<Symbol> cols = recorrelcolumns(instr.ChildNodes.ElementAt(2));
                            foreach(Symbol col in cols){
                                String type = "";
                                if (col.getTipo() == "int")
                                {
                                    type = "Int32";
                                }
                                else {
                                    type = col.getTipo();
                                }
                                if (col.getPrimaryKey())
                                {
                                    primarias.Add(col.getNombre());
                                }
                                newtable.Columns.Add(new DataColumn(col.getNombre(), Type.GetType("System." + type)));
                            }
                            DataColumn[] keyCol = new DataColumn[primarias.Count];
                            int ind = 0;
                            foreach(String primary in primarias){
                                keyCol[ind] = newtable.Columns[primary];
                                ind++;
                            }
                            newtable.PrimaryKey = keyCol;
                            db.Tables.Add(newtable);
                            Sintactico.basesDatos[index] = db;
                        }
                        else {
                            if (!flagExist)
                            {
                                Error e = new Error();
                                e.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                                e.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("Ya existe una tabla con ese nombre en la base de datos");
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                    }
                    else
                    {
                        Error e = new Error();
                        e.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("La base de datos no existe");
                        TablaSimbolos.errorList.Add(e);
                        flagExist = false;
                    }
                }else{
                    Error e = new Error();
                    e.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se ha realizado la accion USE sobre una base de datos");
                    TablaSimbolos.errorList.Add(e);
                }
                
            }
            else if (token1[0].ToLower() == "user")
            {
                Resultados user = recorrevals(instr.ChildNodes.ElementAt(1));
                Resultados pass = recorrevals(instr.ChildNodes.ElementAt(4));
                DataSet dbuser = null;
                int conta = 0;
                if (Sintactico.userLogged == "admin")
                {
                    if (dbActual != "")
                    {
                        foreach (DataSet db in Sintactico.basesDatos)
                        {
                            if (db.DataSetName == "System")
                            {
                                dbuser = db;
                                return;
                            }
                            conta++;
                        }
                        if (dbuser != null)
                        {
                            DataRow userNew = dbuser.Tables["User"].NewRow();
                            userNew["user"] = user.getValor();
                            userNew["pass"] = pass.getValor();
                            dbuser.Tables["User"].Rows.Add(userNew);
                            Sintactico.basesDatos[conta] = dbuser;

                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                            e.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("Por alguna razon no encontro la base de datos System");
                            TablaSimbolos.errorList.Add(e);
                        }

                    }
                    else
                    {
                        Error e = new Error();
                        e.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("No se ha realizado la accion USE sobre una base de datos");
                        TablaSimbolos.errorList.Add(e);
                    }
                }
                else {
                    Error e = new Error();
                    e.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se cuenta con un usuario Admin para crear usuarios");
                    TablaSimbolos.errorList.Add(e);
                }
                
                
            }
        }

        /*
         * LCOLUMS.Rule =      MakePlusRule(LCOLUMS, COL); 
         */
        private List<Symbol> recorrelcolumns(ParseTreeNode lcolums)
        {
            List<Symbol> lcols = new List<Symbol>();
            Symbol primaries = null;
            foreach(ParseTreeNode cols in lcolums.ChildNodes){
                Symbol sim = recorrecol(cols);
                if (sim.getNombre() == "primaries" && sim.getTipo() == "primaries") {
                    primaries = sim;
                }else{
                    lcols.Add(sim);
                }
            }
            if(primaries!=null){
                foreach(String names in primaries.namesCol){
                    foreach(Symbol col in lcols){
                        if(names==col.getNombre()){
                            col.setPrimaryKey(primaries.getPrimaryKey());
                            break;
                        }
                    }
                }
            }
            return lcols;
        }

        /*
         * COL.Rule =          id + TIPO + primary + key
                                | id + TIPO
                                | primary + key + "(" + LIDS + ")";
         */
        private Symbol recorrecol(ParseTreeNode col)
        {
            if (col.ChildNodes.Count == 4)
            {
                Symbol sim = new Symbol();
                sim.setNombre(col.ChildNodes.ElementAt(0).Token.Value.ToString());
                sim.setTipo(recorretipo(col.ChildNodes.ElementAt(1)));
                sim.setPrimaryKey(true);
                return sim;
            }
            else if (col.ChildNodes.Count == 2)
            {
                Symbol sim = new Symbol();
                sim.setNombre(col.ChildNodes.ElementAt(0).Token.Value.ToString());
                sim.setTipo(recorretipo(col.ChildNodes.ElementAt(1)));
                sim.setPrimaryKey(false);
                return sim;
            }
            else 
            {
                Symbol sim = new Symbol();
                sim.setNombre("primaries");
                sim.setTipo("primaries");
                sim.setPrimaryKey(true);
                sim.namesCol = recorrelids(col.ChildNodes.ElementAt(2));
                return sim;
            }
        }

        /*INFODB.Rule =       ifs + EXIST + id
         *                      |id;
         *                      */
        private String recorreInfodb(ParseTreeNode infodb)
        {
            if (infodb.ChildNodes.Count == 1)
            {
                flagExist = false;
                return infodb.ChildNodes.ElementAt(0).Token.Value.ToString();
            }
            else {
                flagExist = true;
                return infodb.ChildNodes.ElementAt(0).Token.Value.ToString();
            }
        }

        /*
         * EXIST.Rule =        not + exist
                             | exist;
         */
        private void recorreexist(ParseTreeNode exist)
        {
            if (exist.ChildNodes.Count == 2)
            {

            }
            else
            {

            }

        }

        /*
         * ALTERDATA.Rule =    type + id + CAMBIA
                            | table + id + ALTERTABLE;
         */
        private void recorrealterdata(ParseTreeNode alterdata)
        {
            String[] token1 = alterdata.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "type")
            {
                //recorrecambia-> trae list<Retorno con el primer dato + o - depende si es eliminar o agregar
                List<Symbol> dato = recorrecambia(alterdata.ChildNodes.ElementAt(2));
                if(dato.ElementAt(0).getValor()=="+"){
                    for (int i = 1; i < dato.Count; i++ )
                    {
                        dato[i].setAmbito(alterdata.ChildNodes.ElementAt(1).Token.Value.ToString());
                        dato[i].setColumna(alterdata.ChildNodes.ElementAt(0).Token.Location.Column);
                        dato[i].setLinea(alterdata.ChildNodes.ElementAt(1).Token.Location.Line);
                        dato[i].setRol(1);
                        TablaSimbolos.symbolList.Add(dato[i]);
                    }
                }else{
                    for (int i = 1; i < dato.Count; i++)
                    {
                        int index = TablaSimbolos.getDato(dato[i].getNombre(), alterdata.ChildNodes.ElementAt(1).Token.Value.ToString());
                        if(index!=-1){
                            TablaSimbolos.symbolList.RemoveAt(index);
                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(alterdata.ChildNodes.ElementAt(0).Token.Location.Line);
                            e.setColumna(alterdata.ChildNodes.ElementAt(0).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se encuentra el atributo: "+dato[i].getNombre()+" declarado en el sistema.");
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                }
            }
            else
            {
                recorrealtertable(alterdata.ChildNodes.ElementAt(2), alterdata.ChildNodes.ElementAt(1).Token.Value.ToString());
            }
        }

        /*
         * ALTERTABLE.Rule =   add + LVALUSER + ";"
                            | drop + LIDS + ";";
         */
        private void recorrealtertable(ParseTreeNode alterdate, String nombre)
        {
            String[] token1 = alterdate.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "add")
            {
                List<Symbol> lvalores = recorrelvaluser(alterdate.ChildNodes.ElementAt(1));
                for (int i = 0; i < Sintactico.basesDatos.Count; i++) 
                {
                    if(Sintactico.basesDatos[i].DataSetName==dbActual){
                        if (Sintactico.basesDatos[i].Tables.Contains(nombre))
                        {
                            foreach(Symbol val in lvalores){
                                if (val.getTipo() == "int")
                                {
                                    Sintactico.basesDatos[i].Tables[nombre].Columns.Add(new DataColumn(val.getNombre(), Type.GetType("System.Int32")));
                                }
                                else 
                                {
                                    Sintactico.basesDatos[i].Tables[nombre].Columns.Add(new DataColumn(val.getNombre(), Type.GetType("System."+val.getTipo())));
                                }
                            }
                        }
                        else {
                            Error e = new Error();
                            e.setLinea(alterdate.ChildNodes.ElementAt(0).Token.Location.Line);
                            e.setColumna(alterdate.ChildNodes.ElementAt(0).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se encuentra la tabla: " + nombre + " en la base de datos.");
                            TablaSimbolos.errorList.Add(e);
                        }
                        return;
                    }
                }
            }
            else
            {
                List<String> lids = recorrelids(alterdate.ChildNodes.ElementAt(1));
                for (int i = 0; i < Sintactico.basesDatos.Count; i++)
                {
                    if (Sintactico.basesDatos[i].DataSetName == dbActual)
                    {
                        if (Sintactico.basesDatos[i].Tables.Contains(nombre))
                        {
                            foreach (String val in lids)
                            {
                                Sintactico.basesDatos[i].Tables[nombre].Columns.Remove(val);
                            }
                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(alterdate.ChildNodes.ElementAt(0).Token.Location.Line);
                            e.setColumna(alterdate.ChildNodes.ElementAt(0).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se encuentra la tabla: " + nombre + " en la base de datos.");
                            TablaSimbolos.errorList.Add(e);
                        }
                        return;
                    }
                }
            }
        }

        /*
         * DROPDATA.Rule =     table + INFODB
                            | db + id + ";";
         */
        private TipoDrop recorredropdata(ParseTreeNode dropdata)
        {
            String[] token1 = dropdata.ChildNodes.ElementAt(0).ToString().Split(' ');
            TipoDrop data = new TipoDrop();
            flagExist = false;
            data.setNombre(recorreInfodb(dropdata.ChildNodes.ElementAt(1)));
            data.setValida(flagExist);
            if (token1[0].ToLower() == "table")
            {   
                data.setTipo("table");
            }
            else
            {
                data.setTipo("db");
            }
            return data;
        }

        /*
         * EXPSELECT.Rule =    EXP + ORDERLIMIT
                            | indato + EXP + ORDERLIMIT
                            | ORDERLIMIT;
         */
        private void recorreexpselect(ParseTreeNode expselect)
        {
            if (expselect.ChildNodes.ElementAt(0).ToString() == "EXP")
            {

            }
            else if (expselect.ChildNodes.ElementAt(0).ToString() == "ORDERLIMIT")
            {

            }
            else
            {

            }
        }

        /*
         * EXPSELECTVAL.Rule = EXP + order + by + LORDER + limit + EXP 
                            | EXP + order + by + LORDER 
                            | EXP + limit + EXP 
                            | EXP 
                            | indato + EXP + order + by + LORDER + limit + EXP 
                            | indato + EXP + order + by + LORDER 5
                            | indato + EXP + limit + EXP 
                            | indato + EXP 
                            | order + by + LORDER + limit + EXP 5
                            | order + by + LORDER 
                            | limit + EXP; 
         * */
        private void recorreexpselectval(ParseTreeNode expSelectVal)
        {
            String[] token1 = expSelectVal.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (expSelectVal.ChildNodes.Count == 1)
            {

            }
            else if (expSelectVal.ChildNodes.Count == 2)
            {
                if (token1[0].ToLower() == "limit")
                {

                }
                else
                {

                }
            }
            else if (expSelectVal.ChildNodes.Count == 3)
            {
                if (token1[0].ToLower() == "exp")
                {

                }
                else
                {

                }
            }
            else if (expSelectVal.ChildNodes.Count == 4)
            {
                if (token1[0].ToLower() == "exp")
                {

                }
                else
                {

                }
            }
            else if (expSelectVal.ChildNodes.Count == 5)
            {
                if (token1[0].ToLower() == "order")
                {

                }
                else
                {

                }
            }
            else if (expSelectVal.ChildNodes.Count == 6)
            {

            }
            else if (expSelectVal.ChildNodes.Count == 7)
            {

            }
        }

        /*
         * TIPO.Rule =         PRIMITIVO
                                | map + "<" + PRIMITIVO + ">"
                                | list + "<" + PRIMITIVO + ">"
                                | set + "<" + PRIMITIVO + ">";
         */
        private String recorretipo(ParseTreeNode tipo)
        {
            String[] token1 = tipo.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "set")
            {
                return "set";
            }
            else if (token1[0].ToLower() == "list")
            {
                return "list";
            }
            else if (token1[0].ToLower() == "map")
            {
                return "map";
            }
            else
            {
                return recorreprimitivo(tipo.ChildNodes.ElementAt(0));
            }
        }

        /*
         * PRIMITIVO.Rule =    entero
                                | dec
                                | strings
                                | boolean
                                | date
                                | time
                                | id;
         */
        private String recorreprimitivo(ParseTreeNode primitivo)
        {
            String[] token1 = primitivo.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "int")
            {
                return "int";
            }
            else if (token1[0].ToLower() == "double")
            {
                return "double";
            }
            else if (token1[0].ToLower() == "string")
            {
                return "string";
            }
            else if (token1[0].ToLower() == "boolean")
            {
                return "boolean";
            }
            else if (token1[0].ToLower() == "date")
            {
                return "date";
            }
            else if (token1[0].ToLower() == "time")
            {
                return "time";
            }
            else
            {
                tipoVar = primitivo.ChildNodes.ElementAt(0).Token.Value.ToString();
                return "id";
            }
        }

        /*
         * EXP.Rule =          EXP + or + EXP
                            | EXP + and + EXP
                            | EXP + xor + EXP
                            | EXP + dif + EXP
                            | EXP + igual + EXP
                            | EXP + menor + EXP
                            | EXP + mayor + EXP
                            | EXP + mayorigual + EXP
                            | EXP + menorigual + EXP
                            | EXP + mas + EXP
                            | EXP + menos + EXP
                            | EXP + por + EXP
                            | EXP + div + EXP
                            | EXP + mod + EXP
                            | EXP + pot + EXP
                            | notsymbol + EXP
                            | menos + EXP
                            | EXP + aumenta
                            | EXP + disminuye
                            | ToTerm("(") + EXP + ToTerm(")")
                            | VAL;
         */
        private Resultados recorreexp(ParseTreeNode exp)
        {
            if(exp.ChildNodes.Count==1)
            {
                Resultados res = recorrevals(exp.ChildNodes.ElementAt(0));
                return res;
            }else{
                String[] token1 = exp.ChildNodes.ElementAt(1).ToString().Split(' ');
                String[] token0 = exp.ChildNodes.ElementAt(0).ToString().Split(' ');
                if (token1[0].ToLower() == "||")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));
                    if(res1.getTipo()=="boolean" && res2.getTipo()=="boolean"){
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((Convert.ToBoolean(res1.getValor()) || Convert.ToBoolean(res2.getValor())).ToString());
                        return res;
                    }
                    Error e = new Error();
                    e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se logro hacer la operacion, los tipos de datos que intenta operar no son booleanos");
                    TablaSimbolos.errorList.Add(e);
                }
                else if (token1[0].ToLower() == "&&")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));
                    if (res1.getTipo() == "boolean" && res2.getTipo() == "boolean")
                    {
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((Convert.ToBoolean(res1.getValor()) && Convert.ToBoolean(res2.getValor())).ToString());
                        return res;
                    }
                    Error e = new Error();
                    e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se logro hacer la operacion, los tipos de datos que intenta operar no son booleanos");
                    TablaSimbolos.errorList.Add(e);
                }
                else if (token1[0].ToLower() == "^")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));
                    if (res1.getTipo() == "boolean" && res2.getTipo() == "boolean")
                    {
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        Boolean result = (Convert.ToBoolean(res1.getValor()) && !Convert.ToBoolean(res2.getValor())) || (!Convert.ToBoolean(res1.getValor()) && Convert.ToBoolean(res2.getValor()));
                        res.setValor(result.ToString());
                        return res;
                    }
                    Error e = new Error();
                    e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se logro hacer la operacion, los tipos de datos que intenta operar no son booleanos");
                    TablaSimbolos.errorList.Add(e);
                }
                else if (token1[0].ToLower() == "!=")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));
                
                    if(res1.getTipo()=="int" && res2.getTipo()=="double" || res1.getTipo()=="double" && res2.getTipo()=="int" || 
                        res1.getTipo()=="double" && res2.getTipo()=="double" || res1.getTipo()=="int" && res2.getTipo()=="int" ||
                        res1.getTipo()=="string" && res2.getTipo()=="string" || res1.getTipo()=="boolean" && res2.getTipo()=="boolean" ||
                        res1.getTipo()=="date" && res2.getTipo()=="date" || res1.getTipo()=="time" && res2.getTipo()=="time"){
                            Resultados res = new Resultados();
                            res.setTipo("boolean");
                            res.setValor((res1.getValor() != res2.getValor()).ToString());
                            return res;
                    }
                    Error e = new Error();
                    e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : "+res1.getTipo()+ " != " +res2.getTipo());
                    TablaSimbolos.errorList.Add(e);
                }
                else if (token1[0].ToLower() == "==")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double" || res1.getTipo() == "int" && res2.getTipo() == "int" ||
                        res1.getTipo() == "string" && res2.getTipo() == "string" || res1.getTipo() == "boolean" && res2.getTipo() == "boolean" ||
                        res1.getTipo() == "date" && res2.getTipo() == "date" || res1.getTipo() == "time" && res2.getTipo() == "time")
                    {
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((res1.getValor() == res2.getValor()).ToString());
                        return res;
                    }
                    Error e = new Error();
                    e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(e);
                }
                else if (token1[0].ToLower() == "<")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor() + ", Error: " + excep.ToString());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "int") {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor()+": "+excep);
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "double") {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor()+": "+excep);
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor()+": "+excep);
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "double") {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor()+": "+excep);
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "string")
                    {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0);
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "string")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0); 
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "date" && res2.getTipo() == "date")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1 < fecha2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "time" && res2.getTipo() == "time")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1.TimeOfDay < fecha2.TimeOfDay).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == ">")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "int")
                    {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "string")
                    {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0);
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "string")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0);
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 > num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "date" && res2.getTipo() == "date")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1 > fecha2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "time" && res2.getTipo() == "time")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1.TimeOfDay > fecha2.TimeOfDay).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == ">=")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "int")
                    {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "string")
                    {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0);
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "string")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0);
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 >= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "date" && res2.getTipo() == "date")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1 >= fecha2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "time" && res2.getTipo() == "time")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1.TimeOfDay >= fecha2.TimeOfDay).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "<=")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "int")
                    {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "double" && res2.getTipo() == "string")
                    {
                        double num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0);
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res1.getValor()), 0);
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "string")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(res2.getValor()), 0);
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 <= num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "date" && res2.getTipo() == "date")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1 <= fecha2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "time" && res2.getTipo() == "time")
                    {
                        DateTime fecha1 = DateTime.Now;
                        DateTime fecha2 = DateTime.Now;
                        try
                        {
                            fecha1 = DateTime.Parse(res1.getValor());
                            fecha2 = DateTime.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son fecha : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((fecha1.TimeOfDay <= fecha2.TimeOfDay).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "+")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("double");
                        res.setValor((num1 + num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor((num1 + num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "string" && res2.getTipo() == "int" || res1.getTipo() == "int" && res2.getTipo() == "string" ||
                        res1.getTipo() == "string" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "string" ||
                        res1.getTipo() == "string" && res2.getTipo() == "boolean" || res1.getTipo() == "boolean" && res2.getTipo() == "string" ||
                        res1.getTipo() == "string" && res2.getTipo() == "string")
                    {
                        Resultados res = new Resultados();
                        res.setTipo("string");
                        res.setValor((res1.getValor() + res2.getValor()).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "-")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));
                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("double");
                        res.setValor((num1 - num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor((num1 - num2).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "*")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("double");
                        res.setValor((num1 * num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor((num1 * num2).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "/")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        double num1 = 0;
                        double num2 = 0;
                        try
                        {
                            num1 = Convert.ToDouble(res1.getValor());
                            num2 = Convert.ToDouble(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("double");
                        res.setValor((num1 / num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor(Convert.ToInt32(num1 / num2).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "%")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToInt32(Convert.ToDouble(res1.getValor()));
                            num2 = Convert.ToInt32(Convert.ToDouble(res2.getValor()));
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor((num1 & num2).ToString());
                        return res;
                    }
                    else if (res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Int32.Parse(res1.getValor());
                            num2 = Int32.Parse(res2.getValor());
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor(Convert.ToInt32(num1 % num2).ToString());
                        return res;
                    }
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "**")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));

                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double" || res1.getTipo() == "int" && res2.getTipo() == "int")
                    {
                        int num1 = 0;
                        int num2 = 0;
                        try
                        {
                            num1 = Convert.ToInt32(Convert.ToDouble(res1.getValor()));
                            num2 = Convert.ToInt32(Convert.ToDouble(res2.getValor()));
                        }
                        catch (FormatException excep)
                        {
                            Error e = new Error();
                            e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor() + " != " + res2.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("double");
                        res.setValor((Math.Pow(num1,num2)).ToString());
                        return res;
                    }
                
                    Error error = new Error();
                    error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    error.setTipo("Semantico");
                    error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
                    TablaSimbolos.errorList.Add(error);
                }
                else if (token1[0].ToLower() == "++")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    if (res1.getValor() != "null") {
                        if(res1.getTipo()=="double"){
                            double num1 = 0;
                            try
                            {
                                num1 = Convert.ToDouble(res1.getValor());
                            }
                            catch (FormatException excep)
                            {
                                Error e = new Error();
                                e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                            Resultados res = new Resultados();
                            res.setTipo("double"); 
                            res.setValor((num1 + 1).ToString());
                            return res;
                        }
                        else if (res1.getTipo() == "int") {
                            int num1 = 0;
                            try
                            {
                                num1 = Convert.ToInt32(res1.getValor());
                            }
                            catch (FormatException excep)
                            {
                                Error e = new Error();
                                e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                            Resultados res = new Resultados();
                            res.setTipo("int");
                            res.setValor((num1 + 1).ToString());
                            return res;
                        }
                        Error error = new Error();
                        error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                        error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                        error.setTipo("Semantico");
                        error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo());
                        TablaSimbolos.errorList.Add(error);
                    }
                }
                else if (token1[0].ToLower() == "--")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    if (res1.getValor() != "null")
                    {
                        if (res1.getTipo() == "double")
                        {
                            double num1 = 0;
                            try
                            {
                                num1 = Convert.ToDouble(res1.getValor());
                            }
                            catch (FormatException excep)
                            {
                                Error e = new Error();
                                e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                            Resultados res = new Resultados();
                            res.setTipo("double");
                            res.setValor((num1 - 1).ToString());
                            return res;
                        }
                        else if (res1.getTipo() == "int")
                        {
                            int num1 = 0;
                            try
                            {
                                num1 = Convert.ToInt32(res1.getValor());
                            }
                            catch (FormatException excep)
                            {
                                Error e = new Error();
                                e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                            Resultados res = new Resultados();
                            res.setTipo("int");
                            res.setValor((num1 - 1).ToString());
                            return res;
                        }
                        Error error = new Error();
                        error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                        error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                        error.setTipo("Semantico");
                        error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo());
                        TablaSimbolos.errorList.Add(error);
                    }
                }
                else if (token0[0].ToLower() == "!")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(1));
                    if (res1.getTipo() == "boolean")
                    {
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((!Convert.ToBoolean(res1.getValor())).ToString());
                        return res;
                    }
                    Error e = new Error();
                    e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se logro hacer la operacion, los tipos de datos que intenta operar no son booleanos");
                    TablaSimbolos.errorList.Add(e);
                }
                else if (token0[0].ToLower() == "-")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(1));
                    if (res1.getValor() != "null")
                    {
                        if (res1.getTipo() == "double")
                        {
                            double num1 = 0;
                            try
                            {
                                num1 = Convert.ToDouble(res1.getValor());
                            }
                            catch (FormatException excep)
                            {
                                Error e = new Error();
                                e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                            Resultados res = new Resultados();
                            res.setTipo("double");
                            res.setValor((num1 * -1).ToString());
                            return res;
                        }
                        else if (res1.getTipo() == "int")
                        {
                            int num1 = 0;
                            try
                            {
                                num1 = Convert.ToInt32(res1.getValor());
                            }
                            catch (FormatException excep)
                            {
                                Error e = new Error();
                                e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                            Resultados res = new Resultados();
                            res.setTipo("int");
                            res.setValor((num1 * -1).ToString());
                            return res;
                        }
                        else if (res1.getTipo() == "boolean")
                        {
                            Boolean num1 = false;
                            try
                            {
                                num1 = Convert.ToBoolean(res1.getValor());
                            }
                            catch (FormatException excep)
                            {
                                Error e = new Error();
                                e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("No se logro hacer la operacion, los tipos no son los esperados : " + res1.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                            Resultados res = new Resultados();
                            res.setTipo("boolean");
                            res.setValor((!num1).ToString());
                            return res;
                        }
                        Error error = new Error();
                        error.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                        error.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                        error.setTipo("Semantico");
                        error.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo());
                        TablaSimbolos.errorList.Add(error);
                    }
                }
                else
                {
                    if (exp.ChildNodes.ElementAt(0).ToString().ToLower() == "exp")
                    {
                        Resultados res = recorreexp(exp.ChildNodes.ElementAt(0));
                        return res;
                    }
                
                }
            }
            Resultados resfail = new Resultados();
            resfail.setTipo("null");
            resfail.setValor("null");
            return resfail;
        }

        /*
         * PUNTOID.Rule =      MakePlusRule(PUNTOID, ToTerm("."), IDES);
            
         */
        private Symbol recorrepuntoid(ParseTreeNode puntoid)
        {
            Symbol info = null;
            int index = ambito.Count - 1;
            ambitoVals = ambito.ElementAt(index);
            foreach (ParseTreeNode ident in puntoid.ChildNodes)
            {
                String nombre = recorreides(ident);
                foreach (String ambit in ambito)
                {
                    info = TablaSimbolos.buscaDato(nombre, ambitoVals);
                    if (info == null)
                    {
                        if (index > 0)
                        {
                            index--;
                            ambitoVals = ambito.ElementAt(index);
                        }
                        else
                        {
                            Error e = new Error();
                            e.setLinea(0);
                            e.setColumna(0);
                            e.setTipo("Semantico");
                            e.setMsj("No se encontro el dato de la variable: " + nombre);
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                    else
                    {
                        ambitoVals = info.getNombre();
                        break;
                    }
                }

            }
            return info;
        }

        /*
         * IDES.Rule =         id
                                | "@" + id;
         */
        private String recorreides(ParseTreeNode ides)
        {
            return ides.ChildNodes.ElementAt(0).Token.Value.ToString();
        }

        /*
         * VAL.Rule =          number
                                | cadena
                                | decimales
                                | verdadero
                                | falso
                                | fecha
                                | hora
                                | news + id
                                | "{" + LVALORES + "}"
                                | PUNTOID --
                                | "(" + TIPO + ")" + VAL
                                | ToTerm("null")
                                | select + DATASELECT + froms + id + SELECTWHEREVAL
                                | FUNAGREG + "(" + "<" + select + DATASELECT + froms + id + SELECTWHEREVAL + ">" + ")"
                                | PUNTOID + ToTerm(".") + "[" + VAL + "]"
                                | PUNTOID + "[" + VAL + "]"
                                | "[" + LVALORES + "]"
                                | "(" + EXP + ")" + "?" + EXP + ":" + EXP
                                | PUNTOID + "." + insert + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + get + "(" + VAL + ")"
                                | PUNTOID + "." + set + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + remove + "(" + VAL + ")"
                                | PUNTOID + "." + size + "(" + ")"
                                | PUNTOID + "." + clear + "(" + ")"
                                | PUNTOID + "." + contain + "(" + EXP + ")"
                                | PUNTOID + "(" + PARMVALS
                                | call + PUNTOID + "(" + PARMVALS
                                | PUNTOID + "." + length + "(" + ")"
                                | PUNTOID + "." + upper + "(" + ")"
                                | PUNTOID + "." + lower + "(" + ")"
                                | PUNTOID + "." + starts + "(" + VAL + ")"
                                | PUNTOID + "." + ends + "(" + VAL + ")"
                                | PUNTOID + "." + substr + "(" + VAL + "," + VAL + ")"
                                | PUNTOID + "." + year + "(" + ")"
                                | PUNTOID + "." + month + "(" + ")"
                                | PUNTOID + "." + day + "(" + ")"
                                | PUNTOID + "." + hora + "(" + ")"
                                | PUNTOID + "." + min + "(" + ")"
                                | PUNTOID + "." + second + "(" + ")"
                                | today + "(" + ")"
                                | now + "(" + ")";
         */
        private Resultados recorrevals(ParseTreeNode vals)
        {
            if(vals.ChildNodes.ElementAt(0).Term.Name=="number"){
                Resultados res = new Resultados();
                res.setTipo("int");
                res.setValor(vals.ChildNodes.ElementAt(0).Token.Value.ToString());
                Boolean tiene = res.getValor().Contains(',');
                if(tiene){
                    res.setTipo("double");
                }
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name=="cadena")
            {
                Resultados res = new Resultados();
                res.setTipo("string");
                res.setValor(vals.ChildNodes.ElementAt(0).Token.Value.ToString());
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "decimales") {
                Resultados res = new Resultados();
                res.setTipo("double");
                res.setValor(vals.ChildNodes.ElementAt(0).Token.Value.ToString());
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "true")
            {
                Resultados res = new Resultados();
                res.setTipo("boolean");
                res.setValor(vals.ChildNodes.ElementAt(0).Token.Value.ToString());
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "false")
            {
                Resultados res = new Resultados();
                res.setTipo("boolean");
                res.setValor(vals.ChildNodes.ElementAt(0).Token.Value.ToString());
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "news")
            {
                Resultados res = new Resultados();
                String tipo =recorretipo(vals.ChildNodes.ElementAt(1));
                res.setTipo(tipo);
                if (tipo == "list") {
                    res.setIsArray(1);
                    res.setContenido(new List<Resultados>());
                    res.setValor("");
                }
                else if (tipo == "map") {
                    res.setIsArray(3);
                    res.setContenido(new List<Resultados>());
                    res.setValor("");
                }
                else if (tipo == "set")
                {
                    res.setIsArray(2);
                    res.setContenido(new List<Resultados>());
                    res.setValor("");
                }
                else if (tipo == "int" || tipo == "double")
                {
                    res.setValor("0");
                }else if(tipo=="boolean"){
                    res.setValor("false");
                }
                else if (tipo == "date")
                {
                    res.setValor(DateTime.Today.ToString());
                }
                else if (tipo == "time")
                {
                    res.setValor(DateTime.Now.ToString("hh:mm:ss"));
                }
                else {
                    res.setValor("");
                }
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "fecha")
            {
                Resultados res = new Resultados();
                res.setTipo("date");
                res.setValor(vals.ChildNodes.ElementAt(0).Token.Value.ToString());
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "hora")
            {
                Resultados res = new Resultados();
                res.setTipo("time");
                res.setValor(vals.ChildNodes.ElementAt(0).Token.Value.ToString());
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "LVALORES")
            {
                //arreglos
                List<Resultados> cont = recorrelvalores(vals.ChildNodes.ElementAt(0));
                Resultados res = new Resultados();
                res.setContenido(cont);
                res.setValor("{");
                if(cont.ElementAt(0).getTipo()=="map"){
                    foreach (Resultados ind in cont)
                    {
                        res.setValor(res.getValor()+ind.getClave()+":"+ind.getValor()+",");
                    }
                    res.setValor(res.getValor().Substring(0, res.getValor().Length - 2)+ "}");
                    res.setTipo("map");
                    res.setIsArray(3);
                    return res;
                }
                foreach (Resultados ind in cont)
                {
                    res.setValor(res.getValor() + ind.getValor() + ",");
                }
                res.setValor(res.getValor().Substring(0, res.getValor().Length - 2) + "}");
                res.setTipo("list");
                res.setIsArray(1);
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "TIPO")
            {
                Resultados res = new Resultados();
                res.setTipo(recorretipo(vals.ChildNodes.ElementAt(0)));
                res.setValor(vals.ChildNodes.ElementAt(1).Token.Value.ToString());
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "null")
            {
                Resultados res = new Resultados();
                res.setTipo("null");
                res.setValor("null");
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "select")
            {
                //consultar db
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "FUNAGREG")
            {
                //consultar db
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "EXP")
            {
                Resultados cond = recorreexp(vals.ChildNodes.ElementAt(0));
                if(cond.getValor().ToLower()=="true" && cond.getTipo()=="boolean"){
                    return recorreexp(vals.ChildNodes.ElementAt(1));
                }else{
                    return recorreexp(vals.ChildNodes.ElementAt(2));
                }
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "call")
            {
                //ejecutar funcion desde tabla de simbolos
                List<Resultados> parms = recorreParmvals(vals.ChildNodes.ElementAt(2));
                List<String> tipos = new List<String>();
                foreach (Resultados parm in parms)
                {
                    tipos.Add(parm.getTipo());
                }
                Symbol dato = recorrepuntoid(vals.ChildNodes.ElementAt(1));
                List<String> retList = new List<String>();
                foreach (Retorno ret in dato.getRetornos()) {
                    retList.Add(ret.getTipo());
                }
                if(dato!=null){
                    int index = TablaSimbolos.getProcedure(dato.getNombre(), dato.getAmbito(), dato.getRol(), parms.Count, tipos, dato.getRetornos().Count, retList);
                }else{
                    Error e = new Error();
                    e.setLinea(vals.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(vals.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se encuentra declarado el procedimiento que se intenta llamar: ");
                    TablaSimbolos.errorList.Add(e);
                }
                
                for (int i = 0; i < parms.Count; i++)
                {
                    TablaSimbolos.symbolList[dato.parametros.ElementAt(i).getIndex()].setValor(parms[i].getValor());
                }
                int numret = valoresRet.Count;
                recorrelcql(dato.getCuerpo());
                if (dato.getRetornos().Count > 0 && valoresRet.Count <= numret)
                {
                    Error e = new Error();
                    e.setLinea(vals.ChildNodes.ElementAt(0).Token.Location.Line);
                    e.setColumna(vals.ChildNodes.ElementAt(0).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("El procedimiento no retornaba valores aunque se habia declarado que si: ");
                    TablaSimbolos.errorList.Add(e);
                }
                else {
                    Resultados res = new Resultados();
                    res.setIsArray(1);
                    res.retornos = valoresRet.ElementAt(valoresRet.Count - 1).retornos;
                    if(res.retornos!=null){
                        res.setValor("{");
                        res.setTipo(res.retornos.ElementAt(0).getTipo());
                        int i = 0;
                        foreach(Retorno retorn in res.retornos){
                            res.setValor(res.getValor()+res.retornos.ElementAt(i).getValor());
                            i++;
                        }
                        res.setValor(res.getValor()+"}");
                        res.setTipo("string");
                    }
                    return res;
                }
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "today")
            {
                DateTime Hoy = DateTime.Today;
                String fecha_actual = Hoy.ToString("dd/MM/yyyy");
                
                Resultados res = new Resultados();
                res.setTipo("date");
                res.setValor(fecha_actual);
                return res;
            }
            else if (vals.ChildNodes.ElementAt(0).Term.Name == "now")
            {
                String ahorita = DateTime.Now.ToString("hh:mm:ss");
                Resultados res = new Resultados();
                res.setTipo("time");
                res.setValor(ahorita);
                return res;
            }
            else 
            {//PUNTOID
                if(vals.ChildNodes.Count==1){
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null) {
                        Resultados res = new Resultados();
                        res.setTipo(sim.getTipo());
                        res.setValor(sim.getValor());
                        return res;
                    }
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="get")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if(sim.getIsArray()!=0){
                        Resultados ind = recorreexp(vals.ChildNodes.ElementAt(2));
                        int num1;
                        try {
                            num1 = Int32.Parse(ind.getValor());
                            if (num1 < sim.collection.Count)
                            {
                                return sim.collection.ElementAt(num1);
                            }
                            else {
                                Error e = new Error();
                                e.setLinea(vals.ChildNodes.ElementAt(1).Token.Location.Line);
                                e.setColumna(vals.ChildNodes.ElementAt(1).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("El indice supera el tamaño de la collection: " + num1 + " Tamaño de collection: " + sim.collection.Count);
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        catch(FormatException exc){
                            Error e = new Error();
                            e.setLinea(vals.ChildNodes.ElementAt(1).Token.Location.Line);
                            e.setColumna(vals.ChildNodes.ElementAt(1).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("El tipo de dato no es un numero y no puede ser un indice del get: " + ind.getValor() + " Error: "+ exc.ToString());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }else{
                        Error e = new Error();
                        e.setLinea(vals.ChildNodes.ElementAt(1).Token.Location.Line);
                        e.setColumna(vals.ChildNodes.ElementAt(1).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("El tipo de dato no es un collection: " + sim.getNombre()+ " Error: "+ sim.getIsArray());
                        TablaSimbolos.errorList.Add(e);
                    }
                    //manejo collections            
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="size")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim.getIsArray()!=0)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor(sim.collection.Count.ToString());
                        return res;
                    }
                    Resultados res2 = new Resultados();
                    res2.setTipo("int");
                    res2.setValor("1");
                    return res2;   
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="contain")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    Resultados exp = recorreexp(vals.ChildNodes.ElementAt(2));
                    if(sim.getIsArray()!=0){
                        foreach(Resultados dato in sim.collection){
                            if(exp.getValor()==dato.getValor()){
                                Resultados res = new Resultados();
                                res.setTipo("boolean");
                                res.setValor("true");
                                return res; 
                            }
                        }
                        Resultados res2 = new Resultados();
                        res2.setTipo("boolean");
                        res2.setValor("false");
                        return res2; 
                    }else{
                        if(exp.getValor()==sim.getValor()){
                            Resultados res = new Resultados();
                            res.setTipo("boolean");
                            res.setValor("true");
                            return res; 
                        }
                        Resultados res2 = new Resultados();
                        res2.setTipo("boolean");
                        res2.setValor("false");
                        return res2; 
                    }
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="PARMVALS")
                {
                    List<Resultados> parms = recorreParmvals(vals.ChildNodes.ElementAt(1));
                    List<String> tipos = new List<String>();
                    foreach (Resultados parm in parms)
                    {
                        tipos.Add(parm.getTipo());
                    }
                    Symbol ident = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    int index = TablaSimbolos.getFuncion(ident.getNombre(), ident.getAmbito(), ident.getRol(), ident.getTipo(), parms.Count, tipos);
                    for (int i = 0; i < parms.Count; i++)
                    {
                        TablaSimbolos.symbolList[ident.parametros.ElementAt(i).getIndex()].setValor(parms[i].getValor());
                    }
                    int numret = valoresRet.Count;
                    recorrelcql(ident.getCuerpo());
                    if (valoresRet.Count <= numret)
                    {
                        Error e = new Error();
                        e.setLinea(vals.ChildNodes.ElementAt(0).Token.Location.Line);
                        e.setColumna(vals.ChildNodes.ElementAt(0).Token.Location.Column);
                        e.setTipo("Semantico");
                        e.setMsj("El procedimiento no retornaba valores aunque se habia declarado que si: ");
                        TablaSimbolos.errorList.Add(e);
                    }
                    else
                    {
                        Resultados res = new Resultados();
                        res.retornos = valoresRet.ElementAt(valoresRet.Count - 1).retornos;
                        if (res.retornos != null)
                        {
                            res.setValor(res.retornos.ElementAt(0).getValor());
                            res.setTipo(res.retornos.ElementAt(0).getTipo());
                        }
                        return res;
                    }
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="length")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        res.setValor(sim.getValor().Length.ToString());
                        return res;
                    }           
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="upper")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo(sim.getTipo());
                        res.setValor(sim.getValor().ToUpper());
                        return res;
                    }            
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="lower")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo(sim.getTipo());
                        res.setValor(sim.getValor().ToLower());
                        return res;
                    }    
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="starts")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    Resultados val = recorrevals(vals.ChildNodes.ElementAt(2));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo(sim.getTipo());
                        res.setValor(sim.getValor().StartsWith(val.getValor()).ToString());
                        return res;
                    }           
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="ends")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    Resultados val = recorrevals(vals.ChildNodes.ElementAt(2));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo(sim.getTipo());
                        res.setValor(sim.getValor().EndsWith(val.getValor()).ToString());
                        return res;
                    }            
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="substr")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    Resultados val = recorrevals(vals.ChildNodes.ElementAt(2));
                    Resultados val2 = recorrevals(vals.ChildNodes.ElementAt(3));
                    int num1=0;
                    int num2=0;
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo(sim.getTipo());
                        if (Int32.TryParse(val.getValor(), out num1))
                        {
                            if (Int32.TryParse(val2.getValor(), out num2))
                            {
                                res.setValor(sim.getValor().Substring(num1, num2).ToString());
                                return res;
                            }
                            else {
                                Error e = new Error();
                                e.setLinea(vals.ChildNodes.ElementAt(3).Token.Location.Line);
                                e.setColumna(vals.ChildNodes.ElementAt(3).Token.Location.Column);
                                e.setTipo("Semantico");
                                e.setMsj("El tipo de dato no es un numero y no puede ir en la funcion substring: " + val2.getValor());
                                TablaSimbolos.errorList.Add(e);
                            }
                        }
                        else {
                            Error e = new Error();
                            e.setLinea(vals.ChildNodes.ElementAt(2).Token.Location.Line);
                            e.setColumna(vals.ChildNodes.ElementAt(2).Token.Location.Column);
                            e.setTipo("Semantico");
                            e.setMsj("El tipo de dato no es un numero y no puede ir en la funcion substring: " + val.getValor());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }             
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="year")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        DateTime parsedDate = DateTime.Parse(sim.getValor());
                        res.setValor(parsedDate.Year.ToString());
                        return res;
                    }            
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="month")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        DateTime parsedDate = DateTime.Parse(sim.getValor());
                        res.setValor(parsedDate.Month.ToString());
                        return res;
                    }           
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="day")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("int");
                        DateTime parsedDate = DateTime.Parse(sim.getValor());
                        res.setValor(parsedDate.Day.ToString());
                        return res;
                    }            
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="hora")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("time");
                        DateTime parsedDate = DateTime.Parse(sim.getValor());
                        res.setValor(parsedDate.Hour.ToString());
                        return res;
                    }            
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="min")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("time");
                        DateTime parsedDate = DateTime.Parse(sim.getValor());
                        res.setValor(parsedDate.Minute.ToString());
                        return res;
                    }            
                }
                else if(vals.ChildNodes.ElementAt(1).Term.Name=="second")
                {
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    if (sim != null)
                    {
                        Resultados res = new Resultados();
                        res.setTipo("time");
                        DateTime parsedDate = DateTime.Parse(sim.getValor());
                        res.setValor(parsedDate.Second.ToString());
                        return res;
                    }
                }
                else //llamada id[num] || id.[num]
                {
                    //manejo de arreglos
                    Symbol sim = recorrepuntoid(vals.ChildNodes.ElementAt(0));
                    Resultados ind = recorreexp(vals.ChildNodes.ElementAt(1));
                    if (sim.getIsArray() != 0)
                    {
                        int num1 = Int32.Parse(ind.getValor());
                        try { 
                            if(sim.collection.Count> num1){
                                return sim.collection.ElementAt(num1);
                            }else{
                                Error e = new Error();
                                e.setLinea(0);
                                e.setColumna(0);
                                e.setTipo("Semantico");
                                e.setMsj("El indice es mayor al tamaño de la collection: "+num1+" , Tamaño collection: "+sim.collection.Count);
                                TablaSimbolos.errorList.Add(e);
                            }
                        }catch(FormatException exc){
                            Error e = new Error();
                            e.setLinea(0);
                            e.setColumna(0);
                            e.setTipo("Semantico");
                            e.setMsj("El indice no es de tipo numerico: "+ind.getValor()+ " Error: "+exc.ToString());
                            TablaSimbolos.errorList.Add(e);
                        }
                    }
                    else {
                        Error e = new Error();
                        e.setLinea(0);
                        e.setColumna(0);
                        e.setTipo("Semantico");
                        e.setMsj("La variable no indica ser de tipo collection: "+sim.getNombre()+" Error: "+sim.getIsArray());
                        TablaSimbolos.errorList.Add(e);
                    }
                }
            }
            Resultados resNull = new Resultados();
            resNull.setTipo("null");
            resNull.setValor("null");
            return resNull;
        }

        /*
         * LVALORES.Rule =     MakePlusRule(LVALORES, ToTerm(","), VALUES);
         */
        private List<Resultados> recorrelvalores(ParseTreeNode lvalores)
        {
            List<Resultados> values = new List<Resultados>();
            foreach (ParseTreeNode valor in lvalores.ChildNodes)
            {
                values.Add(recorrevalues(valor));
            }
            return values;
        }

        /*
         * VALUES.Rule =       EXP
                                | VAL + ":" + EXP;
         */
        private Resultados recorrevalues(ParseTreeNode exp)
        {
            if (exp.ChildNodes.Count == 1)
            {
                return recorreexp(exp.ChildNodes.ElementAt(0));
            }
            else
            {
                Resultados clave = recorrevals(exp.ChildNodes.ElementAt(0));
                Resultados valor = recorreexp(exp.ChildNodes.ElementAt(1));
                valor.settipoClave(clave.getTipo());
                valor.setClave(clave.getValor());
                valor.setTipo("map");
                return valor;
            }
        }

    }
}
