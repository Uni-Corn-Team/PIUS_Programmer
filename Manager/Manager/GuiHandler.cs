using Manager.tmpClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class GuiHandler
    {
        public void Init()
        {
            State.Carver = new Carver();
        }

        public void Start() 
        {
            if(State.Settings.WorkMode == WorkMode.Auto)
            {
                State.Carver.IsWorking = true;
                State.Carver.DoAutomaticalySteps();
                return;
            }

            if (State.Settings.WorkMode == WorkMode.Manual)
            {
                State.Carver.DoStep();
                return;
            }
        }

        public void Stop()
        {
            State.Carver.IsWorking = false;
        }

        public void End() 
        {
            //What i should do there???
        }

        public void SetAutoWork() 
        {
            State.Carver.IsAutomatic = true;       
        }

        public void SetManualWork() {
            State.Carver.IsAutomatic = false;
        }

        public void SetSettings(Settings settings) 
        {
            State.Carver.PutInstruction(settings.WorkMode == WorkMode.Auto, settings.XMax, settings.YMax, settings.ZMax, settings.TZad);

        }


    }
}
