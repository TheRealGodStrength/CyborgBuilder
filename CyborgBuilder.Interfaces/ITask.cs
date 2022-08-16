using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyborgBuilder.Events;

namespace CyborgBuilder.Interfaces
{
    public interface ITask 
    {
        TaskType Type { get; }
        int SleepTime { get; }
        double Sleep { set; }
        bool UpdateOnIteration { get; set; }
        int Iterations { get; set; }
        string[] InputText { get; set; }
        void Invoke();

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
        Mouse,
    }
    public class MouseButton
    {
        public enum Left
        {
            None,
            Click,
            DoubleClick,
            Down,
            Up,
        }
        public enum Middle
        {   
            None,
            Click,
            DoubleClick,
            Down,
            Up,
        }
        public enum Right
        {
            None,
            Click,
            DoubleClick,
            Down,
            Up,
        }
    }
    public enum MouseCursor
    {
        None,
        Get,
        Set,
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