using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.tmpClasses
{
    class Carver
    {
        //состояние устройства
        public bool IsWorking { get; set; }
        public bool IsAutomatic { get; set; }

        //инструкция
        int delay;
        int current_x;
        int current_y;
        int current_z;

        //положение ножа
        int x0, y0, z0;
        int dx, dy, dz;

        //деталь
        Detail detail;

        public void PutInstruction(bool isAutomatic, int xMax, int yMax, int zMax, int tZad)
        {
            //инициализировать поля
        }

        public void PutDetail(Detail detail)
        {
            //обновить детать
        }

        public Detail GetDetail()
        {
            //мб и не нужно
            return null;
        }

        public void DoStep()
        {
            //сделать один шаг ножа и один надрез
        }

        public void Cut()
        {
            //сделать надрез
        }

        public void DoAutomaticalySteps() 
        { 
            //повторять одиночные шаги автоматически с задержкой
        }

    }
}
