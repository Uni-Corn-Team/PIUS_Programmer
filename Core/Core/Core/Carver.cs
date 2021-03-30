using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Core
{
    class Carver
    {
        //состояние устройства
        public bool IsWorking { get; set; }
        public bool IsAutomatic { get; set; }
        public bool IsChangedState { get; set; }

        //internal bools
        bool isMovingForward;
        bool isFinishedFigure;

        //KnifeSettings settings;
        int delay;

        //положение ножа
        Point3d startPosition;
        Point3d currentPosition;
        Cube figureToCarve;
        KnifeStroke maxKnife;

        //деталь
        Detail detail;

        public void PutInstruction(bool isAutomatic, Point3d startPosition, Cube figure, KnifeStroke maxKnife, int delay)
        {
            IsAutomatic = isAutomatic;
            this.startPosition = startPosition;
            figureToCarve = figure;
            this.maxKnife = maxKnife;
            this.delay = delay;
        }


        public Detail GetDetailState()
        {
            IsChangedState = false;
            return detail;
        }

        private void Cut(KnifeStroke currentKnife)
        {

            int dX = currentKnife.x <= maxKnife.x ? currentKnife.x : maxKnife.x;
            int dY = currentKnife.y <= maxKnife.y ? currentKnife.y : maxKnife.y;
            int dZ = currentKnife.z <= maxKnife.z ? currentKnife.z : maxKnife.z;

            for (int i = currentPosition.x; i < currentPosition.x + dX; i++)
            {
                for (int j = currentPosition.y; j < currentPosition.y + dY; j++)
                {
                    for (int k = currentPosition.z; k < currentPosition.z + dZ; k++)
                    {
                        detail.state[i][j][k] = 'o';
                    }
                }
            }
            Point3d newPosition = new Point3d(currentPosition.x + currentKnife.x, currentPosition.y, currentPosition.z);
            SetCarverPosition(newPosition);
        }


        public void Stop()
        {
            IsWorking = false;
        }

        public void Start()
        {
            IsWorking = true;
        }


        public void DoStep()
        {
            if (!isFinishedFigure)
            {
                bool isPlaneEnded = false;

                int currentMaxOffsetX = (startPosition.x + figureToCarve.xSize) - currentPosition.x;
                int xPath = maxKnife.x <= currentMaxOffsetX ? maxKnife.x : currentMaxOffsetX;
                xPath = isMovingForward ? xPath : (-1) * xPath;

                int currentMaxOffsetZ = (startPosition.z + figureToCarve.zSize) - currentPosition.z;
                int zSize = maxKnife.z <= currentMaxOffsetZ ? maxKnife.z : currentMaxOffsetZ;

                int currentMaxOffsetY = (startPosition.y + figureToCarve.ySize) - currentPosition.y;
                int ySize = maxKnife.y < (currentMaxOffsetY) ? maxKnife.y : currentMaxOffsetY;

                KnifeStroke currentKnife = new KnifeStroke(xPath, ySize, zSize);
                Cut(currentKnife);

                if (currentPosition.x >= (startPosition.x + figureToCarve.xSize) &&
                    (currentPosition.z + currentKnife.z) >= (startPosition.z + figureToCarve.zSize))
                    isPlaneEnded = true;

                isMovingForward = !isMovingForward;
                IsChangedState = true;

                if (isPlaneEnded)
                {
                    if (currentPosition.y == (startPosition.y + figureToCarve.ySize))
                    {
                        isFinishedFigure = true;
                    }
                    else
                    {
                        Point3d newPosition = new Point3d(currentPosition.x, currentPosition.y + currentKnife.y, currentPosition.z);
                        SetCarverPosition(newPosition);
                    }
                }
                else
                {
                    Point3d newPosition = new Point3d(currentPosition.x, currentPosition.y, currentPosition.z + currentKnife.z);
                    SetCarverPosition(newPosition);
                }
            }
        }

        public void DoAutomaticalySteps()
        {

            while (!isFinishedFigure)
            {
                DoStep();
                Thread.Sleep(delay); //TODO: change to smth else
                if (!IsWorking)
                    break;
            }
        }

        public void SetCarverPosition(Point3d position)
        {
            currentPosition = position;
        }

    }
}