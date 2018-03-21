using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Test
{
    class RSAClass
    {
        public BigInteger N { get; set; }
        public BigInteger s { get; set; }
        public BigInteger e { get; set; }
        
         public void GenerateKeys()
        {
            //generate P, Q, N
            BigInteger Q, P, K;
            do
            {
                P = Prime.RandomBigInt();
                Q = Prime.RandomBigInt();
                K = P * Q;
            } while (K.ToString().Length != 35);

           
            N = K;

            //calculate Euler function d
            BigInteger d = (P - 1) * (Q - 1);
          
            BigInteger S;
            do
            {
                S = new Random().Next(100000, 10000000);
            } while (BigInteger.GreatestCommonDivisor(S, d) != 1);
            s = S;
            // calculate e
            e = FindLockKey(s, d);
        }

        //public RSA()
        //{
        //    this.GenerateKeys();
        //}

        public  BigInteger FindLockKey(BigInteger openKey, BigInteger d)
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

        public  string Encrypt(string text)
        {
            StringBuilder str = new StringBuilder("");
            var blocks = ToBGList(text, 35);
            foreach (var b in blocks)
            {
                BigInteger c = BigInteger.ModPow(b, s, N);
                str.Append(c);
                str.Append('|');
            }
            return str.ToString().Substring(0, str.Length - 1);
        }

        public  string Decrypt(string text)
        {
            StringBuilder str = new StringBuilder("");
            string[] ciphers = text.Split('|');
            foreach (var c in ciphers)
            {
                var m = BigInteger.ModPow(BigInteger.Parse(c), e, N);
                var s = BGToStr(m);
                str.Append(s);
            }
            return str.ToString();
        }


        private static string ToFixedSize(char symbol)
        {
            int code = (int)symbol;
            string str = code.ToString();
            while (str.Length < 4) str = "0" + str;
            return str;
        }

        private static List<BigInteger> ToBGList(string text, int count)
        {
            string[] arr = new string[text.Length];
            List<BigInteger> listb = new List<BigInteger>();
            for (int i = 0; i < text.Length; i++) arr[i] = ToFixedSize(text[i]);
            StringBuilder str = new StringBuilder("");
            for (int i = 0; i < arr.Length; i++)
            {
                if (str.Length + 4 > count)
                {
                    listb.Add(BigInteger.Parse(str.ToString()));
                    str.Clear();
                }
                if (str.Length == 0) str.Append("1" + arr[i]);
                else str.Append(arr[i]);
            }
            listb.Add(BigInteger.Parse(str.ToString()));
            return listb;
        }

        private static string BGToStr(BigInteger b)
        {
            string str = b.ToString();
            str = str.Substring(1, str.Length - 1);
            StringBuilder strb = new StringBuilder("");
            for (int i = 0; i <= str.Length - 4; i += 4)
            {
                string cur = str.Substring(i, 4);
                char c = (char)Convert.ToInt32(cur);
                strb.Append(c);
            }
            return strb.ToString();
        }
    }
}
