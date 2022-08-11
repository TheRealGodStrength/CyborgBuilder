using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyborgBuilder.Interfaces
{
    public interface ITask
    {
        //TaskType Type { get; }
        object[] Signature { get; set; }
        int SleepTime { get; set; }
        bool UpdateOnIteration { get; set; }
        int Iterations { get; set; }
        string[] InputText { get; set; }
        object[] ActionArguments { get; set; }
        void Invoke();
        void TaskFunction(object function);
        ITask LoadFromSignature(object[] signature);
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
