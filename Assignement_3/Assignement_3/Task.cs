using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_3
{
    public class Task
    {
        public string TaskID { get; set; }
        public int TimeNeeded { get; set; }
        public List<string> Dependencies { get; set; }

        public Task(string taskID, int timeNeeded)
        {
            TaskID = taskID;
            TimeNeeded = timeNeeded;
            Dependencies = new List<string>();
        }

        public void addDependencies(List<string> dependencies)
        {
            Dependencies.AddRange(dependencies);
        }


    }
}
