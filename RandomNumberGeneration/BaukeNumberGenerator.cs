using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomNumberGeneration
{
    // Bauke Method
    public class BaukeNumberGenerator : AbstractNumberGenerator
    {
        private ulong[] m_forA = new ulong[9690];
        private ulong[] m_forB = new ulong[6989];
        private ulong[] m_forC = new ulong[1587];
        private ulong[] m_forD = new ulong[472];
        private int m_dirA, m_dirB, m_dirC, m_dirD;

        public BaukeNumberGenerator()
        {
            m_maxDeg = 0;
            m_pMaxDeg = 1;
	        ulong r;
            Random rnd = new Random();
	        for (int i = 9688; i >= 0; --i)
	        {
                r = (ulong)(rnd.Next() * rnd.Next());
                m_forA[i] = r;
		        if (i < 6988)
                    m_forB[i] = r;
		        if (i < 1586)
                    m_forC[i] = r;
		        if (i < 471)
                    m_forD[i] = r;
	        }
            m_dirA = 9688;
            m_dirB = 6987;
            m_dirC = 1585;
            m_dirD = 470;
        }

        override public ulong RandInt()
        {
            ulong ret = m_forA[m_dirA] ^ m_forB[m_dirB] ^ m_forC[m_dirC] ^ m_forD[m_dirD];
            m_forA[m_dirA] = m_forB[m_dirB] = m_forC[m_dirC] = m_forD[m_dirD] = ret;
            --m_dirA;
            --m_dirB;
            --m_dirC;
            --m_dirD;
            if (m_dirA < 0)
                m_dirA = 9688;
            if (m_dirB < 0)
                m_dirB = 6987;
            if (m_dirC < 0)
                m_dirC = 1585;
            if (m_dirD < 0)
                m_dirD = 470;

	        return ret;
        }
    }
}
