using System;
using System.Collections.Generic;
using System.Linq;
using CyborgBuilder.Events;
using CyborgBuilder.Interfaces;
using CyborgBuilder.TaskRepo;

namespace CyborgBuilder.Robot
{
    public sealed class Bot : IBot
    {
        #region Singleton Pattern
        Bot()
        {
            Repo = new Repository();
            Tasks = new List<ITask>();
            TaskRepo.Task.Created += new EventHandler(CaptureNewTask);
        }
        public double SleepTime { get; set; }
        public string Name { get; set; }
        private static readonly object padLock = new object();
        private static Bot instance = null;
        public static Bot Instance
        {
            get
            {
                lock (padLock)
                {
                    if (instance == null)
                    {
                        instance = new Bot();
                    }
                    return instance;
                }
            }
        }
        #endregion
        public void AddFunction(Keyboard.Typing typing)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Keyboard)
                .Typing(typing);
            Repo.Receive(signature);
        }
        public void AddFunctionk(MouseButton.Left mouseButtonEvent)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Mouse)
                .LeftButton(mouseButtonEvent);
            Repo.Receive(signature);
        }
        public void AddFunction(MouseButton.Middle mouseButtonEvent)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Mouse)
                .MiddleButton(mouseButtonEvent);
            Repo.Receive(signature);
        }
        public void AddFunction(MouseButton.Right mouseButtonEvent)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Mouse)
                .RightButton(mouseButtonEvent);
            Repo.Receive(signature);
        }
        public void AddFunction(MouseCursor cursor)
        {
            if (cursor == MouseCursor.Set)
            {
                ISignature signature = new Signature()
                    .Type(Events.TaskType.Keyboard)
                    .Cursor(MouseCursor.Set);
                Repo.Receive(signature);
            }
            throw new Exception();
        }
        void CaptureNewTask(object sender, EventArgs e)
        {
            ///ThreadPool.QueueUserWorkItem(AddTo_Tasks_Repo, sender);
            AddTo_Tasks_Repo(sender);
        }
        void AddTo_Tasks_Repo(object sender)
        {
            var task = (ITask)sender;
            Tasks.Add(task);
            Repo.Add(task);
        }
        public IRepository Repo { get; set; }


        public List<ITask> Tasks { get; set; }

        //public void AddMouseTask(MouseFunctions.Function function)
        //{
        //    ITask mT = new MouseTask(function);
        //    Tasks.Add(mT);
        //    Repo.Add(mT);
        //}
        //public void AddMouseTask(MouseFunctions.Function function, int x, int y)
        //{
        //    ITask mT = new MouseTask(function, x, y).SleepTime(SleepTime);
        //    Tasks.Add(mT);
        //    Repo.Add(mT);
        //    SleepTime = 0;
        //}
        //public void AddMouseTask(MouseFunctions.Function function, params object[] args)
        //{
        //    ITask mT = new MouseTask(function, args);
        //    Tasks.Add(mT);
        //    Repo.Add(mT);
        //}
        public void RunAllTasks()
        {
            foreach (var task in Tasks)
            {
                task.Invoke();
            }
        }

        public static T[] ResizeInitializeArray<T>(int start, int length, ref T[] array) where T : new()
        {
            var count = array.Count();
            Array.Resize(ref array, count + 1);

            for (int i = start; i < length; ++i)
            {
                array[i] = new T();
            }
            return array;
        }
        public ITask[] ExportTasks() //exporter will need to read Task Signature
        {
            var tasks = Tasks.ToArray();
            return tasks;
        }
        public void LoadCasette(ITask[] tasks)
        {
            Tasks.Clear();
            Tasks.AddRange(tasks);
        }
    }
    public static class BotExtensions
    {
        public static Bot Add(this Bot bot, Keyboard.Typing typing)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Keyboard)
                .Typing(typing);
            bot.Repo.Receive(signature);
            return bot;
        }
        public static Bot Add(this Bot bot, MouseButton.Left lftBtn)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Mouse)
                .LeftButton(lftBtn);
            bot.Repo.Receive(signature);
            return bot;
        }
        public static Bot Add(this Bot bot, MouseButton.Middle midBtn)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Mouse)
                .MiddleButton(midBtn);
            bot.Repo.Receive(signature);
            return bot;
        }
        public static Bot Add(this Bot bot, MouseButton.Right ritBtn)
        {
            ISignature signature = new Signature()
                .Type(Events.TaskType.Mouse)
                .RightButton(ritBtn);
            bot.Repo.Receive(signature);
            return bot;
        }
        public static Bot Add(this Bot bot, MouseCursor cursor)
        {
            if(cursor == MouseCursor.Set)
            {
                ISignature signature = new Signature()
                    .Type(Events.TaskType.Mouse)
                    .Cursor(MouseCursor.Set);
                bot.Repo.Receive(signature);
                return bot;
            }
            throw new Exception();
        }
        public static Bot SleepTime(this Bot bot, double inSeconds)
        {
            bot.SleepTime = inSeconds;
            return bot;
        }


        public static Bot Sleep(this Bot bot, double inSeconds)
        {
            ITask task = bot.Tasks.Last();
            task.Sleep = inSeconds;
            return bot;
        }
    }
}
