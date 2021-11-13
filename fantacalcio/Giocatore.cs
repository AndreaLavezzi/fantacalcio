using System;
using System.Collections.Generic;
using System.Text;

namespace fantacalcio
{
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
        public int CompareTo(Giocatore giocatore2)
        {
            if (this.punteggio > giocatore2.punteggio)
            {
                return 1;
            }
            else if (this.punteggio < giocatore2.punteggio)
            {
                return -1;
            }
            return 0;
        }
        public int GetGiocatoriRuolo(string ruolo, string gruppo)
        {
            int portieri = 0;
            int difensori = 0;
            int attaccanti = 0;
            int centrocampisti = 0;
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
}
