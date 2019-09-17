using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using CQL_Teacher.compi.analizador.controlCQL;

namespace CQL_Teacher.compi.analizador.controlCHISON
{
    class RecoleccionDB
    {

        //s -> chison
        public void recorreEntrada(ParseTreeNode raiz)
        {
            //S
        }

        /*CHISON.Rule = inicio + "\"" + bdds + "\"" + "=" + "[" + LDBS + "," + "\""+ usr + "\""
                + "=" + "[" + LUSRS + fin;
         * */
        public void recorrechison(ParseTreeNode chison) { 
            
        }

        /*LDBS.Rule = "]"
                | LDB + "]";
         */
        public void recorreLdbs(ParseTreeNode ldbs) { 
            if(ldbs.ChildNodes.Count>0){
            
            }else{
            
            }
        }

        /*LDB.Rule = MakePlusRule(LDB, ToTerm(","), LOTHERDB);*/
        public void recorreldb(ParseTreeNode ldb) { 
            foreach(ParseTreeNode db in ldb.ChildNodes){
            
            }
        }

        /*LOTHERDB.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + dato
                + "\"" + "=" + "[" + LDATADBS + ">"
                | importini + LIDS + importfin;
         */
        public void recorreLotherddb(ParseTreeNode lotherdb) {
            if (lotherdb.ChildNodes.Count == 4)
            {

            }
            else { 
            
            }
        }

        /* LIDS.Rule = MakePlusRule(LIDS, ToTerm("."), id);*/
        public void recorrelids(ParseTreeNode lids) {
            foreach (ParseTreeNode id in lids.ChildNodes) { 
            
            }
        }

        /* LUSRS.Rule = LUSR + "]"
                | "]";
         */
        public void recorrelusrs(ParseTreeNode lusrs) {
            if (lusrs.ChildNodes.Count > 0)
            {

            }
            else { 
            
            }
        }

        /*LUSR.Rule = MakePlusRule(LUSR, ToTerm(","), LOTHERUSR);*/
        public void recorrelusr(ParseTreeNode lusr) {
            foreach (ParseTreeNode usr in lusr.ChildNodes) { 
            
            }
        }

        /*LOTHERUSR.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + pass + "\""
                + VALUES + "," + "\"" + perm + "\"" + "=" + "[" + LPERMS + ">"
                | importini + LIDS + importfin;
         */
        public void recorrelotherusr(ParseTreeNode lotherusr) {
            if (lotherusr.ChildNodes.Count == 3)
            {

            }
            else { 
            
            }
        }

        /* LPERMS.Rule = LPERM + "]"
                | "]";
         */
        public void recorrelperms(ParseTreeNode lperms) {
            if (lperms.ChildNodes.Count > 0)
            {

            }
            else { 
            
            }
        }

        /*¨LPERM.Rule = MakePlusRule(LPERM, ToTerm(","), LOTHERPERM);*/
        public void recorrelperm(ParseTreeNode lperm) {
            foreach (ParseTreeNode perm in lperm.ChildNodes) { 
            
            }
        }

        /* LOTHERPERM.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + ">"
                | importini + LIDS + importfin;
         */
        public void recorrelotherperm(ParseTreeNode lotherperm) {
            if (lotherperm.ChildNodes.Count == 3)
            {

            }
            else { 
            
            }
        }

        /*LDATADBS.Rule = LDATADB + "]"
                | "]";
         */
        public void recorreldatadbs(ParseTreeNode ldatadbs) {
            if (ldatadbs.ChildNodes.Count > 0)
            {

            }
            else { 
            
            }
        }

        /*LDATADB.Rule = MakePlusRule(LDATADB, ToTerm(","), LDATA);*/
        public void recorreldatadb(ParseTreeNode ldatadb) {
            foreach (ParseTreeNode datadb in ldatadb.ChildNodes) { 
            
            }
        }

        /*LDATA.Rule = importini + LIDS + importfin
                | "<" + "\"" + cqltype + "\"" + "=" + "\"" + INFO;
         */
        public void recorreldata(ParseTreeNode ldata) {
            if (ldata.ChildNodes.Count == 3)
            {

            }
            else { 
            
            }
        }

        /* INFO.Rule = TABLES
                | OBJECTS
                | PROCEDURES;
         */
        public void recorreinfo(ParseTreeNode info) { 
            String[] token1 = info.ChildNodes.ElementAt(0).ToString().Split(' ');
            if(token1[0].ToLower()=="tables"){

            }
            else if (token1[0].ToLower() == "objects")
            {

            }
            else { 
            
            }
        }

        /*TABLES.Rule = table + "\"" + "," + "\"" + name + "\""
                + "=" + "\"" + id + "\"" + "," + "\"" + col + "\"" + "=" + "[" + LCOLS + "," + "\"" + dato
                + "\"" + "=" + "[" + LDATATABS + ">";
         */
        public void recorretables(ParseTreeNode tables) { 
        
        }

        /* LCOLS.Rule = LCOLUM + "]"
                | "]";
         */
        public void recorrelcols(ParseTreeNode lcols) {
            if (lcols.ChildNodes.Count > 0)
            {

            }
            else { 
            
            }
        }

        /* LCOLUM.Rule = MakePlusRule(LCOLUM, ToTerm(","), LCOL); */
        public void recorrelcolum(ParseTreeNode lcolum) {
            foreach (ParseTreeNode lcol in lcolum.ChildNodes) { 
            
            }
        }

        /*LCOL.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + type + "\"" + "="
                + "\"" + TIPOS + "\"" + "," + "\"" + pk + "\"" + "=" + BOOLS + ">"
                | importini + LIDS + importfin;
         */
        public void recorrelcol(ParseTreeNode lcol) {
            if (lcol.ChildNodes.Count == 3)
            {

            }
            else { 
            
            }
        }

        /*LDATATABS.Rule = "<" + LDATATAB + ">" + "]"
                | "]";
         */
        public void recorreldatatabs(ParseTreeNode ldatatabs) {
            if (ldatatabs.ChildNodes.Count > 0)
            {

            }
            else { 
            
            }
        }

        /*LDATATAB.Rule = MakePlusRule(LDATATAB, ToTerm(","), DATATAB);*/
        public void recorreldatatab(ParseTreeNode ldatatab) { 
            foreach(ParseTreeNode datatab in ldatatab.ChildNodes){
            
            }
        }

        /*DATATAB.Rule = "\"" + id + "\"" + "=" + VALUES
                | importini + LIDS + importfin;
         */
        public void recorredatatab(ParseTreeNode datatab) {
            if (datatab.ChildNodes.Count == 3)
            {

            }
            else { 
            
            }
        }

        /*OBJECTS.Rule = obj + "\"" + "," + "\"" + name + "\""
                + "=" + "\"" + id + "\"" + "," + "\"" + attr + "\"" + "=" + "[" + LATTRS + ">";*/
        public void recorreobjects(ParseTreeNode objects) { 
        
        }

        /* LATTRS.Rule = LATTR + "]"
                | "]";
         */
        public void recorrelattrs(ParseTreeNode lattrs) {
            if (lattrs.ChildNodes.Count > 0)
            {

            }
            else { 
            
            }
        }

        /*LATTR.Rule = MakePlusRule(LATTR, ToTerm(","), LATT);*/
        public void recorrelattr(ParseTreeNode lattr) {
            foreach (ParseTreeNode attr in lattr.ChildNodes) { 
            
            }
        }

        /*LATT.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + type + "\"" + "="
                + "\"" + TIPOS + "\"" + ">"
                | importini + LIDS + importfin;
         */
        public void recorrelatt(ParseTreeNode latt) {
            if (latt.ChildNodes.Count == 3)
            {

            }
            else { 
            
            }
        }

        /*PROCEDURES.Rule = proc + "\"" + "," + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" +
                param + "\"" + "=" + "[" + LPARMS + "," + "\"" + instr + "\"" + "=" + tcadena + ">";*/
        public void recorreprocedures(ParseTreeNode procedures) { 
            
        }

        /*LPARMS.Rule = LPARM + "]"
                | "]";
         */
        public void recorrelparms(ParseTreeNode lparms) {
            if (lparms.ChildNodes.Count > 0)
            {

            }
            else { 
            
            }
        }

        /*LPARM.Rule = MakePlusRule(LPARM, ToTerm(","), LPROC);*/
        public void recorrelparm(ParseTreeNode lparm) {
            foreach (ParseTreeNode parm in lparm.ChildNodes) { 
            
            }
        }

        /*LPROC.Rule = importini + LIDS + importfin
                | "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + type + "\"" + "=" + "\"" + TIPOS + "\"" + ","
                + "\"" + ass + "\"" + "=" + FORM + ">";
         */
        public void recorrelproc(ParseTreeNode lproc) {
            if (lproc.ChildNodes.Count == 3)
            {

            }
            else { 
            
            }
        }

        /*FORM.Rule = dentro
                | fuera;
         */
        public void recorreform(ParseTreeNode form) {
            String[] token1 = form.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "dentro")
            {

            }
            else { 
            
            }
        }

        /*VALUES.Rule = MakePlusRule(VALUES, EXP);
         */
        public void recorrevalues(ParseTreeNode values) {
            foreach (ParseTreeNode exp in values.ChildNodes) { 
            
            }
        }

        /*EXP.Rule = EXP + "||" + EXP
                                | EXP + "&&" + EXP
                                | EXP + "^" + EXP
                                | EXP + "<>" + EXP
                                | EXP + "==" + EXP
                                | EXP + "<" + EXP
                                | EXP + ">" + EXP
                                | EXP + ">=" + EXP
                                | EXP + "<=" + EXP
                                | EXP + "+" + EXP
                                | EXP + "-" + EXP
                                | EXP + "*" + EXP
                                | EXP + "/" + EXP
                                | EXP + "%" + EXP
                                | EXP + "**" + EXP
                                | "not" + EXP
                                | "-" + EXP
                                | EXP + "++"
                                | EXP + "--"
                                | ToTerm("(") + EXP + ToTerm(")")
                                | VAL;
         
        public void recorreexp(ParseTreeNode exp) {
            if (exp.ChildNodes.Count == 1)
            {//val

            }
            else {
                String[] token1 = exp.ChildNodes.ElementAt(1).ToString().Split(' ');
                if(token1[0]=="||"){
                
                }
                else if (token1[0] == "&&")
                {

                }
                else if (token1[0] == "^")
                {

                }
                else if (token1[0] == "<>")
                {

                }
                else if (token1[0] == "==")
                {

                }
                else if (token1[0] == "<")
                {

                }
                else if (token1[0] == ">")
                {

                }
                else if (token1[0] == "<=")
                {

                }
                else if (token1[0] == ">=")
                {

                }
                else if (token1[0] == "+")
                {

                }
                else if (token1[0] == "-")
                {

                }
                else if (token1[0] == "*")
                {

                }
                else if (token1[0] == "/")
                {

                }
                else if (token1[0] == "%")
                {

                }
                else if (token1[0] == "**")
                {

                }
                else if (token1[0] == "++")
                {

                }
                else if (token1[0] == "--")
                {

                }
                else {
                    String[] token0 = exp.ChildNodes.ElementAt(0).ToString().Split(' ');
                    if (token0[0] == "not")
                    {

                    }
                    else if (token0[0] == "-")
                    {

                    }
                    else { 
                    
                    }
                }
            }
        }*/
        private Resultados recorreexp(ParseTreeNode exp)
        {
            if (exp.ChildNodes.Count == 1)
            {
                Resultados res = recorreval(exp.ChildNodes.ElementAt(0));
                return res;
            }
            else
            {
                String[] token1 = exp.ChildNodes.ElementAt(1).ToString().Split(' ');
                String[] token0 = exp.ChildNodes.ElementAt(0).ToString().Split(' ');
                if (token1[0].ToLower() == "||")
                {
                    Resultados res1 = recorreexp(exp.ChildNodes.ElementAt(0));
                    Resultados res2 = recorreexp(exp.ChildNodes.ElementAt(2));
                    if (res1.getTipo() == "boolean" && res2.getTipo() == "boolean")
                    {
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

                    if (res1.getTipo() == "int" && res2.getTipo() == "double" || res1.getTipo() == "double" && res2.getTipo() == "int" ||
                        res1.getTipo() == "double" && res2.getTipo() == "double" || res1.getTipo() == "int" && res2.getTipo() == "int" ||
                        res1.getTipo() == "string" && res2.getTipo() == "string" || res1.getTipo() == "boolean" && res2.getTipo() == "boolean" ||
                        res1.getTipo() == "date" && res2.getTipo() == "date" || res1.getTipo() == "time" && res2.getTipo() == "time")
                    {
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((res1.getValor() != res2.getValor()).ToString());
                        return res;
                    }
                    Error e = new Error();
                    e.setLinea(exp.ChildNodes.ElementAt(1).Token.Location.Line);
                    e.setColumna(exp.ChildNodes.ElementAt(1).Token.Location.Column);
                    e.setTipo("Semantico");
                    e.setMsj("No se logro hacer la operacion, la combinacion de tipos no esta permitida : " + res1.getTipo() + " != " + res2.getTipo());
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
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor() + ": " + excep);
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
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
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son numerico y dice que si : " + res1.getValor() + " != " + res2.getValor() + ": " + excep);
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
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor() + ": " + excep);
                            TablaSimbolos.errorList.Add(e);
                        }
                        Resultados res = new Resultados();
                        res.setTipo("boolean");
                        res.setValor((num1 < num2).ToString());
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
                            e.setMsj("No se logro hacer la operacion, los tipos de dato no son los esperados : " + res1.getValor() + " != " + res2.getValor() + ": " + excep);
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
                        res.setValor((Math.Pow(num1, num2)).ToString());
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
                            res.setValor((num1 + 1).ToString());
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

        /*VAL.Rule = number
                | id
         *      | tcadena
                | BOOLS;
         */
        public Resultados recorreval(ParseTreeNode val) {
            String[] token1 = val.ChildNodes.ElementAt(0).ToString().Split(' ');
            Resultados res = new Resultados();
            if (token1[0].ToLower() == "number")
            {   
                res.setValor(val.ChildNodes.ElementAt(0).Token.Value.ToString());
                res.setTipo("int");
                
            }
            else if (token1[0].ToLower() == "id")
            {

            }
            else { 
            
            }
            return res;
        }

        /*TIPOS.Rule = datoint
                | datostring
                | datodouble
                | datodate
                | datotime
                | datobool
                | datocounter;
         */
        public void recorretipos(ParseTreeNode tipos) {
            String[] token1 = tipos.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "datoint")
            {

            }
            else if (token1[0].ToLower() == "datostring")
            {

            }
            else if (token1[0].ToLower() == "datodouble")
            {

            }
            else if (token1[0].ToLower() == "datodate")
            {

            }
            else if (token1[0].ToLower() == "datotime")
            {

            }
            else if (token1[0].ToLower() == "datobool")
            {

            }
            else
            {//datocounter

            }
        }

        /*BOOLS.Rule = verdad
                | falso;
         */
        public void recorrebools(ParseTreeNode bools) { 
            String[] token1 = bools.ChildNodes.ElementAt(0).ToString().Split(' ');
            if (token1[0].ToLower() == "verdad")
            {

            }
            else { 
            
            }
        }
    }
}
