using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyborgBuilder.Interfaces;
namespace CyborgBuilder.Interfaces
{
    public interface IRepository
    {
        ISignature Signatures { get; set; }
        List<ITask> Tasks { get; set; }
        void Add(ITask task);
        void Receive(ISignature signature);
    }
}
