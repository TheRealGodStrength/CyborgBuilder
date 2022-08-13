using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using CyborgBuilder.Interfaces;
using System.Windows.Forms;
namespace CyborgBuilder.TaskRepo
{  
    [XmlRoot("Repository")]
    public class Repository : IRepository
    {

        [XmlArrayItem("Signatures", typeof(object[]),Form = System.Xml.Schema.XmlSchemaForm.Unqualified,IsNullable = false)]
        public ISignature Signatures { get; set; }

        public readonly List<ITask> Tasks = new List<ITask>(); 
        public void Add(ITask task)
        {

        }
        public void Receive(ISignature signature, ITask task)
        {
            if(task.Type == Events.TaskType.Mouse)
            {
                
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
    public static class RepositoryExtensions
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
