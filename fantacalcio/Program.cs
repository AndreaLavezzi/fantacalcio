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

        class Salvataggio
        {
            public void CreaSalvataggio(Fantacalcio fantacalcio)
            {

                string output = fantacalcio.nomeSalvataggio + ";" + fantacalcio.fase + ";" + JsonConvert.SerializeObject(fantacalcio.GetGiocatori(), Formatting.Indented);  /*viene creata una stringa che contiene il nome dell'istanza della partita
                                                                                                                                                                * appena creata e la lista di giocatori registrati che le appartiene; entrambe
                                                                                                                                                                * vengono convertite ad un file con estensione json tramite il metodo "SerializeObject"
                                                                                                                                                                * della classe JsonConvert della libreria Newtonsoft.Json*/
                File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + ".json", output);    //viene salvata la stringa convertita a json in un file con estensione .json
            }
            string[] GetFileSalvataggi()
            {
                string[] salvataggi = Directory.GetFiles("saveFiles/", "*.json");   //ottiene tutte le directory dei file con estensione json nella cartella saveFiles
                return salvataggi;
            }
            public List<Fantacalcio> GetPartite()
            {
                string[] salvataggi = GetFileSalvataggi();
                List<Fantacalcio> partite = new List<Fantacalcio>();    //lista di partite esistenti

                if (salvataggi.Length != 0)     //se esistono file di salvataggio
                {
                    for (int i = 0; i < salvataggi.Length; i++)
                    {
                        string input = File.ReadAllText(salvataggi[i]);
                        string[] words = input.Split(";");

                        string nomeSalvataggio = words[0];
                        int fase = Int32.Parse(words[1]);
                        List<Giocatore> giocatori = JsonConvert.DeserializeObject<List<Giocatore>>(words[2]);

                        partite.Add(new Fantacalcio(nomeSalvataggio, giocatori, fase));
                    }
                    return partite;
                }
                else
                {
                    return null;
                }
            }
            public string MostraSalvataggi()
            {
                if(GetPartite() == null)
                {
                    return "Non esistono file di salvataggio";
                }
                else
                {
                    List<Fantacalcio> partite = GetPartite();
                    string stringaPartite = "";
                    for (int i = 0; i < partite.Count; i++)
                    {
                        stringaPartite += i + 1 + " -> " + partite[i].nomeSalvataggio + "\n";
                    }
                    return stringaPartite;
                }
            }
            public int EliminaSalvataggio(Fantacalcio partita)
            {
                string daEliminare = "saveFiles/" + partita.nomeSalvataggio + ".json";
                if (File.Exists(daEliminare))
                {
                    File.Delete(daEliminare);
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        #endregion

        static string percorsoSalvataggi;
        static Fantacalcio partitaInCorso;  //assume il valore della partita caricata attualmente
        static Salvataggio salvataggio = new Salvataggio();

        static void Main(string[] args)     //il main chiama solo il metodo che mostra il menù principale
        {
            Menu();
        }

        static void Menu()
        {
            Console.Clear();
            string risposta;    //contiene la risposta inserita dall'utente da tastiera
            bool nonValida;     //indica se la risposta inserita dall'utente rientra o meno nelle possibilità offerte
            Console.Write("1 - Inizia nuova partita\n2 - Gestisci partite\nRisposta: "); //se l'utente inserisce 1 verrà iniziata la procedura di creazione di una partita, se inserisce 2 verrà mostrata una lista di salvataggi su cui l'utente potrà eseguire varie azioni
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
                        CaricaFile();       //se l'utente inserisce "2" verrà mostrata la lista dei salvataggi esistenti su cui si potranno eseguire delle azioni)
                        break;
                    default:
                        nonValida = true;   //se l'utente inserisce qualcosa di non gestito, la risposta non sarà valida e verrà chiesto nuovamente l'inserimento di una risposta
                        Console.Write("Inserimento non valido. Reiserire: ");   //comunico all'utente che ciò che ha inserito non è valido e chiedo di reinserire un valore
                        break;
                }
            } while (nonValida == true);    //il ciclo do-while si ripete finchè la risposta non è valida
            
        }

        #region GestionePartita
        /*inizia una nuova partita creando un oggetto di tipo "Fantacalcio" che rappresenta la partita.*/
        static void NuovaPartita()
        {
            Console.Clear();
            //Salvataggio salvataggio = new Salvataggio();
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
                } while (!ControlloNome(-1, nomeTorneo, new List<Giocatore>()));    //il ciclo do-while si ripete finchè il controllo non va a buon fine, la lista è vuota e serve solamente a chiamare la funzione

                Fantacalcio fantacalcio = new Fantacalcio(nomeTorneo, CreaGiocatori(), 0);     //viene creata un'istanza della classe salvataggio che rappresenta ciò che l'utente ha inserito

                salvataggio.CreaSalvataggio(fantacalcio);
                CaricaPartita(fantacalcio);
            }   
        }
        static void CaricaFile()
        {
            List<Fantacalcio> partite = salvataggio.GetPartite();
            if(partite == null)
            {
                Console.WriteLine("Non esistono file di salvataggio. Premi un tasto qualsiasi per continuare.");
                Console.ReadKey();
                Menu();
            }
            else
            {
                GestisciPartite(partite);
            }
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
        static string MostraPartite(List<Fantacalcio> partite)
        {
            string partiteDisponibili = "";
            for (int i = 0; i < partite.Count; i++)
            {
                partiteDisponibili += i + 1 + " -> " + partite[i].nomeSalvataggio + "\n";
            }
            return partiteDisponibili;
        }
        static void GestisciPartite(List<Fantacalcio> partite)
        {
            int indiceFile;
            string idFileSelezionato;
            Fantacalcio partitaSelezionata;

            bool nonValida = true;

            Console.Clear();
            Console.Write("Su quale salvataggio vuoi compiere un'azione?\n\nSalvataggi disponibili:\n" + MostraPartite(partite) + "\nRisposta: ");
            idFileSelezionato = Console.ReadLine();

            while (!int.TryParse(idFileSelezionato, out indiceFile) || indiceFile > partite.Count || indiceFile < 0)
            {
                Console.Write("\nRisposta non valida. Reinserire: ");
                idFileSelezionato = Console.ReadLine();
            }
            partitaSelezionata = partite[Int32.Parse(idFileSelezionato) - 1];
            Console.WriteLine("E' stato selezionato il file numero {0}, cosa vuoi fare?", idFileSelezionato);
            Console.WriteLine("1 - Carica File\n2 - Elimina File\n3 - Annulla selezione");
            Console.Write("Risposta: ");

            do
            {
                nonValida = false;
                string azione = Console.ReadLine();
                switch (azione)
                {
                    case "1":
                        CaricaPartita(partitaSelezionata);
                        break;
                    case "2":
                        EliminaPartita(partitaSelezionata);
                        break;
                    case "3":
                        GestisciPartite(partite);
                        break;
                    default:
                        nonValida = true;
                        Console.Write("\nRisposta non valida; Reinserire: ");
                        break;

                }
            } while (nonValida == true);
        }

        static void EliminaPartita(Fantacalcio partita)
        {
            salvataggio.EliminaSalvataggio(partita);
            switch (salvataggio.EliminaSalvataggio(partita))
            {
                case 1:
                    Console.WriteLine("File eliminato. Premi un tasto qualsiasi per continuare.");
                    break;
                case -1:
                    Console.WriteLine("File non esistente. Premi un tasto qualsiasi per continuare.");
                    break;
            }
            Console.ReadKey();
            CaricaFile();
        }

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
            //Console.WriteLine("Caricamento dei giocatori, Attendere...");
            //string[] inputCalciatori = File.ReadAllLines("calciatori.txt");
            //List<Calciatore> listaCalciatori = new List<Calciatore>();
            //for(int i = 0; i < inputCalciatori.Length; i++)
            //{
            //    listaCalciatori.Add(new Calciatore(inputCalciatori[i].Split(",")[0], inputCalciatori[i].Split(",")[1]));
            //}
            //Console.Clear();
            //Console.WriteLine("Caricamento Completato.");
            //Console.WriteLine("Inizio serializzazione lista...");
            //string output = JsonConvert.SerializeObject(listaCalciatori, Formatting.Indented);
            //Console.WriteLine("Serializzazione completata.");
            //File.WriteAllText("newCalciatori.txt", output);
            //Console.WriteLine("Scrittura su file completata.");
            //if(partitaInCorso != null)
            //{
            //    Console.Write("Questa è la lista dei giocatori che parteciperanno all'asta:");
            //    for (int i = 0; i < partitaInCorso.GetGiocatori().Count; i++)
            //    {
            //        Console.WriteLine(i + 1 + " - " + partitaInCorso.GetGiocatori()[i].nome);
            //    }
            //}

            Console.WriteLine("Il gioco è ancora in via di sviluppo. Verrai riportato al menu principale.");
            Menu();
            
        }
        #endregion
    }
}
