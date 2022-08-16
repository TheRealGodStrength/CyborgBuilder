using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyborgBuilder.Events;
namespace CyborgBuilder.Interfaces
{
    public interface ISignature
    {
        TaskType Type { get; set; }
        Keyboard.Typing Typing { get; set; }
        Queue<string> TextQueue { get; set; }
        MouseButton.Left LeftButton { get; set; }
        MouseButton.Middle MiddleButton { get; set; }
        MouseButton.Right RightButton { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int[] UpdatePoints { get; set; }
        MouseCursor Cursor { get; set; }
        void GetPropertyValues(Object obj);
    }
}
