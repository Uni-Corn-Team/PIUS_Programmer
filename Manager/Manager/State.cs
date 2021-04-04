using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public static class State
    {
        public static Settings Settings { get; set; }
        public static Carver Carver { get; set; }
        public static Action VisualizeFunc { get; set; }

        public static int[][][] Detail { get => Carver.detail.state; }

        public static int CoordinateX 
        {
            get
            {
                if (Carver != null && Carver.currentPosition != null)
                { return Carver.currentPosition.x; }
                else
                { return 0; }
            }
        }
        public static int CoordinateY { 
            get
            {
                if (Carver != null && Carver.currentPosition != null)
                { return Carver.currentPosition.y; }
                else
                { return 0; }
            }
        }
        public static int CoordinateZ {
            get
            {
                if (Carver != null && Carver.currentPosition != null)
                { return Carver.currentPosition.z; }
                else
                { return 0; }
            }
        }

    }
}
