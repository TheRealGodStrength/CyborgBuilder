using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CyborgBuilder.Keyboard;
using CyborgBuilder.Interfaces;
using CyborgBuilder.Mouse;

namespace CyborgBuilder.TaskRepo
{
    [XmlRoot("Repository")]
    public class Repository : IRepository
    {

        [XmlArrayItem("Signatures", typeof(object[]), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ISignature Signatures { get; set; }

        public List<ITask> Tasks { get; set; }
        public Repository()
        {
            Tasks = new List<ITask>();
        }
        public void Add(ITask task)
        {

        }
        private readonly Type kType = typeof(KeyboardTask);
        private readonly Type mType = typeof(MouseTask);
        private readonly Type sType = typeof(Signature);
        public void Receive(ISignature signature)
        {

            var taskType = signature.Type;
            if(taskType == Events.TaskType.Keyboard)
            {
                ITask kt = new KeyboardTask();

                var kProperties = kType.GetProperties().Where(p => p.CanRead && p.CanWrite);
                foreach(var property in kProperties)
                {
                    PropertyInfo pi = sType.GetProperty(property.Name);
                    if(pi != null)
                    {
                        property.SetValue(kt, pi.GetValue(signature));
                    }
                }
                Tasks.Add(kt);
            }
            else if (taskType == Events.TaskType.Mouse)
            {
                ITask mt = new MouseTask();


                var mProperties = mType.GetProperties().Where(p => p.CanRead && p.CanWrite);
                foreach (var property in mProperties)
                {
                    PropertyInfo pi = sType.GetProperty(property.Name);
                    if(pi != null)
                    property.SetValue(mt, pi.GetValue(signature));
                }
                Tasks.Add(mt);
            }
           // MouseTask mt2 = new MouseTask()
           // {
           //     LeftButton = (Events.MouseButton.Left)1
           // };
           // Type t2 = typeof(MouseTask);
           // Type _t = typeof(Signature);
           // //var properties = t.GetProperties().Where(p => p.CanRead && p.CanWrite);

           //// PropertyInfo pi = t.GetProperty("LeftButton");
           // PropertyInfo _pi = _t.GetProperty(pi.Name);
           // pi.SetValue(mt, _pi.GetValue(signature));

            //CopyValues((MouseTask)mt,(Signature)signature);

            //Type type = signature.GetType().GetProperty("Type").GetType();
            //var value = signature.GetType().GetProperty("Type").GetValue(signature, null);
            //PropertyInfo[] props = type.GetProperties();

            //IMouseTask mt = new MouseTask();
            //Type t = typeof(MouseTask);
            //PropertyInfo[] mpi = mt.GetType().GetProperties();
            //PropertyInfo pi = t.GetProperty("Type");
            //foreach (PropertyInfo p in mpi)
            //{
            //    var propinfo = t.GetProperty(p.Name);
            //    var propVal = signature.GetType().GetProperty(p.Name).GetValue(signature, null);
            //    if (propVal != null)
            //    {
            //        propinfo.SetValue(mt, propVal);
            //    }
            //}
            //pi.SetValue(mt, value);


        }
        public void CopyValues<T>(T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value);
            }
        }
        void Serialize(string fileName)
        {
            XmlSerializer s = new XmlSerializer(typeof(Repository));
            TextWriter tw = new StreamWriter(fileName);
            s.Serialize(tw, this);
            tw.Close();
        }
        static Repository DeSerialize(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            XmlSerializer deSerializer = new XmlSerializer(typeof(Repository));
            var result = (Repository)deSerializer.Deserialize(reader);
            return result;
        }

    }
    public static class RepositoryExt
    {
        public static Repository ImportSignatures(this Repository repo, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            XmlSerializer deSerializer = new XmlSerializer(typeof(Repository));
            repo = (Repository)deSerializer.Deserialize(reader);
            return repo;
        }
    }
}
