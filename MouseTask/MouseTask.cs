using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using CyborgBuilder.Events;
using CyborgBuilder.Interfaces;

namespace CyborgBuilder.Mouse
{
    public class MouseTask : ITask, IMouseTask
    {
        public new Events.TaskType Type { get; set; }
        public new ISignature Signature { get; set; }
        private static MouseTask instance = null;
        public static MouseTask Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MouseTask();
                }
                return instance;
            }
        }
        //Func<MouseCursor, Action> DoWork = new Func<MouseCursor, Action>();


        public int SleepTime { get; set; }
        public double Sleep { get; set; }
        public static event EventHandler<TaskEventArgs> IteratorSet;
        private bool updateOnIteration = false;
        
        public bool UpdateOnIteration 
        { 
            get { return updateOnIteration; }
            set
            {
                updateOnIteration = value;
                IteratorSet?.Invoke(this, new TaskEventArgs { Iterate = Iterations });
            }
        }
        public int Iterations { get; set; }
        public string[] InputText { get; set; }
        public object[] ActionArguments { get; set; }
        public int? X { get; set; }
        public int? Y { get;set; }
        public int[] UpdatePoints { get;set; }
        public MouseButton.Left LeftButton { get;set; }
        public MouseButton.Middle MiddleButton { get;set; }
        public MouseButton.Right RightButton { get;set; }
        public MouseCursor MouseCursor { get;set; }

        public MouseTask()
        {

        }
        private Func<object, bool, Action[]> DoWork = new Func<object, bool, Action[]>(MouseFunctions.Do);
        public void Invoke()
        {
            if (UpdateOnIteration)
            {
                X += UpdatePoints[0];
                Y += UpdatePoints[1];
            }
            if(MouseCursor == MouseCursor.Set)
            {

            }
            //var action = DoWork(Function, ActionArguments);
           // action.Invoke();
            if (SleepTime > 0) System.Threading.Thread.Sleep(SleepTime);
        }

        public void TaskFunction(object function)
        {
            throw new NotImplementedException();
        }
    }
    public static class MouseTaskExt
    {
        public static ITask LoadFromSignature(this ITask task, object[] signature)
        {
            task.LoadFromSignature(signature);
            return task;
        }
        public static MouseTask FromSignature(this MouseTask mT, object[] args)
        {
            if (args[1] != null && args[1].GetType() == typeof(int[]))
            {
                mT.UpdatePoints = (int[])args[1];
                mT.UpdateOnIteration = true;
            }
            return mT;
        }
        public static MouseTask SleepTime(this MouseTask mT, double inSeconds)
        {
            float m = (float)inSeconds * 1000;
            mT.SleepTime = (int)m;

            return mT;
        }
        public static MouseTask UpdateOnIteration(this MouseTask mT, bool updateOnIteration, int x, int y)
        {
            mT.UpdateOnIteration = updateOnIteration;
            if (updateOnIteration)
            {
                mT.UpdatePoints[0] = x;
                mT.UpdatePoints[1] = y;
            }
            return mT;
        }
    }
    public static class MouseFunctions
    {
        public static Action[] Do(object button, bool setCursorPos = false)
        {
            var actions = new Action[2];
            if (setCursorPos)
            {
                var point = GetCursorPosition();

                actions[0] = new Action(delegate ()
                {
                    SetCursorPosition(point);
                });

                Type t = button.GetType();
                switch (t)
                {
                    case Type leftBtnType when leftBtnType == typeof(MouseButton.Left):
                        actions[1] = Do((MouseButton.Left)button);
                        return actions;
                    case Type middleBtnType when middleBtnType == typeof(MouseButton.Middle):
                        actions[1] = Do((MouseButton.Middle)button);
                        return actions;
                    case Type rightBtnType when rightBtnType == typeof(MouseButton.Right):
                        actions[1] = Do((MouseButton.Right)button);
                        return actions;
                    default: throw new Exception();
                }
            }
            else
            {
                Array.Resize(ref actions, 1);
                Type t = button.GetType();
                switch (t)
                {
                    case Type leftBtnType when leftBtnType == typeof(MouseButton.Left):
                        actions[0] = Do((MouseButton.Left)button);
                        return actions;
                    case Type middleBtnType when middleBtnType == typeof(MouseButton.Middle):
                        actions[0] = Do((MouseButton.Middle)button);
                        return actions;
                    case Type rightBtnType when rightBtnType == typeof(MouseButton.Right):
                        actions[0] = Do((MouseButton.Right)button);
                        return actions;
                    default: throw new Exception();
                }
            }

        }
        public static Action Do(Events.MouseButton.Left leftButton)
        {
            switch (leftButton)
            {
                case MouseButton.Left.Click:
                    return new Action(delegate ()
                    {
                        MouseClick(MouseEventFlags.LeftDown);
                    });
                case MouseButton.Left.DoubleClick:
                    return new Action(delegate ()
                    {
                        MouseClick_Double(MouseEventFlags.LeftDown);
                    });
                case MouseButton.Left.Down:
                    return new Action(delegate ()
                    {
                        MouseClick_Hold(MouseEventFlags.LeftDown);
                    });
                case MouseButton.Left.Up:
                    return new Action(delegate ()
                    {
                        MouseClick_Release(MouseEventFlags.LeftUp);
                    });
            }
            throw new Exception();
        }
        public static Action Do(MouseButton.Middle middleButton)
        {
            switch (middleButton)
            {
                case MouseButton.Middle.Click:
                    return new Action(delegate ()
                    {
                        MouseClick(MouseEventFlags.MiddleDown);
                    });
                case MouseButton.Middle.DoubleClick:
                    return new Action(delegate ()
                    {
                        MouseClick_Double(MouseEventFlags.MiddleDown);
                    });
                case MouseButton.Middle.Down:
                    return new Action(delegate ()
                    {
                        MouseClick_Hold(MouseEventFlags.MiddleDown);
                    });
                case MouseButton.Middle.Up:
                    return new Action(delegate ()
                    {
                        MouseClick_Release(MouseEventFlags.MiddleUp);
                    });
            }
            throw new Exception();
        }
        public static Action Do(MouseButton.Right rightButton)
        {
            switch (rightButton)
            {
                case MouseButton.Right.Click:
                    return new Action(delegate ()
                    {
                        MouseClick(MouseEventFlags.RightDown);
                    });
                case MouseButton.Right.DoubleClick:
                    return new Action(delegate ()
                    {
                        MouseClick_Double(MouseEventFlags.RightDown);
                    });
                case MouseButton.Right.Down:
                    return new Action(delegate ()
                    {
                        MouseClick_Hold(MouseEventFlags.RightDown);
                    });
                case MouseButton.Right.Up:
                    return new Action(delegate ()
                    {
                        MouseClick_Release(MouseEventFlags.RightUp);
                    });
            }
            throw new Exception();
        }
        public static Action Do(MouseCursor mouseCursor)
        {
            switch(mouseCursor)
            {
                case MouseCursor.Get:
                    return new Action(delegate ()
                    {
                        GetCursorPosition();
                    });
                case MouseCursor.Set:
                    var point = GetCursorPosition();
                    return new Action(delegate ()
                    {
                        SetCursorPosition(point);
                    });
                default: throw new Exception();
            }
        }
        public static Action Do(MouseCursor mouseCursor, int x, int y)
        {
            if(mouseCursor != MouseCursor.Set) throw new Exception();
            return new Action(delegate ()
            {
                SetCursorPosition(x, y);
            });
        }
        public static Action Do(MouseCursor mouseCursor, Point point)
        {
            if (mouseCursor != MouseCursor.Set) throw new Exception();
            return new Action(delegate ()
            {
                SetCursorPosition(point);
            });
        }


        private static Point GetCursorPosition()
        {
            GetCursorPos(out Point lpPoint);
            return lpPoint;
        }
        private static void SetCursorPosition(Point point)
        {
            SetCursorPos(point.X, point.Y);
        }
        private static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }
        private static void MouseClick(MouseEventFlags value)
        {
            switch (value)
            {
                case MouseEventFlags.LeftDown:
                    MouseEvent(MouseEventFlags.LeftDown);
                    MouseEvent(MouseEventFlags.LeftUp);
                    break;
                case MouseEventFlags.RightDown:
                    MouseEvent(MouseEventFlags.RightDown);
                    MouseEvent(MouseEventFlags.RightUp);
                    break;
            }
        }
        private static void MouseClick_Double(MouseEventFlags value)
        {
            MouseClick(value);
            MouseClick(value);

        }
        private static void MouseClick_Hold(MouseEventFlags value)
        {
            if (value == MouseEventFlags.LeftUp) value = MouseEventFlags.LeftDown;
            if (value == MouseEventFlags.RightUp) value = MouseEventFlags.RightDown;
            MouseEvent(value);
        }
        private static void MouseClick_Release(MouseEventFlags value)
        {
            if (value == MouseEventFlags.LeftDown) value = MouseEventFlags.LeftUp;
            if (value == MouseEventFlags.RightDown) value = MouseEventFlags.RightUp;
            MouseEvent(value);
        }
        private static void MouseEvent(MouseEventFlags value)
        {
            Point point = GetCursorPosition();

            mouse_event
                ((int)value,
                point.X,
                point.Y,
                0,
                0);
        }
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x0000008,
            RightUp = 0x00000010
        }
        [Flags]
        public enum MouseButtons
        {
            Left = MouseEventFlags.LeftDown,
            Middle = MouseEventFlags.MiddleDown,
            Right = MouseEventFlags.RightDown
        }
        public enum Function
        {
            Left_Click,
            Middle_Click,
            Right_Click,
            Left_ClickHold,
            Middle_ClickHold,
            Right_ClickHold,
            Left_ClickRelease,
            Middle_ClickRelease,
            Right_ClickRelease,
            Left_DoubleClick,
            Middle_DoubleClick,
            Right_DoubleClick,
            SetCursorPosition,
            GetCursorPosition
        }
        public static Point GetCursorPosition(this Function function)
        {
            if (function != Function.GetCursorPosition) throw new Exception();
            return GetCursorPosition();
        }
        public static T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }
            return array;
        }
        public static T[] ResizeInitializeArray<T>(int start, int length, ref T[] array) where T : new()
        {
            Array.Resize(ref array, length);

            for (int i = start; i < length; ++i)
            {
                array[i] = new T();
            }
            return array;
        }
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    }
    public enum TaskEvents
    {
        Iterate
    }
    public class TaskEventArgs : EventArgs
    {
        public int Iterate = 0;
        public TaskEventArgs TaskEventArg { get; set; }
    }
}

