using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Ast;
using Irony.Parsing;
using WINGRAPHVIZLib;
using CQL_Teacher.compi.analizador.controlCQL;
using CQL_Teacher.compi.analizador.controlCHISON;
using Irony;
using System.Data;


namespace CQL_Teacher.compi.analizador
{
    class Sintactico: Grammar
    {
        public static String respuestaCLI = "";
        public static List<DataSet> basesDatos = new List<DataSet>();
        public static List<ManejoCHISON> backupdb = new List<ManejoCHISON>();
        public static int idUser = 1;
        public static int idRol = 1;
        public static String userLogged = "";

        public static bool analizarCHISON(String cadena) {
            gramaticaCHISON gramatica = new gramaticaCHISON();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;

            if (raiz == null)
            {
                for (int i = 0; i < arbol.ParserMessages.Count; i++)
                {
                    LogMessage error = arbol.ParserMessages[i];
                    string tipo = "";
                    if (error.Message.StartsWith("Invalid"))
                    {
                        tipo = "Error Lexico";
                    }
                    else if (error.Message.StartsWith("Syntax"))
                    {
                        tipo = "Error Sintactico";
                    }
                    Console.WriteLine(tipo + " | " + error.Location.Line + " | " + error.Location.Column + " | " + error.Message.Replace("Syntax error, expected", "Se esperaba").Replace("Invalid character", "Caracter invalido"));
                    Error e = new Error();
                    e.setLinea(error.Location.Line);
                    e.setColumna(error.Location.Column);
                    e.setTipo(tipo);
                    e.setMsj(error.Message.Replace("Syntax error, expected", "Se esperaba").Replace("Invalid character", "Caracter invalido"));
                    TablaSimbolos.errorList.Add(e);
                }
                MessageBox.Show("No se completo el arbol de analisis de sintaxis");
                //editor.MostrarErrores("CI");
                // }
                return false;
            }
            generarImagen(raiz);
            MessageBox.Show("Imagen generada correctamente");
            RecoleccionDB recolecta = new RecoleccionDB();
            //recolecta info
            recolecta.recorreEntrada(raiz);
            EjecucionDB ejecuta = new EjecucionDB();
            //ejecuta.recorreEntrada(raiz);
            return true;
        }

        public static bool analizarLUP(String cadena)
        {
            GramaticaLUP gramatica = new GramaticaLUP();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;

            if (raiz == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool analizarCQL(String cadena)
        {
            GramaticaCQL gramatica = new GramaticaCQL();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;

            if (raiz == null)
            {
                /*if (arbol.ParserMessages.Count == 0)
                {

                    DialogResult verImagen = MessageBox.Show("¿Desea visualizar el AST de la cadena ingresada?", "AST", MessageBoxButtons.YesNo);
                    if (verImagen == DialogResult.Yes)
                    {
                        Graficador graficador = new Graficador("AST", raiz, false);
                        graficador.GenerarImagen();
                    }
                }
                else
                {*/
                    for (int i = 0; i < arbol.ParserMessages.Count; i++)
                    {
                        LogMessage error = arbol.ParserMessages[i];
                        string tipo = "";
                        if (error.Message.StartsWith("Invalid"))
                        {
                            tipo = "Error Lexico";
                        }
                        else if (error.Message.StartsWith("Syntax"))
                        {
                            tipo = "Error Sintactico";
                        }
                        Console.WriteLine(tipo + " | " + error.Location.Line + " | " + error.Location.Column + " | " + error.Message.Replace("Syntax error, expected", "Se esperaba").Replace("Invalid character", "Caracter invalido"));
                        Error e = new Error();
                        e.setLinea(error.Location.Line);
                        e.setColumna(error.Location.Column);
                        e.setTipo(tipo);
                        e.setMsj(error.Message.Replace("Syntax error, expected", "Se esperaba").Replace("Invalid character", "Caracter invalido"));
                        TablaSimbolos.errorList.Add(e);
                    }
                    MessageBox.Show("No se completo el arbol de analisis de sintaxis");
                    //editor.MostrarErrores("CI");
               // }
                return false;
            }
            generarImagen(raiz);
            MessageBox.Show("Imagen generada correctamente");
            Acciones recolecta = new Acciones();
            //recolecta info
            recolecta.recorreEntrada(raiz);
            Ejecucion ejecuta = new Ejecucion();
            ejecuta.recorreEntrada(raiz);
            return true;

        }

        private static void generarImagen(ParseTreeNode raiz)
        {
            String grafoDOT = controlDOT.ControlDOT.getDOT(raiz);
            /*DOT dot = new DOT();
            BinaryImage img = dot.ToJPEG(grafoDOT);
            img.Save("AST.png");*/
            try
            {//Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\Users\\Gary\\Desktop\\Compi2\\AST.dot");//Write a line of text
                sw.WriteLine(grafoDOT);//Write a second line of text
                sw.Close();
            } catch(Exception e){
                Console.WriteLine("Exception: " + e.Message);
            } finally {
                Console.WriteLine("Executing finally block.");
            }
                ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\Graphviz 2.28\\bin\\dot.exe")
                {
                Arguments = "-Tpng C:\\Users\\Gary\\Desktop\\Compi2\\AST.dot -o  C:\\Users\\Gary\\Desktop\\Compi2\\AST.png"
                };
                Process.Start(startInfo);
                Thread.Sleep(3000);
                System.Diagnostics.Process.Start("C:\\Users\\Gary\\Desktop\\Compi2\\AST.png");
        }
    }
}
