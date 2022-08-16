using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CyborgBuilder.Events;
using CyborgBuilder.Interfaces;

namespace CyborgBuilder.TaskRepo
{
    public class Signature : ISignature
    {
        public Events.TaskType Type { get; set; }
        public Events.Keyboard.Typing Typing { get; set; }
        public Queue<string> TextQueue { get;set; }
        public MouseButton.Left LeftButton { get; set; }
        public MouseButton.Middle MiddleButton { get; set; }
        public MouseButton.Right RightButton { get; set;}
        public int X { get; set; }
        public int Y { get; set; }
        public MouseCursor Cursor { get; set; }
        int[] ISignature.UpdatePoints { get; set; }

        public Signature()
        {

        }
        public void GetPropertyValues(Object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            var type =  from PropertyInfo p in props
                       where p.Name == "Type"
                       select p;

            MessageBox.Show(type.First().Name);

            foreach(var prop in props)
            {
                try
                {
                   
                    if (prop.GetIndexParameters().Length == 0)
                    {
                        MessageBox.Show("1: " + prop.Name + " " + prop.PropertyType.ToString() + " " + prop.GetValue(obj).ToString());
                    }
                    else
                    {
                        MessageBox.Show("2: " + prop.Name + " " + prop.GetType().ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    continue;
                }
            }
        }
    }
    public static class SignatureExtensions
    {
        public static Signature Type(this Signature sig, Events.TaskType taskType)
        {
            sig.Type = taskType;
            return sig;
        }
        public static Signature Typing(this Signature sig, Events.Keyboard.Typing typing)
        {
            sig.Typing = typing;
            return sig;
        }
        public static Signature TextQueue(this Signature sig, Queue<string>textQueue)
        {
            sig.TextQueue = textQueue;
            return sig;
        }
        public static Signature LeftButton(this Signature sig, MouseButton.Left leftBtn)
        {
            sig.LeftButton = leftBtn;
            return sig;
        }
        public static Signature MiddleButton(this Signature sig, MouseButton.Middle middleBtn)
        {
            sig.MiddleButton = middleBtn;
            return sig;
        }
        public static Signature RightButton(this Signature sig, MouseButton.Right rightBtn)
        {
            sig.RightButton = rightBtn;
            return sig;
        }
        public static Signature Cursor(this Signature sig, MouseCursor cursor)
        {
            sig.Cursor = cursor;
            return sig;
        }
    }
}
