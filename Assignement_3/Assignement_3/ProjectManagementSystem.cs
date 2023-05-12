using System;
using System.Collections.Generic;
using System.IO;


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
                        List<string> dependencies = new List<string>();
                        for (int i = 2; i < parts.Length; i++)
                        {
                            string dependencyID = parts[i].Trim();

                            // Add the dependency to the task's dependency list
                            if (!dependencies.Contains(dependencyID))
                            {
                                dependencies.Add(dependencyID);
                            }
                        }

                        task.Dependencies = dependencies;
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
            // Check if the dependencies list contains any duplicates
            if (dependencies.Distinct().Count() != dependencies.Count)
            {
                Console.WriteLine("Error: Duplicate dependencies detected. Please ensure each dependency is unique.");
                return;
            }

            Task task = new Task(taskID, timeNeeded);
            task.Dependencies = dependencies;
            tasks.Add(task);
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
        public List<string> FindTaskSequence()
        {
            List<string> sequence = new List<string>();
            Dictionary<string, int> incomingEdges = new Dictionary<string, int>();

            // Initialize the incoming edges count for each task
            foreach (Task task in tasks)
            {
                incomingEdges[task.TaskID] = task.Dependencies.Count;
            }

            // Create a queue to hold tasks with no dependencies
            Queue<string> queue = new Queue<string>();

            // Enqueue tasks with no incoming edges (dependencies)
            foreach (Task task in tasks)
            {
                if (incomingEdges[task.TaskID] == 0)
                {
                    queue.Enqueue(task.TaskID);
                }
            }

            // Process the tasks in the queue
            while (queue.Count > 0)
            {
                string taskID = queue.Dequeue();
                sequence.Add(taskID);

                // Decrement incoming edges count for dependent tasks
                foreach (Task task in tasks)
                {
                    if (task.Dependencies.Contains(taskID))
                    {
                        incomingEdges[task.TaskID]--;

                        // If a task has no more incoming edges, enqueue it
                        if (incomingEdges[task.TaskID] == 0)
                        {
                            queue.Enqueue(task.TaskID);
                        }
                    }
                }
            }

            // Check if there are any remaining tasks with incoming edges (dependencies)
            foreach (int edgesCount in incomingEdges.Values)
            {
                if (edgesCount > 0)
                {
                    Console.WriteLine("Cannot find a valid task sequence. There are cyclic dependencies.");
                    return new List<string>();
                }
            }

            return sequence;
        }

        // Function to find the earliest possible commencement time for each task
        public string FindEarliestTimes()
        {
            Dictionary<string, int> earliestTimes = new Dictionary<string, int>();

            // Initialize visited flag for each task
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (Task task in tasks)
            {
                visited[task.TaskID] = false;
            }

            // Perform DFS for each task
            foreach (Task task in tasks)
            {
                if (!visited[task.TaskID])
                {
                    DFS(task, visited, earliestTimes);
                }
            }

            // Format the earliest times as a string
            string output = string.Empty;
            foreach (var kvp in earliestTimes)
            {
                output += $"{kvp.Key}, {kvp.Value - GetTimeNeeded(kvp.Key)}\n";
            }

            return output;
        }

        // Depth-first search for finding earliest times
        private void DFS(Task task, Dictionary<string, bool> visited, Dictionary<string, int> earliestTimes)
        {
            visited[task.TaskID] = true;

            int maxDependencyTime = 0;
            foreach (string dependencyID in task.Dependencies)
            {
                // Skip if dependency not found in tasks list
                if (!tasks.Any(t => t.TaskID == dependencyID))
                {
                    continue;
                }

                Task dependencyTask = tasks.First(t => t.TaskID == dependencyID);

                // Perform DFS for the dependency task if not visited
                if (!visited[dependencyTask.TaskID])
                {
                    DFS(dependencyTask, visited, earliestTimes);
                }

                // Update the maximum dependency time
                maxDependencyTime = Math.Max(maxDependencyTime, earliestTimes[dependencyTask.TaskID]);
            }

            // Calculate the earliest time for the current task
            int earliestTime = maxDependencyTime + GetTimeNeeded(task.TaskID);

            // Store the earliest time for the current task
            earliestTimes[task.TaskID] = earliestTime;
        }

        // Function to get the time needed for a task
        private int GetTimeNeeded(string taskID)
        {
            foreach (Task task in tasks)
            {
                if (task.TaskID == taskID)
                {
                    return task.TimeNeeded;
                }
            }
            return 0;
        }
    }
}
