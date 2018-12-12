using System;
namespace ServipagMobile.Classes
{
    public class Automata
    {

        public int id_banco;
        public string actionName;
        public string usuario;
        public string tx;
        public string tipo;
        public string email;
        public string tipovalidacionrut;
        public string Nombreparametrorut;
        public string cuenta;

        public Automata(int id_banco,
         string actionName,
         string usuario,
         string tx,
         string tipo,
         string email,
         string tipovalidacionrut,
         string Nombreparametrorut,
         string cuenta)
        {

            this.id_banco = id_banco;
            this.actionName = actionName;
            this.usuario = usuario;
            this.tx = tx;
            this.tipo = tipo;
            this.email = email;
            this.tipovalidacionrut = tipovalidacionrut;
            this.Nombreparametrorut = Nombreparametrorut;
            this.cuenta = cuenta;

        }
    }
}
