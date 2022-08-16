using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyborgBuilder.Events;

namespace CyborgBuilder.Interfaces
{
    public interface IMouseTask : ITask
    {
        int X { get; set; }
        int Y { get; set; }
        int[] UpdatePoints { get; set; }
        MouseButton.Left LeftButton { get; set; }
        MouseButton.Middle MiddleButton { get; set; }
        MouseButton.Right RightButton { get; set; }
        MouseCursor MouseCursor { get; set; }
        ISignature Signature { get; set; }
        
    }
}
