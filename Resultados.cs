using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQL_Teacher.compi.analizador.controlCQL
{
    class Resultados
    {
        String respCliente = "";
        String pestañasCHISON = "";
        List<Int32> accion;
        public List<Retorno> retornos = new List<Retorno>();
        public Boolean retornoInf;
        int isArray=0; // 0 -variable, 1- List, 2-Set, 3-Map 
        List<Resultados> contenido;
        String valor;
        String tipo;
        String clave;
        String tipoClave;

        public void setContenido(List<Resultados> content)
        {
            this.contenido = content;
        }

        public List<Resultados> getContenido()
        {
            return this.contenido;
        }

        public void settipoClave(String tipoKey)
        {
            this.tipoClave = tipoKey;
        }

        public String gettipoClave()
        {
            return this.tipoClave;
        }

        public void setClave(String key)
        {
            this.clave = key;
        }

        public String getClave()
        {
            return this.clave;
        }
                
        public void setIsArray(int isArreglo)
        {
            this.isArray = isArreglo;
        }

        public int getIsArray()
        {
            return this.isArray;
        }
        public void setValor(String val){
            this.valor = val;
        }
        public String getValor() {
            return this.valor;
        }

        public void setTipo(String type) {
            this.tipo = type;
        }
        public String getTipo() {
            return this.tipo;
        }

        
    }
}
