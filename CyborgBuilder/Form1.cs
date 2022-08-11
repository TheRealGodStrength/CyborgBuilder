using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using CyborgBuilder.Robot;
using CyborgBuilder.Interfaces;
namespace CyborgBuilder
{
    public partial class Form1 : Form
    {
        readonly Bot Cyborg = Bot.Instance;
        readonly Stopwatch Sw;
        public Form1()
        {

        }
        public Form1(Stopwatch sw)
        {
                   
            Sw = sw;
            InitializeComponent();
        }

        void LoadProc(object stateInfo)
        {
            Mouse.MouseTask mT = new Mouse.MouseTask(Mouse.MouseFunctions.Function.SetCursorPosition, 0, 0);
           // string file = @"c:\users\djimenez\desktop\kt.xml";
            Invoke((Action)(() =>
            {
                listBox1.DataSource = Cyborg.Tasks;
            }));
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //var kT = new KeyboardTask(KeyboardFunctions.Lines.Single);
            //var kt2 = new KeyboardTask(KeyboardFunctions.Lines.Multiple);
            //var kt3 = new KeyboardTask(KeyboardFunctions.Lines.Single);


            ThreadPool.QueueUserWorkItem(LoadProc, null);


            ///LoadProc(null);


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

            System.Drawing.Point pt = Mouse.MouseFunctions.GetCursorPosition(Mouse.MouseFunctions.Function.GetCursorPosition);
            Cyborg.AddMouseTask(Mouse.MouseFunctions.Function.SetCursorPosition, pt.X, pt.Y)
                .Sleep(2.0);
            Cyborg.AddMouseTask(Mouse.MouseFunctions.Function.Left_DoubleClick);
            Cyborg.AddMouseTask(Mouse.MouseFunctions.Function.Left_DoubleClick);
                
            
            //Cyborg.AddMouseTask(Mouse.MouseFunctions.Function.SetCursorPosition, pt.X, pt.Y);
            
            //Cyborg.AddMouseTask(Mouse.MouseFunctions.Function.Left_DoubleClick);
            
            //Cyborg.AddMouseTask(Mouse.MouseFunctions.Function.Left_DoubleClick);
            listBox1.DataSource = Cyborg.Tasks;
        }


        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                ThreadPool.QueueUserWorkItem(ListBoxItemSelected);
            }
        }
        void ListBoxItemSelected(object stateInfo)
        {
            ITask task = null;
            Invoke((Action)(() =>
            {
                task = (ITask)listBox1.SelectedItem;
            }));

            if (task != null) task.Invoke();
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            Sw.Stop();
            double t = Sw.ElapsedMilliseconds / 1000f;
            Text = t.ToString();
        }
    }

}
