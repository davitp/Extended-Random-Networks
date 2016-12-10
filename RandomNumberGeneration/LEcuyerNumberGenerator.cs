using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomNumberGeneration
{
    public class LEcuyerNumberGenerator : AbstractNumberGenerator
    {
        private int[] m_x = new int[4];
        private int[] m_y = new int[4];

        public LEcuyerNumberGenerator()
        {
            Random rnd = new Random();
	        for (int i = 0; i < 3; ++i)
	        {
                m_x[i] = rnd.Next() * rnd.Next();
                m_y[i] = rnd.Next() * rnd.Next();
	        }
        }

        override public ulong RandInt() 
        {
            // Every other access function simply transforms the numbers extracted here
            m_x[3] = (63308 * m_x[1] - 183326 * m_x[0]) % 2147483647;
            m_y[3] = (86098 * m_y[2] - 539608 * m_y[0]) % 2145483479;
            m_x[0] = m_x[1];
            m_x[1] = m_x[2];
            m_x[2] = m_x[3];
            m_y[0] = m_y[1];
            m_y[1] = m_y[2];
            m_y[2] = m_y[3];
            return (ulong)((m_x[2] - m_y[2]) % 2147483647);
        }
    }
}
