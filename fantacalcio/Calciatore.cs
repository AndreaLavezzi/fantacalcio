/**
 * \file    Calciatore.cs
 * \author  Sandstorm
 * \brief   Sistema di gestione del gioco del FANTACALCIO
 * \date    18/10/2021
 */
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace fantacalcio
{
    /**
     * \class   Calciatore
     * \brief   Rappresenta i calciatori, hanno un nome e un ruolo
     */
    class Calciatore
    {
        /// \brief Attributo di sola lettura che contiene il nome del calciatore
        public string nome { get; }

        /// \brief Attributo di sola lettura che contiene il ruolo del calciatore
        public string ruolo { get; } 

        /**
         * \brief Meotodo costruttore, riceve in input il nome e il ruolo del calciatore
         */
        public Calciatore(string nome, string ruolo)
        {
            this.nome = nome;
            this.ruolo = ruolo;
        }
        /**
         * \fn      public double GeneraAzioni()
         * \brief   Genera un numero randomico di azioni, i cui punteggi verranno poi aggiunti al punteggio totale del calciatore
         * \param   double punti: I punti totali ottenuti dal calciatore
         * \param   int pesoMassimo: Il totale della somma tra i pesi delle varie azioni possibili
         * \param   List<Azione> azioni: Lista delle azioni disponibili in base al ruolo del calciatore
         * \param   Random random: Istanza della classe Random che permette di generare numeri randomici
         * \param   int numeroAzioni: Numero randomico di azioni da generare
         * \param   int azioneRandom: L'indice dell'azione da eseguire nella lista di azioni
         * \param   int eseguiAzione: Numero random tra 0 e 100
         * \return  double: Ritorna il punteggio totale fatto dal calciatore
         * \param   int probabilità: La percentuale di possibilità che una certa azione venga eseguita
         * \details Il metodo ottiene dal file <ruolo>.json (dove <ruolo> è il ruolo del calciatore corrente) tramite il metodo DeserializeObject della classe JsonConvert, dalla libreria Newtonsoft.Json.
         * Viene poi calcolato il peso massimo sommando i pesi di ogni azione nella lista delle azioni. In un ciclo for, che si ripete un numero di volte pari al numero di azioni, si estrae un numero random
         * tra 0 e il numero di azioni disponibili, per estrarre l'azione da eseguire. Si estrae poi un numero random che indicherà se l'azione verrà eseguita o meno. Viene poi calcolata la probabilità che
         * l'azione avvenga, moltiplicando il peso dell'azione estratta per 200 e poi dividendolo per il peso massimo, tutto dentro al metodo Math.Round() che arrotonda il valore double in un intero.
         * Se il numero random eseguiAzione è minore o uguale alla probabilità, p'azione viene eseguita e viene aggiunto il punteggio dell'azione ai punti totali. Se non viene eseguita l'azione si decrementa
         * il valore di i.
         */
        public double GeneraAzioni()
        {
            double punti = 0;
            int pesoMassimo = 0;
            List<Azione> azioni = JsonConvert.DeserializeObject<List<Azione>>(File.ReadAllText(ruolo + ".json"));
            Random random = new Random();
            int numeroAzioni = random.Next(100);
            foreach (Azione azione in azioni)
            {
                pesoMassimo += azione.peso;
            }

            for (int i = 0; i < numeroAzioni; i++)
            {
                int azioneRandom = random.Next(azioni.Count);
                int eseguiAzione = random.Next(100);
                int probabilità = (int)Math.Round((double)(azioni[azioneRandom].peso * 200) / pesoMassimo);
                if (eseguiAzione <= probabilità)
                {
                    punti += azioni[azioneRandom].punteggio;
                }
                else
                {
                    i--;
                }
            }
            return punti;
        }

        /**
         * \fn      public override string ToString()
         * \brief   Ritorna una stringa con le informazioni del calciatore
         * \return  string: La stringa ritornata contiene il nome e il ruolo in maiuscolo del calciatore corrente
         */
        public override string ToString()
        {
            return $"Nome: {nome.ToUpper()}\nRuolo: {ruolo.ToUpper()}";
        }
    }
}
