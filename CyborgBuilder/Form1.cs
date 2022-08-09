using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CyborgBuilder.Keyboard;
using CyborgBuilder.Keyboard.Operations;
using CyborgBuilder.Mouse.Operations;
using CyborgBuilder.Mouse;

namespace CyborgBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KeyboardTask kT = new KeyboardTask(KeyboardFunctions.Lines.Single);
            MouseTask mT = new MouseTask(MouseFunctions.Function.SetCursorPosition, 0, 0)
                .UpdateOnIteration(true, 50, 50);
            for (int i = 0; i < 10; i++)
            {
                mT.Invoke();
                Thread.Sleep(1500);
            }
        }
    }
}
