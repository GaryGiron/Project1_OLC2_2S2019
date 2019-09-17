using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace CQL_Teacher.compi.analizador
{
    class GramaticaCQL: Grammar
    {
        public GramaticaCQL() : base(caseSensitive: false)

        {
            #region TERMINAL
            //palabras reservadas
            var create = ToTerm("Create");
            var type = ToTerm("type");
            var not = ToTerm("not");
            var exist = ToTerm("exists");
            var news = ToTerm("new");
            var entero = ToTerm("int");
            var dec = ToTerm("double");
            var strings = ToTerm("string");
            var boolean = ToTerm("boolean");
            var date = ToTerm("date");
            var time = ToTerm("time");
            var usertype = ToTerm("User_type");
            var alter = ToTerm("alter");
            var add = ToTerm("add");
            var delete = ToTerm("delete");

            //Definicion de Datos (DDL)
            var db = ToTerm("database");
            var use = ToTerm("Use");
            var drop = ToTerm("drop");
            var counter = ToTerm("counter");
            var primary = ToTerm("primary ");
            var key = ToTerm("key");
            var map = ToTerm("map");
            var set = ToTerm("set");
            var list = ToTerm("list");
            var truncate = ToTerm("truncate");

            //Control de Transacciones (TCL)
            var commit = ToTerm("commit");
            var rollback = ToTerm("rollback");

            //Control de Datos (DCL)"
            var user = ToTerm("User");
            var with = ToTerm("with");
            var pass = ToTerm("password");
            var grant = ToTerm("grant");
            var on = ToTerm("on");
            var revoke = ToTerm("revoke");

            //Lenguaje Manipulacion de Datos (DML)
            var insert = ToTerm("insert");
            var into = ToTerm("into");
            var values = ToTerm("values");
            var update = ToTerm("update");
            var where = ToTerm("where");
            var froms = ToTerm("from");
            var table = ToTerm("table");
            var select = ToTerm("select");
            var indato = ToTerm("in");
            var order = ToTerm("order");
            var verdadero = ToTerm("true");
            var falso = ToTerm("falso");
            var by = ToTerm("by");
            var asc = ToTerm("asc");
            var desc = ToTerm("desc");
            var begin = ToTerm("begin");
            var batch = ToTerm("batch");
            var apply = ToTerm("apply");
            var count = ToTerm("count");
            var min = ToTerm("min");
            var max = ToTerm("max");
            var sum = ToTerm("sum");
            var avg = ToTerm("avg");
            var limit = ToTerm("limit");

            //Lenguaje de Control de flujo (FCL)
            var elses = ToTerm("else");
            var ifs = ToTerm("if");
            var switchs = ToTerm("switch");
            var cases = ToTerm("case");
            var defaults = ToTerm("default");
            var whiles = ToTerm("while");
            var does = ToTerm("do");
            var fors = ToTerm("for");
            var get = ToTerm("get");
            var remove = ToTerm("remove");
            var size = ToTerm("size");
            var clear = ToTerm("clear");
            var contain = ToTerm("contains");
            var returns = ToTerm("return");
            var breaks = ToTerm("break");
            var procedure = ToTerm("procedure");
            var call = ToTerm("call");
            var length = ToTerm("length  ");
            var upper = ToTerm("touppercase");
            var lower = ToTerm("tolowercase");
            var starts = ToTerm("startswith");
            var ends = ToTerm("endswith");
            var substr = ToTerm("substring");
            var year = ToTerm("getyear");
            var month = ToTerm("getmonth");
            var day = ToTerm("getday");
            var hour = ToTerm("gethour");
            var minute = ToTerm("getminuts   ");
            var second = ToTerm("getseconds");
            var today = ToTerm("today");
            var now = ToTerm("now");
            var continues = ToTerm("continue");
            var cursor = ToTerm("cursor");
            var iss = ToTerm("is");
            var each = ToTerm("each");
            var open = ToTerm("open");
            var close = ToTerm("close");
            var log = ToTerm("log");
            var throws = ToTerm("throw");
            var trys = ToTerm("try");
            var catchs = ToTerm("catch");
            var ins = ToTerm("in");
            var TypeAlreadyExists = ToTerm("TypeAlreadyExists");
            var TypeDontExists = ToTerm("TypeDontExists");
            var BDAlreadyExists = ToTerm("BDAlreadyExists");
            var BDDontExists = ToTerm("BDDontExists");
            var UseBDException = ToTerm("UseBDException");
            var TableAlreadyExists = ToTerm("TableAlreadyExists");
            var TableDontExists = ToTerm("TableDontExists");
            var CounterTypeException = ToTerm("CounterTypeException");
            var UserAlreadyExists = ToTerm("UserAlreadyExists");
            var UserDontExists = ToTerm("UserDontExists");
            var ValuesException = ToTerm("ValuesException");
            var ColumnException = ToTerm("ColumnException");
            var BatchException = ToTerm("BatchException");
            var IndexOutException = ToTerm("IndexOutException");
            var ArithmeticException = ToTerm("ArithmeticException");
            var NullPointerException = ToTerm("NullPointerException");
            var NumberReturnsException = ToTerm("NumberReturnsException");
            var FunctionAlreadyExists = ToTerm("FunctionAlreadyExists");
            var ProcedureAlreadyExists = ToTerm("ProcedureAlreadyExists");
            var ObjectAlreadyExists = ToTerm("ObjectAlreadyExists");

            //simbolos

            var or = ToTerm("||");
            var and = ToTerm("&&");
            var xor = ToTerm("^");
            var dif = ToTerm("!=");
            var igual = ToTerm("==");
            var menor = ToTerm("<");
            var mayor = ToTerm(">");
            var mayorigual = ToTerm(">=");
            var menorigual = ToTerm("<=");
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var div = ToTerm("/");
            var mod = ToTerm("%");
            var pot = ToTerm("**");
            var notsymbol = ToTerm("!");
            var aumenta = ToTerm("++");
            var disminuye = ToTerm("--");
            #endregion

            #region ER
            IdentifierTerminal id = new IdentifierTerminal("ID");
            //RegexBasedTerminal ids = new RegexBasedTerminal("id2", "@[" + id.ToString() + "]");
            RegexBasedTerminal fecha = new RegexBasedTerminal("fecha", "'[0-3][0-9]/[0-1][0-9]/[0-9][0-9]'");
            RegexBasedTerminal hora = new RegexBasedTerminal("hora", "[0-2][0-9]:[0-5][0-9]:[0-5][0-9]");
            //RegexBasedTerminal decimales = new RegexBasedTerminal("decimal", "[0-9]+.[0-9]+");
            NumberLiteral number = new NumberLiteral("number");
            StringLiteral cadena = new StringLiteral("cadena", "\"");
            /* se indica que la cadena va a empezar con " (comillas dobles) y con
            esto acepta cualquier cosa que venga despues de las comillas dobles */
            #endregion

            #region NO TERMINAL
            NonTerminal S = new NonTerminal("S");
            NonTerminal CQL = new NonTerminal("CQL");
            NonTerminal TYPEEXCEPTION = new NonTerminal("TYPEEXCEPTION");
            NonTerminal RETURNVAL = new NonTerminal("RETURNVAL");
            NonTerminal CQLBREAK = new NonTerminal("CQLBREAK");
            NonTerminal CQLCYCLE = new NonTerminal("CQLCYCLE");
            NonTerminal DECLARACION = new NonTerminal("DECLARACION");
            NonTerminal LVARIABLES = new NonTerminal("LVARIABLES");
            NonTerminal IDENT = new NonTerminal("IDENT");
            NonTerminal PARMVALS = new NonTerminal("PARMVALS");
            NonTerminal PARAMETERS = new NonTerminal("PARAMETERS");
            NonTerminal LPARMS = new NonTerminal("LPARMS");
            NonTerminal SENTENCIA = new NonTerminal("SENTENCIA");
            NonTerminal FORES = new NonTerminal("FORES");
            NonTerminal LCASES = new NonTerminal("LCASES");
            NonTerminal DEFAULTS = new NonTerminal("DEFAULTS");
            NonTerminal IFS = new NonTerminal("IFS");
            NonTerminal ELSEIFS = new NonTerminal("ELSEIFS");
            NonTerminal ELSE = new NonTerminal("ELSE");
            NonTerminal FUNAGREG = new NonTerminal("FUNAGREG");
            NonTerminal LSENTDML = new NonTerminal("LSENTDML");
            NonTerminal SENTDML = new NonTerminal("SENTDML");
            NonTerminal SELECTWHERE = new NonTerminal("SELECTWHERE");
            NonTerminal SELECTWHEREVAL = new NonTerminal("SELECTWHEREVAL");
            NonTerminal ORDERLIMIT = new NonTerminal("ORDERLIMIT");
            NonTerminal ORDERLIMITVAL = new NonTerminal("ORDERLIMITVAL");
            NonTerminal LORDER = new NonTerminal("LORDER");
            NonTerminal ASCDESC = new NonTerminal("ASCDESC");
            NonTerminal LIMITDATA = new NonTerminal("LIMITDATA");
            NonTerminal DATASELECT = new NonTerminal("DATASELECT");
            NonTerminal LASIGNACION = new NonTerminal("LASIGNACION");
            NonTerminal DELETEDATA = new NonTerminal("DELETEDATA");
            NonTerminal WHEREEXP = new NonTerminal("WHEREEXP");
            NonTerminal INSERTDATA = new NonTerminal("INSERTDATA");
            NonTerminal LVALUSER = new NonTerminal("LVALUSER");
            NonTerminal TRUNCATEDATA = new NonTerminal("TRUNCATEDATA");
            NonTerminal ASIGNA = new NonTerminal("ASIGNA");
            NonTerminal SIGNO = new NonTerminal("SIGNO");
            NonTerminal CAMBIA = new NonTerminal("CAMBIA");
            NonTerminal INSTR = new NonTerminal("INSTR");
            NonTerminal LCOLUMS = new NonTerminal("LCOLUMS");
            NonTerminal COL = new NonTerminal("COL");
            NonTerminal INFODB = new NonTerminal("INFODB");
            NonTerminal EXIST = new NonTerminal("EXIST");
            NonTerminal ALTERDATA = new NonTerminal("ALTERDATA");
            NonTerminal ALTERTABLE = new NonTerminal("ALTERTABLE");
            NonTerminal DROPDATA = new NonTerminal("DROPDATA");
            NonTerminal EXPSELECT = new NonTerminal("EXPSELECT");
            NonTerminal EXPSELECTVAL = new NonTerminal("EXPSELECTVAL");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal PRIMITIVO = new NonTerminal("PRIMITIVO");
            NonTerminal EXP = new NonTerminal("EXP");
            NonTerminal PUNTOID = new NonTerminal("PUNTOID");
            NonTerminal VAL = new NonTerminal("VAL");
            NonTerminal LVALORES = new NonTerminal("LVALORES");
            NonTerminal VALUES = new NonTerminal("VALUES");
            NonTerminal LCQL = new NonTerminal("LCQL");
            NonTerminal LISTLAN = new NonTerminal("LISTLAN");
            NonTerminal LANGUAGE = new NonTerminal("LANGUAGE");
            NonTerminal LVAL = new NonTerminal("LVAL");
            NonTerminal LCYCLE = new NonTerminal("LCYCLE");
            NonTerminal LBREAKCQL = new NonTerminal("LBREAKCQL");
            NonTerminal LIDS = new NonTerminal("LIDS");
            NonTerminal IDENTIFICADOR = new NonTerminal("IDENTIFICADOR");
            NonTerminal CASO = new NonTerminal("CASO");
            NonTerminal ELSEIFSES = new NonTerminal("ELSEIFSES");
            NonTerminal ORDENA = new NonTerminal("ORDENA");
            NonTerminal TIPOASIG = new NonTerminal("TIPOASIG");
            NonTerminal TIPOUSER = new NonTerminal("TIPOUSER");
            NonTerminal IDES = new NonTerminal("IDES");
            #endregion

            #region COMMENT
            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n"); //si viene una nueva linea se termina de reconocer el comentario.
            CommentTerminal comentarioBloque = new CommentTerminal("comentarioBloque", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioBloque);
            #endregion


            #region GRAMATICA
            S.Rule =            LISTLAN;

            LISTLAN.Rule =      MakePlusRule(LISTLAN,LANGUAGE);

            LANGUAGE.Rule =     procedure + id + "(" + PARAMETERS + "," + "(" + PARAMETERS + "{" + LCQL + "}"
                                | TIPO + id + "(" + PARAMETERS + "{" + LCQL + "}"
                                | CQL;

            CQL.Rule =          create + INSTR
                                | TIPO + LVARIABLES + ASIGNA
                                | alter + ALTERDATA
                                | use + id + ";"
                                | drop + DROPDATA
                                | truncate + table + id + ";"
                                | commit + ";"//falta
                                | rollback + ";"//falta
                                | grant + id + on + id + ";"
                                | revoke + id + on + id + ";"
                                | SENTDML
                                | select + DATASELECT + froms + id + SELECTWHERE//falta
                                | begin + batch + LSENTDML + apply + batch + ";"//falta
                                | FUNAGREG + "(" + "<" + select + DATASELECT + froms + id + SELECTWHEREVAL + ">" + ")" + ";"//falta
                                | FUNAGREG + "(" + "<" + select + DATASELECT + froms + id + ">" + ")" + ";"//falta
                                | IFS
                                | switchs + "(" + EXP + ")" + "{" + LCASES + DEFAULTS
                                | whiles + "(" + EXP + ")" + "{" + LCYCLE + "}"
                                | does + "{" + LCYCLE + "}" + whiles + "(" + EXP + ")" + ";"
                                | fors + FORES
                                | PUNTOID + "(" + PARMVALS + ";"
                                | call + PUNTOID + "(" + PARMVALS + ";"
                                | returns + RETURNVAL
                                | cursor + "@" + id + iss + select + DATASELECT + froms + id + SELECTWHERE//falta
                                | open + "@" + id + ";"//falta
                                | close + "@" + id + ";"//falta
                                | log + "(" + EXP + ")" + ";"
                                | throws + news + TYPEEXCEPTION + ";"//falta
                                | trys + "{" + LCQL + "}" + catchs + "(" + TYPEEXCEPTION + id + ")" + "{" + LCQL + "}"//falta
                                | PUNTOID + "." + insert + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + insert + "(" + EXP + ")"
                                | PUNTOID + "." + set + "(" + VAL + "," + EXP + ")"
                                | PUNTOID + "." + remove + "(" + VAL + ")"
                                | PUNTOID + "." + clear + "(" + ")"
                                | PUNTOID + aumenta + ";"
                                | PUNTOID + disminuye + ";"
                                | PUNTOID + SIGNO + EXP + ";"
                                | PUNTOID + "[" + EXP + "]" + SIGNO + EXP + ";"
                               ;
            CQL.ErrorRule =     SyntaxError + ";";
            CQL.ErrorRule =     SyntaxError + "}";

            LCQL.Rule =         MakePlusRule(LCQL, CQL);         
                
            TYPEEXCEPTION.Rule = TypeAlreadyExists
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
                                | ObjectAlreadyExists;

            RETURNVAL.Rule =    LVAL + ";"
                                | ";";

            CQLBREAK.Rule =     CQL
                                | breaks + ";";

            CQLCYCLE.Rule =     CQLBREAK
                                | continues + ";";

            LVAL.Rule =         MakePlusRule(LVAL, EXP);

            LVARIABLES.Rule = MakePlusRule(LVARIABLES, ToTerm(","), IDENT); 

            IDENT.Rule =        "@" + id
                                | id;

            PARMVALS.Rule =     LVAL + ")" 
                                | ")";
            PARMVALS.ErrorRule = SyntaxError + ")";
        
            PARAMETERS.Rule =   LPARMS + ")"
                                | ")";
            PARAMETERS.ErrorRule = SyntaxError + ")";

            LPARMS.Rule =       MakePlusRule(LPARMS, ToTerm(","), IDENTIFICADOR);

            IDENTIFICADOR.Rule = TIPO + "@" + id;

            /*SENTENCIA.Rule =    
            SENTENCIA.Rule =    SyntaxError + ")";*/

            LCYCLE.Rule =       MakePlusRule(LCYCLE, CQLCYCLE);

            LBREAKCQL.Rule =    MakePlusRule(LBREAKCQL, CQLBREAK);

            FORES.Rule =        "(" + TIPO + IDENT + SIGNO + EXP + ";" + EXP + ";" + EXP + ")" + "{" + LCYCLE + "}"
                                | each + "(" + LPARMS + ins + "@" + id + "{" + LCYCLE + "}";

            LCASES.Rule =       MakePlusRule(LCASES, CASO); 

            CASO.Rule =         cases + EXP + ":" + LBREAKCQL;

            DEFAULTS.Rule =     defaults + ":" + LBREAKCQL + "}"
                                | "}";

            IFS.Rule =          ifs + "(" + EXP + ")" + "{" + LCQL + "}" + ELSEIFS + ELSE
                                | ifs + "(" + EXP + ")" + "{" + LCQL + "}" + ELSEIFS
                                | ifs + "(" + EXP + ")" + "{" + LCQL + "}" + ELSE
                                | ifs + "(" + EXP + ")" + "{" + LCQL + "}";

            ELSEIFS.Rule =      MakePlusRule(ELSEIFS, ELSEIFSES); 

            ELSEIFSES.Rule =    elses + ifs + "(" + EXP + ")" + "{" + LCQL + "}";

            ELSE.Rule =         elses + "{" + LCQL + "}";

            FUNAGREG.Rule =     count
                                | min
                                | max
                                | sum
                                | avg;

            LSENTDML.Rule =     MakePlusRule(LSENTDML, SENTDML);

            SENTDML.Rule =      insert + into + id + INSERTDATA
                                | update + id + set + LASIGNACION + WHEREEXP
                                | delete + DELETEDATA;

            SELECTWHERE.Rule =  where + EXP + EXPSELECT;

            SELECTWHEREVAL.Rule = where + EXP + EXPSELECTVAL;

            ORDERLIMIT.Rule =   order + by + LORDER + LIMITDATA
                                | LIMITDATA;

            ORDERLIMITVAL.Rule = order + by + LORDER + limit + EXP
                                | order + by + LORDER
                                | limit + EXP;

            LORDER.Rule =       MakePlusRule(LORDER, ToTerm(","), ORDENA); 

            ORDENA.Rule =       id + ASCDESC;

            ASCDESC.Rule =      asc
                                | desc;

            LIMITDATA.Rule =    limit + EXP + ";"
                                | ";";

            DATASELECT.Rule =   LVAL
                                | ToTerm("*");

            LASIGNACION.Rule = MakePlusRule(LASIGNACION, ToTerm(","), TIPOASIG);

            TIPOASIG.Rule =     id + "=" + VAL;

            DELETEDATA.Rule =   type + id + ";"
                                | froms + id + WHEREEXP;

            WHEREEXP.Rule =     where + EXP + ";"
                                | ";";

            INSERTDATA.Rule =   values + "(" + LVAL + ")"
                                | "(" + LVAL + ")" + values + "(" + LVAL + ")";

            LVALUSER.Rule =     MakePlusRule(LVALUSER, TIPOUSER);

            TIPOUSER.Rule =     id + TIPO;

            ASIGNA.Rule =       ";"
                                | SIGNO + EXP + ";";

            SIGNO.Rule =        ToTerm("=")
                                | ToTerm("+=")
                                | ToTerm("-=")
                                | ToTerm("*=")
                                | ToTerm("/=");


            CAMBIA.Rule =       add + "(" + LVALUSER + ")" + ";"
                                | delete + "(" + LIDS + ")" + ";";

            LIDS.Rule =         MakePlusRule(LIDS, ToTerm(","), id);

            INSTR.Rule =        type + id + "(" + LVALUSER + ")" + ";"
                                | db + INFODB + ";"
                                | table + INFODB + "(" + LCOLUMS + ")" + ";"
                                | user + VAL + with + pass + VAL + ";";

            LCOLUMS.Rule =      MakePlusRule(LCOLUMS, COL); 

            COL.Rule =          id + TIPO + primary + key
                                | id + TIPO
                                | primary + key + "(" + LIDS + ")";

            INFODB.Rule =       ifs + EXIST + id
                                |id;

            EXIST.Rule =        not + exist
                                | exist;

            ALTERDATA.Rule =    type + id + CAMBIA
                                | table + id + ALTERTABLE;

            ALTERTABLE.Rule =   add + LVALUSER + ";"
                                | drop + LIDS + ";";

            DROPDATA.Rule =     table + INFODB + ";"
                                | db + INFODB + ";";

            EXPSELECT.Rule =    EXP + ORDERLIMIT
                                | indato + EXP + ORDERLIMIT
                                | ORDERLIMIT;

            EXPSELECTVAL.Rule = EXP + order + by + LORDER + limit + EXP
                                | EXP + order + by + LORDER
                                | EXP + limit + EXP
                                | EXP
                                | indato + EXP + order + by + LORDER + limit + EXP
                                | indato + EXP + order + by + LORDER
                                | indato + EXP + limit + EXP
                                | indato + EXP
                                | order + by + LORDER + limit + EXP
                                | order + by + LORDER
                                | limit + EXP;

            TIPO.Rule =         PRIMITIVO
                                | map + "<" + PRIMITIVO + "," + PRIMITIVO + ">"
                                | list + "<" + PRIMITIVO + ">"
                                | set + "<" + PRIMITIVO + ">";

            PRIMITIVO.Rule =    entero
                                | dec
                                | strings
                                | boolean
                                | date
                                | time
                                | id;

            EXP.Rule =          EXP + or + EXP
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

            PUNTOID.Rule =      MakePlusRule(PUNTOID, ToTerm("."), IDES);
            
            IDES.Rule =         id
                                | "@" + id;

            VAL.Rule =          number
                                | cadena
                                //| decimales
                                | verdadero
                                | falso
                                | fecha
                                | hora
                                | news + TIPO
                                | "{" + LVALORES + "}"
                                | PUNTOID
                                | "(" + TIPO + ")" + VAL
                                | ToTerm("null")
                                | select + DATASELECT + froms + id + SELECTWHEREVAL
                                | FUNAGREG + "(" + "<" + select + DATASELECT + froms + id + SELECTWHEREVAL + ">" + ")"
                                | PUNTOID + ToTerm(".") + "[" + EXP + "]"
                                | PUNTOID + "[" + EXP + "]"//
                                | "[" + LVALORES + "]"
                                | "(" + EXP + ")" + "?" + EXP + ":" + EXP 
                                | PUNTOID + "." + size + "(" + ")"
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
                                | PUNTOID + "." + get + "(" + VAL + ")"
                                | today + "(" + ")"
                                | now + "(" + ")";


            LVALORES.Rule =     MakePlusRule(LVALORES, ToTerm(","), VALUES);

            VALUES.Rule =       EXP
                                | VAL + ":" + EXP;


            #endregion

            #region PREFERENCIA
            this.Root = S;
            this.RegisterOperators(11, Associativity.Neutral, aumenta, disminuye);
            this.RegisterOperators(10, Associativity.Right, menos, notsymbol);
            this.RegisterOperators(9, Associativity.Left, pot);
            this.RegisterOperators(8, Associativity.Left, por, div, mod);
            this.RegisterOperators(7, Associativity.Left, mas, menos);
            this.RegisterOperators(6, Associativity.Neutral, menor, mayor, menorigual, mayorigual);
            this.RegisterOperators(5, Associativity.Left, igual, dif);
            this.RegisterOperators(4, Associativity.Left, xor);
            this.RegisterOperators(3, Associativity.Left, and);
            this.RegisterOperators(2, Associativity.Left, or);
            this.MarkPunctuation("(", ")", ",", "{", "}", "@", ":", ";", "?");
            #endregion
        }
    }
}
