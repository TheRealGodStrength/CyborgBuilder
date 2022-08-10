using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CyborgBuilder.TaskRepo;


namespace CyborgBuilder.Mouse
{
    public class MouseTask : Task, ITask
    {
        /* Signature includes
         * Type
         * MouseFunction
         * Y
         * X
         * UpdatePoints
         * UpdateOnIteration
         
         */

        public TaskType Type { get; }
        public int Y { get; set; }
        public int X { get; set; }

        public int[] UpdatePoints = new int[2] { 0, 0 };
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
        MouseTask() { Type = TaskType.Mouse; }
        public MouseFunctions.Function Function { get; set; }
        public Func<MouseFunctions.Function, object[], Action> DoWork = new Func<MouseFunctions.Function, object[], Action>(MouseFunctions.Do);
        
        public MouseTask(MouseFunctions.Function function, params object[] args)
        {
            Type = TaskType.Mouse;
            Function = function;
            ActionArguments = args;
            UpdateOnIteration = false;
            CreateSignature();
        }
        public MouseTask(MouseFunctions.Function function, int x, int y)
        {
            Type = TaskType.Mouse;
            Function = function;
            X = x;
            Y = y;
            ActionArguments = new object[] { x, y };
            UpdateOnIteration = false;
            CreateSignature();
        }
        public MouseTask(object[] signature, string st)
        {
            Type = TaskType.Mouse;
            if (signature.Length != 3) throw new Exception();
            if (signature[0].GetType() != typeof(MouseFunctions.Function)) throw new Exception();
            if (signature[1].GetType() != typeof(object[])) throw new Exception();
            if (signature[2].GetType() != typeof(object[]) && signature[2] != null) throw new Exception();

            Function = (MouseFunctions.Function)signature[0];
            ActionArguments = (object[])signature[1];
            if (signature[2] != null && signature[2].GetType() == typeof(int[]))
            {
                UpdatePoints = (int[])signature[2];
                UpdateOnIteration = true;
            }
        }
        public ITask LoadFromSignature(object[] signature)
        {

            return this.From(signature);
        }
        private void CreateSignature()
        {
            object[] signature = new object[6];
            signature[0] = Type;
            signature[1] = Function;
            signature[2] = Y;
            signature[3] = X;
            signature[4] = UpdatePoints;
            signature[5] = UpdateOnIteration;
            Signature = signature;
        }
        public new void Invoke()
        {
            if(UpdateOnIteration)
            {
                X += UpdatePoints[0];
                Y += UpdatePoints[1];
                ActionArguments = new object[] { X, Y };
            }
            var action = DoWork(Function, ActionArguments);
            action.Invoke();
            if (SleepTime > 0) System.Threading.Thread.Sleep(SleepTime);
        }
    }
    public static class MouseTaskExt
    {
        public static ITask LoadFromSignature(this ITask task, object[] signature)
        {
            task.LoadFromSignature(signature);
            return task;
        }
        public static ITask From(this ITask task, object[] signature)
        {
            if (signature.Length != 2) throw new Exception();
            if (signature[0].GetType() != typeof(int)) throw new Exception();
            if (signature[1].GetType() != typeof(object[])) throw new Exception();
           // if (signature[2].GetType() != typeof(object[]) && signature[2] != null) throw new Exception();

            task = new MouseTask((MouseFunctions.Function)signature[0], (object[])signature[1])
                .FromSignature(signature);
            return task;
        }
        public static MouseTask FromSignature(this MouseTask mT, object[] args)
        {
            if(args[1] != null && args[1].GetType() == typeof(int[]))
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

        public static Action Do(Function function, params object[] obj)
        {
            switch (obj.Length)
            {
                case 1:
                    if (function != Function.SetCursorPosition) throw new Exception();
                    if (obj[0].GetType() == typeof(Point))
                    {
                        var pt = (Point)obj[0];
                        return new Action(delegate ()
                        {
                            SetCursorPosition(pt);
                        });
                    }
                    throw new Exception();
                case 2:
                    if (function != Function.SetCursorPosition) throw new Exception();
                    if (obj[0].GetType() == typeof(int))
                        if (obj[0].GetType() == obj[1].GetType())
                        {
                            return new Action(delegate ()
                            {
                                SetCursorPosition((int)obj[0], (int)obj[1]);
                            });
                        }
                    throw new Exception();
                case 0:
                    switch (function)
                    {
                        case Function.Left_Click:
                            return new Action(delegate ()
                            {
                                MouseClick(MouseEventFlags.LeftDown);
                            });
                        case Function.Middle_Click:
                            return new Action(delegate ()
                            {
                                MouseClick(MouseEventFlags.MiddleDown);
                            });
                        case Function.Right_Click:
                            return new Action(delegate ()
                            {
                                MouseClick(MouseEventFlags.RightDown);
                            });
                        case Function.Left_ClickHold:
                            return new Action(delegate ()
                            {
                                MouseClick_Hold(MouseEventFlags.LeftDown);
                            });
                        case Function.Middle_ClickHold:
                            return new Action(delegate ()
                            {
                                MouseClick_Hold(MouseEventFlags.MiddleDown);
                            });
                        case Function.Right_ClickHold:
                            return new Action(delegate ()
                            {
                                MouseClick_Hold(MouseEventFlags.RightDown);
                            });
                        case Function.Left_ClickRelease:
                            return new Action(delegate ()
                            {
                                MouseClick_Release(MouseEventFlags.LeftUp);
                            });
                        case Function.Middle_ClickRelease:
                            return new Action(delegate ()
                            {
                                MouseClick_Release(MouseEventFlags.MiddleUp);
                            });
                        case Function.Right_ClickRelease:
                            return new Action(delegate ()
                            {
                                MouseClick_Release(MouseEventFlags.RightUp);
                            });
                        case Function.Left_DoubleClick:
                            return new Action(delegate ()
                            {
                                MouseClick_Double(MouseEventFlags.LeftDown);
                            });
                        case Function.Middle_DoubleClick:
                            return new Action(delegate ()
                            {
                                MouseClick_Double(MouseEventFlags.MiddleDown);
                            });
                        case Function.Right_DoubleClick:
                            return new Action(delegate ()
                            {
                                MouseClick_Double(MouseEventFlags.RightDown);
                            });
                        case Function.GetCursorPosition:
                            return new Action(delegate ()
                            {
                                GetCursorPosition();
                            });
                        case Function.SetCursorPosition:
                            var point = GetCursorPosition();
                            return new Action(delegate ()
                            {
                                SetCursorPosition(point);
                            });

                    }
                    throw new Exception();
                default: throw new Exception();
            }
            throw new Exception();
        }
        public static Action Do(this Function function, Point point)
        {
            if (function != Function.SetCursorPosition) throw new Exception();
            var action = Do(function, point);
            return action;
        }
        public static Action Do(this Function function, int x, int y)
        {
            if (function != Function.SetCursorPosition) throw new Exception();
            var action = Do(function, x, y);
            return action;
        }
        public static Action Do(this Function function)
        {
            object[] none = Array.Empty<object>();
            var action = Do(function, none);
            return action;
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
}

