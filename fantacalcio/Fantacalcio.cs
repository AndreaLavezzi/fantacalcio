using System;
using System.Collections.Generic;
using System.Text;

namespace fantacalcio
{
    //classe che rappresenta una partita, ha un nome, una lista di giocatori registrati e un metodo che ritorna la lista di giocatori

    class Fantacalcio
    {
        public string nomeSalvataggio { get; }      //nome del salvataggio che verrà assegnato al file di salvataggio
        public int fase { get; set; }  //fase 0 -> appena creata, fase 1 -> asta finita, fase 2 -> 
        List<Giocatore> giocatori = new List<Giocatore>();      //lista contenente i giocatori registrati nella partita corrente  

        public Fantacalcio(string nomeSalvataggio, List<Giocatore> giocatori, int fase)   //metodo costruttore, ottiene in ingresso il nome del salvataggio e la lista dei giocatori registrati
        {
            this.nomeSalvataggio = nomeSalvataggio;
            this.giocatori = giocatori;
            this.fase = fase;
        }

        public string GetListaGiocatori()
        {
            string stringaGiocatori = "";
            for (int i = 0; i < giocatori.Count; i++)
            {
                stringaGiocatori += "\n" + (i + 1) + " -> " + giocatori[i].ToString();
            }
            return stringaGiocatori;
        }

        public List<Giocatore> GetGiocatori()   //metodo pubblico che ritorna la lista dei giocatori registrati nella partita corrente
        {
            return giocatori;
        }
    }
}
