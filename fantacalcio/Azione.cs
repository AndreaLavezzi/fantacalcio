﻿/**
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
    class Azione
    {
        public string nome;
        public double punteggio;
        public int peso;
        public Azione(string nome, double punteggio, int peso)
        {
            this.nome = nome;
            this.punteggio = punteggio;
            this.peso = peso;
        }
    }
}
