using System;
using System.Threading;
using System.Windows.Forms;
using CyborgBuilder.Robot;
using CyborgBuilder.Interfaces;
namespace CyborgBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Cyborg.Name = "First Bot";
            InitializeComponent();
        }

        readonly Bot Cyborg = Bot.Instance;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //var kT = new KeyboardTask(KeyboardFunctions.Lines.Single);
            //var kt2 = new KeyboardTask(KeyboardFunctions.Lines.Multiple);
            //var kt3 = new KeyboardTask(KeyboardFunctions.Lines.Single);

            

            
            string file = @"c:\users\djimenez\desktop\kt.xml";

            //Cyborg.AddMouseTask(MouseFunctions.Function.SetCursorPosition, 50, 50);
            //Cyborg.AddMouseTask(MouseFunctions.Function.SetCursorPosition, 100, 100);
            //Cyborg.AddMouseTask(MouseFunctions.Function.SetCursorPosition, 150, 150);

            //Cyborg.ImportSignatureRepository(file);

            //foreach(ITask task in Cyborg.Tasks)
            //{
            //    task.Invoke();
            //    Thread.Sleep(2000);
            //}
            //Thread.Sleep(2000);
            



            //KeyboardTask kT = new KeyboardTask(KeyboardFunctions.Lines.Single);
            //MouseTask mT = new MouseTask(MouseFunctions.Function.SetCursorPosition, 0, 0)
            //    .UpdateOnIteration(true, 50, 50);
            //for (int i = 0; i < 10; i++)
            //{
            //    mT.Invoke();
            //    Thread.Sleep(1500);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cyborg.AddMouseTask(Mouse.MouseFunctions.Function.SetCursorPosition, 0, 0);
            listBox1.DataSource = Cyborg.Tasks;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                var task = (ITask)listBox1.SelectedItem;
                task.Invoke();
            }
        }
    }
}
