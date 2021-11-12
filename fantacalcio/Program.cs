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
            public double GeneraAzioni()
            {
                double punti = 0;
                int pesoMassimo = 0;
                List<Azione> azioni = JsonConvert.DeserializeObject<List<Azione>>(File.ReadAllText(ruolo + ".json"));
                Random random = new Random();
                int numeroAzioni = random.Next(100);
                foreach(Azione azione in azioni)
                {
                    pesoMassimo += azione.peso;
                }

                for(int i = 0; i < numeroAzioni; i++)
                {
                    int azioneRandom = random.Next(azioni.Count);
                    int eseguiAzione = random.Next(100);
                    int probabilità = (int)Math.Round((double)(azioni[azioneRandom].peso * 100) / pesoMassimo);
                    if(eseguiAzione <= probabilità)
                    {
                        punti += azioni[azioneRandom].punteggio;
                    }
                }
                return punti;
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
            public int punteggio { get; set; }       //punteggio che decreterà il vincitore finale della partita
            public int fantaMilioni { get; set; }                   //crediti a disposizione del giocatore, usati per comprare i calciatori
            public bool primaPartita { get; set; }
            List<Calciatore> rosa = new List<Calciatore>();
            List<Calciatore> titolari = new List<Calciatore>();
            List<Calciatore> squadra = new List<Calciatore>();

            public Giocatore(string nome)       //metodo costruttore, riceve in ingresso il nome del giocatore 
            {
                this.nome = nome;
                fantaMilioni = 500;
                primaPartita = true;
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
            public void AddPunteggio(int punti)
            {
                punteggio += punti;
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
            public void SetSquadraAttuale(List<Calciatore> squadra)
            {
                this.squadra = squadra;
            }
            public List<Calciatore> GetSquadraAttuale()
            {
                return squadra;
            }
            public string GetStringSquadra(string gruppo)
            {
                string stringa = "";
                List<Calciatore> daControllare = new List<Calciatore>();

                if (gruppo == "r" && rosa != null)
                {
                    daControllare = rosa;
                }
                else if (gruppo == "t" && titolari != null)
                {
                    daControllare = titolari;
                }
                else if (gruppo == "a" && squadra != null)
                {
                    daControllare = squadra;
                }

                for (int i = 0; i < daControllare.Count; i++)
                {
                    stringa += i + 1 + $" -> Nome: {daControllare[i].nome}, Ruolo: {daControllare[i].ruolo}\n";
                }
                return stringa;
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
                List<Calciatore> daControllare = new List<Calciatore>();
                if(gruppo == "r" && rosa != null)
                {
                    daControllare = rosa;
                }
                else if (gruppo == "t" && titolari != null)
                {
                    daControllare = titolari;
                }
                else if(gruppo == "a" && squadra != null)
                {
                    daControllare = squadra;
                }
                foreach (Calciatore calciatore in daControllare)
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
            public void SalvaAbbinamenti(Fantacalcio fantacalcio, Giocatore[,] abbinamenti)
            {
                string output = JsonConvert.SerializeObject(abbinamenti, Formatting.Indented);
                File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/abbinamenti.json", output);
            }
            public Giocatore[,] CaricaAbbinamenti(Fantacalcio fantacalcio)
            {
                string input = File.ReadAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/abbinamenti.json");
                Giocatore[,] abbinamenti = JsonConvert.DeserializeObject<Giocatore[,]>(input);
                return abbinamenti;
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

                            if(File.Exists(cartellaGiocatori + "/titolari.json"))
                            {
                                string fileInput = File.ReadAllText(cartellaGiocatori + "/titolari.json");
                                List<Calciatore> listaCalciatori = JsonConvert.DeserializeObject<List<Calciatore>>(fileInput);
                                foreach(Calciatore calciatore in listaCalciatori)
                                {
                                    partite[i].GetGiocatori()[j].AddTitolari(calciatore);
                                }   
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
                    InizioTorneo();
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
                    int soldiPuntati;
                    bool success;
                    do
                    {
                        success = true;
                        soldiPuntati = InserimentoOfferta(creditiGiocatore, puntataMaggiore);
                        if(soldiPuntati > giocatoreSelezionato.fantaMilioni - (25 - giocatoreSelezionato.GetGiocatoriRuolo("tot", "r")))
                        {
                            Console.Write("Non puoi inserire un numero di fantamilioni tale che ti renda incapacitato di comprare ulteriori giocatori; Reinserire: ");
                            success = false;
                        }

                    } while (!success);
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
                if (!nonValida && (indice < 0 || puntataMaggiore == 0 && indice > giocatori.Count + 1 || puntataMaggiore != 0 && indice > giocatori.Count || puntataMaggiore == 0 && indice == 0))
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
                Console.Write("\nInserisci il modulo che vuoi utilizzare (Esempio: 2-4-4 => 2 -> Difensori, 4 -> Centrocampisti, 4 => Attaccanti)\nRisposta: ");
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
                        else if(modulo[2] > 6)
                        {
                            Console.Write("\nImpossibile mettere più di 6 in posizione 3 del modulo. Reinserire: ");
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
            Console.Write(giocatoreCorrente.GetStringSquadra("r"));
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
                indice = OttieniIndiceSquadra(giocatoreCorrente.GetRosa());
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

        static int OttieniIndiceSquadra(List<Calciatore> squadra)
        {
            bool success;
            int indice;

            do
            {
                success = Int32.TryParse(Console.ReadLine(), out indice);
                if (indice > squadra.Count || indice < 1)
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
            int numeroPartite = giocatori.Count * (giocatori.Count - 1) / 2;
            int indicePartita = 0;
            Giocatore[,] abbinamenti = new Giocatore[numeroPartite, 2];

            for (int i = 0; i < giocatori.Count; i++)
            {
                for (int j = i; j < giocatori.Count; j++)
                {
                    if(i != j)
                    {
                        abbinamenti[indicePartita, 0] = giocatori[i];
                        abbinamenti[indicePartita, 1] = giocatori[j];
                        indicePartita++;
                    }
                }
            }

            salvataggio.SalvaAbbinamenti(partitaInCorso, abbinamenti);
        }

        static void InizioTorneo()
        {
            Giocatore[,] abbinamenti;
            if(!File.Exists("saveFiles/" + partitaInCorso.nomeSalvataggio + "/abbinamenti.json"))
            {
                GeneraAbbinamenti();
            }
            abbinamenti = salvataggio.CaricaAbbinamenti(partitaInCorso);

            for (int i = 0; i < abbinamenti.GetLength(0); i++)
            {
                foreach(Giocatore giocatore in partitaInCorso.GetGiocatori())
                {
                    if(giocatore.nome == abbinamenti[i, 0].nome)
                    {
                        abbinamenti[i, 0] = giocatore;
                    }
                    else if(giocatore.nome == abbinamenti[i, 1].nome)
                    {
                        abbinamenti[i, 1] = giocatore;
                    }
                }
                PrePartita(ref abbinamenti[i, 0], ref abbinamenti[i, 1]);
            }
        }

        static void PrePartita(ref Giocatore giocatore1, ref Giocatore giocatore2)
        {
            string nomeG1 = giocatore1.nome.ToUpper();
            string nomeG2 = giocatore2.nome.ToUpper();
            bool success;
            Console.Clear();
            Console.WriteLine("Sta per avere inizio la partita tra {0} e {1}", nomeG1, nomeG2);
            for(int i = 0; i < 2; i++)
            {
                Giocatore giocatoreCorrente;
                string nome;
                if(i == 0)
                {
                    giocatoreCorrente = giocatore1;
                    nome = nomeG1;
                }
                else
                {
                    giocatoreCorrente = giocatore2;
                    nome = nomeG2;
                }
                Console.Write("\n{0}, Quale squadra vuoi usare per questa partita?\n1 -> Solo titolari\n2 -> Nuova formazione", nome);
                if (!giocatoreCorrente.primaPartita)
                {
                    Console.Write("\n3 -> Usa formazione partita precedente");
                }
                Console.Write("\nRisposta: ");

                do
                {
                    success = true;
                    switch (Console.ReadLine())
                    {
                        case "1":
                            giocatoreCorrente.SetSquadraAttuale(giocatoreCorrente.GetTitolari());
                            break;
                        case "2":
                            giocatoreCorrente.SetSquadraAttuale(ModificaSquadra(giocatoreCorrente));
                            break;
                        case "3":
                            if (giocatoreCorrente.primaPartita)
                            {
                                Console.Write("Inserire uno dei valori proposti");
                                success = false;
                            }
                            else
                            {
                                giocatoreCorrente.GetSquadraAttuale();
                            }
                            break;
                        default:
                            Console.Write("Inserire uno dei valori proposti");
                            success = false;
                            break;
                    }
                } while (!success);

                if(i == 0)
                {
                    giocatore1 = giocatoreCorrente;
                }
                else
                {
                    giocatore2 = giocatoreCorrente;
                }
                
            }
            Partita(ref giocatore1, ref giocatore2);

        }

        static List<Calciatore> ModificaSquadra(Giocatore giocatoreCorrente)
        {
            if (giocatoreCorrente.primaPartita)
            {
                giocatoreCorrente.SetSquadraAttuale(giocatoreCorrente.GetTitolari());
            }
            List<Calciatore> squadraAttuale = giocatoreCorrente.GetSquadraAttuale();
            List<Calciatore> rosaCalciatori = giocatoreCorrente.GetRosa();
            Calciatore calciatore1, calciatore2;

            int indice1, indice2;
            while (true)
            {
                Console.WriteLine("Squadra corrente:\n" + giocatoreCorrente.GetStringSquadra("a"));
                Console.WriteLine("Lista calciatori posseduti:\n" + giocatoreCorrente.GetStringSquadra("r"));

                Console.Write("Vuoi effettuare uno scambio? Inserisci 'exit' per annullare, inserisci altro per continuare.\nRisposta: ");
                if (Console.ReadLine().ToLower() == "exit")
                {
                    Console.Clear();
                    return squadraAttuale;
                }

                Console.Write("\nQuale calciatore dalla squadra corrente vuoi scambiare? \nRisposta: ");

                indice1 = OttieniIndiceSquadra(squadraAttuale);

                Console.Write("\nCon quale calciatore lo vuoi scambiare? \nRisposta: ");

                indice2 = OttieniIndiceSquadra(rosaCalciatori);

                calciatore1 = squadraAttuale[indice1 - 1];
                calciatore2 = rosaCalciatori[indice2 - 1];

                bool nomeDoppio = false; ;
                foreach (Calciatore calciatore in squadraAttuale)
                {
                    if (calciatore.nome == calciatore2.nome)
                    {
                        nomeDoppio = true;
                        break;
                    }
                }

                if (nomeDoppio)
                {
                    Console.WriteLine("Non è possibile scambiare un giocatore con un giocatore già presente nella squadra.");
                }
                else if (calciatore1.ruolo == "portiere" && calciatore2.ruolo != "portiere" || calciatore1.ruolo != "portiere" && calciatore2.ruolo == "portiere")
                {
                    Console.WriteLine("Non è possibile scambiare un portiere con qualcuno che non sia un portiere.");
                }
                else if (giocatoreCorrente.GetGiocatoriRuolo(calciatore1.ruolo, "a") == 1 && calciatore1.ruolo != calciatore2.ruolo)
                {
                    Console.WriteLine("Non è possibile avere nessun {0} nella squadra.", calciatore1.ruolo);
                }
                else
                {
                    Console.WriteLine("Stai per scambiare {0} con {1}. Premi INVIO per annullare; premi qualsiasi altro tasto per confermare.", calciatore1.nome.ToUpper(), calciatore2.nome.ToUpper());
                    if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("Hai annullato lo scambio. Premi un tasto qualsiasi per continuare...");
                    }
                    else
                    {
                        squadraAttuale[indice1 - 1] = calciatore2;
                        giocatoreCorrente.SetSquadraAttuale(squadraAttuale);
                        Console.WriteLine("Scambio completato. Premi un tasto qualsiasi per continuare...");
                    }

                }
                Console.ReadKey(true);
                Console.Clear();
            }
        }
        static void Partita(ref Giocatore giocatore1, ref Giocatore giocatore2)
        {
            double puntiG1 = 0;
            double puntiG2 = 0;
            foreach(Calciatore calciatore in giocatore1.GetSquadraAttuale())
            {
                puntiG1 += calciatore.GeneraAzioni();
            }
            foreach (Calciatore calciatore in giocatore2.GetSquadraAttuale())
            {
                puntiG2 += calciatore.GeneraAzioni();
            }
            if(puntiG1 == puntiG2)
            {
                Console.WriteLine("PAREGGIO!");
                giocatore1.AddPunteggio(1);
                giocatore2.AddPunteggio(1);
            }else if(puntiG1 > puntiG2)
            {
                Console.WriteLine("VINCE {0} CON {1} PUNTI!", giocatore1.nome.ToUpper(), puntiG1);
                giocatore1.AddPunteggio(3);
            }
            else
            {
                Console.WriteLine("VINCE {0} CON {1} PUNTI!!", giocatore2.nome.ToUpper(), puntiG2);
                giocatore2.AddPunteggio(3);
            }
            Console.WriteLine("\nPremi un tasto qualsiasi per continuare...");
            Console.ReadKey(true);
        }

        static string GetStringSquadra(List<Calciatore> squadra)
        {
            string stringSquadra = "";
            for (int i = 0; i < squadra.Count; i++)
            {
                stringSquadra += i + 1 + $" -> Nome: {squadra[i].nome}, Ruolo: {squadra[i].ruolo}\n";
            }
            return stringSquadra;
        }

        #endregion

        #endregion
    }
}
