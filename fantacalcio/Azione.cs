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
     * \brief   Rappresenta tutte le azioni che un giocatore può effettuare in base al suo ruolo.
     * \param   public string nome: Il nome dell'azione
     * \param   public double punteggio: Il punteggio che l'azione conferisce
     * \param   public int peso: Un valore che, diviso per la somma di tutti i pesi di tutte le azioni possibili dà la probabilità per il calciatore di eseguire una certa azione
     */
    class Azione
    {
        public string nome { get; set; }
        public double punteggio { get; set; }
        public int peso { get; set; }

        public Azione(string nome, double punteggio, int peso)
        {
            this.nome = nome;
            this.punteggio = punteggio;
            this.peso = peso;
        }
    }
}
