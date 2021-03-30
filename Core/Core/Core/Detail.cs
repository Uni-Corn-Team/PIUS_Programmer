using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class Detail
    {
        public char[][][] state;
        Detail(int x, int y, int z, char s)
        {
            state = new char[x][][];
            for (int i = 0; i < x; i++)
            {
                state[i] = new char[y][];
                for (int j = 0; j < y; j++)
                {
                    state[i][j] = new char[z];
                    for (int k = 0; k < z; k++)
                    {
                        state[i][j][k] = s;
                    }
                }
            }
        }
    }
}