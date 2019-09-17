using CQL_Teacher.compi.analizador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CQL_Teacher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public DataSet basesys = new DataSet("System");

        DataTable tableuser = new DataTable("User");

        DataTable tablerol = new DataTable("Rol");
        
        public void analisis()
        {
            //bool resultado = Sintactico.analizarCHISON("");//enviar cadena de entrada
            //bool resultado = Sintactico.analizarLUP("");
                        
            tableuser.Columns.Add(new DataColumn("user", Type.GetType("System.String")));
            tableuser.Columns.Add(new DataColumn("pass", Type.GetType("System.String")));
            basesys.Tables.Add(tableuser);

            tablerol.Columns.Add(new DataColumn("user", Type.GetType("System.String")));
            tablerol.Columns.Add(new DataColumn("db", Type.GetType("System.String")));
            basesys.Tables.Add(tablerol);

            DataRow userAdmin = tableuser.NewRow();
            userAdmin["user"] = "admin";
            userAdmin["pass"] = "admin";
            tableuser.Rows.Add(userAdmin);

            DataRow rolAdmin = tablerol.NewRow();
            rolAdmin["user"] = "admin";
            rolAdmin["db"] = "System";
            tablerol.Rows.Add(rolAdmin);
            
            Sintactico.basesDatos.Add(basesys);
            bool resultado = Sintactico.analizarCQL("List<int> persona = {2,3,4,5};");
            if (resultado)
            {
                Console.WriteLine("Resultado exitoso");  
            }
            else
            {
                Console.WriteLine("No funciono");
            }
        }
    }
}
