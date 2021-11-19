/**
 * \file    Giocatore.cs
 * \author  Sandstorm
 * \brief   Sistema di gestione del gioco del FANTACALCIO
 * \date    18/10/2021
 */
using System.Collections.Generic;

namespace fantacalcio
{
    /**
     * \class   Giocatore
     * \brief   Rappresenta i giocatori che parteciperanno al torneo.
     */
    class Giocatore
    {
        /// \brief Il nome del giocatore. Non può contenere caratteri speciali, non può essere più lungo di 12 caratteri o più corto di 4.
        public string nome { get; }

        /// \brief Il punteggio del giocatore. Partita vinta: +3 punti, partita persa: 0 punti, partita pareggiata: +1 punto.
        public int punteggio { get; set; }

        /// \brief I crediti con cui il giocatore può acquistare i calciatori
        public int fantaMilioni { get; set; }

        /// \brief Indica se la partita che sta venendo giocata dal giocatore è la sua prima partita del torneo.
        public bool primaPartita { get; set; }

        /// \brief I calciatori posseduti dal giocatore
        List<Calciatore> rosa = new List<Calciatore>();

        /// \brief La lista di calciatori titolati scelti dal giocatore
        List<Calciatore> titolari = new List<Calciatore>();

        /// \brief La squadra che verrà usata in partita dal giocatore
        List<Calciatore> squadra = new List<Calciatore>(); 

        /**
         * \brief Metodo costruttore, riceve in input il nome e imposta i crediti iniziali a 500 e il valore di primaPartita a true.
         */
        public Giocatore(string nome)
        {
            this.nome = nome;
            fantaMilioni = 500;
            primaPartita = true;
        }

        /**
         * \fn      public void CaricaLista(List<Calciatore> calciatori)
         * \brief   Carica una lista di calciatori nella rosa del giocatore
         * \param   List<Calciatore> calciatori: La lista di calciatori che verrà impostata come valore alla rosa dei calciatori del giocatore
         */
        public void CaricaLista(List<Calciatore> calciatori)
        {
            rosa = calciatori;
        }

        /**
         * \fn      public void AddCalciatore(Calciatore calciatore, int prezzo)
         * \brief   Permette al giocatore di acquistare un calciatore per aggiungerlo alla rosa dei calciatori
         * \param   Calciatore calciatore: Il calciatore acquistato
         * \param   int prezzo: Il prezzo a cui il calciatore è stato acquistato
         * \details Viene aggiunto il calciatore acquistato alla rosa del giocatore tramite il metodo Add delle liste. Viene poi sottratto al numero di crediti che il giocatore possiede il prezzo a cui il calciatore è stato comprato.
         */
        public void AddCalciatore(Calciatore calciatore, int prezzo)
        {
            rosa.Add(calciatore);
            fantaMilioni -= prezzo;
        }

        /**
         * \fn      public void AddPunteggio(int punti)
         * \brief   Aggiunge al punteggio del giocatore il valore ottenuto come parametro
         * \param   int punti: I punti assegnati al giocatore in base al risultato della partita
         */
        public void AddPunteggio(int punti)
        {
            punteggio += punti;
        }

        /**
         * \fn      public List<Calciatore> GetRosa()
         * \brief   Ritorna la lista contenente la rosa di calciatori del giocatore
         * \return  List<Calciatore>: Il metodo ritorna la lista di calciatori che rappresenta la rosa del giocatore
         */
        public List<Calciatore> GetRosa()
        {
            return rosa;
        }

        /**
         * \fn      public List<Calciatore> GetTitolari()
         * \brief   Ritorna la lista  di calciatori titolari del giocatore
         * \return  List<Calciatore>: Il metodo ritorna la lista di calciatori titolari del giocatore
         */
        public List<Calciatore> GetTitolari()
        {
            return titolari;
        }

        /**
         * \fn      public void AddTitolari(Calciatore calciatore)
         * \brief   Aggiunge alla lista di calciatori titolari il calciatore che viene passato come parametro
         * \param   Calciatore calciatore: Il calciatore da aggiungere alla lista di titolari
         */
        public void AddTitolari(Calciatore calciatore)
        {
            titolari.Add(calciatore);
        }

        /**
         * \fn      public void SetSquadraAttuale(List<Calciatore> squadra)
         * \brief   Imposta il valore della squadra che il giocatore userà nella prossima partita pari a quello della lista ottenuta come parametro
         * \param   List<Calciatore> squadra: La squadra che il giocatore userà nella prossima partita
         */
        public void SetSquadraAttuale(List<Calciatore> squadra)
        {
            this.squadra = squadra;
        }

        /**
          * \fn      public List<Calciatore> GetSquadraAttuale()
          * \brief   Ritorna la lista di calciatori che il giocatore userà nella prossima partita
          * \return  List<Calciatore>: Il metodo ritorna la lista di calciatori che il giocatore userà nella prossima partita
          */
        public List<Calciatore> GetSquadraAttuale()
        {
            return squadra;
        }

        /**
         * \fn      public string GetStringSquadra(string gruppo)
         * \brief   Il metodo ritorna una stringa contenente una lista di calciatori, che variano in base al valore della stringa ricevuta in ingresso, a cui è assegnato un indice pari alla loro posizione nella lista di appartenenza + 1
         * \param   string gruppo: Il gruppo di cui si vuole ottenere la stringa: "r" = rosa del giocatore, "t" = giocatori titolari, "a" = squadra attuale
         * \param   string stringa: La stringa contenente la lista di calciatori
         * \param   List<Calciatore> daControllare: La lista di calciatori da cui si ricaverà la stringa
         * \return  string: Il metodo ritorna una stringa contenente una lista in cui ogni riga contiene il nome e il ruolo di un calciatore e il suo indice.
         * \details Viene controllato il valore della stringa ricevuta come parametro, e in base ad esso si imposta il valore della lista daControllare a quello della lista desiderata. Con un ciclo for, che si ripete
         * per un numero di volte pari al numero di calciatori presenti nella lista da controllare, viene scritta una nuova riga in cui si inserisce l'indice del calciatore, che corrisponde al numero dell'iterazione corrente + 1,
         * il nome e il ruolo del giocatore nella posizione pari al numero dell'iterazione corrente nella lista da controllare. Viene infine ritornata la stringa.
         */
        public string GetStringSquadra(string gruppo)
        {
            string stringa = "";
            List<Calciatore> daControllare = new List<Calciatore>();

            if (gruppo == "r" && rosa != null)
            {
                daControllare = rosa;
            }
            else if (gruppo == "t" && titolari != null)
            {
                daControllare = titolari;
            }
            else if (gruppo == "a" && squadra != null)
            {
                daControllare = squadra;
            }

            for (int i = 0; i < daControllare.Count; i++)
            {
                stringa += i + 1 + $" -> Nome: {daControllare[i].nome}, Ruolo: {daControllare[i].ruolo}\n";
            }
            return stringa;
        }

        /**
         * \fn      public override string ToString()
         * \brief   Ritorna una stringa che comunica il nome e i crediti del giocatore
         * \return  string: Il metodo ritorna una stringa che comunica il nome e i crediti del giocatore
         */
        public override string ToString()
        {
            return $"Nome: {nome.ToUpper()}, Crediti: {fantaMilioni}";
        }

        /**
         * \fn      public int CompareTo(Giocatore giocatore2)
         * \brief   Compara il punteggio del giocatore con quello del giocatore ricevuto come parametro
         * \param   Giocatore giocatore2: Il giocatore a cui si deve comparare il giocatore che chiama questo metodo
         * \return  int: Il metodo ritorna 1 se il giocatore che chiama il metodo ha un punteggio maggiore del secondo, -1 se il punteggio è minore e 0 se il punteggio è uguale.
         */
        public int CompareTo(Giocatore giocatore2)
        {
            if (this.punteggio > giocatore2.punteggio)
            {
                return 1;
            }
            else if (this.punteggio < giocatore2.punteggio)
            {
                return -1;
            }
            return 0;
        }

        /**
         * \fn      public int GetGiocatoriRuolo(string ruolo, string gruppo)
         * \brief   Ritorna il numero di giocatori in un certo ruolo in un certo gruppo
         * \param   string ruolo: Il ruolo dei giocatori che si vogliono contare. Se viene inserito "tot" restituisce il numero di giocatori totale.
         * \param   string gruppo: Il gruppo dei giocatori tra cui si vuole contare: "r" = rosa, "t" = giocatori titolari, "a" = squadra attuale
         * \param   int portieri: Il numero di portieri nel gruppo considerato
         * \param   int difensori: Il numero di difensori nel gruppo considerato
         * \param   int attaccanti: Il numero di attaccanti nel gruppo considerato
         * \param   int centrocampisti: Il numero di centrocampisti nel gruppo considerato
         * \return  int: Il metodo ritorna il numero di calciatori del ruolo specificato nel gruppo desiderato.
         * \details Viene controllato il valore della stringa gruppo. In base ad esso si decide in quale lista vanno contati i calciatori. Tramite un'istruzione switch dentro un ciclo foreach si incrementa la variabile
         * corrispondente al ruolo del calciatore considerato nella lista desiderata. Poi, in base al ruolo desiderato, si ritorna il valore della variabile corrispondente. Se al posto del ruolo è stato inserito "tot", viene
         * ritornato il numero di calciatori totali.
         */
        public int GetGiocatoriRuolo(string ruolo, string gruppo)
        {
            int portieri = 0;
            int difensori = 0;
            int attaccanti = 0;
            int centrocampisti = 0;
            List<Calciatore> daControllare = new List<Calciatore>();
            if (gruppo == "r" && rosa != null)
            {
                daControllare = rosa;
            }
            else if (gruppo == "t" && titolari != null)
            {
                daControllare = titolari;
            }
            else if (gruppo == "a" && squadra != null)
            {
                daControllare = squadra;
            }
            foreach (Calciatore calciatore in daControllare)
            {
                switch (calciatore.ruolo)
                {
                    case "portiere":
                        portieri++;
                        break;
                    case "attaccante":
                        attaccanti++;
                        break;
                    case "centrocampista":
                        centrocampisti++;
                        break;
                    case "difensore":
                        difensori++;
                        break;
                }
            }
            switch (ruolo)
            {
                case "portiere":
                    return portieri;
                case "attaccante":
                    return attaccanti;
                case "centrocampista":
                    return centrocampisti;
                case "difensore":
                    return difensori;
                case "tot":
                    return rosa.Count;
                default:
                    return -1;
            }
        }
    }
}
