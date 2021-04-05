using Manager.tmpClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Manager
{
    public class GuiHandler
    {

        public static void Init(Action visualizeFunc)
        {
            State.Carver = new Carver();
            State.VisualizeFunc = visualizeFunc;

            //TimerCallback timerCB = new TimerCallback(Visualizer);
            //Timer t = new Timer(timerCB, null, 0, 1000);

        }

        private static void Visualizer()
        {
            while (true)
            {

                State.VisualizeFunc.Invoke();
                Thread.Sleep(State.Settings?.TZad ?? 1000);
            }

        }

        public static bool isFinished => State.Carver.IsFinishedFigure;
        public static bool isChanged => State.Carver.IsChangedState;

        public static void Start()
        {
            if (State.Settings.WorkMode == WorkMode.Auto)
            {
                State.Carver.Start();
                var thread = new Thread(State.Carver.DoAutomaticalySteps) { IsBackground = true };
                thread.Start();
                return;
            }

            if (State.Settings.WorkMode == WorkMode.Manual)
            {
                State.Carver.DoStep();
                return;
            }

        }

        public static void DoStep(object state)
        {
            State.Carver.DoStep();
            State.VisualizeFunc.Invoke();

        }

        public static void Stop()
        {
            State.Carver.Stop();
        }

        public static void End()
        {
            State.Carver = new Carver();
            State.Settings = null;
        }

        public static void SetAutoWork()
        {
            State.Carver.SetAutomatic();
            if (State.Settings != null)
            {
                State.Settings.WorkMode = WorkMode.Auto;
            }
        }

        public static void SetManualWork()
        {
            State.Carver.SetManual();
            if (State.Settings != null)
            {
                State.Settings.WorkMode = WorkMode.Manual;
            }
        }

        public static void SetSettings(Settings settings)
        {
            State.Settings = new Settings(settings);
            State.Carver.PutInstruction(settings.WorkMode == WorkMode.Auto, new Point3d(0, 0, 0), new Cube(settings.XMax, settings.YMax, settings.ZMax), new KnifeStroke(1, 1, 1), settings.TZad, new Detail(20, 4, 2));
            var thread = new Thread(Visualizer) { IsBackground = true };
            thread.Start();
        }


    }
}