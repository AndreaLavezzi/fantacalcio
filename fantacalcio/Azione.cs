/**
 * \file    Azione.cs
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
     * \class   Azione
     * \brief   Rappresenta tutte le azioni che un calciatore può effettuare in base al suo ruolo.
     */
    class Azione
    {
        /// \brief Il nome dell'azione
        public string nome { get; set; }
        
        /// \brief Il punteggio che l'azione conferisce
        public double punteggio { get; set; } 

        /// \brief Un valore che, diviso per la somma di tutti i pesi di tutte le azioni possibili dà la probabilità per il calciatore di eseguire una certa azione
        public int peso { get; set; } 

        /**
         * \brief   Metodo costruttore, riceve in input il nome, il punteggio e il peso dell'azione
         */
        public Azione(string nome, double punteggio, int peso)
        {
            this.nome = nome;
            this.punteggio = punteggio;
            this.peso = peso;
        }
    }
}
