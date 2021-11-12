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

            public override string ToString()
            {
                return $"Nome: {nome.ToUpper()}\nRuolo: {ruolo.ToUpper()}";
            }
        }

        //coloro che giocano al fantacalcio, hanno un nome, un punteggio, dei crediti, una lista di giocatori posseduti, possono comprare giocatori
        class Giocatore
        {
            public string nome { get; }         //identifica il giocatore 
            public int punteggio { get; }       //punteggio che decreterà il vincitore finale della partita
            public int fantaMilioni { get; set; }                   //crediti a disposizione del giocatore, usati per comprare i calciatori
            List<Calciatore> rosa = new List<Calciatore>();
            List<Calciatore> titolari = new List<Calciatore>();

            public Giocatore(string nome)       //metodo costruttore, riceve in ingresso il nome del giocatore 
            {
                this.nome = nome;
                fantaMilioni = 500;
            }

            public void CaricaLista(List<Calciatore> calciatori)
            {
                rosa = calciatori;
            }

            public void AddCalciatore(Calciatore calciatore, int prezzo)
            {
                rosa.Add(calciatore);
                fantaMilioni -= prezzo;
            }
            public List<Calciatore> GetRosa()
            {
                return rosa;
            }
            public List<Calciatore> GetTitolari()
            {
                return titolari;
            }
            public void AddTitolari(Calciatore calciatore)
            {
                titolari.Add(calciatore);
            }
            public string GetStringRosa()
            {
                string stringRosa = "";
                for(int i = 0; i < rosa.Count; i++)
                {
                    stringRosa += i + 1 + $" -> Nome: {rosa[i].nome}, Ruolo: {rosa[i].ruolo}\n";
                }
                return stringRosa;
            }
            public override string ToString()
            {
                return $"Nome: {nome.ToUpper()}, Crediti: {fantaMilioni}";
            }

            public int GetGiocatoriRuolo(string ruolo, string gruppo)
            {
                int portieri = 0;
                int difensori = 0;
                int attaccanti = 0;
                int centrocampisti = 0;
                if(gruppo == "r" && rosa != null)
                {
                    foreach(Calciatore calciatore in rosa)
                    {
                        switch (calciatore.ruolo)
                        {
                            case "portiere":
                                portieri++;
                                break;
                            case "attaccante":
                                attaccanti++;
                                break;
                            case "centrocampista":
                                centrocampisti++;
                                break;
                            case "difensore":
                                difensori++;
                                break;
                        }
                    }
                }
                if (gruppo == "t" && titolari != null)
                {
                    foreach (Calciatore calciatore in titolari)
                    {
                        switch (calciatore.ruolo)
                        {
                            case "portiere":
                                portieri++;
                                break;
                            case "attaccante":
                                attaccanti++;
                                break;
                            case "centrocampista":
                                centrocampisti++;
                                break;
                            case "difensore":
                                difensori++;
                                break;
                        }
                    }
                }
                switch (ruolo)
                {
                    case "portiere":
                        return portieri;
                    case "attaccante":
                        return attaccanti;
                    case "centrocampista":
                        return centrocampisti;
                    case "difensore":
                        return difensori;
                    case "tot":
                        return rosa.Count;
                    default:
                        return -1;
                }
            }
        }

        //classe che rappresenta una partita, ha un nome, una lista di giocatori registrati e un metodo che ritorna la lista di giocatori
        class Fantacalcio
        {
            public string nomeSalvataggio { get; }      //nome del salvataggio che verrà assegnato al file di salvataggio
            public int fase { get; set; }  //fase 0 -> appena creata, fase 1 -> asta finita, fase 2 -> 
            List<Giocatore> giocatori = new List<Giocatore>();      //lista contenente i giocatori registrati nella partita corrente  
            
            public Fantacalcio(string nomeSalvataggio, List<Giocatore> giocatori, int fase)   //metodo costruttore, ottiene in ingresso il nome del salvataggio e la lista dei giocatori registrati
            {
                this.nomeSalvataggio = nomeSalvataggio;
                this.giocatori = giocatori;
                this.fase = fase;
            }

            public string GetListaGiocatori()
            {
                string stringaGiocatori = "";
                for(int i = 0; i < giocatori.Count; i++)
                {
                    stringaGiocatori += "\n" + ( i + 1 ) + " -> " + giocatori[i].ToString();
                }
                return stringaGiocatori;
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
                string directorySalvataggi = "saveFiles/";
                if (!Directory.Exists(directorySalvataggi))
                {
                    Directory.CreateDirectory(directorySalvataggi);
                }
                if (!Directory.Exists(directorySalvataggi + fantacalcio.nomeSalvataggio))
                {
                    Directory.CreateDirectory(directorySalvataggi + fantacalcio.nomeSalvataggio);
                }
                if(!File.Exists(directorySalvataggi + fantacalcio.nomeSalvataggio + "/calciatoriDisponibili.json"))
                {
                    File.WriteAllText(directorySalvataggi + fantacalcio.nomeSalvataggio + "/calciatoriDisponibili.json", File.ReadAllText("calciatori.json"));
                }
                string output = fantacalcio.nomeSalvataggio + ";" + fantacalcio.fase + ";" + JsonConvert.SerializeObject(fantacalcio.GetGiocatori(), Formatting.Indented);  /*viene creata una stringa che contiene il nome dell'istanza della appena creata e la lista di giocatori registrati che le appartiene; vengono convertite ad un file con estensione json tramite il metodo "SerializeObject della classe JsonConvert della libreria Newtonsoft.Json*/
                SalvaSquadre(fantacalcio);

                File.WriteAllText(directorySalvataggi + fantacalcio.nomeSalvataggio + "/saveFile.json", output);    //viene salvata la stringa convertita a json in un file con estensione .json
            }
            public void SalvaCalciatoriDisponibili(List<Calciatore> calciatoriDisponibili, Fantacalcio fantacalcio)
            {
                string output = JsonConvert.SerializeObject(calciatoriDisponibili, Formatting.Indented);
                File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/calciatoriDisponibili.json", output);
            }
            void SalvaSquadre(Fantacalcio fantacalcio)
            {
                List<Giocatore> giocatori = fantacalcio.GetGiocatori();
                for (int i = 0; i < giocatori.Count; i++)
                {
                    string cartellaGiocatori = "saveFiles/" + fantacalcio.nomeSalvataggio + "/giocatori/" + giocatori[i].nome;
                    string file = cartellaGiocatori + "/rosa.json";
                    string listaCalciatori = JsonConvert.SerializeObject(giocatori[i].GetRosa(), Formatting.Indented);
                    if (!Directory.Exists(cartellaGiocatori))
                    {
                        Directory.CreateDirectory(cartellaGiocatori);
                    }

                    File.WriteAllText(file, listaCalciatori);
                }
            }
            public void SalvaTitolari(Fantacalcio fantacalcio)
            {
                List<Giocatore> giocatori = fantacalcio.GetGiocatori();
                for (int i = 0; i < giocatori.Count; i++)
                {
                    string cartellaGiocatori = "saveFiles/" + fantacalcio.nomeSalvataggio + "/giocatori/" + giocatori[i].nome;
                    string file = cartellaGiocatori + "/titolari.json";
                    string listaCalciatori = JsonConvert.SerializeObject(giocatori[i].GetTitolari(), Formatting.Indented);
                    if (!Directory.Exists(cartellaGiocatori))
                    {
                        Directory.CreateDirectory(cartellaGiocatori);
                    }

                    File.WriteAllText(file, listaCalciatori);
                }
            }
            string[] GetCartelleSalvataggi()
            {
                string[] cartelleSalvataggi = Directory.GetDirectories("saveFiles/");
                return cartelleSalvataggi;
            }
            string[] GetFileSalvataggi()
            {
                string[] salvataggi = new string[GetCartelleSalvataggi().Length];

                for(int i = 0; i < GetCartelleSalvataggi().Length; i++)
                {
                    salvataggi[i] = GetCartelleSalvataggi()[i] + "/saveFile.json";
                }
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

                    for(int i = 0; i < partite.Count; i++)
                    {
                        for (int j = 0; j < partite[i].GetGiocatori().Count; j++)
                        {
                            string cartellaGiocatori = "saveFiles/" + partite[i].nomeSalvataggio + "/giocatori/" + partite[i].GetGiocatori()[j].nome;
                            Giocatore giocatoreCorrente = partite[i].GetGiocatori()[j];
                            if (File.Exists(cartellaGiocatori + "/rosa.json"))
                            {
                                string fileInput = File.ReadAllText(cartellaGiocatori + "/rosa.json");
                                List<Calciatore> listaCalciatori = JsonConvert.DeserializeObject<List<Calciatore>>(fileInput);
                                partite[i].GetGiocatori()[j].CaricaLista(listaCalciatori);
                            }
                        }
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
                string daEliminare = "saveFiles/" + partita.nomeSalvataggio;
                if (Directory.Exists(daEliminare))
                {
                    Directory.Delete(daEliminare, true);
                    return 1;
                }
                return -1;
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
            if(Directory.GetDirectories("saveFiles/").Length >= 3)  //se esistono già 3 file di salvataggio si impedisce di crearne di nuovi
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
            }
            Console.WriteLine("Premere un tasto per continuare...");
            Console.ReadKey();
            Menu();
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
                    SelezioneTitolari();
                    break;
                case 2:
                    GeneraAbbinamenti();
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
            Console.WriteLine("\nE' stato selezionato il file numero {0}, cosa vuoi fare?", idFileSelezionato);
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
                    if (nomeDaControllare.ToLower() == giocatore.nome.ToLower())
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
        #region Asta
        static void Asta()
        {
            string fileCalciatori = File.ReadAllText("saveFiles/" + partitaInCorso.nomeSalvataggio + "/calciatoriDisponibili.json");
            List<Calciatore> calciatoriDisponibili = JsonConvert.DeserializeObject<List<Calciatore>>(fileCalciatori);
            
            while (!controlloAsta())
            {
                Random random = new Random();
                Calciatore calciatoreEstratto = calciatoriDisponibili[random.Next(0, calciatoriDisponibili.Count)];

                Console.Clear();
                Offerte(calciatoreEstratto, ref calciatoriDisponibili);
                salvataggio.SalvaCalciatoriDisponibili(calciatoriDisponibili, partitaInCorso);
            }
            partitaInCorso.fase = 1;
            salvataggio.CreaSalvataggio(partitaInCorso);
            SelezioneTitolari();
        }

        static bool controlloAsta()
        {
            List<Giocatore> giocatori = partitaInCorso.GetGiocatori();
            foreach(Giocatore giocatore in giocatori)
            {
                if(giocatore.GetGiocatoriRuolo("tot", "r") < 25)
                {
                    return false;
                }
            }
            return true;
        }

        static void Offerte(Calciatore calciatore, ref List<Calciatore> calciatoriDisponibili)
        {
            int indice = -1;
            int puntataMaggiore = 0;
            int creditiGiocatore;

            List<Giocatore> giocatori = partitaInCorso.GetGiocatori();
            Giocatore maggiorOfferente = new Giocatore("PLACEHOLDER");
            Giocatore giocatoreSelezionato = new Giocatore("PLACEHOLDER");

            

            while (indice != 0) //se si vuole effettuare una ulteriore puntata
            {
                Console.Clear();
                

                if (puntataMaggiore == 0) //e se si tratta della prima puntata
                {
                    Console.WriteLine($"Ha inizio l'asta per: \n{calciatore.ToString()}");
                    Console.WriteLine("\nQuale giocatore vuole fare un'offerta per questo calciatore?");
                }
                else
                {
                    Console.WriteLine($"Continua l'asta per: \n{calciatore.ToString()}");
                    Console.WriteLine($"La puntata maggiore attuale è di {puntataMaggiore} fantamilioni da parte di {maggiorOfferente.nome.ToUpper()}");
                    Console.WriteLine("\nQuale giocatore vuole offrire di più?");
                    Console.Write("\n0 -> Nessuna ulteriore offerta");
                }

                Console.Write(partitaInCorso.GetListaGiocatori());

                if(puntataMaggiore == 0)
                {
                    Console.Write("\n" + (giocatori.Count + 1) + " -> Salta giocatore\n");
                }

                Console.Write("\nRisposta: ");
                indice = OttieniIndiceOfferente(giocatori, puntataMaggiore);

                if(indice == 0 && puntataMaggiore != 0)
                {
                    break;
                } 
                else if(indice == giocatori.Count + 1 && puntataMaggiore == 0)
                {
                    return;
                }

                giocatoreSelezionato = giocatori[indice - 1];
                creditiGiocatore = giocatoreSelezionato.fantaMilioni;
                
                if(!PuoComprare(calciatore.ruolo, giocatoreSelezionato.GetGiocatoriRuolo(calciatore.ruolo, "r")))
                {
                    Console.WriteLine("Non puoi comprare più calciatori di questo ruolo; premi un tasto per tornare indietro...");
                    Console.ReadKey();
                }
                else
                {
                    Console.Write("\n(Scrivere 'exit' per tornare indietro) Quanti soldi vuoi puntare? Risposta: ");
                    int soldiPuntati = InserimentoOfferta(creditiGiocatore, puntataMaggiore);
                    if (soldiPuntati != -1)
                    {
                        puntataMaggiore = soldiPuntati;
                        maggiorOfferente = giocatoreSelezionato;
                    }
                }
            }

            Console.WriteLine("\nL'asta per il calciatore: \n{0} \nE' stata vinta da {1} per la{2} cifra di {3} fantamilioni!", calciatore.ToString(), maggiorOfferente.nome.ToUpper(), StimaPrezzo(puntataMaggiore), puntataMaggiore);
            Console.WriteLine("Premi un tasto per continuare...");
            Console.ReadKey();
            maggiorOfferente.AddCalciatore(calciatore, puntataMaggiore);
            calciatoriDisponibili.Remove(calciatore);
            salvataggio.CreaSalvataggio(partitaInCorso);
        }

        static int InserimentoOfferta(int creditiGiocatore, int puntataMaggiore)
        {
            int soldiPuntati = 0;
            bool nonValida;
            do
            {
                nonValida = false;
                string risposta = Console.ReadLine();
                if(risposta.ToLower() == "exit")
                {
                    return -1;
                } 
                else if(!Int32.TryParse(risposta, out soldiPuntati))
                {
                    Console.Write("\n(Scrivere 'exit' per tornare indietro) Ciò che hai inserito non è un numero intero. Reinserire: ");
                    nonValida = true;
                }

                if (!nonValida)
                {
                    if (soldiPuntati <= 0)
                    {
                        Console.Write("\nImpossibile inserire un numero di fantamilioni minore o uguale a 0. Reinserire: ");
                        nonValida = true;
                    }
                    else if (soldiPuntati > creditiGiocatore)
                    {
                        Console.Write("\nImpossibile inserire un numero di fantamilioni maggiore di quelli posseduti. Reinserire: ");
                        nonValida = true;
                    }
                    else if (soldiPuntati <= puntataMaggiore)
                    {
                        Console.Write("\nImpossibile inserire un numero di fantamilioni minore o uguale all'attuale puntata maggiore di {0} fantamilioni. \nReinserire: ", puntataMaggiore);
                        nonValida = true;
                    }
                }
            } while (nonValida);

            return soldiPuntati;
        }

        static int OttieniIndiceOfferente(List<Giocatore> giocatori, int puntataMaggiore)
        {
            int indice = 0;
            bool nonValida;

            do
            {
                nonValida = false;
                try
                {
                    indice = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.Write("\nRisposta non valida. Reinserire: ");
                    nonValida = true;
                }
                if (!nonValida && (indice < 0 || puntataMaggiore == 0 && indice > giocatori.Count + 1 || puntataMaggiore != 0 && indice > giocatori.Count))
                {
                    Console.Write("Risposta non valida; inserire uno dei valori proposti: ");
                    nonValida = true;
                }
            } while (nonValida);

            return indice;
        }

        static bool PuoComprare(string ruoloCalciatore, int numCalciatoriPosseduti)
        {
            int calciatoriMassimi = 0;

            switch (ruoloCalciatore)
            {
                case "attaccante":
                    calciatoriMassimi = 6;
                    break;
                case "difensore":
                    calciatoriMassimi = 8;
                    break;
                case "portiere":
                    calciatoriMassimi = 3;
                    break;
                case "centrocampista":
                    calciatoriMassimi = 8;
                    break;
            }
            if (numCalciatoriPosseduti < calciatoriMassimi)
            {
                return true;
            }
            return false;
        }

        static string StimaPrezzo(int puntata)
        {
            if(puntata < 10)
            {
                return " esigua";
            }
            else if(puntata > 10 && puntata < 100 && puntata != 69)
            {
                return " modesta";
            }
            else if(puntata == 69)
            {
                return " PAZZA";
            }
            else if(puntata >= 100 && puntata < 250)
            {
                return " goliardica";
            }
            else if(puntata >= 250 && puntata < 500 && puntata != 420)
            {
                return " altissima";
            }
            else if (puntata == 420)
            {
                return " SGRAVATA";
            }
            return "";
        }
        #endregion

        #region Pre-Partita
        static void SelezioneTitolari()
        {
            List<Giocatore> giocatori = partitaInCorso.GetGiocatori();
            Console.Clear();
            Console.WriteLine("Ha inizio la selezione dei giocatori titolari.");
            Console.WriteLine("Premi un qualsiasi tasto per continuare...");
            Console.ReadKey();
            
            for(int i = 0; i < giocatori.Count; i++)
            {
                int[] modulo = { 0, 0, 0};
                bool success;
                Giocatore giocatoreCorrente = giocatori[i];
                Console.Clear();
                Console.WriteLine("E' il turno di {0} di scegliere i titolari", giocatoreCorrente.nome.ToUpper());
                Console.Write("\nInserisci il modulo che vuoi utilizzare (Esempio: 2-4-5 => 2 -> Difensori, 4 -> Centrocampisti, 5 => Attaccanti)\nRisposta: ");
                do
                {
                    success = true;
                    try
                    {
                        string risposta = Console.ReadLine();
                        modulo = Array.ConvertAll(risposta.Split("-"), int.Parse);
                        if (modulo.Length != 3)
                        {
                            Console.Write("\nInserire tre elementi separati da trattini: ");
                            success = false;
                        } else if(modulo[0] + modulo[1] + modulo[2] != 10)
                        {
                            Console.Write("\nLa somma dei numeri deve essere 10; reinserire: ");
                            success = false;
                        }else if(modulo[0] <= 0 || modulo[1] <= 0 || modulo[2] <= 0)
                        {
                            Console.Write("\nI numeri non possono essere 0 o minori; reinserire: ");
                            success = false;
                        }
                    }
                    catch
                    {
                        Console.Write("\nInserimento non valido; reinserire: ");
                        success = false;
                    }
                } while (!success);


                if (giocatoreCorrente.GetGiocatoriRuolo("portiere", "t") == 0)
                {
                    ControlloRuolo(giocatoreCorrente, "portiere");
                }

                while (giocatoreCorrente.GetGiocatoriRuolo("difensore", "t") < modulo[0])
                {
                    ControlloRuolo(giocatoreCorrente, "difensore");
                }

                while (giocatoreCorrente.GetGiocatoriRuolo("centrocampista", "t") < modulo[1])
                {
                    ControlloRuolo(giocatoreCorrente, "centrocampista");
                }

                while (giocatoreCorrente.GetGiocatoriRuolo("attaccante", "t") < modulo[2])
                {
                    ControlloRuolo(giocatoreCorrente, "attaccante");
                }

            }
            Console.Clear();
            Console.WriteLine("La selezione dei titolari è stata completata. Ora inizierà la fase del torneo. Buona fortuna!");
            salvataggio.SalvaTitolari(partitaInCorso);
            partitaInCorso.fase = 2;
            salvataggio.CreaSalvataggio(partitaInCorso);
            Console.WriteLine("Premi un tasto qualsiasi per continuare...");
            Console.ReadKey();
            
        }

        static void MostraGiocatoriRosa(Giocatore giocatoreCorrente)
        {
            Console.Clear();
            Console.WriteLine("Giocatori disponibili:\n");
            Console.Write(giocatoreCorrente.GetStringRosa());
        }

        static void ControlloRuolo(Giocatore giocatoreCorrente, string ruolo)
        {
            Calciatore calciatoreScelto;
            bool success;
            int indice;
            MostraGiocatoriRosa(giocatoreCorrente);
            Console.Write($"\nScegli un {ruolo} titolare: ");
            do
            {
                success = true;
                indice = OttieniIndice(giocatoreCorrente);
                calciatoreScelto = giocatoreCorrente.GetRosa()[indice - 1];

                if (calciatoreScelto.ruolo != ruolo)
                {
                    Console.Write($"\nIl calciatore selezionato non è un {ruolo}. Reinserire: ");
                    success = false;
                }
                else
                {
                    foreach (Calciatore calciatore in giocatoreCorrente.GetTitolari())
                    {
                        if (calciatoreScelto.nome == calciatore.nome)
                        {
                            Console.Write("Impossibile inserire un giocatore già scelto. Reinserire: ");
                            success = false;
                        }
                    }
                }
            } while (!success);
            Console.WriteLine("Hai selezionato {0}!\nPremi un tasto per continuare...", calciatoreScelto.nome.ToUpper());
            Console.ReadKey();
            giocatoreCorrente.AddTitolari(giocatoreCorrente.GetRosa()[indice - 1]);
        }

        static int OttieniIndice(Giocatore giocatoreCorrente)
        {
            bool success;
            int indice;

            do
            {
                success = Int32.TryParse(Console.ReadLine(), out indice);
                if (indice > giocatoreCorrente.GetRosa().Count || indice < 1)
                {
                    Console.Write("\nRisposta non valida, reinserire: ");
                    success = false;
                }
            } while (!success);

            return indice;
        }

        #endregion

        #region Partite
        static void GeneraAbbinamenti()
        {
            List<Giocatore> giocatori = partitaInCorso.GetGiocatori();
            //g1 g2, g3, g4 a due a due
            //i = 0 
            for(int i = 1; i < giocatori.Count; i++)
            {
                for(int j = 0; j < giocatori.Count; j = j + 2)
                {
                    int team1 = j;
                    int team2 = j + i;
                    if(team1 >= giocatori.Count)
                    {
                        team1 -= giocatori.Count;
                    }else if(team2 >= giocatori.Count)
                    {
                        team2 -= giocatori.Count;
                    }
                    Console.WriteLine(team1 + " " + team2);
                }
            }
        }
        #endregion

        #endregion
    }
}
