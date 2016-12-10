using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomNumberGeneration
{
    // Tausworthe by L'Ecuyer
    public class TauswortheNumberGenerator : AbstractNumberGenerator
    {
        private int m_s1, m_s2, m_s3;

        public TauswortheNumberGenerator()
        {
            Random rnd = new Random();
            m_s1 = rnd.Next() * rnd.Next();
            m_s2 = rnd.Next() * rnd.Next();
            m_s3 = rnd.Next() * rnd.Next();
        }

        override public ulong RandInt()
        {
            m_s1 = ((int)((m_s1 & 4294967294) << 12) ^ (((m_s1 << 13) ^ m_s1) >> 19));
            m_s2 = ((int)((m_s2 & 4294967288) << 4) ^ (((m_s2 << 2) ^ m_s2) >> 25));
            m_s3 = ((int)((m_s3 & 4294967280) << 17) ^ (((m_s3 << 3) ^ m_s3) >> 11));
            return (ulong)(m_s1 ^ m_s2 ^ m_s3);
        }
    }
}
