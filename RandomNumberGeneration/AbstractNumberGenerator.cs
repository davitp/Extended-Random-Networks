using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomNumberGeneration
{
    public abstract class AbstractNumberGenerator
    {
        protected ulong m_lastP;    // last p for rand1DivPn function
        protected ulong m_maxDeg;   // m_maxDeg is maximum degree of p lesser than uint32 Limit
        protected ulong m_pMaxDeg;  // m_pMaxDeg = pow(p, m_maxDeg)
        protected ulong[] m_psDegrees = new ulong[30];  // degrees of p here

        abstract public ulong RandInt();

        // integer in [0,n] for n < 2^32
        public ulong RandInt(ulong n)
        {
            // Find which bits are used in n
            ulong used = n;
            used |= used >> 1;
            used |= used >> 2;
            used |= used >> 4;
            used |= used >> 8;
            used |= used >> 16;

            // Draw numbers until one is found in [0,n]
            ulong i;
            do
                i = RandInt() & used;  // toss unused bits to shorten search
            while (i > n);

            return i;
        }

        // real number in [0,1]
        public double Rand()
        {
            return RandInt() * (1.0 / 4294967295.0);
        }

        // real number in [0,n]
        public double Rand(double n)
        {
            return Rand() * n;
        }

        // real number in [0,1)
        public double RandExc()
        {
            return RandInt() * (1.0 / 4294967296.0);
        }

        // real number in [0,n)
        public double RandExc(double n)
        {
            return RandExc() * n;
        }

        // real number in (0,1)
        public double RandDblExc()
        {
            return (RandInt() + 0.5) * (1.0 / 4294967296.0);
        }

        // real number in (0,n)
        public double RandDblExc(double n)
        {
            return RandDblExc() * n;
        }

        // returns true with probability 1/p
        public bool Rand1DivP(ulong p)
        {
            return RandInt((4294967296 / p) * p) < (4294967296 / p);
        }

        // returns true with probability 1/(p^n)
        public bool Rand1DivPn(ulong p, ulong n)
        {
            if (m_lastP != p)
            {
                m_lastP = p;
                m_maxDeg = 0;
                m_pMaxDeg = 1;
                m_psDegrees[m_maxDeg] = m_pMaxDeg;
                while (m_pMaxDeg < 4294967296 / p)
                {
                    m_pMaxDeg *= p;
                    ++m_maxDeg;
                    m_psDegrees[m_maxDeg] = m_pMaxDeg;
                }
            }

            bool ret = true;
            ulong nDivMaxDeg = n / m_maxDeg;
            int i;
            for (i = 0; i < (int)nDivMaxDeg; ++i)
                if (!(ret &= Rand1DivP(m_pMaxDeg)))
                    return false;

            if (!(ret &= Rand1DivP(m_psDegrees[n % m_maxDeg])))
                return false;

            return true;
        }
    }
}
