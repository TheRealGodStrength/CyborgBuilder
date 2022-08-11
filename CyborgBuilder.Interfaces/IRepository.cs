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
        object[] Signatures { get; set; }
        void Add(ITask task);
        void ExportSignatures(string fileName);
        
    }
}
