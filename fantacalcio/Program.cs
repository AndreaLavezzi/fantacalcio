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
            public string nomeSalvataggio { get; }
            List<Giocatore> giocatori = new List<Giocatore>();
            
            public Fantacalcio(string nomeSalvataggio, List<Giocatore> giocatori)
            {
                this.nomeSalvataggio = nomeSalvataggio;
                this.giocatori = giocatori;
            }

            public List<Giocatore> GetGiocatori()
            {
                return giocatori;
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
            string nomeTorneo;
            if(Directory.GetFiles("saveFiles/", "*.json").Length >= 3)
            {
                Console.WriteLine("Impossibile creare più di 3 file di salvataggio. Eliminarne per poterne creare di nuovi.");
                return null;
            }
            else
            {
                Console.Write("Inserire il nome del torneo: ");
                do
                {
                    nomeTorneo = Console.ReadLine();
                } while (ControlloNome(-1, nomeTorneo, new List<Giocatore>()) == false);
                Fantacalcio fantacalcio = new Fantacalcio(nomeTorneo, CreaGiocatori());
                string output = JsonConvert.SerializeObject(fantacalcio) + ";" + JsonConvert.SerializeObject(fantacalcio.GetGiocatori(), Formatting.Indented);

                File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + ".json", output);
                return fantacalcio;
            }   
        }

        //funzione di test del caricamento di un file JSON nel programma
        static void MostraFile()
        {
            string[] salvataggi = Directory.GetFiles("saveFiles/", "*.json");
            if (salvataggi.Length != 0)
            {
                foreach(string salvataggio in salvataggi)
                {
                    string input = File.ReadAllText(salvataggio);
                    string[] words = input.Split(input, ";");
                }
                //Console.WriteLine("Esiste un file. Vuoi Caricarlo? [y/n]");
                //if(Console.ReadKey(true).Key == ConsoleKey.Y)
                //{
                //    //deserializza
                //    //string input = File.ReadAllText("bro.json");
                //    //List<Giocatore> giocatoriDeserializzati = JsonConvert.DeserializeObject<List<Giocatore>>(input);

                //    //foreach (Giocatore giocatore in giocatoriDeserializzati)
                //    //{
                //    //    Console.WriteLine(giocatore.nome);
                //    //}
                //}
            }
            else
            {
                Console.WriteLine("Non esistono file di salvataggio.");
            }
        }

        /* chiede di inserire un numero di giocatori uguale o maggiore di 2 e minore o uguale a 8,
         * successivamente chiede di inserire i nomi dei giocatori, li mette in una lista */
        static List<Giocatore> CreaGiocatori()
        {
            int numeroGiocatori;
            string nome;
            List<Giocatore> giocatori = new List<Giocatore>();


            Console.Write("Quanti giocatori parteciperanno al torneo? [MINIMO 2 GIOCATORI]\nRisposta: ");
            while (!Int32.TryParse(Console.ReadLine(), out numeroGiocatori) || numeroGiocatori < 2 || numeroGiocatori > 8)
            {
                Console.Write("\nInserimento non valido. Reinserire: ");
            }
            
            for (int i = 0; i < numeroGiocatori; i++)
            {
                Console.Write("\nInserire il nome del giocatore numero {0}: ", i + 1);
                do
                {
                    nome = Console.ReadLine();
                }
                while (ControlloNome(0, nome, giocatori) == false);

                giocatori.Add(new Giocatore(nome));
            }

            return giocatori;
        }

        static bool ControlloNome(int codice, string nomeDaControllare, List<Giocatore> giocatori)
        {

            if(nomeDaControllare.Length < 4 || nomeDaControllare.Length > 12)
            {
                Console.Write("Inserire un nome compreso tra 4 e 12 caratteri: ");
                return false;
            }
            string caratteriSpeciali = "|\\!\"£$%&/()='?^<>[]{}*+@°#§ "; 
            //string[] caratteriSpeciali = { "|", "\\", "!", "\"", "£", "$", "%", "&", "/", "(", ")", "=", "?", "^", "<", ">", ";", ",", ":", ":", "-", "_", "@", "°", "#", "§", "[", "{", "]", "}", "*", "+", };
            for (int i = 0; i < nomeDaControllare.Length; i++)
            {
                for(int j = 0; j < caratteriSpeciali.Length; j++)
                {
                    if(nomeDaControllare[i].ToString() == caratteriSpeciali[j].ToString())
                    {
                        Console.Write("Impossibile inserire nel nome il carattere \"{0}\". Reinserire: ", caratteriSpeciali[j].ToString());
                        return false;
                    }
                }
            }

            if(codice == 0)
            {
                foreach (Giocatore giocatore in giocatori)
                {
                    if (nomeDaControllare == giocatore.nome)
                    {
                        Console.Write("Inserire un nome che non sia già stato scelto: ");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
