using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CyborgBuilder.Interfaces;

namespace CyborgBuilder.TaskRepo
{  
    [XmlRoot("Repository")]
    public class Repository : IRepository
    {

        [XmlArrayItem("Signatures", typeof(object[]),Form = System.Xml.Schema.XmlSchemaForm.Unqualified,IsNullable = false)]
        public object[] Signatures { get; set; }

        public void Add(ITask task)
        {
            var count = Signatures.Count();
            var sigs = Signatures;
            if (count < 1)
            {
                Array.Resize(ref sigs, 1);
                sigs[0] = task.Signature;
                Signatures = sigs;
            }
            else
            {
                Array.Resize(ref sigs, count + 1);
                sigs[count] = task.Signature;
                Signatures = sigs;
            }

        }
        public void test()
        {
            
        }
        public Repository()
        {
            Signatures = Array.Empty<object[]>();
        }
        public void ExportSignatures(string fileName)
        {
            Serialize(fileName);
        }
        void Serialize(string fileName)
        {
            XmlSerializer s = new XmlSerializer(typeof(Repository));
            TextWriter tw = new StreamWriter(fileName);
            s.Serialize(tw, this);
            tw.Close();
        }
        public static Repository ImportSignatures(string fileName)
        {
            return DeSerialize(fileName);
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
