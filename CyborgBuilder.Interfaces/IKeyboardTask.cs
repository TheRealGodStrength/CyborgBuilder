using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyborgBuilder.Interfaces
{
    public interface IKeyboardTask
    {
        Events.TaskType Type { get; set; }
        Queue<string> TextQueue { get; set; }
        Events.Keyboard.Typing Typing { get; set; }
        void Invoke();
    }
}
