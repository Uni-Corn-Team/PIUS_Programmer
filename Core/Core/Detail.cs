using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Detail
    {
        public int[][][] state;
        public Detail(int x, int y, int z)
        {
            state = new int[x][][];
            for (int i = 0; i < x; i++)
            {
                state[i] = new int[y][];
                for (int j = 0; j < y; j++)
                {
                    state[i][j] = new int[z];
                    for (int k = 0; k < z; k++)
                    {
                        state[i][j][k] = 1;
                    }
                }
            }
        }
    }
}