using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Test
{
    class Program
    {
        static BigInteger N, e, s, p,q, D;
        static void Main(string[] args)
        {
            GenerateKeys();
            //string text = "Алгоритм Миллера-Рабина является модификацией алгоритма " +
            //    "Миллера, разработанного Гари Миллером в 1976 году. " +
            //    "Алгоритм Миллера является детерминированным";
            //string enc = Encrypt(text);
            //WriteLine(text);
            //WriteLine(enc);
            //string dec = Decrypt(enc);

            //WriteLine(dec.Normalize());

            string text = "English letters work well";

            var b = ToBlocks(text, 35);
            foreach (var v in b)
            {
                WriteLine(BiToStr(v));
                //WriteLine(v);
            }
            //var text = "программа может обрабатывать русские буквы";
            //var b = ToBlocks1(text, 35);
            //foreach (var v in b) WriteLine(v);

        }

        static void GenerateKeys()
        {
            //generate P, Q, N
            BigInteger Q, P, K;
            do
            {
                P = PrimeT.RandomBigInt();
                Q = PrimeT.RandomBigInt();
                K = P * Q;
            } while (K.ToString().Length != 35);

            q = Q;
            p = P;
            N = K;

            //calculate Euler function d
            BigInteger d = (P - 1) * (Q - 1);
            D = d;
            BigInteger S;
            do
            {
                S = new Random().Next(100000, 10000000);
            } while (BigInteger.GreatestCommonDivisor(S, d) != 1);
            s = S;
            // calculate e
             
            e = FindLockKey(s, d);

        }

        public static BigInteger FindLockKey(BigInteger openKey, BigInteger d)
        {
            BigInteger m0 = d;
            BigInteger y = 0;
            BigInteger x = 1;
            while (openKey > 1)
            {
                BigInteger q = openKey / d;  // q - коэффициент
                BigInteger t = d; // запоминаем d
                d = openKey % d;
                openKey = t;
                t = y;
                // обновляем y и x
                y = x - q * y;
                x = t;
            }
            if (x < 0) // Делаем x положительным, если он меньше 0
                x += m0;
            return x;
        }


        static BigInteger StrToBI(string text) => new BigInteger(Encoding.UTF7.GetBytes(text));
        static  string BiToStr(BigInteger bi) => Encoding.UTF7.GetString(bi.ToByteArray());
       
        public  static BigInteger findC(BigInteger m) => BigInteger.ModPow(m, s, N);
        public  static BigInteger findM(BigInteger c) => BigInteger.ModPow(c, e, N);
        

        public static string Encrypt(string text)
        {
            StringBuilder str = new StringBuilder("");
            var blocks = ToBlocks1(text, 35);
            foreach (var b in blocks)
            {
                str.Append(findC(b));
                str.Append('|');
            } 
            str.Remove(str.Length - 1, 1);
            return  str.ToString();
        }

        public static string Encrypt1(string text) => findC(StrToBI(text)).ToString();       
        

        public static string Decrypt1(string text) => BiToStr(findM(BigInteger.Parse(text))); 
        

        public static string Decrypt(string text)
        {
            StringBuilder str = new StringBuilder();
            string[] ciphers = text.Split('|');
            foreach(var c in ciphers)
            {
                var m = findM(BigInteger.Parse(c));
                var s = BiToStr(m);
                s = s.Substring(s.Length - 1, 1);
                str.Append(s);
            }
            return str.ToString();
        }

        public static List<BigInteger> ToBlocks(string text, int N)
        {
            var utf = Encoding.UTF7;
            List<BigInteger> bg = new List<BigInteger>();
            var t = utf.GetBytes(text);
            int lenCounter = t[0].ToString().Length;
            int start = 0;
            for (int i = 1; i < t.Length; i++)
            {
                if(lenCounter + t[i].ToString().Length > N)
                {
                    bg.Add(new BigInteger(getChunk(t, start, i)));
                    start = i;
                    lenCounter = 0;
                }
                lenCounter += t[i].ToString().Length;
            }
            bg.Add(new BigInteger(getChunk(t, start, t.Length)));
            return bg;
        } 

        public static List<BigInteger> ToBlocks1(string text, int N)
        {
            List<BigInteger> bg = new List<BigInteger>();
            List<string> sg = new List<string>();
           // char[] arr = text.ToCharArray();
            int lenCounter = 0;
            int start = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if(lenCounter + ((int)text[i]).ToString().Length > N)
                {
                    sg.Add(text.Substring(start, i - start));
                    lenCounter = 0;
                    start = i;
                }
                lenCounter += ((int)text[i]).ToString().Length;
            }
            sg.Add(text.Substring(start, text.Length-start));

            foreach(var v in sg)
            {
                byte[] arr = new byte[v.Length];
                for (int i = 0; i < v.Length; i++)
                {
                    arr[i] = (byte)v[i];
                }

                bg.Add(new BigInteger(arr));
            }
            return bg;
        }

        private static byte[] getChunk(byte[] b, int lo, int hi)
        {
            byte[] bn = new byte[hi - lo + 1];
            
            int index = 0;
            
            for (int i = lo; i < hi; i++)
            {
                bn[index] = b[i];
                index++;
            }
            bn[index] = 1;
            return bn;
        }

       
        

    }
}
