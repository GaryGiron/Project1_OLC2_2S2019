using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CQL_Teacher.compi.analizador
{
    class gramaticaCHISON : Grammar
    {
        public gramaticaCHISON() : base(caseSensitive: false)

        {
            #region TERMINAL
            var inicio = ToTerm("$<");
            var fin = ToTerm(">$");
            var importini = ToTerm("${");
            var importfin = ToTerm("}$");
            var bdds = ToTerm("DATABASES");
            var usr = ToTerm("USERS");
            var name = ToTerm("NAME");
            var dato = ToTerm("DATA");
            var pass = ToTerm("PASSWORD");
            var perm = ToTerm("PERMISSIONS");
            var cqltype = ToTerm("CQL-TYPE");
            var table = ToTerm("TABLE");
            var col = ToTerm("COLUMNS");
            var type = ToTerm("TYPE");
            var pk = ToTerm("PK");
            var verdad = ToTerm("TRUE");
            var falso = ToTerm("FALSE");
            var obj = ToTerm("OBJECT");
            var attr = ToTerm("ATTRS");
            var proc = ToTerm("PROCEDURE");
            var param = ToTerm("PARAMETERS");
            var instr = ToTerm("INSTR");
            var ass = ToTerm("AS");
            var dentro = ToTerm("IN");
            var fuera = ToTerm("OUT");
            var datoint = ToTerm("Int");
            var datodouble = ToTerm("Double");
            var datostring = ToTerm("String");
            var datodate = ToTerm("Date");
            var datotime = ToTerm("Time");
            var datobool = ToTerm("Boolean");
            var datocounter = ToTerm("Counter");
            var lista = ToTerm("List");
            var mapa = ToTerm("Map");
            var set = ToTerm("Set");
            #endregion

            #region ER
            //RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[0-9]+");
            //RegexBasedTerminal ID = new RegexBasedTerminal("ID", "[A-Za-z_@][A-Za-z0-9_@]*");
            NumberLiteral number = new NumberLiteral("number");
            IdentifierTerminal id = new IdentifierTerminal("ID");
            StringLiteral tcadena = new StringLiteral("cadena", "$");
            /* se indica que la cadena va a empezar con " (comillas dobles) y con
            esto acepta cualquier cosa que venga despues de las comillas dobles */
            #endregion

            #region NO TERMINAL
            NonTerminal S = new NonTerminal("S");
            NonTerminal CHISON = new NonTerminal("CHISON");
            NonTerminal INFO = new NonTerminal("INFO");
            NonTerminal LDBS = new NonTerminal("LDBS");
            NonTerminal LDB = new NonTerminal("LDB");
            NonTerminal LOTHERDB = new NonTerminal("LOTHERDB");
            NonTerminal LUSRS = new NonTerminal("LUSRS");
            NonTerminal LUSR = new NonTerminal("LUSR");

            NonTerminal LIDS = new NonTerminal("LIDS");
            NonTerminal LOTHERUSR = new NonTerminal("LOTHERUSR");
            NonTerminal LPERMS = new NonTerminal("LPERMS");
            NonTerminal LPERM = new NonTerminal("LPERM");
            NonTerminal LOTHERPERM = new NonTerminal("LOTHERPERM");
            NonTerminal LDATADBS = new NonTerminal("LDATADBS");
            NonTerminal LDATADB = new NonTerminal("LDATADB");
            NonTerminal LDATA = new NonTerminal("LDATA");

            NonTerminal TABLES = new NonTerminal("TABLES");
            NonTerminal LCOLS = new NonTerminal("LCOLS");
            NonTerminal LCOL = new NonTerminal("LCOL");
            NonTerminal LCOLUM = new NonTerminal("LCOLUM");
            NonTerminal LDATATABS = new NonTerminal("LDATATABS");
            NonTerminal LDATATAB = new NonTerminal("LDATATAB");
            NonTerminal OBJECTS = new NonTerminal("OBJECTS");
            NonTerminal LATTRS = new NonTerminal("LATTRS");

            NonTerminal LATTR = new NonTerminal("LATTR");
            NonTerminal LATT = new NonTerminal("LATT");
            NonTerminal PROCEDURES = new NonTerminal("PROCEDURES");
            NonTerminal LPARMS = new NonTerminal("LPARMS");
            NonTerminal LPARM = new NonTerminal("LPARM");
            NonTerminal LPROC = new NonTerminal("LPROC");
            NonTerminal FORM = new NonTerminal("FORM");
            NonTerminal VALUES = new NonTerminal("VALUES");
            NonTerminal TIPOS = new NonTerminal("TIPOS");
            NonTerminal BOOLS = new NonTerminal("BOOLS");
            NonTerminal DATATAB = new NonTerminal("DATATAB");
            NonTerminal VAL = new NonTerminal("VAL");
            NonTerminal EXP = new NonTerminal("EXP");
            #endregion

            #region COMMENT
            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n"); //si viene una nueva linea se termina de reconocer el comentario.
                CommentTerminal comentarioBloque = new CommentTerminal("comentarioBloque", "/*", "*/");
                NonGrammarTerminals.Add(comentarioLinea);
                NonGrammarTerminals.Add(comentarioBloque);
            #endregion


            #region GRAMATICA
            S.Rule = CHISON;
            CHISON.Rule = inicio + "\"" + bdds + "\"" + "=" + "[" + LDBS + "," + "\""+ usr + "\""
                + "=" + "[" + LUSRS + fin;
            LDBS.Rule = "]"
                | LDB + "]";

            LDB.Rule = MakePlusRule(LDB, ToTerm(","), LOTHERDB);

            LOTHERDB.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + dato
                + "\"" + "=" + "[" + LDATADBS + ">"
                | importini + LIDS + importfin;

            LIDS.Rule = MakePlusRule(LIDS, ToTerm("."), id);
                
            LUSRS.Rule = LUSR + "]"
                | "]";

            LUSR.Rule = MakePlusRule(LUSR, ToTerm(","), LOTHERUSR);
                
            LOTHERUSR.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + pass + "\""
                + VALUES + "," + "\"" + perm + "\"" + "=" + "[" + LPERMS + ">"
                | importini + LIDS + importfin;

            LPERMS.Rule = LPERM + "]"
                | "]";

            LPERM.Rule = MakePlusRule(LPERM, ToTerm(","), LOTHERPERM);
                
            LOTHERPERM.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + ">"
                | importini + LIDS + importfin;

            LDATADBS.Rule = LDATADB + "]"
                | "]";

            LDATADB.Rule = MakePlusRule(LDATADB, ToTerm(","), LDATA);
                
            LDATA.Rule = importini + LIDS + importfin
                | "<" + "\"" + cqltype + "\"" + "=" + "\"" + INFO;

            INFO.Rule = TABLES
                | OBJECTS
                | PROCEDURES;

            TABLES.Rule = table + "\"" + "," + "\"" + name + "\""
                + "=" + "\"" + id + "\"" + "," + "\"" + col + "\"" + "=" + "[" + LCOLS + "," + "\"" + dato
                + "\"" + "=" + "[" + LDATATABS + ">";

            LCOLS.Rule = LCOLUM + "]"
                | "]";

            LCOLUM.Rule = MakePlusRule(LCOLUM, ToTerm(","), LCOL);

            LCOL.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + type + "\"" + "="
                + "\"" + TIPOS + "\"" + "," + "\"" + pk + "\"" + "=" + BOOLS + ">"
                | importini + LIDS + importfin;

            LDATATABS.Rule = "<" + LDATATAB + ">" + "]"
                | "]";

            LDATATAB.Rule = MakePlusRule(LDATATAB, ToTerm(","), DATATAB);
                
            DATATAB.Rule = "\"" + id + "\"" + "=" + EXP
                | importini + LIDS + importfin;

            OBJECTS.Rule = obj + "\"" + "," + "\"" + name + "\""
                + "=" + "\"" + id + "\"" + "," + "\"" + attr + "\"" + "=" + "[" + LATTRS + ">";

            LATTRS.Rule = LATTR + "]"
                | "]";

            LATTR.Rule = MakePlusRule(LATTR, ToTerm(","), LATT);
                
            LATT.Rule = "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + type + "\"" + "="
                + "\"" + TIPOS + "\"" + ">"
                | importini + LIDS + importfin;

            PROCEDURES.Rule = proc + "\"" + "," + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" +
                param + "\"" + "=" + "[" + LPARMS + "," + "\"" + instr + "\"" + "=" + tcadena + ">";

            LPARMS.Rule = LPARM + "]"
                | "]";

            LPARM.Rule = MakePlusRule(LPARM, ToTerm(","), LPROC);
                
            LPROC.Rule = importini + LIDS + importfin
                | "<" + "\"" + name + "\"" + "=" + "\"" + id + "\"" + "," + "\"" + type + "\"" + "=" + "\"" + TIPOS + "\"" + ","
                + "\"" + ass + "\"" + "=" + FORM + ">";

            FORM.Rule = dentro
                | fuera;

            VALUES.Rule = MakePlusRule(VALUES, EXP);

            EXP.Rule = EXP + "||" + EXP
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
            
            VAL.Rule = number
                | id
                | tcadena
                | BOOLS;

            TIPOS.Rule = datoint
                | datostring
                | datodouble
                | datodate
                | datotime
                | datobool
                | datocounter;

            BOOLS.Rule = verdad
                | falso;
            #endregion

            #region PREFERENCIA
            this.Root = S;
            this.MarkPunctuation("[", "]", "<", ">", ",", "=", "\"", ".");
            #endregion
        }
    }
}
