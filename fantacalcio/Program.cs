//Nickname: @Sandstorm
//Data: 18/10/2021
/*Consegna: Progettare un sistema di gestione del FANTACALCIO.
* Il livello di complessità del regolamento dovrà essere gestito autonomamente e giustificato nella relazione.


* Funzionalità minime
* - Almeno 2 giocatori
* - gestione dei crediti per l'acquisto giocatori (all'inizio X crediti, ogni giocatore vale y1, y2, y3..yn crediti
* - gestione settimanale con inserimento punteggio singolo giocatori.
* - gestione della classifica parziale al termine di ogni aggiornamento settimanale.


* Il progetto DEVE essere svolto in modalità CONSOLE.*/
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace fantacalcio
{
    class Program
    {
        //i calciatori, hanno un nome e un ruolo, viene inoltre salvato il prezzo a cui vengono comprati
        class Calciatore
        {
            string nome, ruolo;
            int prezzo;
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

        //partita in corso
        class Fantacalcio
        {
            List<Giocatore> giocatori = new List<Giocatore>();
            public Fantacalcio(List<Giocatore> giocatori)
            {
                this.giocatori = giocatori;
            }
        }

        static void Main(string[] args)
        {
            Menu();
            MostraFile();

            //aggiungi giocatori alla lista
            //List<Giocatore> giocatoriDaSerializzare = new List<Giocatore>();
            //for(int i = 0; i < 10; i++)
            //{
            //    string pname = $"g{i}";
            //    giocatoriDaSerializzare.Add(new Giocatore(pname));
            //}
            //serializza

            //string output = JsonConvert.SerializeObject(giocatoriDaSerializzare);
            //File.WriteAllText("bro.json", output);



            //deserializza
            //string input = File.ReadAllText("bro.json");
            //List<Giocatore> giocatoriDeserializzati = JsonConvert.DeserializeObject<List<Giocatore>>(input);

            //foreach(Giocatore giocatore in giocatoriDeserializzati)
            //{
            //    Console.WriteLine(giocatore.nome);
            //}

        }

        static void Menu()
        {
            string risposta;
            bool nonValida;
            Console.Write("1 - Inizia nuova partita\n2 - Carica partita\nRisposta: ");
            risposta = Console.ReadLine();
            do
            {
                switch (risposta)
                {
                    case "1":
                        nonValida = false;
                        NuovaPartita();
                        break;
                    case "2":
                        ControllaFile();
                        nonValida = false;
                        break;
                    default:
                        nonValida = true;
                        Console.Write("Inserimento non valido. Reiserire: ");
                        risposta = Console.ReadLine();
                        break;
                }
            } while (nonValida == true);
            
        }

        static void ControllaFile()
        {

        }

        /*inizia una nuova partita creando un oggetto di tipo "Fantacalcio" che rappresenta la partita.*/
        static Fantacalcio NuovaPartita()
        {      
            Fantacalcio fantacalcio = new Fantacalcio(CreaGiocatori());
            return fantacalcio;
        }

        //funzione di test del caricamento di un file JSON nel programma
        static void MostraFile()
        {
            if (File.Exists("bro.json"))
            {
                Console.WriteLine("Esiste un file. Vuoi Caricarlo? [y/n]");
                if(Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    //deserializza
                    string input = File.ReadAllText("bro.json");
                    List<Giocatore> giocatoriDeserializzati = JsonConvert.DeserializeObject<List<Giocatore>>(input);

                    foreach (Giocatore giocatore in giocatoriDeserializzati)
                    {
                        Console.WriteLine(giocatore.nome);
                    }
                }
            }
        }

        /* chiede di inserire un numero di giocatori uguale o maggiore di 2 e minore o uguale a 8,
         * successivamente chiede di inserire i nomi dei giocatori, li mette in una lista */
        static List<Giocatore> CreaGiocatori()
        {
            int numeroGiocatori;
            string nome;
            bool nomeDoppio = false;
            List<Giocatore> giocatori = new List<Giocatore>();


            Console.Write("Quanti giocatori parteciperanno al torneo? [MINIMO 2 GIOCATORI]\nRisposta: ");
            while (!Int32.TryParse(Console.ReadLine(), out numeroGiocatori) || numeroGiocatori < 2 || numeroGiocatori > 8)
            {
                Console.Write("\nInserimento non valido. Reinserire: ");
            }
            
            for (int i = 0; i < numeroGiocatori; i++)
            {
                Console.Write("\nInserire il nome del giocatore numero {0}: ", i + 1);
                nome = Console.ReadLine();
                do
                {
                    foreach (Giocatore giocatore in giocatori)
                    {
                        if (nome == giocatore.nome)
                        {
                            nomeDoppio = true;
                            Console.Write("Inserire un nome che non sia già stato scelto: ");
                            nome = Console.ReadLine();
                            break;
                        }
                        else
                        {
                            nomeDoppio = false;
                        }
                    }
                } while (nomeDoppio == true);


                giocatori.Add(new Giocatore(nome));
            }

            return giocatori;
        }
    }
}
