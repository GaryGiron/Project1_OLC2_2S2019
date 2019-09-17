using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;
namespace CQL_Teacher.compi.analizador
{
    class GramaticaLUP: Grammar
    {
        public GramaticaLUP() : base(caseSensitive: false)

        {
            #region TERMINAL
            var login = ToTerm("LOGIN");
            var logout = ToTerm("LOGOUT");
            var usr = ToTerm("USER");
            var pass = ToTerm("PASS");
            var success = ToTerm("SUCESS");
            var fail = ToTerm("FAIL");
            var query = ToTerm("QUERY");
            var data = ToTerm("DATA");
            var msg = ToTerm("MESSAGE");
            var error = ToTerm("ERROR");
            var line = ToTerm("LINE");
            var col = ToTerm("COLUMN");
            var cols = ToTerm("COLUMNS");
            var type = ToTerm("TYPE");
            var desc = ToTerm("DESC");
            var semantico = ToTerm("Semantico");
            var sintactico = ToTerm("Sintactico");
            var lexico = ToTerm("Lexico");
            var structs = ToTerm("STRUCT");
            var dbs = ToTerm("DATABASES");
            var db = ToTerm("DATABASE");
            var name = ToTerm("NAME");
            var table = ToTerm("TABLES");
            var proc = ToTerm("PROCEDURES");
            var attr = ToTerm("ATTRIBUTES");
            #endregion

            #region ER
            NumberLiteral number = new NumberLiteral("number");
            IdentifierTerminal id = new IdentifierTerminal("ID");
            StringLiteral cadena = new StringLiteral("cadena", "$#");
            /* $# JSDJGLSDLKSDAFJKL $# */
            #endregion

            #region NO TERMINAL
            NonTerminal S = new NonTerminal("S");
            NonTerminal LUPS = new NonTerminal("LUPS");
            NonTerminal LUP = new NonTerminal("LUP");
            NonTerminal LOGIN = new NonTerminal("LOGIN");
            NonTerminal LOGOUT = new NonTerminal("LOGOUT");
            NonTerminal QUERY = new NonTerminal("QUERY");
            NonTerminal DATA = new NonTerminal("DATA");
            NonTerminal MESSAGE = new NonTerminal("MESSAGE");
            NonTerminal ERROR = new NonTerminal("ERROR");

            NonTerminal STRUCT = new NonTerminal("STRUCT");
            NonTerminal VALUES = new NonTerminal("VALUES");
            NonTerminal RESP = new NonTerminal("RESP");
            NonTerminal TYPEERROR = new NonTerminal("TYPEERROR");
            NonTerminal DATABASE = new NonTerminal("DATABASE");
            NonTerminal LDATABASE = new NonTerminal("LDATABASE");
            NonTerminal DBASE = new NonTerminal("DBASE");
            NonTerminal LINFODB = new NonTerminal("LINFODB");

            NonTerminal INFODB = new NonTerminal("INFODB");
            NonTerminal LTABLAS = new NonTerminal("LTABLAS");
            NonTerminal LTYPE = new NonTerminal("LTYPE");
            NonTerminal LIDS = new NonTerminal("LIDS");
            NonTerminal TABLAS = new NonTerminal("TABLAS");
            NonTerminal TYPES = new NonTerminal("TYPES");
            NonTerminal VAL = new NonTerminal("VAL");
            #endregion

            #region COMMENT
            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n"); //si viene una nueva linea se termina de reconocer el comentario.
            CommentTerminal comentarioBloque = new CommentTerminal("comentarioBloque", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioBloque);
            #endregion


            #region GRAMATICA
            S.Rule = LUPS;

            LUPS.Rule = LOGIN
                | LOGOUT
                | QUERY
                | DATA
                | MESSAGE
                | ERROR
                | STRUCT
                | DATABASE;

            LOGIN.Rule = "[" + "+" + login + "]" + "[" + "+" + usr + "]" + VALUES + "[" + "-" + usr + "]" + "[" + "+" + pass + "]" + VALUES + "[" + "-"
                + pass + "]" + "[" + "-" + login + "]"
                | "[" + "+" + login + "]" + RESP + "[" + "-" + login + "]";

            LOGOUT.Rule = "[" + "+" + logout + "]" + "[" + "+" + usr + "]" + VALUES + "[" + "-" + usr + "]" + "[" + "-" + logout + "]"
                | "[" + "+" + logout + "]" + RESP + "[" + "-" + logout + "]";

            QUERY.Rule = "[" + "+" + query + "]" + "[" + "+" + usr + "]" + VALUES + "[" + "-" + usr + "]" + DATA + "[" + "-" + query + "]";

            DATA.Rule = "[" + "+" + data + "]" + cadena + "[" + "-" + data + "]";

            MESSAGE.Rule = "[" + "+" + msg + "]" + VALUES + "[" + "-" + msg + "]";

            ERROR.Rule = "[" + "+" + error + "]" + "[" + "+" + line + "]" + number + "[" + "-" + line + "]" + "[" + "+" + col + "]" + number +
                "[" + "-" + col + "]" + "[" + "+" + type + "]" + TYPEERROR + "[" + "-" + type + "]" + "[" + "+" + desc + "]" + VALUES + "[" + "-" + desc + "]"
                + "[" + "-" + error + "]";

            TYPEERROR.Rule = semantico
                | sintactico
                | lexico;

            STRUCT.Rule = "[" + "+" + structs + "]" + "[" + "+" + usr + "]" + VALUES + "[" + "-" + usr + "]" + "[" + "-" + structs + "]";

            DATABASE.Rule = "[" + "+" + dbs + "]" + LDATABASE + "[" + "-" + dbs + "]";

            LDATABASE.Rule = LDATABASE + DBASE
                | DBASE;

            DBASE.Rule = "[" + "+" + db + "]" + "[" + "+" + name + "]" + id + "[" + "-" + name + "]" + LINFODB + "[" + "-" + db + "]";

            LINFODB.Rule = LINFODB + INFODB
                | INFODB;

            INFODB.Rule = "[" + "+" + table + "]" + LTABLAS + "[" + "-" + table + "]"
                | "[" + "+" + type + "]" + LTYPE + "[" + "-" + type + "]"
                | "[" + "+" + proc + "]" + LIDS + "[" + "-" + proc + "]";

            LTABLAS.Rule = LTABLAS + TABLAS
                | TABLAS;

            TABLAS.Rule = "[" + "+" + table + "]" + "[" + "+" + name + "]" + VALUES + "[" + "-" + name + "]" + "[" + "+" + cols + "]" + VALUES + "[" + "-"
                + cols + "]" + "[" + "-" + table + "]"
                | "[" + "+" + table + "]" + VALUES + "[" + "-" + table + "]";

            LTYPE.Rule = LTYPE + TYPES
                | TYPES;

            TYPES.Rule = "[" + "+" + type + "]" + "[" + "+" + name + "]" + VALUES + "[" + "-" + name + "]" + "[" + "+" + attr + "]" + VALUES + "[" + "-"
                + attr + "]" + "[" + "-" + type + "]"
                | "[" + "+" + type + "]" + VALUES + "[" + "-" + type + "]";

            RESP.Rule = success
                | fail;

            VALUES.Rule = VALUES + VAL
                | VAL;

            VAL.Rule = id
                | number;

            LIDS.Rule = LIDS + id
                | id;

            #endregion

            #region PREFERENCIA
            this.Root = S;
            #endregion
        }
    }
}
