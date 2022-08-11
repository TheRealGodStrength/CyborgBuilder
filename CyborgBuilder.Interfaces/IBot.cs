using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CyborgBuilder.TaskRepo;

namespace CyborgBuilder.Interfaces
{
    public interface IBot
    {
        string Name { get; set; }
        IRepository Repo { get; set; }
        List<ITask> Tasks { get; set; }
        void RunAllTasks();
    }
    public static class IBotExtensions
    {
        public static IBot Instance(this IBot iBot)
        {
            return iBot;
        }
    }
}
