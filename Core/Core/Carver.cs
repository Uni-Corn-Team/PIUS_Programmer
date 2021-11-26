using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Core
{
    public class Carver
    {
        public Carver()
        {
            detail = new Detail(0,0,0);
        }

        //external bools
        public bool IsWorking { get; private set; }
        public bool IsAutomatic { get; private set; }
        public bool IsChangedState { get; private set; }
        public bool IsFinishedFigure { get; private set; }

        //internal bools
        private bool isMovingForward;
        private bool isMovingDown;

        //KnifeSettings settings;
        int delay;
        public KnifeStroke maxKnife;
        public Cube figureToCarve;

        //Knife position
        public Point3d startPosition;
        public Point3d currentPosition;

        //Detail
        public Detail detail;

        public void PutInstruction(bool isAutomatic, Point3d startPosition, Cube figure, KnifeStroke maxKnife, int delay, Detail detail)
        {
            IsAutomatic = isAutomatic;
            this.startPosition = startPosition;
            figureToCarve = figure;
            this.maxKnife = maxKnife;
            this.delay = delay;
            this.detail = detail;
            currentPosition = startPosition;
            isMovingForward = true;
            IsWorking = true;
            isMovingDown = true;
        }
        public Detail GetDetailState()
        {
            IsChangedState = false;
            return detail;
        }
        private Point3d AdaptCoordinates(int x, int y, int z)
        {
            return new Point3d(x, detail.state[x].Length - y, detail.state[x][y].Length - z);
        }
        private void Cut(KnifeStroke currentKnife)
        {
 
            int dX = Math.Abs(currentKnife.x) <= maxKnife.x ? currentKnife.x : maxKnife.x * Math.Sign(currentKnife.x);
            int dY = Math.Abs(currentKnife.y) <= maxKnife.y ? currentKnife.y : maxKnife.y * Math.Sign(currentKnife.y);
            int dZ = Math.Abs(currentKnife.z) <= maxKnife.z ? currentKnife.z : maxKnife.z * Math.Sign(currentKnife.z);

            Point3d end;
            Point3d start = AdaptCoordinates(currentPosition.x, currentPosition.y, currentPosition.z);
                end = AdaptCoordinates(currentPosition.x + dX, currentPosition.y + dY, currentPosition.z + dZ);

            for (int i = Math.Min(start.x, end.x); i < Math.Max(start.x, end.x); i++)
            {
                for (int j = Math.Min(start.y, end.y); j < Math.Max(start.y, end.y); j++)
                {
                    for (int k = Math.Min(start.z, end.z); k < Math.Max(start.z, end.z); k++)
                    {
                        detail.state[i][j][k]--;
                    }
                }
            }
            Point3d newPosition = new Point3d(currentPosition.x + currentKnife.x, currentPosition.y, currentPosition.z);
            SetCarverPosition(newPosition);
        }
        public void DoStep()
        {
            if (!IsFinishedFigure)
            {
                bool isPlaneEnded = false;
                bool isStripEnded = false;

                //Calculate current knife stroke
                int currentMaxOffsetX;
                if (isMovingForward)
                    currentMaxOffsetX = (startPosition.x + figureToCarve.xSize) - currentPosition.x;
                else
                    currentMaxOffsetX = currentPosition.x - startPosition.x;
                int xPath = maxKnife.x <= currentMaxOffsetX ? maxKnife.x : currentMaxOffsetX;
                xPath = isMovingForward ? xPath : (-1) * xPath;

                int currentMaxOffsetZ = (startPosition.z + figureToCarve.zSize) - currentPosition.z;
                int zSize = maxKnife.z <= currentMaxOffsetZ ? maxKnife.z : currentMaxOffsetZ;

                int currentMaxOffsetY = (startPosition.y + figureToCarve.ySize) - currentPosition.y;
                int ySize = maxKnife.y < (currentMaxOffsetY) ? maxKnife.y : currentMaxOffsetY;

                KnifeStroke currentKnife = new KnifeStroke(xPath, ySize, zSize);
                Cut(currentKnife);

                //Check if current strip (x-axis) ended
                if ((isMovingForward && currentPosition.x == (startPosition.x + figureToCarve.xSize)) ||
                    (!isMovingForward && currentPosition.x == startPosition.x))
                {
                    isStripEnded = true;
                    isMovingForward = !isMovingForward;
                }
                //Check if current plane (x-z plane) ended
                if (isStripEnded &&
                    ((!isMovingDown && (currentPosition.z <= startPosition.z)) ||
                    (isMovingDown && (currentPosition.z + currentKnife.z) >= (startPosition.z + figureToCarve.zSize))))
                {
                    isPlaneEnded = true;
                    isMovingDown = !isMovingDown;
                }
                //Move carver blade to next position
                if (isPlaneEnded)
                {
                    if (currentPosition.y + currentKnife.y >= (startPosition.y + figureToCarve.ySize))
                    {
                        IsFinishedFigure = true;
                    }
                    else
                    {
                        Point3d newPosition = new Point3d(currentPosition.x, currentPosition.y + currentKnife.y, currentPosition.z);
                        SetCarverPosition(newPosition);
                    }
                }
                else
                {
                    if (isStripEnded)
                    {
                        Point3d newPosition;
                        if (isMovingDown)
                            newPosition = new Point3d(currentPosition.x, currentPosition.y, currentPosition.z + currentKnife.z);
                        else
                            newPosition = new Point3d(currentPosition.x, currentPosition.y, currentPosition.z - maxKnife.z);

                        SetCarverPosition(newPosition);
                    }
                }
                IsChangedState = true;
            }
        }

        public void Start()
        {
            IsWorking = true;
        }
        public void Pause()
        {
            IsWorking = false;
        }
        public void Stop()
        {
            IsWorking = false;
        }
        public void SetAutomatic()
        {
            IsAutomatic = true;
        }
        public void SetManual()
        {
            IsAutomatic = false;
        }
        public void DoAutomaticalySteps()
        {
            while (!IsFinishedFigure && IsAutomatic)
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
