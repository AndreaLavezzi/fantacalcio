﻿//Nickname: @Sandstorm
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
        #region Classi
        //i calciatori, hanno un nome e un ruolo, viene inoltre salvato il prezzo a cui vengono comprati
        class Calciatore
        {
            public string nome { get; }
            public string ruolo { get; }
            int prezzo;
            public Calciatore(string nome, string ruolo)
            {
                this.nome = nome;
                this.ruolo = ruolo;
            }
        }

        //coloro che giocano al fantacalcio, hanno un nome, un punteggio, dei crediti, una lista di giocatori posseduti, possono comprare giocatori
        class Giocatore
        {
            public string nome { get; }         //identifica il giocatore 
            public int punteggio { get; }       //punteggio che decreterà il vincitore finale della partita
            public int fantaMilioni { get; }    //crediti a disposizione del giocatore, usati per comprare i calciatori
            public Giocatore(string nome)       //metodo costruttore, riceve in ingresso il nome del giocatore 
            {
                this.nome = nome;    
            }
        }

        //classe che rappresenta una partita, ha un nome, una lista di giocatori registrati e un metodo che ritorna la lista di giocatori
        class Fantacalcio
        {
            public string nomeSalvataggio { get; }      //nome del salvataggio che verrà assegnato al file di salvataggio
            public int fase { get; }  //fase 0 -> appena creata, fase 1 -> asta finita, fase 2 -> 
            List<Giocatore> giocatori = new List<Giocatore>();      //lista contenente i giocatori registrati nella partita corrente  
            
            public Fantacalcio(string nomeSalvataggio, List<Giocatore> giocatori, int fase)   //metodo costruttore, ottiene in ingresso il nome del salvataggio e la lista dei giocatori registrati
            {
                this.nomeSalvataggio = nomeSalvataggio;
                this.giocatori = giocatori;
                this.fase = fase;
            }

            public List<Giocatore> GetGiocatori()   //metodo pubblico che ritorna la lista dei giocatori registrati nella partita corrente
            {
                return giocatori;
            }
        }
        #endregion

        static Fantacalcio partitaInCorso;  //assume il valore della partita caricata attualmente

        static void Main(string[] args)     //il main chiama solo il metodo che mostra il menù principale
        {
            Menu();
        }

        static void Menu()
        {
            string risposta;    //contiene la risposta inserita dall'utente da tastiera
            bool nonValida;     //indica se la risposta inserita dall'utente rientra o meno nelle possibilità offerte
            Console.Write("1 - Inizia nuova partita\n2 - Carica partita\nRisposta: "); //se l'utente inserisce 1 verrà iniziata la procedura di creazione di una partita, se inserisce 2 verrà mostrata una lista di salvataggi su cui l'utente potrà eseguire varie azioni
            do
            {
                risposta = Console.ReadLine(); //si ottene una risposta da tastiera 
                nonValida = false;  //la risposta è valida a meno che non si inserisca uno dei casi non gestiti dall'iterazione switch
                switch (risposta)
                {
                    case "1": 
                        NuovaPartita();     //se l'utente inserisce "1" verrà iniziata la procedura di creazione di una nuova partita
                        break;  
                    case "2":
                        MostraFile();       //se l'utente inserisce "2" verrà mostrata la lista dei salvataggi esistenti su cui si potranno eseguire delle azioni)
                        break;
                    default:
                        nonValida = true;   //se l'utente inserisce qualcosa di non gestito, la risposta non sarà valida e verrà chiesto nuovamente l'inserimento di una risposta
                        Console.Write("Inserimento non valido. Reiserire: ");   //comunico all'utente che ciò che ha inserito non è valido e chiedo di reinserire un valore
                        break;
                }
            } while (nonValida == true);    //il ciclo do-while si ripete finchè la risposta non è valida
            
        }

        #region GestioneFile
        static void ControllaFile()
        {
            
        }

        static void CaricaPartita(Fantacalcio partita)  //riceve in input una partita ricavata da un file di salvataggio
        {
            partitaInCorso = partita;   //imposta la partita come partita in corso
            switch (partitaInCorso.fase)
            {
                case 0:
                    Asta();
                    break;
                case 1:
                    break;
            }
        }

        /*inizia una nuova partita creando un oggetto di tipo "Fantacalcio" che rappresenta la partita.*/
        static void NuovaPartita()
        {
            string nomeTorneo;  //indica il nome del torneo, e verrà assegnato come nome al file
            if(Directory.GetFiles("saveFiles/", "*.json").Length >= 3)  //se esistono già 3 file di salvataggio si impedisce di crearne di nuovi
            {
                Console.WriteLine("Impossibile creare più di 3 file di salvataggio. Eliminarne per poterne creare di nuovi.");  //viene comunicato all'utente che non può creare più di 3 file di salvataggio
            }
            else //se invece è possibile creare un file
            {
                Console.Write("Inserire il nome del torneo: "); //viene chiesto all'utente di inserire da tastiera il nome del torneo
                do
                {
                    nomeTorneo = Console.ReadLine();    //inserimento da tastiera del nome da parte dell'utente
                } while (!ControlloNome(-1, nomeTorneo, new List<Giocatore>()));    //il ciclo do-while si ripete finchè il controllo non va a buon fine

                Fantacalcio fantacalcio = new Fantacalcio(nomeTorneo, CreaGiocatori(), 0);     //viene creata un'istanza della classe salvataggio che rappresenta ciò che l'utente ha inserito

                string output = fantacalcio.nomeSalvataggio + ";" + fantacalcio.fase + ";" + JsonConvert.SerializeObject(fantacalcio.GetGiocatori(), Formatting.Indented);  /*viene creata una stringa che contiene il nome dell'istanza della partita
                                                                                                                                                                * appena creata e la lista di giocatori registrati che le appartiene; entrambe
                                                                                                                                                                * vengono convertite ad un file con estensione json tramite il metodo "SerializeObject"
                                                                                                                                                                * della classe JsonConvert della libreria Newtonsoft.Json*/

                File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + ".json", output);    //viene salvata la stringa convertita a json in un file con estensione .json
                CaricaPartita(fantacalcio);
            }   
        }


        static void MostraFile()
        {
            string[] salvataggi = Directory.GetFiles("saveFiles/", "*.json");   //ottiene tutte le directory dei file con estensione json nella cartella saveFiles
            if (salvataggi.Length != 0)     //se esistono file di salvataggio
            {
                List<Fantacalcio> partite = new List<Fantacalcio>();    //lista di partite esistenti

                for(int i = 0; i < salvataggi.Length; i++)
                {
                    string input = File.ReadAllText(salvataggi[i]);
                    string[] words = input.Split(";");
                    partite.Add(new Fantacalcio(words[0], JsonConvert.DeserializeObject<List<Giocatore>>(words[2]), Int32.Parse(words[1])));
                }

                for(int i = 0; i < partite.Count; i++)
                {
                    Console.WriteLine(i + 1 +  " -> " + partite[i].nomeSalvataggio);
                }

                SelezionaFile(partite);

            }
            else
            {
                Console.WriteLine("Non esistono file di salvataggio.");
            }
        }

        /* chiede di inserire un numero di giocatori uguale o maggiore di 2 e minore o uguale a 8,
         * successivamente chiede di inserire i nomi dei giocatori, li mette in una lista */

        static bool ControlloNome(int codice, string nomeDaControllare, List<Giocatore> giocatori)
        {
            string caratteriSpeciali = "|\\!\"£$%&/()='?^<>[]{}*+@°#§ ";

            if (nomeDaControllare.Length < 4 || nomeDaControllare.Length > 12)
            {
                Console.Write("Inserire un nome compreso tra 4 e 12 caratteri: ");
                return false;
            }

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

        static void SelezionaFile(List<Fantacalcio> partite)
        {
            int indiceFile;
            string risposta;
            bool nonValida = false;
            Console.WriteLine("Su quale salvataggio vuoi compiere un'azione?");
            risposta = Console.ReadLine();
            while (!int.TryParse(risposta, out indiceFile) || indiceFile > partite.Count || indiceFile < 0)
            {
                Console.WriteLine("Risposta non valida. Reinserire:");
                risposta = Console.ReadLine();
            }
            Console.WriteLine("E' stato selezionato il file numero {0}, cosa vuoi fare?", risposta);
            Console.WriteLine("1 - Carica File");
            Console.Write("Risposta: ");
            string risposta2 = "e";
            while(risposta2 != "1")
            {
                risposta2 = Console.ReadLine();
            }
            switch (risposta2)
            {
                case "1":
                    CaricaPartita(partite[Int32.Parse(risposta)]);
                    break;
            }
        }

        static void AzioniFile(string fileSelezionato)
        {
            
        }

        #endregion

        #region Gioco
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

        static void Asta()
        {
            Console.WriteLine("Caricamento dei giocatori, Attendere...");
            string[] inputCalciatori = File.ReadAllLines("calciatori.txt");
            List<Calciatore> listaCalciatori = new List<Calciatore>();
            for(int i = 0; i < inputCalciatori.Length; i++)
            {
                listaCalciatori.Add(new Calciatore(inputCalciatori[i].Split(",")[0], inputCalciatori[i].Split(",")[1]));
            }
            Console.Clear();
            Console.WriteLine("Caricamento Completato.");
            Console.WriteLine("Inizio serializzazione lista...");
            string output = JsonConvert.SerializeObject(listaCalciatori, Formatting.Indented);
            Console.WriteLine("Serializzazione completata.");
            File.WriteAllText("newCalciatori.txt", output);
            Console.WriteLine("Scrittura su file completata.");
            if(partitaInCorso != null)
            {
                Console.Write("Questa è la lista dei giocatori che parteciperanno all'asta:");
                for (int i = 0; i < partitaInCorso.GetGiocatori().Count; i++)
                {
                    Console.WriteLine(i + 1 + " - " + partitaInCorso.GetGiocatori()[i].nome);
                }
            }
            


        }
        #endregion
    }
}
