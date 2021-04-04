using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public enum WorkMode { Manual, Auto };
    public enum WorkState { Run, Stop};

    public class Settings
    {
        public WorkMode WorkMode { get; set; }
        public int XMax { get; set; }
        public int YMax { get; set; }
        public int ZMax { get; set; }
        public int TZad { get; set; }

        public Settings(WorkMode workMode, int xMax, int yMax, int zMax, int tZad)
        {
            WorkMode = workMode;
            XMax = xMax;
            YMax = yMax;
            ZMax = zMax;
            TZad = tZad;
        }

        public Settings(Settings copy)
        {
            WorkMode = copy.WorkMode;
            XMax = copy.XMax;
            YMax = copy.YMax;
            ZMax = copy.ZMax;
            TZad = copy.TZad;
        }
    }
}
