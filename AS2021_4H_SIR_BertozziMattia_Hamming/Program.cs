using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_4H_SIR_BertozziMattia_Hamming
{
    class Program
    {
        //Contiene il numero di bit di controllo necessari per la sequenza di bit inserita
        static public int nBitControllo { get; set; }

        static void Main(string[] args)
        {
            Console.Write("Programma di SIR 'Hamming' di Mattia Bertozzi 4H");

            //contiene lista con con il numeroinserito
            List<char> SequenzaBit = new List<char>();

            //contiene lista con con il numeroinserito e gli spazi
            List<char> SequenzaConspazi = new List<char>();

            //riempe la lista
            ImpostazioneLista(SequenzaBit);

            //Visualizzo la lista con in numero inserito
            VediListaChar(SequenzaBit);

            //metodo che inserisce gli spazi nella sequenza
            //e li insericsco nell'altro lista 'SequenzaConspazi'
            SequenzaConspazi = AggiungiSpazi(SequenzaBit);

            //output della sequenza con l'aggiunta degli spazi inserita
            Console.WriteLine("lista con spazi");
            VediListaChar(SequenzaConspazi);

            Console.Write("Bit Controllo Necessari:");
            Console.WriteLine(nBitControllo.ToString());

            //Va a fare tutti i calcoli per sostituire gli spazi con i giusti valori 0 o 1
            CalcolaBitParita(SequenzaConspazi);

            VediListaChar(SequenzaConspazi);

            //contiene lista con con il numeroinserito e gli spazi
            List<char> SequenzaRicevuta = new List<char>();

            ImpostazioneLista(SequenzaRicevuta);

            CalcolaBitParitaSecondo(SequenzaRicevuta);

            Console.Write("Sequenza Corretta:");
            VediListaChar(SequenzaRicevuta);
        }

        //prende i dati in input e li carica in una lista correttamente
        static void ImpostazioneLista(List<char> SequenzaBit)
        {
            //richiesta di input della prima sequenza bit
            Console.Write("Inserisci la sequenza di bit: ");
            string BitInputstring = Console.ReadLine();

            //converto la string in vettore di char
            char[] BitInputChar = BitInputstring.ToCharArray();

            //metodo che serve a riempire la lista con la sequenza
            PopolaListaSequenza(SequenzaBit, BitInputChar);
        }


        //prende la lista in ingresso e gli aggiunge gli spazi dei bit di controllo parità (potenze del 2)
        static List<char> AggiungiSpazi(List<char>sequenzaBit)
        {
            //2^0-1....fino a 2^8-1
            int[] vettorepotenze = new int[] { 0, 1, 3, 7, 15, 31, 63, 127, 255};

            //crea gli spazi nella sequenza
            for (int i = 0; i < sequenzaBit.Count; i++)
            {
                foreach (int c in vettorepotenze)
                {
                    if (c == i)
                    {
                        sequenzaBit.Insert(c, '_');
                        //IndicenBitControllo.Add(Convert.ToChar(c));
                        nBitControllo++;
                    }

                }
            }

            return sequenzaBit;
        }


        //prende in ingresso la sequenza ricevuta (errata) 
        static void CalcolaBitParitaSecondo(List<char> SequenzaInput)
        {
            int nBitTotali = SequenzaInput.Count;

            //contiene il numero di 1 trovati e poi serviranno per fale lo XOR
            int nUno = 0;

            //il numero di cicli che impiega il for più interno che gira tutta la sequenza
            //e va a calcolare in numero di 1 presenti
            int nCicliq = 0;

            //somma finale dei bit di controllo
            //quindi posizione del bit errato
            int sindromeErrore = 0;

            //gira la sequenza per il numero di bit di controllo,quindi il numero di spazi
            for (int i = 0; i < nBitControllo; i++)
            {
                //calcolo il bit di controllo
                //inizierà con 2^0 poi (2^i)
                int parita = Convert.ToInt32(Math.Pow(2, i));

                //FONTI UTILIZZATE PER CAPIRE HAMMING
                //Sito
                //http://informatica.abaluth.com/il-computer/le-informazioni/codice-correttore-codice-di-hamming/
                //Video YouTube
                //https://www.youtube.com/watch?v=1kFsdvlGkWs&t=893s

                for (int j = parita - 1; j < SequenzaInput.Count; j++)
                {
                    for (int q = 0; q < parita && (q + j) < SequenzaInput.Count; q++)
                    {
                        if (SequenzaInput[j + q] == '1')
                            nUno++;

                        nCicliq++;
                    }
                    j += nCicliq + i;
                    nCicliq = 0;
                }

                //va ad aggiungere la posizione del bit errato nel caso il numero d 1 sia dispari
                if (nUno % 2 != 0)
                {
                    sindromeErrore += parita;
                }
                nUno = 0;
            }
            Correggi(SequenzaInput, sindromeErrore);
        }

        //prendendo in ingresso la lista con la sequenza con gli spazi
        //li va a riempire
        //in conclusione,dopo il metodo,la lista è completa dei bit di controllo
        static void CalcolaBitParita(List<char> SequenzaConspazi)
        {
            //contiene la lunghezza in bit sella sequenza
            int nBitTotali = SequenzaConspazi.Count;

            //contiene il numero di 1 trovati e poi serviranno per fale lo XOR
            int nUno = 0;

            //il numero di cicli che impiega il for più interno che gira tutta la sequenza
            //e va a calcolare in numero di 1 presenti
            int nCicliq = 0;

            //gira la sequenza per il numero di bit di controllo,quindi il numero di spazi
            for(int i=0;i<nBitControllo;i++)
            {
                VediListaChar(SequenzaConspazi);

                //calcolo il bit di controllo
                //inizierà con 2^0 poi (2^i)
                int parita = Convert.ToInt32(Math.Pow(2, i));

                //FONTI UTILIZZATE PER CAPIRE HAMMING
                //Sito
                //http://informatica.abaluth.com/il-computer/le-informazioni/codice-correttore-codice-di-hamming/
                //Video YouTube
                //https://www.youtube.com/watch?v=1kFsdvlGkWs&t=893s


                for (int j = parita-1 ; j < SequenzaConspazi.Count; j++)
                {
                    for(int q=0;q < parita && (q+j) < SequenzaConspazi.Count;q++)
                    {
                        if (SequenzaConspazi[j + q] == '1')
                            nUno++;

                        nCicliq++;
                    }
                    j += nCicliq + i;
                    nCicliq = 0;
                }

                //Va a calcolare lo XOR e sostituisce lo spazio con 0 o 1
                XOR(SequenzaConspazi, nUno, parita);

                nUno = 0;
            }

        }


        //Questo Esegue lo XOR
        //avendo in ingresso la lista,ilo numero di 1 presenti e il bit da cambiare
        static void XOR(List<char> SequenzaConspazi,int nUno,int parita)
        {
            //Bit1 Bit2 XOR
            //0    0    0
            //0    1    1
            //1    0    1
            //1    1    0

            //Quindi de il numero di '1' sarà dispari -> 1
            //altrimenti -> 0

            if (nUno % 2 == 0)
                SequenzaConspazi[parita - 1] = '0';
            else
                SequenzaConspazi[parita - 1] = '1';
        }


        //prende in ingresso la sequenza ricevuta (errata) e la posizione del bit errato quindi da correggere
        static void Correggi(List<char> SequenzaInput, int sindromeErrore)
        {
            //conterrà 0 o 1 in base al risultato dello XOR

            Console.WriteLine($"Posizione Bit errato: {sindromeErrore}");

            if(SequenzaInput[sindromeErrore - 1]=='0')
                SequenzaInput[sindromeErrore - 1] = '1';
            else
                SequenzaInput[sindromeErrore - 1] = '0';
        }


        //prende in input il vettore con la sequenza di bit inserita e la carica nella lista SequenzaBit
        static void PopolaListaSequenza(List<char> SequenzaBit, char[] BitInputChar)
        {
            for (int i = 0; i < BitInputChar.Length; i++)
                SequenzaBit.Add(BitInputChar[i]);
        }


        //Metodo che serve a Visualizzare una Lista di Char
        static void VediListaChar(List<char> stringa)
        {
            foreach (var a in stringa)
                Console.Write(Convert.ToString(a));
            Console.WriteLine("\n");
        }

        //Outdated
        //Fa il confronto tra la seguenza mandata e una sequenza sbagliata simulata
        //static void Confronto(List<char> Tx, List<char> Rx)
        //{
        //    for(int i=0;i<Tx.Count;i++)
        //    {
        //        if(Tx[i]!= Rx[i])//errore
        //        {
        //            Console.WriteLine($"l'errore si trova al bit {i+1}");
        //            if (Rx[i] == '0')
        //                Rx[i] = '1';
        //            else
        //                Rx[i] = '0';
        //        }
        //    }

        //    Console.WriteLine($"Sequenza Ricevuta Corretta:");
        //    VediListaChar(Rx);
        //}
    }
}
