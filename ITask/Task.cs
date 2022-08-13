using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using CyborgBuilder.Events;
using CyborgBuilder.Interfaces;

namespace CyborgBuilder.TaskRepo
{
    public class Task : ITask
    {
        public static event EventHandler Created;
        public Events.TaskType Type { get; }
        public ISignature Signature { get; set; }
        public int SleepTime { get; set; }
        public double Sleep
        {
            set
            {
                SleepTime = (int)value * 1000;
            }
        }
        public bool UpdateOnIteration { get; set; }
        public object[] ActionArguments { get; set; }
        public int Iterations { get; set; }
        public string[] InputText { get; set; }
        public int X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int[] UpdatePoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MouseButton.Left LeftButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MouseButton.Middle MiddleButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MouseButton.Right RightButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MouseCursor MouseCursor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void TaskFunction(object function) { }
        public Task()
        {
            if(Created != null)
            {
                Created(this, null);
            }
        }
        public void Invoke()
        {

        }
        public ITask LoadFromSignature(object[] lfs)
        {
            throw new NotImplementedException();
        }
    }
    public enum TaskType
    {
        Keyboard,
        Mouse
    }
}
