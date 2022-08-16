using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CyborgBuilder.Interfaces;

namespace CyborgBuilder.Keyboard
{
    public class KeyboardTask : ITask
    {
        public Events.TaskType Type { get; set; }
        private static KeyboardTask instance = null;
        public static KeyboardTask Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new KeyboardTask();
                }
                return instance;
            }
        }
        public Queue<string> TextQueue { get; set; }
        public string Text { get; set; }
        public Func<string, Action> DoWork { get; set; }//  = new Func<Lines, string[], Action>(KeyboardFunctions.InputText);
        private Events.Keyboard.Typing typing;
        public Events.Keyboard.Typing Typing
        {
            get
            {
                return typing;
            }
            set
            {
                typing = value;
                string[] inputText = Array.Empty<string>();
                if (Typing == Events.Keyboard.Typing.FromQueue)
                {
                    Input(ref inputText);
                    foreach (string s in InputText)
                    {
                        TextQueue.Enqueue(s);
                    }
                    var result = MessageBox.Show("Do you want to set the iteration count relative to the queue?", "Update Iteration", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) { Iterations = TextQueue.Count; }
                }
                else
                {
                    Input(ref inputText);
                    Text = inputText[0];
                    DoWork = new Func<string, Action>(SinglePhrase);
                }
            }
        }
        public int SleepTime { get { return (int)sleep; } }
        private double sleep;
        public double Sleep
        {
            get
            {
                return sleep;
            }
            set
            {
                float millisec = (float)value * 1000;
                sleep = millisec;
            }
        }
        public static event EventHandler<TaskEventArgs> IteratorSet;
        private bool updateOnIteration = false;
        public bool UpdateOnIteration 
        {  
            get { return updateOnIteration; }
            set
            {
                updateOnIteration = value;
                TaskEventArgs tArgs = new TaskEventArgs();
                IteratorSet?.Invoke(this, new TaskEventArgs { Iterate = Iterations });
            }
        }
        public int Iterations { get; set; }
        public string[] InputText { get; set; }

        public void Invoke()
        {
            Action action;
            if (Typing == Events.Keyboard.Typing.FromQueue && TextQueue.Count > 0)
            {
                action = new Action(delegate ()
                {
                    Clipboard.SetText(TextQueue.Dequeue());
                    SendKeys.Send("^(v)");
                });
                action.Invoke();
            }
            else if (Typing == Events.Keyboard.Typing.Single)
            {
                action = DoWork(Text);
                action.Invoke();
            }
            if (SleepTime > 0) System.Threading.Thread.Sleep(SleepTime);
        }
        private static Action SinglePhrase(string text)
        {
            return new Action(delegate ()
            {
                Clipboard.SetText(text);
                SendKeys.Send("^(v)");
            });
        }
        private static void Input(ref string[] text)
        {
            var result = InputBox(ref text);
            if (result != DialogResult.OK) return;
        }
        private static DialogResult InputBox(ref string[] input)
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
    public enum TaskEvents
    {
        Iterate
    }
    public class TaskEventArgs : EventArgs
    {
        public int Iterate = 0;
        public TaskEventArgs TaskEventArg { get; set; }
    }
}

