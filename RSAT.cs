using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class RSAT
    {
        //BigInteger N, s, e;
        public BigInteger N { get; set; }
        public BigInteger s { get; set; }
        public BigInteger e { get; set; }

        public RSAT()
        {
            this.GenerateKeys();
        }
         void GenerateKeys()
        {
            
            //generate P, Q, N
            BigInteger  P, Q;
            do
            {
                P = PrimeT.RandomBigInt();
                Q = PrimeT.RandomBigInt();
                this.N = P * Q;
            } while (N.ToString().Length != 32);

            //calculate Euler function d
            BigInteger d = (P - 1) * (Q - 1);

            // calculate s
            // BigInteger s;
            do
            {
                this.s = PrimeT.RandomBigInt();
            } while (s > d || BigInteger.GreatestCommonDivisor(s, d) != 1);

            // calculate e
            // BigInteger e;
            do
            {
                this.e = PrimeT.RandomBigInt();
            } while (e * s % d != 1);
        }

         BigInteger StrToBI(string text) => new BigInteger(Encoding.UTF8.GetBytes(text));
         string BiToStr(BigInteger bi) => Encoding.UTF8.GetString(bi.ToByteArray());

        public BigInteger findC(BigInteger m)
        {
            return BigInteger.ModPow(m, this.s, this.N);
        }
        public BigInteger findM(BigInteger c)
        {
            return BigInteger.ModPow(c, this.e, this.N);
        }


    }
}
