using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_3
{
    public class ProjectManagementSystem
    {
        private List<Task> tasks; // List to store tasks

        public ProjectManagementSystem()
        {
            tasks = new List<Task>();
        }

        // Function to read task information from a text file
        public void ReadTasksFromFile(string fileName)
        {
            try
            {
                // Read the file
                string[] lines = File.ReadAllLines(fileName);

                // Clear existing tasks
                tasks.Clear();

                // Process each line in the file
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    // Extract task information
                    string taskID = parts[0].Trim();
                    int timeNeeded = int.Parse(parts[1].Trim());

                    // Create a new task
                    Task task = new Task(taskID, timeNeeded);

                    // Check for dependencies
                    if (parts.Length > 2)
                    {
                        // Extract dependencies
                        for (int i = 2; i < parts.Length; i++)
                        {
                            string dependencyID = parts[i].Trim();

                            // Add the dependency to the task's dependency list
                            task.Dependencies.Add(dependencyID);
                        }
                    }

                    // Add the task to the tasks list
                    tasks.Add(task);
                }

                Console.WriteLine("Tasks loaded from the file successfully.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while reading the file: {e.Message}");
            }
        }

        // Function to add a new task to the project
        public void AddTask(string taskID, int timeNeeded, List<string> dependencies)
        {

            Task t = new Task(taskID, timeNeeded);

            t.addDependencies(dependencies);
            tasks.Add(t);
        }

        // Function to remove a task from the project
        public void RemoveTask(string taskID)
        {
            foreach (Task t in tasks) 
            {
                if(t.TaskID == taskID)
                {
                    tasks.Remove(t);
                }
            }
        }

        // Function to change the time needed to complete a task
        public void ChangeTimeNeeded(string taskID, int newTimeNeeded)
        {
            foreach (Task t in tasks)
            {
                if (t.TaskID == taskID)
                {
                    t.TimeNeeded = newTimeNeeded;
                }
            }
        }

        // Function to save the task information back to the input text file
        public void SaveTasksToFile(string fileName)
        {
            try
            {
                if (!fileName.EndsWith(".txt"))
                {
                    fileName += ".txt";
                }

                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (Task task in tasks)
                    {
                        // Format the task information as a string
                        string line = $"{task.TaskID}, {task.TimeNeeded}";

                        if (task.Dependencies.Count > 0)
                        {
                            string dependencies = string.Join(", ", task.Dependencies);
                            line += $", {dependencies}";
                        }

                        // Write the task information to the file
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("Tasks saved to the file successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while saving the file: {e.Message}");
            }
        }

        // Function to find a sequence of tasks that does not violate any dependencies
        //public List<string> FindTaskSequence()
        //{
        // Implement a suitable algorithm (e.g., topological sorting) to find the task sequence
        // Return the sequence as a list of task IDs
        //}

        // Function to find the earliest possible commencement time for each task
        //public Dictionary<string, int> FindEarliestTimes()
        //{
        // Implement a suitable algorithm (e.g., depth-first search) to find the earliest times
        // Return the earliest times as a dictionary with task IDs as keys and times as values
        //}
    }
}
