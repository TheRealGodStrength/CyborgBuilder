using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyborgBuilder.Keyboard;
using CyborgBuilder.Mouse;
using CyborgBuilder.TaskRepo;

namespace CyborgBuilder.Robot
{
    public sealed class Bot
    {
        #region Singleton Pattern
        Bot() { } public string Name { get; set; }
        private static readonly object padLock = new object();
        private static Bot instance = null;
        public static Bot Instance
        {
            get
            {
                lock(padLock)
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
        private Repository Repo = new Repository();
        public void ExportSignatureRepository(string fileName)
        {
            Repo.Serialize(fileName);
        }
        public void ImportSignatureRepository(string fileName)
        {
            Repo = null;
            Repo = Repository.DeSerialize(fileName);
        }

        public List<ITask> Tasks = new List<ITask>();

        public void AddKeyboardTask(KeyboardFunctions.Lines function, bool updateOnIteration = false)
        {
            KeyboardTask kT = new KeyboardTask(function).UpdateOnIteration(updateOnIteration);
            Tasks.Add(kT);
            Repo.Add(kT);
        }
        public void AddMouseTask(MouseFunctions.Function function)
        {
            MouseTask mT = new MouseTask(function);
            Tasks.Add(mT);
            Repo.Add(mT);
        }
        public void AddMouseTask(MouseFunctions.Function function, int x, int y)
        {
            MouseTask mT = new MouseTask(function, x, y);
            Tasks.Add(mT);
            Repo.Add(mT);
        }
        public void AddMouseTask(MouseFunctions.Function function, params object[] args)
        {
            MouseTask mT = new MouseTask(function, args);
            Tasks.Add(mT);
            Repo.Add(mT);
        }
        public void RunAllTasks()
        {
            foreach(var task in Tasks)
            {
                task.Invoke();
            }
        }
        public void UnloadFromRepo()
        {
            foreach(object[] s in Repo.Signatures)
            {
                switch (s[0].GetType())
                {

                }
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
}
