using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyborgBuilder.TaskRepository;

namespace CyborgBuilder.TaskRepository
{
    public interface ITask
    {
        object[] Signature { get; set; }
        int SleepTime { get; set; }
        bool UpdateOnIteration { get; set; }
        object[] ActionArguments { get; set; }
        void Invoke();
        ITask LoadFromSignature(object[] signature);
    }
public static class ITaskExt
    {
        public static ITask LoadFromSignature(this ITask task, object[] signature) 
        {
             

            return task;
        }
    }
}
