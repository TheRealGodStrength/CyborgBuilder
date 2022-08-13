using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyborgBuilder.Events;

namespace CyborgBuilder.Interfaces
{
    public interface ITask : IMouseTask
    {
        TaskType Type { get; }
        int SleepTime { get; set; }
        double Sleep { set; }
        bool UpdateOnIteration { get; set; }
        int Iterations { get; set; }
        string[] InputText { get; set; }
        object[] ActionArguments { get; set; }
        void Invoke();
        void TaskFunction(object function);

    }

    public enum Lines
    {
        Single,
        Multiple,
        FromQueue
    }
    public static class ITaskExt
    {
        public static ITask LoadFromSignature(this ITask task, object[] signature)
        {
            return task;
        }
    }
}
namespace CyborgBuilder.Events
{
    public enum TaskType
    {
        Keyboard,
        Mouse
    }
    public class MouseButton
    {
        public enum Left
        {
            Click,
            DoubleClick,
            Down,
            Up
        }
        public enum Middle
        {
            Click,
            DoubleClick,
            Down,
            Up
        }
        public enum Right
        {
            Click,
            DoubleClick,
            Down,
            Up
        }
    }
    public enum MouseCursor
    {
        Get,
        Set
    }
    public static class Keyboard
    {
        public enum Typing
        {
            Single,
            FromQueue
        }
    }
}