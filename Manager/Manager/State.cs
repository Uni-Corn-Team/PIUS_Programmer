using Manager.tmpClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    static class State
    {
        public static Settings Settings { get; set; }
        public static Carver Carver { get; set; }
        public static Action<int[][][]> VisualizeFunc { get; set; }
    }
}
