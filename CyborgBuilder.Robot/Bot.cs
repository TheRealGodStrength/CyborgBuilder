﻿using System;
using System.Collections.Generic;
using System.Linq;
using CyborgBuilder.Interfaces;
using CyborgBuilder.Keyboard;
using CyborgBuilder.Mouse;
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
        } 
        public string Name { get; set; }
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
        Keyboard.KeyboardTask t = new Keyboard.KeyboardTask();

        public void pass()
        {
            t.KT_Test(this);
        }
        public IRepository Repo { get;set; }


        public List<ITask> Tasks {get; set; }

        public void AddKeyboardTask(Lines function, bool updateOnIteration = false)
        {
            ITask kT = new KeyboardTask(function).UpdateOnIteration(updateOnIteration);
            Tasks.Add(kT);
            Repo.Add(kT);
        }
        public void AddMouseTask(MouseFunctions.Function function)
        {
            ITask mT = new MouseTask(function);
            Tasks.Add(mT);
            Repo.Add(mT);
        }
        public void AddMouseTask(MouseFunctions.Function function, int x, int y)
        {
            ITask mT = new MouseTask(function, x, y);
            Tasks.Add(mT);
            Repo.Add(mT);
        }
        public void AddMouseTask(MouseFunctions.Function function, params object[] args)
        {
            ITask mT = new MouseTask(function, args);
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
