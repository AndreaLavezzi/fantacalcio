/**
 * \file    Salvataggio.cs
 * \author  Sandstorm
 * \brief   Sistema di gestione del gioco del FANTACALCIO
 * \date    18/10/2021
 */ 

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace fantacalcio
{
    /**
     * \class Salvataggio
     * \brief Classe che usa metodi statici per salvare su file o caricare informazioni da file
     */
    class Salvataggio
    {
        /**
         * \fn      public void CreaSalvataggio(Fantacalcio fantacalcio)
         * \brief   Salva le informazioni della partita che accetta come parametro in un file json
         * \param   Fantacalcio fantacalcio: La partita da salvare
         * \param   string directorySalvataggi: Stringa contenente la directory dove i file di salvataggio vengono salvati
         * \param   string output: Contiene i dati da scrivere su file
         * \details Innanzitutto viene controllato se esiste la directory dove verranno messi tutti i file di salvataggio. Se non esiste, viene creata col metodo Directory.CreateDirectory(). Se non esiste, viene
         * poi creata la directory contenente i file di salvataggio della partita in corso. Viene poi creato il file dei calciatori disponibili, sempre se non esiste, inserendo inizialmente tutti i calciatori presenti
         * nel file dei calciatori. Viene poi costruita la stringa di output: si salva il nome, la fase, il numero della partita e la lista di giocatori registrati, tutto diviso da dei punti e virgola. Viene scritto sul
         * file di salvataggio principale "saveFile.json", contenuto nella cartella col nome del salvataggio, il contenuto della variabile output. Vengono poi salvate le squadra dei giocatori tramite il metodo SalvaSquadre().
         */
        static public void CreaSalvataggio(Fantacalcio fantacalcio)
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
            if (!File.Exists(directorySalvataggi + fantacalcio.nomeSalvataggio + "/calciatoriDisponibili.json"))
            {
                File.WriteAllText(directorySalvataggi + fantacalcio.nomeSalvataggio + "/calciatoriDisponibili.json", File.ReadAllText("calciatori.json"));
            }

            string output = fantacalcio.nomeSalvataggio + ";" + fantacalcio.fase + ";" + fantacalcio.numeroPartita + ";" + JsonConvert.SerializeObject(fantacalcio.GetGiocatori(), Formatting.Indented);  /*viene creata una stringa che contiene il nome dell'istanza della appena creata e la lista di giocatori registrati che le appartiene; vengono convertite ad un file con estensione json tramite il metodo "SerializeObject della classe JsonConvert della libreria Newtonsoft.Json*/

            File.WriteAllText(directorySalvataggi + fantacalcio.nomeSalvataggio + "/saveFile.json", output);    //viene salvata la stringa convertita a json in un file con estensione .json
            SalvaSquadre(fantacalcio);
        }

        /**
         * \fn      public void SalvaCalciatoriDisponibili(List<Calciatore> calciatoriDisponibili, Fantacalcio fantacalcio)
         * \brief   Salva la lista di giocatori non acquistati in un file .json
         * \param   string output: Contiene i dati da scrivere su file
         * \details Nella stringa di output viene inserita la lista di calciatori disponibili convertita in formato Json dal metodo JsonConvert.SerializeObject(). Il suo contenuto viene poi scritto nel file
         * "calciatoriDisponibili.json" contenuto nella cartella di salvataggio della partita tramite il metodo File.WriteAllText().
         */
        static public void SalvaCalciatoriDisponibili(List<Calciatore> calciatoriDisponibili, Fantacalcio fantacalcio)
        {
            string output = JsonConvert.SerializeObject(calciatoriDisponibili, Formatting.Indented);
            File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/calciatoriDisponibili.json", output);
        }

        /**
         * \fn      void SalvaSquadre(Fantacalcio fantacalcio)
         * \param   Fantacalcio fantacalcio: Partita della quale bisogna salvare le squadre
         * \param   List<Giocatore> giocatori: La lista di giocatori che partecipano al torneo
         * \param   string cartellaGiocatori: Directory della cartella del singolo giocatore. Equivale a saveFiles/nomeTorneo/nomeGiocatore
         * \param   string file: Directory del file su cui viene salvata la squadra
         * \param   string listaCalciatori: Lista dei calciatori posseduti dal giocatore convertita in formato Json.
         * \details Viene popolata la lista di giocatori registrati tramite il metodo Fantacalcio.GetGiocatori(). In un ciclo for, che si ripete tante volte quanti sono i giocatori registrati, viene convertita la loro 
         * rosa di calciatori ottenuti tramite il metodo Giocatore.GetRosa() in formato Json tramite il metodo JsonConvert.SerializeObject(). Se la cartella del giocatore non esiste, viene creata tramite il metodo 
         * Directory.CreateDirectory(). Viene infine scritto sul file nella directory contenuta dalla stringa "file" la lista di calciatori convertita in Json.
         */
        static void SalvaSquadre(Fantacalcio fantacalcio)
        {
            List<Giocatore> giocatori = fantacalcio.GetGiocatori();
            string cartellaGiocatori, file, listaCalciatori;

            for (int i = 0; i < giocatori.Count; i++)
            {
                cartellaGiocatori = "saveFiles/" + fantacalcio.nomeSalvataggio + "/giocatori/" + giocatori[i].nome;
                file = cartellaGiocatori + "/rosa.json";
                listaCalciatori = JsonConvert.SerializeObject(giocatori[i].GetRosa(), Formatting.Indented);
                if (!Directory.Exists(cartellaGiocatori))
                {
                    Directory.CreateDirectory(cartellaGiocatori);
                }

                File.WriteAllText(file, listaCalciatori);
            }
        }

        /**
         * \fn      public void SalvaAbbinamenti(string nomeSalvataggio, Giocatore[,] abbinamenti)
         * \brief   Salva gli abbinamenti per le partite su file.
         * \param   string nomeSalvataggio: Il nome del torneo del quale si salvano gli abbinamenti
         * \param   Giocatore[,] abbinamenti Array bidimensionale dove ogni coppia di giocatori rappresenta un abbinamento che si sfiderà in una partita
         * \param   string Output: Contiene i dati che andranno scritti su file
         * \details Il metodo JsonConvert.SerializeObject converte l'array di abbinamenti in una stringa in formato .Json. Successivamente viene scritta sul file abbinamenti.json nella cartella del salvataggio.
         */
        static public void SalvaAbbinamenti(string nomeSalvataggio, Giocatore[,] abbinamenti)
        {
            string output = JsonConvert.SerializeObject(abbinamenti, Formatting.Indented);
            File.WriteAllText("saveFiles/" + nomeSalvataggio + "/abbinamenti.json", output);
        }
        
        /**
         * \fn      static public Giocatore[,] CaricaAbbinamenti(Fantacalcio fantacalcio)
         * \brief   Ottiene un array bidimensionale con gli abbinamenti per le partite dei giocatori da un file .json, e viene ritornato dal metodo.
         * \param   Fantacalcio fantacalcio: La partita per cui vanno caricati gli abbinamenti
         * \param   string input: Il testo ottenuto dal file di input
         * \param   Giocatore[,] abbinamenti: L'array bidimensionale contenente gli abbinamenti dei giocatori
         * \return  Giocatore[,]: Il metodo ritorna l'array bidimensionale di tipo Giocatore ottenuto dal file degli abbinamenti.
         * \details Il metodo legge il testo contenuto nel file abbinamenti.json presente nella cartella corrispondente al salvataggio ottenuto come parametro. Il testo viene poi convertito in un array bidimensionale tramite
         * il metodo JsonConvert.DeserializeObject(). Il metodo ritorna infine l'array bidimensionale.
         */
        static public Giocatore[,] CaricaAbbinamenti(Fantacalcio fantacalcio)
        {
            string input = File.ReadAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/abbinamenti.json");
            Giocatore[,] abbinamenti = JsonConvert.DeserializeObject<Giocatore[,]>(input);
            return abbinamenti;
        }

        /**
          * \fn      void SalvaTitolari(Fantacalcio fantacalcio)
          * \brief   Salva su file i calciatori titolari per ogni giocatore
          * \param   Fantacalcio fantacalcio: Partita della quale bisogna salvare le squadre
          * \param   List<Giocatore> giocatori: La lista di giocatori che partecipano al torneo
          * \param   string cartellaGiocatori: Directory della cartella del singolo giocatore. Equivale a saveFiles/nomeTorneo/nomeGiocatore
          * \param   string file: Directory del file su cui viene salvata la squadra
          * \param   string listaCalciatori: Lista dei calciatori titolari del giocatore convertita in formato Json.
          * \details Viene popolata la lista di giocatori registrati tramite il metodo Fantacalcio.GetGiocatori(). In un ciclo for, che si ripete tante volte quanti sono i giocatori registrati, vengono convertiti i loro 
          *  calciatori titolari ottenuti tramite il metodo Giocatore.GetTitolari() in formato Json tramite il metodo JsonConvert.SerializeObject(). Se la cartella del giocatore non esiste, viene creata tramite il metodo 
          *  Directory.CreateDirectory(). Viene infine scritto sul file nella directory contenuta dalla stringa "file" la lista di calciatori convertita in Json.
          */
        static public void SalvaTitolari(Fantacalcio fantacalcio)
        {
            List<Giocatore> giocatori = fantacalcio.GetGiocatori();
            string cartellaGiocatori, file, listaCalciatori;

            for (int i = 0; i < giocatori.Count; i++)
            {
                cartellaGiocatori = "saveFiles/" + fantacalcio.nomeSalvataggio + "/giocatori/" + giocatori[i].nome;
                file = cartellaGiocatori + "/titolari.json";
                listaCalciatori = JsonConvert.SerializeObject(giocatori[i].GetTitolari(), Formatting.Indented);
                if (!Directory.Exists(cartellaGiocatori))
                {
                    Directory.CreateDirectory(cartellaGiocatori);
                }

                File.WriteAllText(file, listaCalciatori);
            }
        }

        /**
         * \fn      public List<Fantacalcio> GetPartite()
         * \brief   Restituisce una lista di oggetti di tipo Fantacalcio che rappresentano le partite salvate ottenuta dai file salvati 
         * \param   string[] salvataggi: Un array di stringhe in cui ogni stringa è una directory di una partita salvata
         * \param   string[] datiPartita: I singoli dati di una partita, nel file sono divisi da dei punti e virgola
         * \param   string input: Testo conenuto in un file di salvataggio
         * \param   string nomeSalvataggio: Il nome del salvataggio da caricare
         * \param   string cartellaGiocatori: Directory della cartella del giocatore
         * \param   string fileInputSquadra: Il contenuto del file della lista di calciatori da caricare per il giocatore
         * \param   int fase: La fase della partita in corso, 0 = Asta, 1 = Selezione titolari, 2 = Partite, 3 = Fine torneo
         * \param   int numeroPartita: Il numero di sfide già giocate
         * \param   List<Fantacalcio> partite: La lista di partite salvate
         * \param   List<Giocatore> giocatori: La lista di giocatori registrati al torneo
         * \param   List<Calciatori> calciatori: La lista di calciatori appartenenti a una squadra
         * \param   Giocatore giocatoreCorrente: Il giocatore considerato all'attuale iterazione del ciclo for
         * \return  List<Fantacalcio>: Il metodo ritorna una lista di partite, ognuna contiene dei giocatori con i propri punti, le proprie squadre, i propri crediti
         * \details Inizialmente viene popolato l'array di directory con quello che il metodo GetFileSalvataggi() restituisce. Viene controllato che la lunghezza dell'array sia diversa da 0, che vuol dire che esiste
         * almeno un file di salvataggio. Se non esistono file di salvataggio, il metodo ritorna null. Se invece esistono, in un ciclo for che si ripete per ogni file si ottiene il contenuto di esso tramite il metodo
         * File.ReadAllText(). Il contenuto, in cui ogni dato è diviso da un punto e virgola, viene diviso dal metodo String.Split(). Il nome del salvataggio corrisponde al primo dato nel file. La fase al secondo. Le
         * partite giocate al terzo. La lista di giocatori al quarto però in una stringa in formato Json, per cui si converte in una lista tramite il metodo JsonConvert.DeserializeObject(). La partita ottenuta viene
         * aggiunta alla lista di partite tramite il metodo Add() delle liste. La partita aggiunta è creata tramite il metodo costruttore della classe Fantacalcio, a cui viene passato il nome, la lista di giocatori, la
         * fase e il numero di partite giocate. Per ogni giocatore in ogni partita bisogna impostare la squadra che gli appartiene. Questo viene fatto tramite un doppio ciclo for: il più esterno si ripete per un numero
         * di volte pari al numero di partite. Il più interno per un numero di volte pari al numero di giocatori registrati nella partita considerata. Si imposta il giocatoreCorrente pari al giocatore in posizione pari al
         * numero dell'iterazione corrente del for interno nella lista di giocatori della partita in posizione pari al numero dell'iterazione corrente del for esterno nella lista di partite salvate. La cartella del giocatore
         * è invece nella directory col nome uguale al nome del salvataggio, dentro alla cartella saveFiles. Viene controllato se nella cartella del giocatore esiste il file rosa.json. Se esiste, si legge il suo contenuto e
         * si deserializza in una lista di calciatori. Viene poi caricata nella lista di tutti i calciatori posseduti dal giocatore tramite il metodo Giocatore.CaricaLista(). Un procedimento analogo viene fatto per i calciatori
         * titolari, ma in questo caso ognuno viene aggiunto singolarmente tramite il metodo Giocatore.AddTitolari(). Viene infine ritornata la lista di partite.
         */
        static public List<Fantacalcio> GetPartite()
        {
            string[] salvataggi = GetFileSalvataggi(), datiPartita;
            string input, nomeSalvataggio, cartellaGiocatori, fileInputSquadra;
            int fase, numeroPartita;
            List<Fantacalcio> partite = new List<Fantacalcio>();    //lista di partite esistenti
            List<Giocatore> giocatori;
            List<Calciatore> calciatori;
            Giocatore giocatoreCorrente;

            if (salvataggi.Length == 0)     //se non esistono file di salvataggio
            {
                return null;
            }

            for (int i = 0; i < salvataggi.Length; i++)
            {
                input = File.ReadAllText(salvataggi[i]);
                datiPartita = input.Split(";");

                nomeSalvataggio = datiPartita[0];
                fase = Int32.Parse(datiPartita[1]);
                numeroPartita = Int32.Parse(datiPartita[2]);
                giocatori = JsonConvert.DeserializeObject<List<Giocatore>>(datiPartita[3]);

                partite.Add(new Fantacalcio(nomeSalvataggio, giocatori, fase, numeroPartita));
            }

            for (int i = 0; i < partite.Count; i++)
            {
                for (int j = 0; j < partite[i].GetGiocatori().Count; j++)
                {
                    giocatoreCorrente = partite[i].GetGiocatori()[j];
                    cartellaGiocatori = "saveFiles/" + partite[i].nomeSalvataggio + "/giocatori/" + partite[i].GetGiocatori()[j].nome;
                    if (File.Exists(cartellaGiocatori + "/rosa.json"))
                    {
                        fileInputSquadra = File.ReadAllText(cartellaGiocatori + "/rosa.json");
                        calciatori = JsonConvert.DeserializeObject<List<Calciatore>>(fileInputSquadra);
                        giocatoreCorrente.CaricaLista(calciatori);
                    }

                    if (File.Exists(cartellaGiocatori + "/titolari.json"))
                    {
                        fileInputSquadra = File.ReadAllText(cartellaGiocatori + "/titolari.json");
                        calciatori = JsonConvert.DeserializeObject<List<Calciatore>>(fileInputSquadra);
                        foreach (Calciatore calciatore in calciatori)
                        {
                            giocatoreCorrente.AddTitolari(calciatore);
                        }
                    }
                }
            }
            return partite;
        }

        /**
         * \fn      public string MostraSalvataggi()
         * \brief   Ritorna una stringa contenente i nomi di tutti i salvataggi, e ad ognuno viene assegnato un indice.
         * \param   List<Fantacalcio> partite: Le partite salvate su file
         * \param   string stringaPartite: La stringa che contiene i nomi e gli indice dei salvataggi
         * \return  string: Il metodo ritorna una stringa che contiene i nomi delle partite salvate e l'indice a loro assegnato. Se non esistono partite salvate, la stringa lo comunica.
         * \details Vengono ottenute le partite salvate tramite il metodo GetPartite(). Si controlla se la lista è uguale a null: se lo è, viene ritornata una stringa che comunica che non esistono file di salvataggio.
         * Se la lista non è uguale a null, con un ciclo for si inserisce nella variabile stringaPartite una nuova riga in cui è presente l'indice della partita, pari al numero dell'iterazione corrente + 1, e il nome
         * del salvataggio che corrisponde all'indice. Alla la stringa stringaPartite viene ritornata.
         */
        static public string MostraSalvataggi()
        {
            List<Fantacalcio> partite = GetPartite();

            if (partite == null)
            {
                return "Non esistono file di salvataggio";
            }
            else
            {
                string stringaPartite = "";
                for (int i = 0; i < partite.Count; i++)
                {
                    stringaPartite += i + 1 + " -> " + partite[i].nomeSalvataggio + "\n";
                }
                return stringaPartite;
            }
        }

        /**
         * \fn      public int EliminaSalvataggio(Fantacalcio partita)
         * \brief   Elimina la partita che viene data come parametro al metodo
         * \param   Fantacalcio partita: Partita da eliminare
         * \param   string daEliminare: Directory del salvataggio da eliminare
         * \return  int: Il metodo ritorna 1 se l'eliminazione ha avuto successo, altrimenti ritorna -1.
         * \details Il metodo ricava la directory del salvataggio da eliminare, e viene successivamente controllato se la directory del file da eliminare esiste.
         * Se esiste, elimina la directory del file tramite il metodo Directory.Delete(), dentro cui si passa il valore "true" che fa in modo che il metodo elimini anche
         * tutte le sottodirectory.
         */
        static public int EliminaSalvataggio(Fantacalcio partita)
        {
            string daEliminare = "saveFiles/" + partita.nomeSalvataggio;
            if (Directory.Exists(daEliminare))
            {
                Directory.Delete(daEliminare, true);
                return 1;
            }
            return -1;
        }

        /**
         * \fn      string[] GetCartelleSalvataggi()
         * \brief   Ottiene le directory contenute nella cartella di salvataggi, le mette in un array e ritorna l'array.
         * \param   string[] cartelleSalvataggi: Le cartelle all'interno della directory "saveFiles/", le quali contengono i salvataggi delle partite.
         * \return  string[]: Il metodo ritorna un array di stringhe in cui ogni stringa è una directory di un salvataggio di una partita.
         */
        static string[] GetCartelleSalvataggi()
        {
            string[] cartelleSalvataggi = Directory.GetDirectories("saveFiles/");
            return cartelleSalvataggi;
        }

        /**
         * \fn      string[] GetFileSalvataggi()
         * \brief   Ottiene le directory di tutti i file di salvataggio all'interno delle cartelle dei salvataggi, le mette in un array e le ritorna.
         * \param   string[] salvataggi: Array di stringhe in cui ogni stringa è la directory di un file di salvataggio principale di una partita.
         * \return  string[]: Il metodo ritorna un array di stringhe, in cui ogni stringa è una directory di un file di salvataggio principale di una partita
         * \details Viene creato un array di lunghezza pari al numero di cartelle di salvataggio ottenute dal metodo GetCartelleSalvataggi(). Con un ciclo for viene poi messa
         * nell'array "salvataggi" la directory del file di salvataggio principale presente in ogni cartella, chiamato "saveFile.json". Infine viene ritornato l'array.
         */
        static string[] GetFileSalvataggi()
        {
            string[] salvataggi = new string[GetCartelleSalvataggi().Length];

            for (int i = 0; i < GetCartelleSalvataggi().Length; i++)
            {
                salvataggi[i] = GetCartelleSalvataggi()[i] + "/saveFile.json";
            }
            return salvataggi;
        }
    }
}
