/**
 * \file    Program.cs
 * \author  Sandstorm
 * \brief   Sistema di gestione del gioco del FANTACALCIO
 * \date    18/10/2021
 * Consegna: Progettare un sistema di gestione del FANTACALCIO.
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
        /**
         * \brief   Assume il valore della partita caricata attualmente
         */ 
        static Fantacalcio partitaInCorso;

        /**
         * \fn      void Main()
         * \brief   La funzione Main chiama il metodo Menu(), che mostra il menù principale.
         * \details La variabile è globale in modo che tutti i metodi abbiano accesso ad essa. Quando viene caricata una partita di tipo Fantacalcio da un file verrà assegnato il valore di quella partita a questa variabile.
         */
        static void Main()  
        {
            Menu();
        }

        /**
         * \fn      void Menu()
         * \brief   La funzione Menu permette all'utente di decidere se creare una nuova partita o visualizzare le partite esistenti.
         * \param   string risposta: contiene l'indice dell'azione che l'utente vuole compiere
         * \param   bool nonValida: indica se la risposta inserita dall'utente rientra o meno nelle possibilità offerte
         * \details Alla chiamata della funzione viene pulita la console e vengono mostrate le opzioni possibili. In un ciclo do si 
         * imposta la variabile NonValida a false e si chiede all'utente di inserire una risposta che sia "1" o "2". La risposta 
         * viene controllata con un'istruzione switch: se è 1 viene chiamata la funzione NuovaPartita(), se è 2 viene chiamata la 
         * funzione CaricaFile(). Nel caso default, quindi un caso non gestito, si comunica all'utente che la risposta non è valida 
         * e si imposta la variabile nonValida a true. Il ciclo si ripete quando la variabile NonValida è settata a true. 
         */
        static void Menu()
        {
            Console.Clear();
            string risposta;    
            bool nonValida;     
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
                        GestisciPartite();       //se l'utente inserisce "2" verrà mostrata la lista dei salvataggi esistenti su cui si potranno eseguire delle azioni)
                        break;
                    default:
                        nonValida = true;   //se l'utente inserisce qualcosa di non gestito, la risposta non sarà valida e verrà chiesto nuovamente l'inserimento di una risposta
                        Console.Write("Inserimento non valido. Reiserire: ");   //comunico all'utente che ciò che ha inserito non è valido e chiedo di reinserire un valore
                        break;
                }
            } while (nonValida == true);    //il ciclo do-while si ripete finchè la risposta non è valida
            
        }

        #region GestionePartita
        /**
         * \fn      NuovaPartita()
         * \brief   Inizia una nuova partita creando un oggetto di tipo "Fantacalcio" che rappresenta la partita.
         * \param   string nomeTorneo: Nome del torneo che verrà assegnato al salvataggio
         * \param   Fantacalcio fantacalcio: Salvataggio del torneo appena creato
         * \details La funzione NuovaPartita() pulisce la console, e subito dopo controlla quanti file di salvataggio esistono. Se sono 
         * più di tre, impedisce all'utente di creare nuovi file di salvataggio e viene comunicato di eliminare un salvataggio per crearne
         * uno nuovo. Se invece esistono meno di tre file, viene chiesto all'utente di inserire il nome del torneo. Dentro a un ciclo do-while
         * viene fatto inserire il nome, che viene immagazzinato nella variabile nomeTorneo. Il ciclo si ripete fino a che il controllo del nome
         * effettuato con la funzione ControlloNome() non restituisce 'true'. Una volta terminato l'inserimento viene creata un'istanza della classe
         * Fantacalcio, il cui costruttore chiede di inserire il nome del torneo, una lista di giocatori che partecipano al torneo (ritornati dalla 
         * funzione di tipo List<Giocatore> CreaGiocatori()), la fase della partita e il numero di partite giocate nel torneo. L'istanza viene poi
         * salvata su file dal metodo Salvataggio.CreaSalvataggio(), appartenente alla classe Salvataggio. Viene poi chiesto all'utente di premere 
         * un tasto qualsiasi per continuare.
         */
        static void NuovaPartita()
        {
            Fantacalcio fantacalcio;
            string nomeTorneo;  //indica il nome del torneo, e verrà assegnato come nome al file

            Console.Clear();
            if (Directory.GetDirectories("saveFiles/").Length >= 3)  //se esistono già 3 file di salvataggio si impedisce di crearne di nuovi
            {
                Console.WriteLine("Impossibile creare più di 3 file di salvataggio. Eliminarne per poterne creare di nuovi.");  //viene comunicato all'utente che non può creare più di 3 file di salvataggio
            }
            else //se invece è possibile creare un file
            {
                Console.Write("Inserire il nome del torneo: "); //viene chiesto all'utente di inserire da tastiera il nome del torneo
                do
                {
                    nomeTorneo = Console.ReadLine();    //inserimento da tastiera del nome da parte dell'utente
                } while (!ControlloNome(1, nomeTorneo, new List<Giocatore>()));    //il ciclo do-while si ripete finchè il controllo non va a buon fine, la lista è vuota e serve solamente a chiamare la funzione

                fantacalcio = new Fantacalcio(nomeTorneo, CreaGiocatori(), 0, 0); 

                Salvataggio.CreaSalvataggio(fantacalcio);
            }
            Console.WriteLine("Premere un tasto per continuare...");
            Console.ReadKey();
            Menu();
        }

        /**
         * \fn      void CaricaPartita(Fantacalcio partita)
         * \brief   Ottiene come argomento una partita salvata, la imposta come partita in corso e chiama una funzione diversa in base alla fase della partita
         * \param   Fantacalcio partita: Partita ricavata da un file di salvataggio
         * \details La funzione imposta alla variabile globale partitaInCorso il valore di partita, poi in base alla fase della partita in corso
         * l'istruzione switch chiama una funzione diversa; nel caso la fase sia 0 viene chiamata la funzione Asta(), nel caso la fase sia 1 viene
         * chiamata la funzione SelezioneTitolari(), nel caso la fase sia 3, viene chiamata la funzione FinePartita().
         */
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
                case 3:
                    FinePartita();
                    break;
            }
        }

        /**
         * \fn      string MostraPartite(List<Fantacalcio> partite)
         * \brief   Ottiene i nomi delle partite salvate e li immagazzina nella variabile partiteDisponibili  
         * \param   string partiteDisponibili: Stringa contenente una lista delle partite disponibili
         * \return  int: La funzione ritorna una stringa contenente la lista di partite salvate
         * \details La funzione contiene un ciclo for, che per ogni partita presente nella lista di partite aggiunge alla 
         * stringa partiteDisponibili una riga contenente l'indice della partita, dato dal numero della iterazione corrente 
         * del ciclo for sommato a 1, una freccia di corrispondenza e il nome della partita alla i-esima posizione della lista
         * di partite; alla fine della riga è presente un carattere di new line. Una volta terminato il ciclo for, viene ritornata
         * la stringa risultante.
         */
        static string MostraPartite(List<Fantacalcio> partite)
        {
            string partiteDisponibili = "";
            for (int i = 0; i < partite.Count; i++)
            {
                partiteDisponibili += i + 1 + " -> " + partite[i].nomeSalvataggio + "\n";
            }
            return partiteDisponibili;
        }

        /**
         * \fn      GestisciPartite()
         * \brief   La funzione mostra, se ne esistono, le partite disponibili, e permette all'utente di eseguire varie azioni su di esse.
         * \param   List<Fantacalcio> partite:
         * \param   int indiceFile: contiene la posizione del salvataggio selezionato nella lista di partite
         * \param   string idFileSelezionato: contiene l'id del salvataggio su cui l'utente vuole compiere un'azione. Corrisponde alla posizione del salvataggio nella lista di partite più 1
         * \param   Fantacalcio partitaSelezionata: Contiene il salvataggio selezionato dall'utente su cui si vuole compiere un'azione
         * \param   bool nonValida: Indica se la risposta inserita da tastiera dall'utente rientra nelle opzioni offerte
         * \param   string azione: Indica l'indice dell'azione che l'utente vuole compiere sul salvataggio
         * \details La funzione ottiene, tramite il metodo Salvataggio.GetPartite() della classe Salvataggio la lista di partite salvate.
         * Successivamente controlla se la lista di partite è uguale a null, il che vuol dire che non esistono file di salvataggio. Se non
         * ci sono partite salvate viene comunicato all'utente, che verrà riportato al menu principale. Se invece esistono partite salvate,
         * la console viene pulita e in un ciclo while viene chiesto all'utente di inserire l'id del salvataggio su cui vuole agire. Una 
         * volta inserito viene controllato che l'id sia un numero intero e che non sia un numero più alto del numero di partite salvate 
         * o minore o uguale a 0. Il ciclo while si ripete finchè ciò che viene inserito non è valido. Quando il valore inserito è valido,
         * si imposta alla variabile partitaSelezionata la partita in posizione dell'indice inserito dall'utente meno uno. Viene poi comunicato
         * il nome del torneo selezionato, viene mostrata una lista di azioni eseguibili sul salvataggio e viene chiesto all'utente di inserire
         * l'id dell'azione che vuole compiere. In un ciclo do-while l'utente può inserire ciò che desidera: se l'id inserito è 1, viene chiamata
         * la funzione CaricaPartita() per impostare la partita selezionata come partita in corso, se l'id inserito è 2 viene chiamata la funzione 
         * EliminaPartita() per eliminare la partita selezionata. Se l'id è 3 viene chiamata la funzione GestisciPartite() per annullare la selezione.
         * Se viene inserito qualcosa che non rientra in ciò che viene proposto, viene settata la variabile nonValida a true e viene ripetuto il ciclo.
         */
        static void GestisciPartite()
        {
            List<Fantacalcio> partite = Salvataggio.GetPartite();
            bool nonValida;
            string idFileSelezionato;
            int indiceFile;
            Fantacalcio partitaSelezionata;
            string azione;

            if (partite == null)
            {
                Console.WriteLine("Non esistono file di salvataggio. Premi un tasto qualsiasi per continuare.");
                Console.ReadKey();
                Menu();
            }

            Console.Clear();
            Console.Write("Su quale salvataggio vuoi compiere un'azione?\n\nSalvataggi disponibili:\n" + MostraPartite(partite) + "\nRisposta: ");
            idFileSelezionato = Console.ReadLine();

            while (!int.TryParse(idFileSelezionato, out indiceFile) || indiceFile > partite.Count || indiceFile <= 0)
            {
                Console.Write("\nRisposta non valida. Reinserire: ");
                idFileSelezionato = Console.ReadLine();
            }
            partitaSelezionata = partite[Int32.Parse(idFileSelezionato) - 1];
            Console.WriteLine("\nE' stato selezionato il torneo {0}, cosa vuoi fare?", partitaSelezionata.nomeSalvataggio);
            Console.WriteLine("1 - Carica File\n2 - Elimina File\n3 - Annulla selezione");
            Console.Write("Risposta: ");

            do
            {
                nonValida = false;
                azione = Console.ReadLine();
                switch (azione)
                {
                    case "1":
                        CaricaPartita(partitaSelezionata);
                        break;
                    case "2":
                        EliminaPartita(partitaSelezionata);
                        break;
                    case "3":
                        GestisciPartite();
                        break;
                    default:
                        nonValida = true;
                        Console.Write("\nRisposta non valida; Reinserire: ");
                        break;

                }
            } while (nonValida == true);
        }

        /**
         * \fn      EliminaPartita(Fantacalcio partita)
         * \brief   Comunica il risultato riguardo all'eliminazione di una partita
         * \param   Fantacalcio partita: la partita da eliminare
         * \details In base al risultato ottenuto dal metodo Salvataggio.EliminaSalvataggio(), viene comunicato se la partita da eliminare è stata eliminata.
         * Se il metodo chiamato ha restituito 1, viene comunicato che il file è stato eliminato con successo. Se il metodo ha restituito -1, viene comunicato 
         * che il file o non esiste, o non è riuscita l'eliminazione del file. Viene poi chiesta da parte dell'utente la pressione di un tasto qualsiasi per
         * tornare alla gestione delle partite.
         */
        static void EliminaPartita(Fantacalcio partita)
        {
            switch (Salvataggio.EliminaSalvataggio(partita))
            {
                case 1:
                    Console.WriteLine("File eliminato. Premi un tasto qualsiasi per continuare.");
                    break;
                case -1:
                    Console.WriteLine("File non esistente o eliminazione non avvenuta. Premi un tasto qualsiasi per continuare.");
                    break;
            }
            Console.ReadKey();
            GestisciPartite();
        }

        /**
         * \fn      bool ControlloNome(int codice, string nomeDaControllare, List<Giocatore> giocatori)
         * \brief   Controlla il nome inserito del giocatore o del torneo in modo che non contenga un certo numero o un certo tipo di caratteri, o che il nome esisti già.
         * \param   int codice: Indica se bisogna controllare il nome di un giocatore o di un torneo. 0 = Giocatore, 1 = Torneo
         * \param   string nomeDaControllare: Contiene il nome che bisogna controllare.
         * \param   List<Giocatore> giocatori: Contiene la lista di giocatori attualmente iscritti al torneo
         * \param   string caratteriSpeciali: Contiene i caratteri che non è possibile inserire nel nome
         * \param   List<Fantacalcio> partite: Contiene le partite salvate
         * \return  bool: La funzione ritorna true se il controllo è andato a buon fine, ritorna false se il controllo ha rilevato delle anomalie nel nome.
         * \details La funzione controlla inizialmente la lunghezza del nome da controllare. Se è minore di 4 o maggiore di 12 caratteri, viene comunicato
         * la lunghezza di caratteri che il nome dovrebbe avere, e la funzione ritorna false. Se il primo controllo va a buon fine, viene controllato, tramite
         * un doppio ciclo for incapsulato ogni carattere del nome inserito, e se il nome contiene un carattere speciale viene comunicato quale carattere non
         * è possibile inserire e la funzione ritorna false. In seguito, il codice inserito viene controllato da un'istruzione switch. Se il codice inserito è
         * pari a 0, viene controllato che il nome inserito non corrisponda a uno dei nomi già inseriti da un giocatore. Se il codice è pari a 1, viene 
         * controllato che il nome inserito non corrisponda a uno dei nomi dei salvataggi già creati. Se il codice inserito non è riconosicuto, viene comunicato
         * e la funzione ritorna false.
         */
        static bool ControlloNome(int codice, string nomeDaControllare, List<Giocatore> giocatori)
        {
            string caratteriSpeciali = "|\\!\"£$%&/()='?^<>[]{}*+@°#§ ";
            List<Fantacalcio> partite;

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
                        Console.Write("Impossibile inserire nel nome il carattere {0}. Reinserire: ", caratteriSpeciali[j].ToString());
                        return false;
                    }
                }
            }

            switch (codice)
            {
                case 0:
                    foreach (Giocatore giocatore in giocatori)
                    {
                        if (nomeDaControllare.ToLower() == giocatore.nome.ToLower())
                        {
                            Console.Write("Inserire un nome che non sia già stato scelto: ");
                            return false;
                        }
                    }
                    break;
                case 1:
                    partite = Salvataggio.GetPartite();
                    if(partite != null)
                    {
                        foreach(Fantacalcio partita in partite)
                        {
                            if(nomeDaControllare.ToLower() == partita.nomeSalvataggio.ToLower())
                            {
                                Console.Write("Inserire un nome che non sia già stato scelto: ");
                                return false;
                            }
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Attenzione! Codice non riconosciuto");
                    return false;
            }
            return true;
        }
        #endregion

        #region Gioco
        /**
         * \fn      List<Giocatore> CreaGiocatori()
         * \brief   Crea delle istanze della classe Giocatore e le inserisce in una lista, e alla fine restituisce la lista
         * \param   int numeroGiocatori: Il numero di giocatori che parteciperanno al torneo
         * \param   string nome: Il nome del giocatore
         * \param   List<Giocatore> giocatori: Lista contenente i giocatori che pareciperanno al torneo
         * \return  List<Giocatore>: La funzione ritorna una lista di giocatori
         * \details Viene chiesto all'utente quanti giocatori parteciperanno al torneo e viene comunicato il numero minimo di giocatori che possono partecipare.
         * In un ciclo while, che si ripete finchè ciò che l'utente inserisce non è un numero, o finchè il numero è minore di 2 o maggiore di 8, si comunica che
         * l'inserimento non è valido e viene chiesto di reinserire il dato. Una volta ottenuto il numero di giocatori, in un ciclo for che si ripete per un numero
         * di volte pari al numero di giocatori che partecipano al torneo si chiede, in un ciclo do-while, di inserire il nome del giocatore. Il ciclo si ripete
         * finchè la funzione ControlloNome() restituisce false. Se l'inserimento è corretto, una nuova istanza della classe Giocatore con nome uguale a quello 
         * inserito viene aggiunto alla lista di giocatori.
         */
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
                while (!ControlloNome(0, nome, giocatori));

                giocatori.Add(new Giocatore(nome));
            }

            return giocatori;
        }

        #region Asta
        /**
         * \fn      void Asta()
         * \brief   Dà inizio all'asta che permette ai giocatori di comprare dei calciatori da aggiungere alla propria rosa
         * \param   string fileCalciatori: Contenuto del file dei calciatori, che contiene una lista di calciatori disponibili convertita in un file .json
         * \param   List<Calciatore> calciatoriDisponibili: Lista di calciatori disponibili, ottenuta dalla conversione del contenuto dei file in una lista di calciatori
         * \param   Calciatore calciatoreEstratto: Calciatore estratto randomicamente dalla lista di calciatori disponibili, in modo da essere aquistato dai giocatori
         * \details Viene letto il contenuto del file contenente i calciatori disponibili per l'acquisto, contenuto nella cartella del salvataggio corrente
         * Viene poi convertito il contenuto del file json in una lista tramite il metodo JsonConvert.DeserializeObject() della classe JsonConvert, appartenente
         * alla libreria Newtonsoft.JSON. In un ciclo while, che si ripete finchè la funzione ControlloAsta() restituisce false, si estrae un numero randomico. Viene poi
         * chiamata la funzione Offerte() per permettere ai giocatori di offrire i propri crediti in modo da comprare il calciatore estratto. Una volta terminato
         * l'acquisto, viene salvato il file dei calciatori disponibili all'acquisto, da cui è stato rimosso il calciatore che eventualmente è stato comprato.
         * Una volta terminata l'asta, quindi dopo il ciclo while, viene impostata l'attributo "fase" della variabile partitaInCorso a 1, che indica che la partita è
         * ora nella fase di selezione dei titolari. Viene successivamente salvato lo stato della partita tramite il metodo Salvataggio.CreaSalvataggio(). Alla fine,
         * viene chiamata la funzione SelezioneTitolari().
         */
        static void Asta()
        {
            string fileCalciatori = File.ReadAllText("saveFiles/" + partitaInCorso.nomeSalvataggio + "/calciatoriDisponibili.json");
            List<Calciatore> calciatoriDisponibili = JsonConvert.DeserializeObject<List<Calciatore>>(fileCalciatori);
            Calciatore calciatoreEstratto;


            while (!ControlloAsta())
            {
                Random random = new Random();
                calciatoreEstratto = calciatoriDisponibili[random.Next(0, calciatoriDisponibili.Count)];

                Console.Clear();
                Offerte(calciatoreEstratto, ref calciatoriDisponibili);
                Salvataggio.SalvaCalciatoriDisponibili(calciatoriDisponibili, partitaInCorso);
            }
            partitaInCorso.fase = 1;
            Salvataggio.CreaSalvataggio(partitaInCorso);
            SelezioneTitolari();
        }

        /**
         * \fn      bool ControlloAsta()
         * \brief   Controlla che ci siano ancora giocatori che debbano comprare dei calciatori
         * \param   List<Giocatore> giocatori: Lista di giocatori registrati per il torneo
         * \return  bool: Ritorna false se ci sono ancora giocatori che possiedono meno di 25 giocatori, altrimenti ritorna true.
         * \details Viene popolata la lista di giocatori registrati restituita dal metodo Fantacalcio.GetGiocatori() appartenente alla classe Fantacalcio.
         * Successivamente, in un ciclo foreach, viene controllato il numero di giocatori in ogni rosa di ogni giocatore, tramite il metodo 
         * Giocatore.GetGiocatoriRuolo() della classe Giocatore, che restituisce il numero di giocatori totali della rosa del giocatore se nei parametri 
         * passati al metodo vengono inserite le stringhe "tot" e "r". Se il metodo restituisce un numero minore di 25, la funzione ritorna false. Se la
         * funzione termina il ciclo foreach senza ritornare nulla, ritorna true.
         */
        static bool ControlloAsta()
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

        /**
         * 
         */
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
            Salvataggio.CreaSalvataggio(partitaInCorso);
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
            partitaInCorso.fase = 2;
            Salvataggio.SalvaTitolari(partitaInCorso);
            Salvataggio.CreaSalvataggio(partitaInCorso);
            Console.WriteLine("Premi un tasto qualsiasi per continuare...");
            Console.ReadKey();
            InizioTorneo();
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

            Salvataggio.SalvaAbbinamenti(partitaInCorso, abbinamenti);
        }

        static void OrdinaAbbinamenti(ref Giocatore[,] abbinamenti, int numGiocatori)
        {
            for(int i = 0; i < numGiocatori/2; i++)
            {
                for(int j = i + 1; j < abbinamenti.GetLength(0); j++)
                {
                    
                }
            }
        }

        static void InizioTorneo()
        {
            Giocatore[,] abbinamenti;
            if(!File.Exists("saveFiles/" + partitaInCorso.nomeSalvataggio + "/abbinamenti.json"))
            {
                GeneraAbbinamenti();
            }
            abbinamenti = Salvataggio.CaricaAbbinamenti(partitaInCorso);

            for (int i; partitaInCorso.numeroPartita < abbinamenti.GetLength(0); partitaInCorso.numeroPartita++)
            {
                i = partitaInCorso.numeroPartita;
                
                //trova i giocatori che stanno per giocare nella lista di giocatori della partita in corso
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
                Partita(ref abbinamenti[i, 0], ref abbinamenti[i, 1]);

                //Salva stato giocatori
                for (int j = 0; j < partitaInCorso.GetGiocatori().Count; j++)
                {
                    if (partitaInCorso.GetGiocatori()[j].nome == abbinamenti[i, 0].nome)
                    {
                        partitaInCorso.GetGiocatori()[j] = abbinamenti[i, 0];
                    }
                    else if (partitaInCorso.GetGiocatori()[j].nome == abbinamenti[i, 1].nome)
                    {
                        partitaInCorso.GetGiocatori()[j] = abbinamenti[i, 1];
                    }
                }
            }
            FinePartita();
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
            bool success;
            Console.WriteLine("Squadra di {0}", giocatore1.nome.ToUpper());
            foreach(Calciatore calciatore in giocatore1.GetSquadraAttuale())
            {
                double punti = calciatore.GeneraAzioni();
                Console.WriteLine(calciatore.ToString() + "\nPUNTI: {0}\n", punti);
                puntiG1 += punti;
            }
            Console.WriteLine("\nTotale: {0} PUNTI", puntiG1);

            Console.WriteLine("\n\nSquadra di {0}", giocatore2.nome.ToUpper());
            foreach (Calciatore calciatore in giocatore2.GetSquadraAttuale())
            {
                double punti = calciatore.GeneraAzioni();
                Console.WriteLine(calciatore.ToString() + ": {0} punti", punti);
                puntiG2 += punti;
            }
            Console.WriteLine("\nTotale: {0} PUNTI", puntiG2);

            if (puntiG1 == puntiG2)
            {
                Console.WriteLine("\nPAREGGIO!");
                giocatore1.AddPunteggio(1);
                giocatore2.AddPunteggio(1);
            }else if(puntiG1 > puntiG2)
            {
                Console.WriteLine("\nVINCE {0} CON {1} PUNTI!", giocatore1.nome.ToUpper(), puntiG1);
                giocatore1.AddPunteggio(3);
            }
            else
            {
                Console.WriteLine("\nVINCE {0} CON {1} PUNTI!!", giocatore2.nome.ToUpper(), puntiG2);
                giocatore2.AddPunteggio(3);
            }
            Salvataggio.CreaSalvataggio(partitaInCorso);
            Console.Write("\n1 -> Visualizza classifica attuale\n2 -> Continua con la prossima partita\nRisposta: ");
            do
            {
                success = true;
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("\nCLASSIFICA ATTUALE:\n{0}", Classifica());
                        Console.WriteLine("\nPremi un tasto qualsiasi per continuare...");
                        Console.ReadKey(true);
                        break;
                    case "2":
                        break;
                    default:
                        Console.Write("Inserisci uno dei valori proposti: ");
                        success = false;
                        break;
                }
            } while (!success);
        }

        static void FinePartita()
        {
            Console.Clear();
            Console.Write("CLASSIFICA FINALE:\n {0}", Classifica());
            partitaInCorso.fase = 3;
            Salvataggio.CreaSalvataggio(partitaInCorso);
        }
        
        static string Classifica()
        {
            List<Giocatore> giocatori = partitaInCorso.GetGiocatori();
            giocatori = InsertionSort(giocatori);
            string classifica = "";
            for (int i = 0; i < giocatori.Count; i++)
            {
                classifica += i + 1 + " -> " + giocatori[i].nome + " con " + giocatori[i].punteggio + " punti\n";
            }
            return classifica;
        }

        static List<Giocatore> InsertionSort(List<Giocatore> giocatori)
        {
            for (int i = 0; i < giocatori.Count - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    int t = giocatori[j - 1].CompareTo(giocatori[j]);
                    if (t == -1)
                    {
                        Giocatore temp = giocatori[j - 1];
                        giocatori[j - 1] = giocatori[j];
                        giocatori[j] = temp;
                    }
                }
            }
            return giocatori;
        }



        #endregion

        #endregion
    }
}
