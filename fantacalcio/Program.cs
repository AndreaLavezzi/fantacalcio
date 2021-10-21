using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace fantacalcio
{
    class Program
    {
        class Calciatore
        {
            string nome;
            int prezzo;
            int probParata;
        }
        //coloro che giocano al fantacalcio, hanno un nome, un punteggio, dei crediti, una lista di giocatori posseduti, possono comprare giocatori
        class Giocatore
        {
            public string nome { get; }
            public int punteggio { get; }
            public int fantaMilioni { get; }
            public Giocatore(string nome)
            {
                this.nome = nome;
                
            }
        }

        static void Main(string[] args)
        {
            //aggiungi giocatori alla lista
            List<Giocatore> giocatoriDaSerializzare = new List<Giocatore>();
            for(int i = 0; i < 10; i++)
            {
                string pname = $"g{i}";
                giocatoriDaSerializzare.Add(new Giocatore(pname));
            }
            //serializza

            string output = JsonConvert.SerializeObject(giocatoriDaSerializzare);
            File.WriteAllText("bro.json", output);



            //deserializza
            string input = File.ReadAllText("bro.json");
            List<Giocatore> giocatoriDeserializzati = JsonConvert.DeserializeObject<List<Giocatore>>(input);

            foreach(Giocatore giocatore in giocatoriDeserializzati)
            {
                Console.WriteLine(giocatore.nome);
            }

        }

        void CreaGiocatori()
        {

        }
    }
}
