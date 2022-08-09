using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyborgBuilder.TaskRepository
{
    public class Task : ITask
    {
        public object[] Signature { get; set; }
        public bool UpdateOnIteration { get; set; }
        public object[] ActionArguments { get; set; }

        public void Invoke()
        {

        }
    }
}
