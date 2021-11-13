using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace fantacalcio
{
    class Salvataggio
    {
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
            SalvaSquadre(fantacalcio);

            File.WriteAllText(directorySalvataggi + fantacalcio.nomeSalvataggio + "/saveFile.json", output);    //viene salvata la stringa convertita a json in un file con estensione .json
        }
        static public void SalvaCalciatoriDisponibili(List<Calciatore> calciatoriDisponibili, Fantacalcio fantacalcio)
        {
            string output = JsonConvert.SerializeObject(calciatoriDisponibili, Formatting.Indented);
            File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/calciatoriDisponibili.json", output);
        }
        static void SalvaSquadre(Fantacalcio fantacalcio)
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
        static public void SalvaAbbinamenti(Fantacalcio fantacalcio, Giocatore[,] abbinamenti)
        {
            string output = JsonConvert.SerializeObject(abbinamenti, Formatting.Indented);
            File.WriteAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/abbinamenti.json", output);
        }
        static public Giocatore[,] CaricaAbbinamenti(Fantacalcio fantacalcio)
        {
            string input = File.ReadAllText("saveFiles/" + fantacalcio.nomeSalvataggio + "/abbinamenti.json");
            Giocatore[,] abbinamenti = JsonConvert.DeserializeObject<Giocatore[,]>(input);
            return abbinamenti;
        }
        static public void SalvaTitolari(Fantacalcio fantacalcio)
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

        static public List<Fantacalcio> GetPartite()
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
                    int numeroPartita = Int32.Parse(words[2]);
                    List<Giocatore> giocatori = JsonConvert.DeserializeObject<List<Giocatore>>(words[3]);

                    partite.Add(new Fantacalcio(nomeSalvataggio, giocatori, fase, numeroPartita));
                }

                for (int i = 0; i < partite.Count; i++)
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

                        if (File.Exists(cartellaGiocatori + "/titolari.json"))
                        {
                            Console.WriteLine($"Titolari del giocatore {partite[i].GetGiocatori()[j].nome} caricati.");
                            string fileInput = File.ReadAllText(cartellaGiocatori + "/titolari.json");
                            List<Calciatore> listaCalciatori = JsonConvert.DeserializeObject<List<Calciatore>>(fileInput);
                            foreach (Calciatore calciatore in listaCalciatori)
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
        static public string MostraSalvataggi()
        {
            if (GetPartite() == null)
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
        static string[] GetCartelleSalvataggi()
        {
            string[] cartelleSalvataggi = Directory.GetDirectories("saveFiles/");
            return cartelleSalvataggi;
        }

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
