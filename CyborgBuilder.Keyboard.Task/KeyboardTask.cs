using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using CyborgBuilder.TaskRepository;
using CyborgBuilder.Keyboard.Operations;

namespace CyborgBuilder.Keyboard
{
    public class KeyboardTask : Task, ITask
    {
        /* Signature consist of in order:
                KeyboardFunction
                
         
         */
        public int Iterations = -1;
        public Queue<string> TextQueue { get; set; }
        private string[] inputText = Array.Empty<string>();
        public Func<KeyboardFunctions.Lines, string[], Action> DoWork = new Func<KeyboardFunctions.Lines, string[], Action>(KeyboardFunctions.InputText);
        private KeyboardFunctions.Lines lines;
        public KeyboardFunctions.Lines Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
                if(Lines == KeyboardFunctions.Lines.FromQueue)
                {
                    KeyboardFunctions.Input(ref inputText);
                    foreach(string s in inputText)
                    {
                        TextQueue.Enqueue(s);
                    }
                    var result = MessageBox.Show("Do you want to set the iteration count relative to the queue?", "Update Iteration", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes) { Iterations = TextQueue.Count; }                
                }
                else
                {
                    KeyboardFunctions.Input(ref inputText);
                }
            }
        }
        public KeyboardTask(KeyboardFunctions.Lines lines)
        {
            Lines = lines;
            Signature = new object[] { lines, "string one", "string two" };
        }
        public ITask LoadFromSignature(object[] signature)
        {
            return this;
        }
        public KeyboardTask() { }
        public new void Invoke()
        {
            Action action;
            if (Lines == KeyboardFunctions.Lines.FromQueue && TextQueue.Count > 0)
            {
                action = new Action(delegate ()
                {
                    Clipboard.SetText(TextQueue.Dequeue());
                    SendKeys.Send("^(v)");
                });
                action.Invoke();
            }
            else
            {
                action = DoWork(Lines, inputText);
                action.Invoke();
            }
            if (SleepTime > 0) System.Threading.Thread.Sleep(SleepTime);
        }
        public void Serialize(string fileName)
        {
            XmlSerializer s = new XmlSerializer(typeof(KeyboardTask));
            TextWriter tw = new StreamWriter(fileName);
            s.Serialize(tw, this);
            tw.Close();
        }
    }
    public static class KeyboardTaskExt
    {
        public static ITask LoadFromSignature(this ITask task, object[] signature)
        {
            task.LoadFromSignature(signature);
            return task;
        }
        public static KeyboardTask SleepTime(this KeyboardTask kT, double inSeconds)
        {
            float m = (float)inSeconds * 1000;
            kT.SleepTime = (int)m;

            return kT;
        }
        public static KeyboardTask UpdateOnIteration(this KeyboardTask kT, bool updateOnIteration)
        {
            kT.UpdateOnIteration = updateOnIteration;
            return kT;
        }
    }
}
namespace CyborgBuilder.Keyboard.Operations
{
    public static class KeyboardFunctions
    {
        public static Action InputText(this Lines lines, string[] text)
        {

            switch (lines)
            {
                case Lines.Single:
                    return new Action(delegate ()
                    {
                        Clipboard.SetText(text[0]);
                        SendKeys.Send("^(v)");
                    });
                case Lines.Multiple:
                    var sb = new StringBuilder();
                    foreach (string s in text)
                    {
                        sb.Append(s);
                    }
                    return new Action(delegate ()
                    {
                        Clipboard.SetText(sb.ToString());
                        SendKeys.Send("^(v)");
                    });
            }
            throw new Exception();
        }
        public static void Input(ref string[] text)
        {
            var result = InputBox(ref text);
            if (result != DialogResult.OK) return;
        }
        public enum Lines
        {
            Single,
            Multiple,
            FromQueue
        }
        public static Action Input()
        {
            return new Action(delegate ()
            {
                string[] text = new string[1];
                var result = InputBox(ref text);
                if (result != DialogResult.OK) return;
                SendKeys.Send(text[0]);
            });
        }
        public static DialogResult InputBox(ref string[] input)
        {
            TextBox textBox = new TextBox()
            {
                Location = new Point(12, 29),
                Multiline = true,
                Size = new Size(321, 173)
            };
            Button btn_Okay = new Button()
            {
                Location = new Point(177, 208),
                Size = new Size(75, 23),
                Text = "Okay",
                UseVisualStyleBackColor = true,
                DialogResult = DialogResult.OK
            };
            Button btn_Cancel = new Button()
            {
                Location = new Point(258, 208),
                Size = new Size(75, 23),
                Text = "Cancel",
                UseVisualStyleBackColor = true,
                DialogResult = DialogResult.Cancel
            };
            Form form = new Form()
            {
                ClientSize = new Size(351, 254),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,

            };
            form.Controls.AddRange(new Control[] { textBox, btn_Cancel, btn_Okay });

            btn_Okay.Click += new EventHandler(delegate (object sender, EventArgs e)
            {
                form.AcceptButton = btn_Okay;
                int lines = textBox.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).Count();
            });
            btn_Cancel.Click += new EventHandler(delegate (object sender, EventArgs e)
            {
                form.CancelButton = btn_Cancel;
            });
            textBox.KeyDown += new KeyEventHandler(delegate (object sender, KeyEventArgs keyEvent)
            {
                Application.EnableVisualStyles();
                if (keyEvent.KeyCode == Keys.V && (keyEvent.Modifiers == Keys.Control && keyEvent.Modifiers == Keys.Shift))
                {
                    keyEvent.SuppressKeyPress = true;
                    textBox.AppendText(@"^(v)");
                }
                else if (keyEvent.KeyCode == Keys.A && keyEvent.Modifiers == Keys.Control && keyEvent.Modifiers == Keys.Shift)// == Keys.A &&  keyEvent.Modifiers == Keys.Control)
                {
                    keyEvent.SuppressKeyPress = true;
                    textBox.AppendText(@"^(a)");
                }
                else if (keyEvent.KeyCode == Keys.C && keyEvent.Modifiers == Keys.Control)
                {
                    keyEvent.SuppressKeyPress = true;
                    textBox.AppendText(@"^(c)");
                }
            });


            DialogResult dialogResult = form.ShowDialog();

            int totalLines = textBox.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).Count();
            input = new string[totalLines];
            Array.Resize(ref input, textBox.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).Count());
            input = textBox.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            return dialogResult;
        }
    }
}

