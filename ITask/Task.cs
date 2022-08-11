﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CyborgBuilder.Interfaces;

namespace CyborgBuilder.TaskRepo
{
    public class Task : ITask
    {
        public TaskType Type { get; }
        public object[] Signature { get; set; }
        public int SleepTime { get; set; }
        public bool UpdateOnIteration { get; set; }
        public object[] ActionArguments { get; set; }
        public int Iterations { get; set; }
        public string[] InputText { get; set; }
        public void TaskFunction(object function) { }
        public void Invoke()
        {

        }
        public ITask LoadFromSignature(object[] lfs)
        {
            throw new NotImplementedException();
        }
    }
    public enum TaskType
    {
        Keyboard,
        Mouse
    }
}
