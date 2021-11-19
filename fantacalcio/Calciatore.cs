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
     * \param   public string nome { get; }: Attributo di sola lettura che contiene il nome del calciatore
     * \param   public string ruolo { get; }: Attributo di sola lettura che contiene il ruolo del calciatore
     */
    class Calciatore
    {
        public string nome { get; }
        public string ruolo { get; }
        public Calciatore(string nome, string ruolo)
        {
            this.nome = nome;
            this.ruolo = ruolo;
        }
        /**
         * \fn      public double GeneraAzioni()
         * \brief   Genera un numero randomico di azioni, i cui punteggi verranno poi 
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
                int probabilità = (int)Math.Round((double)(azioni[azioneRandom].peso * 100) / pesoMassimo);
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

        public override string ToString()
        {
            return $"Nome: {nome.ToUpper()}\nRuolo: {ruolo.ToUpper()}";
        }
    }
}
