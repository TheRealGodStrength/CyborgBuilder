using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyborgBuilder.TaskRepository
{
    public interface ITask
    {
        object[] Signature { get; set; }
        bool UpdateOnIteration { get; set; }
        void Invoke();
    }
}
