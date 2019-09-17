using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace CQL_Teacher.compi.analizador.controlCQL
{
    //recolecta
    class Acciones
    {
        String tipoVar = "";
        String useDB = "";
        List<String> nomids = new List<String>();
        /*
         * s-> LISTLAN;
         */
        public void recorreEntrada(ParseTreeNode raiz)
        {
            //S
            recorreLISTLAN(raiz.ChildNodes.ElementAt(0));
        }

        /*
         * LISTLAN.Rule =      MakePlusRule(LISTLAN,LANGUAGE);
          */
        private void recorreLISTLAN(ParseTreeNode listlan)
        {
            int num = listlan.ChildNodes.Count;
            for (int i=0; i<num; i++)
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
            if (language.ChildNodes.Count==5)
            {
                Symbol sim = new Symbol();
                sim.setAmbito("global");
                sim.setColumna(language.ChildNodes.ElementAt(0).Token.Location.Column);
                sim.setLinea(language.ChildNodes.ElementAt(0).Token.Location.Line);
                sim.setCuerpo(language.ChildNodes.ElementAt(4));
                sim.setNombre(language.ChildNodes.ElementAt(1).Token.Value.ToString());

                List<Symbol> parms = recorreParameters(language.ChildNodes.ElementAt(2), sim.getNombre());
                sim.parametros = parms;
                //sim.setRetornos()
                List<Symbol> retornos = recorreParameters(language.ChildNodes.ElementAt(3), sim.getNombre());
                foreach(Symbol retorno in retornos)  {
                    Retorno ret = new Retorno();
                    ret.setTipo(retorno.getTipo());
                    ret.setValor(retorno.getValor());
                    sim.retornos.Add(ret);
                }                 
                    
                sim.setRol(3);
                TablaSimbolos.symbolList.Add(sim);
            }
            else if (language.ChildNodes.Count==4)
            {
                Symbol sim = new Symbol();
                sim.setAmbito("global");
                sim.setColumna(language.ChildNodes.ElementAt(0).Token.Location.Column);
                sim.setLinea(language.ChildNodes.ElementAt(0).Token.Location.Line);
                sim.setCuerpo(language.ChildNodes.ElementAt(3));
                sim.setNombre(language.ChildNodes.ElementAt(1).Token.Value.ToString());
                sim.setTipo(recorretipo(language.ChildNodes.ElementAt(0)));
                sim.setRol(2);
                List<Symbol> parms = recorreParameters(language.ChildNodes.ElementAt(2), sim.getNombre());
                sim.parametros = parms;
                TablaSimbolos.symbolList.Add(sim);
            }
            else
            {
                recorreCQL(language.ChildNodes.ElementAt(0));
            }
        }

        /*
         * CQL.Rule =          create + INSTR
                                | TIPO + LVARIABLES + ASIGNA + ";"
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
                                | SENTENCIA
                                | PUNTOID + "(" + PARMVALS + ";"
                                | call + PUNTOID + "(" + PARMVALS + ";"
                                | returns + RETURNVAL
                                | cursor + "@" + id + iss + select + DATASELECT + froms + id + SELECTWHERE
                                | open + "@" + id + ";"
                                | close + "@" + id + ";"
                                | log + "(" + EXP + ")" + ";"
                                | throws + news + TYPEEXCEPTION + ";"
                                | trys + "{" + LCQL + "}" + catchs + "(" + TYPEEXCEPTION + id + ")" + "{" + LCQL + "}"
         *                      | PUNTOID + "." + get + "(" + VAL + ")"
                                | PUNTOID + "." + insert + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + set + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + remove + "(" + VAL + ")"
                                | PUNTOID + "." + clear + "(" + ")"
                                | PUNTOID + aumenta + ";"
                                | PUNTOID + disminuye + ";"
                                | PUNTOID + SIGNO + EXP + ";"
        */
        private void recorreCQL(ParseTreeNode cql)
        {
            String[] token1 = cql.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower()=="create") {

                //ira a recuperar los user types(edd)
                recorreinstr(cql.ChildNodes.ElementAt(1));
            }
            else if (token1[0].ToLower() == "tipo")
            {
                
                recorrelvariable(cql.ChildNodes.ElementAt(1));
                
                foreach(String nombre in nomids){
                    Symbol sim = new Symbol();
                    sim.setNombre(nombre);
                    sim.setAmbito("global");
                    sim.setColumna(0);
                    sim.setLinea(0);
                    sim.setCuerpo(cql.ChildNodes.ElementAt(2));
                    sim.setTipo(recorretipo(cql.ChildNodes.ElementAt(0)));
                    sim.setRol(1);
                    TablaSimbolos.symbolList.Add(sim);
                }
                nomids = new List<String>();
            }
            else if (token1[0].ToLower() == "alter")
            {

            }
            else if (token1[0].ToLower() == "use")
            {
                useDB = cql.ChildNodes.ElementAt(1).Token.Value.ToString();
            }
            else if (token1[0].ToLower() == "drop")
            {

            }
            else if (token1[0].ToLower() == "truncate")
            {

            }
            else if (token1[0].ToLower() == "commit")
            {

            }
            else if (token1[0].ToLower() == "rollback")
            {

            }
            else if (token1[0].ToLower() == "grant")
            {

            }
            else if (token1[0].ToLower() == "revoke")
            {

            }
            else if (token1[0].ToLower() == "sentdml")
            {

            }
            else if (token1[0].ToLower() == "select")
            {

            }
            else if (token1[0].ToLower() == "begin")
            {

            }
            else if (token1[0].ToLower() == "funagreg")
            {

            }
            else if (token1[0].ToLower() == "sentencia")
            {

            }
            else if (token1[0].ToLower() == "puntoid")
            {

            }
            else if (token1[0].ToLower() == "call")
            {
                
            }
            else if (token1[0].ToLower() == "returns")
            {

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

            }
            else if (token1[0].ToLower() == "throws")
            {

            }
            else if (token1[0].ToLower() == "trys")
            {

            }
            else
            {
                //error
            }
        }

        /*
         * LCQL.Rule =         MakePlusRule(LCQL, CQL);      
         */

        private void recorrelcql(ParseTreeNode lcql)
        {
            int num = lcql.ChildNodes.Count;
            for (int i=0; i<num; i++)
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
        private void recorreexception(ParseTreeNode exception)
        {
            String[] token1 = exception.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower()== "typealreadyexists") {

            }
            else if (token1[0].ToLower() == "typedontexists") {

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

            }
        }

        /*
         * RETURNVAL.Rule =    LVAL + ";"
                                | ";";
         */
        private void recorrereturnval(ParseTreeNode returnval) {
            if (returnval.ChildNodes.ElementAt(0).ToString() == "LVAL")
            {

            }
            else { 
            
            }
        }

        /*
         * CQLBREAK.Rule =     CQL
                                | breaks + ";";
         */
        private void recorrecqlbreak(ParseTreeNode cqlbreak) {
            if (cqlbreak.ChildNodes.ElementAt(0).ToString() == "CQL")
            {

            }
            else { 
            
            }
        } 

        /*
         * CQLCYCLE.Rule =     CQLBREAK
                                | continues + ";";
         */

        private void recorrecqlCycle(ParseTreeNode cqlcycle){
            if(cqlcycle.ChildNodes.ElementAt(0).ToString()=="CQLBREAK"){

            }else{
            
            }
        }

            /*LVAL.Rule =         MakePlusRule(LVAL, VAL);*/
        private void recorreLval(ParseTreeNode lval){
            foreach(ParseTreeNode val in lval.ChildNodes){
            
            }
        }
            
            /*LVARIABLES.Rule = MakePlusRule(LVARIABLES, ToTerm(","), IDENT);*/
        private void recorrelvariable(ParseTreeNode lvariable){
            foreach(ParseTreeNode variable in lvariable.ChildNodes){
                nomids.Add(recorreident(variable));
            }
        }

            /*
             * IDENT.Rule =        "@" + id
                                | id;
             */
        private String recorreident(ParseTreeNode ident){
            //siempre solo vendra el id
            return ident.ChildNodes.ElementAt(0).Token.Value.ToString();
        }

        /*  PARMVALS.Rule =     LVAL + ")" 
                                | ")";
         */
        private void recorreParmvals(ParseTreeNode parmVals){
            if(parmVals.ChildNodes.Count>0){
            
            }else{
            
            }
        }
        /*
         * PARAMETERS.Rule =   LPARMS + ")"
                                | ")";
         */
        private List<Symbol> recorreParameters(ParseTreeNode parameters, String ambito){
            if(parameters.ChildNodes.Count>0){
                return recorreLParms(parameters.ChildNodes.ElementAt(0), ambito);
            }else{
                return null;
            }
        }

        /*
         * LPARMS.Rule =       MakePlusRule(LPARMS, ToTerm(","), IDENTIFICADOR);
         */
        private List<Symbol> recorreLParms(ParseTreeNode lparms, String ambito){
            List<Symbol> data = new List<Symbol>();
            foreach(ParseTreeNode parms in lparms.ChildNodes){
                data.Add(recorreIdentificador(parms, ambito));
            }
            return data;
        }

            /*
             * IDENTIFICADOR.Rule = TIPO + "@" + id;
             */
        private Symbol recorreIdentificador(ParseTreeNode identificador, String ambito){
            Symbol s = new Symbol();
            s.setNombre(identificador.ChildNodes.ElementAt(1).Token.Value.ToString());
            s.setLinea(identificador.ChildNodes.ElementAt(1).Token.Location.Line);
            s.setColumna(identificador.ChildNodes.ElementAt(1).Token.Location.Column);
            s.setAmbito(ambito);
            s.setTipo(recorretipo(identificador.ChildNodes.ElementAt(0)));
            s.setIndex(TablaSimbolos.symbolList.Count);
            TablaSimbolos.symbolList.Add(s);
            return s;
        }

        /*
         * SENTENCIA.Rule =    IFS
                                | switchs + "(" + EXP + ")" + "{" + LCASES + DEFAULTS
                                | whiles + "(" + EXP + ")" + "{" + LCYCLE + "}"
                                | does + "{" + LCYCLE + "}" + whiles + "(" + EXP + ")" + ";"
                                | fors + FORES;
         */
        private void recorreSentencia(ParseTreeNode sentencia) {
            String[] token1 = sentencia.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "ifs") {

            }
            else if (token1[0].ToLower() == "switchs") {

            }
            else if (token1[0].ToLower() == "whiles") {

            }
            else if (token1[0].ToLower() == "does") {

            }
            else if (token1[0].ToLower() == "fors") { 
            
            }
        }

        /*
         * LCYCLE.Rule =       MakePlusRule(LCYCLE, CQLCYCLE);
         */
        private void recorrelcycle(ParseTreeNode lcycle){
            foreach(ParseTreeNode cycle in lcycle.ChildNodes){
            
            }
        }

        /*
         * LBREAKCQL.Rule =    MakePlusRule(LBREAKCQL, CQLBREAK);
         */
        private void recorrelbreakcql(ParseTreeNode lbreakcql){
            foreach(ParseTreeNode breakcql in lbreakcql.ChildNodes){
            
            }
        }

        /*
         * FORES.Rule =        "(" + EXP + ";" + EXP + ";" + EXP + ")" + "{" + LCYCLE + "}"
                                | each + "(" + LPARMS + ins + "@" + id + "{" + LCYCLE + "}";
         */
        private void recorreFores(ParseTreeNode fores){
            if(fores.ChildNodes.ElementAt(0).ToString()=="EXP"){
            
            }else{
            
            }
        }

        /*
         * LCASES.Rule =       MakePlusRule(LCASES, CASO); 
         */
        private void recorrelcases(ParseTreeNode lcases){
            foreach(ParseTreeNode cases in lcases.ChildNodes){
            
            }
        }

        /* 
         * CASO.Rule =         cases + EXP + ":" + LBREAKCQL; ;
         */
        private void recorreCaso(ParseTreeNode caso){
            
        }

        /*
         * DEFAULTS.Rule =     defaults + ":" + CQL + "}"
                                | "}";
         */
        private void recorreDefaults(ParseTreeNode defaults){
            if(defaults.ChildNodes.Count()>0){
            
            }
        }

        /*
         * IFS.Rule =          ifs + "(" + EXP + ")" + "{" + CQL + "}" + ELSEIFS + ELSE
                                | ifs + "(" + EXP + ")" + "{" + CQL + "}" + ELSEIFS
                                | ifs + "(" + EXP + ")" + "{" + CQL + "}" + ELSE
                                | ifs + "(" + EXP + ")" + "{" + CQL + "}";
         */
        private void recorreIfs(ParseTreeNode ifs){
            if(ifs.ChildNodes.Count==5){
            
            }else if(ifs.ChildNodes.Count==4){
                if(ifs.ChildNodes.ElementAt(3).ToString()=="ELSEIFS"){
                
                }else{
                
                }
            }else if(ifs.ChildNodes.Count==3){
            
            }
        }

            /*
             * ELSEIFS.Rule =      MakePlusRule(ELSEIFS, ELSEIFSES); 
             */
        private void recorreelseifs(ParseTreeNode elseifs){
            foreach(ParseTreeNode elseif in elseifs.ChildNodes){
            
            }
        }
        
        /*
         * ELSEIFSES.Rule =    elses + ifs + "(" + EXP + ")" + "{" + CQL + "}";
         */
        private void recorreelseifses(ParseTreeNode elseifses){
            
        }

        /*
         * ELSE.Rule =         elses + "{" + CQL + "}";
         */
        private void recorreelse(ParseTreeNode elses){
        
        }

        /*
         * FUNAGREG.Rule =     count
                                | min
                                | max
                                | sum
                                | avg;
         */
        private void recorrefunagreg(ParseTreeNode funagreg){
            String[] token1 = funagreg.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="count"){

            }else if(token1[0].ToLower()=="min"){
                
            }else if(token1[0].ToLower()=="max"){
            
            }else if(token1[0].ToLower()=="sum"){
            
            }else if(token1[0].ToLower()=="avg"){
            
            }
        }

        /*
         * LSENTDML.Rule =     MakePlusRule(LSENTDML, SENTDML);
         */
        private void recorrelsentdml(ParseTreeNode lsentdml){
            foreach(ParseTreeNode sentdml in lsentdml.ChildNodes){
            
            }
        }

        /*
         * SENTDML.Rule =      insert + into + id + INSERTDATA
                                | update + id + set + LASIGNACION + WHEREEXP
                                | delete + DELETEDATA;
         */
        private void recorresentdml(ParseTreeNode sentdml){
            String[] token1 = sentdml.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="insert"){
            
            }else if(token1[0].ToLower()=="update"){
            
            }else if(token1[0].ToLower()=="delete"){
            
            }
        }

        /*
         * SELECTWHERE.Rule =  where + EXP + EXPSELECT;
         */
        private void recorreselectwhere(ParseTreeNode selectwhere){
            
        }

        /*
         * SELECTWHEREVAL.Rule = where + EXP + EXPSELECTVAL;
         */
        private void recorredselectwhereval(ParseTreeNode selectwhereval){
        
        }

        /*
         * ORDERLIMIT.Rule =   order + by + LORDER + LIMITDATA
                                | LIMITDATA;
         */
        private void recorreorderlimit(ParseTreeNode orderlimit){
            if(orderlimit.ChildNodes.ElementAt(0).ToString()=="LIMITDATA"){
            
            }else{
            
            }
        }

        /*
         * ORDERLIMITVAL.Rule = order + by + LORDER + limit + EXP
                                | order + by + LORDER
                                | limit + EXP;
         */
        private void recorreorderlimitval(ParseTreeNode orderlimitval){
            String[] token1 = orderlimitval.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="limit"){
            
            }else{
                if(orderlimitval.ChildNodes.Count==5){
                
                }else{
                
                }
            }
        }

        /*
         * LORDER.Rule =       MakePlusRule(LORDER, ToTerm(","), ORDENA); 
         */
        private void recorrelorder(ParseTreeNode lorder){
            foreach(ParseTreeNode order in lorder.ChildNodes){
            
            }
        }

        /*
         * ORDENA.Rule =       id + ASCDESC;
         */
        private void recorreordena(ParseTreeNode ordena){
                
        }

        /*
         * ASCDESC.Rule =      asc
                                | desc;
         */
        private void recorreascdesc(ParseTreeNode ascdesc){
            String[] token1 = ascdesc.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="asc"){
            
            }else if(token1[0].ToLower()=="desc"){
            
            }
        }

         /*
          * LIMITDATA.Rule =    limit + EXP + ";"
                                | ";";
          */
        private void recorrelimitdata(ParseTreeNode limitdata){
            if(limitdata.ChildNodes.Count>0){
            
            }else{
            
            }
        }

        /*
         * DATASELECT.Rule =   LVAL
                                | ToTerm("*");
         */
        private void recorredataselect(ParseTreeNode dataselect){
            if(dataselect.ChildNodes.ElementAt(0).ToString().ToLower()=="lval"){
            
            }else{
            
            }
        }

        /*
         * LASIGNACION.Rule = MakePlusRule(LASIGNACION, ToTerm(","), TIPOASIG);
         */
        private void recorrelasignacion(ParseTreeNode lasignacion) { 
            foreach(ParseTreeNode asignacion in lasignacion.ChildNodes){
            
            }
        }

        /*
         * TIPOASIG.Rule =     id + "=" + VAL;
         */
        private void recorretipoasig(ParseTreeNode tipoasig){
            
        }

        /* 
         * DELETEDATA.Rule =   type + id + ";"
                                | froms + id + WHEREEXP;
         */
        private void recorredeletedata(ParseTreeNode deletedata){
            String[] token1 = deletedata.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="type"){
            
            }else{
            
            }
        }

            /*
             * wHEREEXP.Rule =     where + EXP + ";"
                                | ";";
             */
        private void recorredwhereexp(ParseTreeNode whereexp){
            if(whereexp.ChildNodes.Count>0){
            
            }else{
            
            }
        }

            /*
             * INSERTDATA.Rule =   values + "(" + LVAL + ")"
                                | "(" + LVAL + ")" + values + "(" + LVAL + ")";
             */
        private void recorreinsertdata(ParseTreeNode insertdata){
            if(insertdata.ChildNodes.ToString()=="LVAL"){
            
            }else{
            
            }
        }

            /*
             * LVALUSER.Rule =     MakePlusRule(LVALUSER, TIPOUSER);
             */
        private void recorrelvaluser(ParseTreeNode lvaluser, String ambitoAnt, String ambitoAct){
            foreach(ParseTreeNode valuser in lvaluser.ChildNodes){
                recorretipouser(valuser, ambitoAnt, ambitoAct);
            }
        }

            /*
             * TIPOUSER.Rule =     id + TIPO;
             */
        private void recorretipouser(ParseTreeNode tipouser, String ambitoAnt, String ambitoAct){
            Symbol sim = new Symbol();
            sim.setAmbitoAnt(ambitoAnt);
            sim.setAmbito(ambitoAct);
            sim.setColumna(tipouser.ChildNodes.ElementAt(0).Token.Location.Column);
            sim.setLinea(tipouser.ChildNodes.ElementAt(0).Token.Location.Line);
            sim.setTipo(recorretipo(tipouser.ChildNodes.ElementAt(1)));
            //sim.setCuerpo(language.ChildNodes.ElementAt(4));
            sim.setNombre(tipouser.ChildNodes.ElementAt(0).Token.Value.ToString());
            sim.setRol(1);
            TablaSimbolos.symbolList.Add(sim);
        }

            /*
             * ASIGNA.Rule =       ";"
                                | SIGNO + VAL + ";";
             */
        private void recorreasigna(ParseTreeNode asigna){
            if(asigna.ChildNodes.Count>0){
            
            }else{
            
            }
        }

           /*
            * SIGNO.Rule =        ToTerm("=")
                                | ToTerm("+=")
                                | ToTerm("-=")
                                | ToTerm("*=")
                                | ToTerm("/=");
            */

        private void recorresigno(ParseTreeNode signo){
            String[] token1 = signo.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="="){
            
            }
            else if(token1[0].ToLower()=="+="){
            
            }
            else if(token1[0].ToLower()=="-="){
            
            }
            else if(token1[0].ToLower()=="*="){
            
            }
            else if(token1[0].ToLower()=="/="){
            
            }
        }


        /*
         * CAMBIA.Rule =       add + "(" + LVALUSER + ")" + ";"
                                | delete + "(" + LIDS + ")" + ";";
         */
        private void recorrecambia(ParseTreeNode cambia){
            String[] token1 = cambia.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="add"){
            
            }
            else if(token1[0].ToLower()=="delete"){
            
            }
        }

          /*
           * LIDS.Rule =        MakePlusRule(LIDS, ToTerm(","), id);
           */
        private void recorrelids(ParseTreeNode lids){
            foreach(ParseTreeNode id in lids.ChildNodes){
            
            }
        }

        /*
         * INSTR.Rule =        type + INFODB + "(" + LVALUSER + ")" + ";"
                                | db + INFODB
                                | table + INFODB + "(" + LCOLUMS + ")" + ";"
                                | user + VAL + with + pass + VAL + ";";
         */
        private void recorreinstr(ParseTreeNode instr){
            String[] token1 = instr.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="type"){
                String nomtype = recorreInfodb(instr.ChildNodes.ElementAt(1), 6);
                if (nomtype != null) {
                    Symbol sim = new Symbol();
                    sim.setAmbito("global");
                    sim.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                    sim.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                    sim.setTipo("userType");
                    //sim.setCuerpo(language.ChildNodes.ElementAt(4));
                    sim.setNombre(nomtype);
                    sim.setRol(4);
                    TablaSimbolos.symbolList.Add(sim);
                    recorrelvaluser(instr.ChildNodes.ElementAt(2), sim.getAmbito(), sim.getNombre());
                }
            }
            else if(token1[0].ToLower()=="db"){
                String nomdb = recorreInfodb(instr.ChildNodes.ElementAt(1), 5);
                if(nomdb!=null){
                    Symbol sim = new Symbol();
                    sim.setAmbito("global");
                    sim.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                    sim.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                    sim.setTipo("db");
                    //sim.setCuerpo(language.ChildNodes.ElementAt(4));
                    sim.setNombre(nomdb);
                    sim.setRol(5);
                    TablaSimbolos.symbolList.Add(sim);
                }
            }
            else if(token1[0].ToLower()=="table"){
                String nomtab = recorreInfodb(instr.ChildNodes.ElementAt(1), 7);
                if(nomtab!=null){
                    if (nomtab != null)
                    {
                        Symbol sim = new Symbol();
                        sim.setAmbito(useDB);
                        sim.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                        sim.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                        sim.setTipo("table");
                        sim.setCuerpo(instr.ChildNodes.ElementAt(2));
                        sim.setNombre(nomtab);
                        sim.setRol(7);
                        TablaSimbolos.symbolList.Add(sim);
                    }
                }
            }
            else if(token1[0].ToLower()=="user"){
                Symbol sim = new Symbol();
                sim.setAmbito("global");
                sim.setColumna(instr.ChildNodes.ElementAt(0).Token.Location.Column);
                sim.setLinea(instr.ChildNodes.ElementAt(0).Token.Location.Line);
                sim.setTipo("user");
                sim.setCuerpo(instr.ChildNodes.ElementAt(1));
                sim.setPass(instr.ChildNodes.ElementAt(4));
                //sim.setNombre(nomtype);
                sim.setRol(6);
                TablaSimbolos.symbolList.Add(sim);
            }
        }

            /*
             * LCOLUMS.Rule =      MakePlusRule(LCOLUMS, COL); 
             */
        private void recorrelcolumns(ParseTreeNode lcolums){
            
        }

        /*
         * COL.Rule =          id + TIPO + primary + key
                                | id + TIPO
                                | primary + key + "(" + LIDS + ")";
         */
        private void recorrecol(ParseTreeNode col){
            if(col.ChildNodes.Count==4){
            
            }
            else if(col.ChildNodes.Count==3){
            
            }
            else if(col.ChildNodes.Count==5){
            
            }
        }

            //INFODB.Rule =       ifs + EXIST + id;
            //                    |id;
        private String recorreInfodb(ParseTreeNode infodb, int tipo){
            if (infodb.ChildNodes.Count>1)
            {
                String name = infodb.ChildNodes.ElementAt(2).Token.Value.ToString();
                bool existe = recorreexist(infodb.ChildNodes.ElementAt(1));
                foreach (Symbol s in TablaSimbolos.symbolList)
                {
                    if (s.getNombre().Equals(name) && s.getRol() == tipo)
                    {
                        existe = !existe;
                        break;
                    }
                }
                if (!existe)
                {
                    return name;
                }
                return null;
            }
            else {
                return infodb.ChildNodes.ElementAt(0).Token.Value.ToString();
            }
            
        }

           /*
            * EXIST.Rule =        not + exist
                                | exist;
            */
        private bool recorreexist(ParseTreeNode exist){
            if(exist.ChildNodes.Count==2){
                return false;
            }else {
                return true;
            }

        }

            /*
             * ALTERDATA.Rule =    type + id + CAMBIA
                                | table + id + ALTERTABLE;
             */
        private void recorrealterdata(ParseTreeNode alterdata){
            String[] token1 = alterdata.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="type"){
            
            }else{
            
            }
        }

            /*
             * ALTERTABLE.Rule =   add + LVALUSER + ";"
                                | drop + LIDS + ";";
             */
        private void recorrealterdate(ParseTreeNode alterdate){
            String[] token1 = alterdate.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="add"){
            
            }else{
            
            }
        }

            /*
             * DROPDATA.Rule =     table + INFODB
                                | db + id + ";";
             */
        private void recorredropdata(ParseTreeNode dropdata){
            String[] token1 = dropdata.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="table"){
            
            }else{
            
            }
        }

            /*
             * EXPSELECT.Rule =    EXP + ORDERLIMIT
                                | indato + EXP + ORDERLIMIT
                                | ORDERLIMIT;
             */
        private void recorreexpselect(ParseTreeNode expselect){
            if(expselect.ChildNodes.ElementAt(0).ToString()=="EXP"){
            
            }else if(expselect.ChildNodes.ElementAt(0).ToString()=="ORDERLIMIT"){
            
            }
            else{
            
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
        private void recorreexpselectval(ParseTreeNode expSelectVal){
            String[] token1 = expSelectVal.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(expSelectVal.ChildNodes.Count==1){
            
            }
            else if(expSelectVal.ChildNodes.Count==2){
                if(token1[0].ToLower()=="limit"){
                
                }else{
                
                }
            }
            else if(expSelectVal.ChildNodes.Count==3){
                if(token1[0].ToLower()=="exp"){
                
                }else{
                
                }
            }
            else if(expSelectVal.ChildNodes.Count==4){
                if(token1[0].ToLower()=="exp"){
                
                }else{
                
                }
            }
            else if(expSelectVal.ChildNodes.Count==5){
                if(token1[0].ToLower()=="order"){
                
                }else{
                
                }
            }
            else if(expSelectVal.ChildNodes.Count==6){
            
            }
            else if(expSelectVal.ChildNodes.Count==7){
            
            }
        }
            
        /*
         * TIPO.Rule =         PRIMITIVO
                                | map + "<" + PRIMITIVO + ">"
                                | list + "<" + PRIMITIVO + ">"
                                | set + "<" + PRIMITIVO + ">";
         */
        private String recorretipo(ParseTreeNode tipo){
            String[] token1 = tipo.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="set"){
                return "set";
            }
            else if(token1[0].ToLower()=="list"){
                return "list";
            }
            else if(token1[0].ToLower()=="map"){
                return "map";
            }
            else{
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
        private String recorreprimitivo(ParseTreeNode primitivo){
            String[] token1 = primitivo.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="int"){
                return "int";
            }else if(token1[0].ToLower()=="double"){
                return "double";
            }
            else if(token1[0].ToLower()=="string"){
                return "string";
            }
            else if(token1[0].ToLower()=="boolean"){
                return "boolean";
            }
            else if(token1[0].ToLower()=="date"){
                return "date";
            }
            else if(token1[0].ToLower()=="time"){
                return "time";
            }
            else{
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
        private void recorreexp(ParseTreeNode exp){
            String[] token1 = exp.ChildNodes.ElementAt(1).ToString().Split(' ');
            String[] token0 = exp.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="||"){
            
            }
            else if(token1[0].ToLower()=="||"){
            
            }
            else if(token1[0].ToLower()=="&&"){
            
            }
            else if(token1[0].ToLower()=="^"){
            
            }
            else if(token1[0].ToLower()=="!="){
            
            }
            else if(token1[0].ToLower()=="=="){
            
            } 
            else if(token1[0].ToLower()=="<"){
            
            }
            else if(token1[0].ToLower()==">"){
            
            }
            else if(token1[0].ToLower()==">="){
            
            }
            else if(token1[0].ToLower()=="<="){
            
            }
            else if(token1[0].ToLower()=="+"){
            
            }
            else if(token1[0].ToLower()=="-"){
            
            }
            else if(token1[0].ToLower()=="*"){
            
            }
            else if(token1[0].ToLower()=="/"){
            
            }
            else if(token1[0].ToLower()=="%"){
            
            } 
            else if(token1[0].ToLower()=="**"){
            
            }
            else if(token1[0].ToLower()=="++"){
            
            }
            else if(token1[0].ToLower()=="--"){
            
            }
            else if(token1[0].ToLower()=="<="){
            
            }
            else if(token0[0].ToLower()=="!"){
            
            }
            else if(token0[0].ToLower()=="-"){
            
            }
            else{
                if(exp.ChildNodes.ElementAt(0).ToString().ToLower()=="exp"){
                
                }else{
                    //val
                
                }
            }
        }

            /*
             * PUNTOID.Rule =      MakePlusRule(PUNTOID, ToTerm("."), IDES);
            
             */
        private void recorrepuntoid(ParseTreeNode puntoid){
            foreach(ParseTreeNode punto in puntoid.ChildNodes){
            
            }
        }

        /*
         * IDES.Rule =         id
                                | "@" + id;
         */
        private void recorreides(ParseTreeNode ides){
            
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
                                | PUNTOID
                                | "(" + TIPO + ")" + VAL
                                | ToTerm("null")
                                | select + DATASELECT + froms + id + SELECTWHEREVAL
                                | FUNAGREG + "(" + "<" + select + DATASELECT + froms + id + SELECTWHEREVAL + ">" + ")"
                                | PUNTOID + "." + "[" + VAL + "]"
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
        private void recorrevals(ParseTreeNode vals){
        
        }

        /*
         * LVALORES.Rule =     MakePlusRule(LVALORES, ToTerm(","), VALUES);
         */
        private void recorrelvalores(ParseTreeNode lvalores){
            foreach(ParseTreeNode valor in lvalores.ChildNodes){
            
            }
        }

        /*
         * VALUES.Rule =       VAL
                                | VAL + ":" + VAL;
         */
        private void recorrevalues(ParseTreeNode values) {
            if (values.ChildNodes.Count == 1)
            {

            }
            else { 
            
            }
        }


    }
}
