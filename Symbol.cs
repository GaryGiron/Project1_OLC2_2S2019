using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace CQL_Teacher.compi.analizador.controlCQL
{
    class Symbol
    {
        int index;
        int linea;
        int columna;
        String tipo; //
        String ambitoAnt;
        int rol;//1-variable; 2-funcion; 3-procedimiento; 4-usertype; 5-db; 6-user; 7-table; 8-dbprocedure 9-collection
        ParseTreeNode cuerpo;
        ParseTreeNode pass;
        String ambito;
        String signo;
        String nombre;
        String valor;
        public List<Retorno> retornos;
        public List<Symbol> parametros;
        //collection
        int isArray = 0; ///0 - variable 1-List 2-Set 3-Map
        public int count;
        public List<Resultados> collection = new List<Resultados>();
        public List<String> clave = new List<String>();
        //columna
        Boolean primaryKey = false;
        public List<String> namesCol = new List<String>();

        public void setPrimaryKey(Boolean primaria)
        {
            this.primaryKey = primaria;
        }

        public Boolean getPrimaryKey()
        {
            return this.primaryKey;
        }

        public void setIsArray(int isArreglo)
        {
            this.isArray = isArreglo;
        }

        public int getIsArray()
        {
            return this.isArray;
        }

        public void setIndex(int index)
        {
            this.index = index;
        }

        public int getIndex()
        {
            return this.index;
        }

        public void setSigno(String sign) {
            this.signo = sign;
        }

        public String getSigno() {
            return this.signo;
        }

        public void setLinea(int line) {
            this.linea = line;
        }
        public int getLinea() {
            return this.linea;
        }

        public void setColumna(int col) {
            this.columna = col;
        }
        public int getColumna() {
            return this.columna;
        }

        public void setRol(int rols) {
            this.rol = rols;
        }
        public int getRol() {
            return this.rol;
        }

        public void setPass(ParseTreeNode pasw) {
            this.pass = pasw;
        }
        public ParseTreeNode getPass() {
            return this.pass;
        }

        public void setCuerpo(ParseTreeNode corp)
        {
            this.cuerpo = corp;
        }
        public ParseTreeNode getCuerpo()
        {
            return this.cuerpo;
        }

        public void setAmbito(String ambit) {
            this.ambito = ambit;
        }
        public String getAmbito() {
            return this.ambito;
        }

        public void setAmbitoAnt(String ambit)
        {
            this.ambitoAnt = ambit;
        }
        public String getAmbitoAnt()
        {
            return this.ambitoAnt;
        }

        public void setNombre(String name) {
            this.nombre = name;
        }
        public String getNombre() {
            return this.nombre;
        }

        public void setValor(String value) {
            this.valor = value;
        }
        public String getValor() {
            return this.valor;
        }

        public void setRetornos(List<Retorno> rets)
        {
            this.retornos = rets;
        }
        public List<Retorno> getRetornos()
        {
            return this.retornos;
        }

        public void setTipo(String type) {
            this.tipo = type;
        }
        public String getTipo() {
            return this.tipo;
        }
    }
}
