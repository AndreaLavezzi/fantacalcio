/**
 * \file    Fantacalcio.cs
 * \author  Sandstorm
 * \brief   Sistema di gestione del gioco del FANTACALCIO
 * \date    18/10/2021
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace fantacalcio
{
    /**
     * \class   Fantacalcio
     * \brief   Rappresenta una partita, contiene tutti i dati di essa
     */
    class Fantacalcio
    {
        /// \brief string nomeSalvataggio: Il nome del torneo.Il file di salvataggio del torneo avrà questo nome
        public string nomeSalvataggio { get; }

        /// \brief int fase: Indica il momento a cui la partita è arrivata. 0 = Inizio Asta, 1 = Selezione titolari, 2 = Partite, 3 = Fine partita
        public int fase { get; set; }

        /// \brief Il numero della sfida tra giocatori a cui si è arrivati
        public int numeroPartita { get; set; }

        /// \brief La lista di giocatori registrati al torneo
        List<Giocatore> giocatori = new List<Giocatore>(); 

        /**
         * \fn      public Fantacalcio(string nomeSalvataggio, List<Giocatore> giocatori, int fase, int numeroPartita)
         * \brief   Metodo costruttore, ottiene in ingresso il nome del salvataggio, la lista dei giocatori registrati, la fase della partita e il numero della sfida a cui si è arrivati
         * \param   string nomeSalvataggio: Il nome del torneo e del file di salvataggio
         * \param   List<Giocatore> giocatori: La lista di giocatori che partecipano al torneo
         * \param   int fase: Indica il momento a cui la partita è arrivata. 0 = Inizio Asta, 1 = Selezione titolari, 2 = Partite, 3 = Fine partita
         * \param   int numeroPartita: Il numero della sfida tra giocatori a cui si è arrivati
         */
        public Fantacalcio(string nomeSalvataggio, List<Giocatore> giocatori, int fase, int numeroPartita)
        {
            this.nomeSalvataggio = nomeSalvataggio;
            this.giocatori = giocatori;
            this.fase = fase;
            this.numeroPartita = numeroPartita;
        }

        /**
         * \fn      public string GetListaGiocatori()
         * \brief   Ritorna una stringa in cui a ogni giocatore è associato un indice
         * \return  string: Ritorna una stringa in cui ogmi giocatore è associato a un indice, dato dalla sua posizione nella lista di giocatori
         * \param   string stringaGiocatori: Contiene una lista in cui a ogni giocatore è associato un indice, dato dalla sua posizione nella lista di giocatori + 1
         * \brief   Tramite un ciclo for, si scorre la lista di giocatori registrati, e si inserisce una nuova linea che contiene l'indice e il nome del giocatore
         */
        public string GetListaGiocatori()
        {
            string stringaGiocatori = "";
            for (int i = 0; i < giocatori.Count; i++)
            {
                stringaGiocatori += "\n" + (i + 1) + " -> " + giocatori[i].ToString();
            }
            return stringaGiocatori;
        }

        /**
         * \fn      public List<Giocatore> GetGiocatori()
         * \return  List<Giocatore>: Ritorna la lista di giocatori registrati 
         */
        public List<Giocatore> GetGiocatori()   //metodo pubblico che ritorna la lista dei giocatori registrati nella partita corrente
        {
            return giocatori;
        }

        /**
         * \fn      void GeneraAbbinamenti()
         * \brief   Popola un array bidimensionale con coppie tutte diverse di giocatori.
         * \param   List<Giocatore> giocatori: La lista di giocatori che partecipano al torneo
         * \param   int numeroPartite: Il numero di partite che verranno giocate
         * \param   int indicePartita: Il numero della partita considerata.
         * \param   Giocatore[,] abbinamenti: Array bidimensionale di giocatori, ogni coppia di giocatori è diversa.
         * \details Viene calcolato il numero di partite che verranno giocate tramite la formula N * (N - 1) / 2 dove N è il numero di giocatori.
         * In un doppio ciclo for viene poi popolato l'array, facendo in modo che un giocatore venga abbinato
         * con se stesso. Alla fine, vengono salvati gli abbinamenti tramite il metodo Salvataggio.SalvaAbbinamenti().
         */
        public void GeneraAbbinamenti()
        {
            int numeroPartite = giocatori.Count * (giocatori.Count - 1) / 2, indicePartita = 0;
            Giocatore[,] abbinamenti = new Giocatore[numeroPartite, 2];

            for (int i = 0; i < giocatori.Count; i++)
            {
                for (int j = i; j < giocatori.Count; j++)
                {
                    if (i != j)
                    {
                        abbinamenti[indicePartita, 0] = giocatori[i];
                        abbinamenti[indicePartita, 1] = giocatori[j];
                        indicePartita++;
                    }
                }
            }

            Salvataggio.SalvaAbbinamenti(nomeSalvataggio, abbinamenti);
        }

        /**
         * \fn      string Classifica()
         * \brief   Scrive in una stringa la classifica dei giocatori in base al punteggio
         * \param   List<Giocatore> giocatori: La lista di giocatori registrati al torneo
         * \param   string classifica: La stringa contenente la classifica
         * \return  string: La funzione ritorna la stringa contenente la classfica in base al punteggio dei giocatori
         * \details Inizialmente viene impostata la lista di giocatori uguale alla lista di giocatori della partita in corso. Viene poi passata come parametro alla funzione InsertionSort() che ordina la lista in base
         * al punteggio. Con un ciclo for vengono poi inserite nella stringa le righe della classifica. Ogni riga contiente l'indice del giocatore, il suo nome e il suo punteggio. Viene infine ritornata la lista.
         */
        public string Classifica()
        {
            List<Giocatore> giocatori = GetGiocatori();
            string classifica = "";
            giocatori = OrdinaGiocatori(giocatori);

            for (int i = 0; i < giocatori.Count; i++)
            {
                classifica += i + 1 + " -> " + giocatori[i].nome + " con " + giocatori[i].punteggio + " punti\n";
            }
            return classifica;
        }

        /**
         * \fn      List<Giocatore> OrdinaGiocatori(List<Giocatore> giocatori)
         * \brief   Ordina la lista di giocatori che viene data in input alla funzione in base al punteggio di ciascuno
         * \param   List<Giocatore> giocatori: La lista di giocatori da ordinare
         * \param   Giocatore temp: Variabile di appoggio che permette di scambiare due giocatori
         * \details Un ciclo for esterno che si ripete tante volte quanti giocatori sono presenti nella lista di giocatori, viene ripetuto un secondo ciclo for che compara un giocatore con il proprio precedente. Se il
         * precedente risulta minore del successivo (e quindi il metodo Giocatore.CompareTo() restituisce -1) i due giocatori vengono scambiati. Alla fine, viene ritornata la lista di giocatori ordinata.
         */
        List<Giocatore> OrdinaGiocatori(List<Giocatore> giocatori)
        {
            for (int i = 0; i < giocatori.Count - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    int t = giocatori[j - 1].CompareTo(giocatori[j]);
                    if (t == -1)
                    {
                        Giocatore temp = giocatori[j - 1];
                        giocatori[j - 1] = giocatori[j];
                        giocatori[j] = temp;
                    }
                }
            }
            return giocatori;
        }
    }
}