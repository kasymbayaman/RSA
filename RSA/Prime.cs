using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;
using static System.Console;
namespace Test
{
    public static class Prime
    {
        //generate truly random numbers
        static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        static Random random = new Random();

        // N = P*Q, where len is length of N
        public static BigInteger peekN(int len)
        {
            BigInteger N, P, Q;
            do
            {
                P = RandomBigInt();
                Q = RandomBigInt();
                N = P * Q;
            } while (N.ToString().Length != len);
            return N;
        }

        //big integer with near 15-16 length
        public static BigInteger RandomBigInt()
        {
            var N = BigInteger.Parse("1000000000000000000");
            byte[] bytes = N.ToByteArray();
            BigInteger R;
            do
            {
                rand.GetBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                R = new BigInteger(bytes);
            } while (R > N || !isPrime(R));

            return R;
        }

        //check is number prime
        public static bool isPrime(BigInteger bigInt)
        {

            var primes = generateResheto(50000);
            foreach (var num in primes) if (bigInt % num == 0) return false;
            return MillerRabinTest(bigInt);
        }

        //then Ryabin - Miller test
        private static bool RMtest(BigInteger bigInt)
        {
            // find t, s where bigInt-1 = t * 2 ^ s;
            var t = bigInt - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s++;
            }

            // because log(bigInt) ~ 25
            for (int i = 0; i <= 100; i++)
            {
                BigInteger a = RandomBetween(bigInt);
                BigInteger x = BigInteger.ModPow(a, t, bigInt);
                if (x == 1 || x == bigInt - 1) continue;
                for (int j = 0; j < s; j++)
                {
                    x = BigInteger.ModPow(x, 2, bigInt);
                    if (x == 1) return false;
                    if (x == bigInt - 1) break;
                }
            }
            return true;
        }

        private static BigInteger RandomBetween(BigInteger bigInt)
        {
            // generate random number between 2 and n-2
            int len = bigInt.ToByteArray().Length;
            BigInteger a;
            do
            {
                int n = random.Next(1, len);
                byte[] bytes = new byte[n];
                rand.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while (a > bigInt - 2 || a < 2);
            return a;
        }

        private static List<int> generateResheto(int n)
        {
            bool[] isPrime = new bool[n + 1];
            for (int i = 2; i <= n; ++i) isPrime[i] = true;
            List<int> primes = new List<int>();
            for (int i = 2; i < n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                    if (i * i < n)
                        for (int j = 0; j <= n; j += i) isPrime[j] = false;
                }
            }
            return primes;
        }

        public static bool MillerRabinTest(BigInteger n)
        {
            int k = 10;
            // если n < 2 или n четное - возвращаем false
            if (n < 2 || n % 2 == 0)
                return false;
            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }
            // повторить k раз
            for (int i = 0; i < k; i++)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;
                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);
                // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
                BigInteger x = BigInteger.ModPow(a, t, n);
                // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
                if (x == 1 || x == n - 1)
                    continue;
                // повторить s − 1 раз
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    x = BigInteger.ModPow(x, 2, n);
                    // если x == 1, то вернуть "составное"
                    if (x == 1)
                        return false;
                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            // вернуть "вероятно простое"
            return true;
        }

    }
}
