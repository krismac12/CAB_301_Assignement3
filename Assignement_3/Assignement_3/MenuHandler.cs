using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_3
{
    public class MenuHandler
    {
        private ProjectManagementSystem projectManager;

        public MenuHandler(ProjectManagementSystem manager)
        {
            projectManager = manager;
        }

        public void RunMenu()
        {
            while (true)
            {
                Console.WriteLine("----- Project Management System -----");
                Console.WriteLine("1. Load tasks from file");
                Console.WriteLine("2. Add a new task");
                Console.WriteLine("3. Remove a task");
                Console.WriteLine("4. Change task completion time");
                Console.WriteLine("5. Save tasks to file");
                Console.WriteLine("6. Find task sequence");
                Console.WriteLine("7. Find earliest task completion times");
                Console.WriteLine("0. Exit");
                Console.WriteLine("-------------------------------------");
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Enter the file name: ");
                        string fileName = Console.ReadLine();
                        fileName = GetFullFilePath(fileName);
                        projectManager.ReadTasksFromFile(fileName);
                        Console.WriteLine("-------------------------------------");
                        break;

                    case "2":
                        EnterTask();
                        break;

                    case "3":
                        RemoveTask();
                        break;

                    case "4":
                        ChangeTime();
                        break;

                    case "5":
                        Console.Write("Enter the file name: ");
                        string fileName2 = Console.ReadLine();
                        fileName2 = GetFullFilePath(fileName2);
                        projectManager.SaveTasksToFile(fileName2);
                        Console.WriteLine("-------------------------------------");
                        break;

                    case "6":
                        FindSequence();
                        break;

                    case "7":
                        FindEarliestTimes();
                        break;

                    case "0":
                        Console.WriteLine("Exiting the program...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void EnterTask()
        {
            Console.Write("Enter Task ID: ");
            string taskID = Console.ReadLine();
            Console.Write("Enter Time Needed: ");
            int timeNeeded = 0;
            try
            {
                timeNeeded = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.Write("Enter Valid Time");
                return;

            }
            List<string> dependencies = new List<string>();
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("1. Add Dependency");
                Console.WriteLine("2. Add Task");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Task t = DisplayAllTasks();
                        if(t == null)
                        {
                            dependencies.Add(t.TaskID);
                        }
                        break;

                    case "2":
                        Task task = new Task(taskID, timeNeeded);
                        if(dependencies.Count > 0)
                        {
                            task.addDependencies(dependencies);
                        }
                        projectManager.AddTask(task);
                        Console.WriteLine("\nAdded Task");
                        loop = false;
                        break;
                }
            }
            Console.WriteLine("-------------------------------------");
        }

        private void RemoveTask()
        {
            Console.WriteLine("1. Remove a Task");
            Console.WriteLine("2. Cancel");
            Console.Write("Enter your choice: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Task task = DisplayAllTasks();
                    if (task != null)
                    {
                        List<string> tasksToRemove = new List<string>();
                        tasksToRemove.Add(task.TaskID);
                        GetDependentTasks(task.TaskID, tasksToRemove);

                        foreach (string taskID in tasksToRemove)
                        {
                            projectManager.RemoveTask(taskID);
                        }

                        Console.WriteLine("Task(s) and their dependencies removed successfully.");
                    }
                    break;

                case "2":
                    break;
            }
            Console.WriteLine("-------------------------------------");
        }

        // Recursive Depth First Search function to get all dependent tasks
        private void GetDependentTasks(string taskID, List<string> tasksToRemove)
        {
            foreach (Task task in projectManager.tasks)
            {
                if (task.Dependencies.Contains(taskID))
                {
                    tasksToRemove.Add(task.TaskID);
                    GetDependentTasks(task.TaskID, tasksToRemove);
                }
            }
        }

        private void ChangeTime()
        {
            Console.WriteLine("1. Change Time");
            Console.WriteLine("2. Cancel");
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Task t = DisplayAllTasks();
                    if (t != null)
                    {
                        Console.Write("Enter new Time: ");
                        string newTimeString = Console.ReadLine();
                        try
                        {
                            int newTime = int.Parse(newTimeString);
                            projectManager.ChangeTimeNeeded(t.TaskID,newTime);
                            Console.WriteLine();
                        }
                        catch
                        {
                            Console.WriteLine("Invalid Time");
                            break;
                        }
                    }
                    break;

                case "2":
                    break;
            }
            Console.WriteLine("-------------------------------------");
        }
        // Function to display all tasks with numbers
        private Task DisplayAllTasks()
        {

            Console.WriteLine("Tasks:");
            if (projectManager.tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return null;
            }

            int index = 1;

            foreach (Task task in projectManager.tasks)
            {
                Console.WriteLine($"{index}. {task.TaskID}");
                index++;
            }
            Console.WriteLine("0. Cancel");

            Console.Write("Enter task number to select: ");
            int selection = int.Parse(Console.ReadLine());

            if (selection <= 0 || selection > projectManager.tasks.Count)
            {
                Console.WriteLine("Invalid selection.");
                return null;
            }
            Console.WriteLine("-------------------------------------");
            return projectManager.tasks[selection - 1];
        }

        private void FindSequence()
        {
            if (projectManager.tasks.Count != 0)
            {
                List<string> sequence = projectManager.FindTaskSequence();
                Console.Write("Enter the filename to save the sequence: ");
                string fileName = Console.ReadLine();

                fileName = GetFullFilePath(fileName);

                try
                {
                    File.WriteAllLines(fileName, sequence);
                    Console.WriteLine("Sequence saved to file successfully.");
                    Console.WriteLine("Sequence:");
                    foreach (string task in sequence)
                    {
                        Console.WriteLine(task);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred while saving the file: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("No tasks.");
            }
            Console.WriteLine("-------------------------------------");
        }

        private void FindEarliestTimes()
        {
            if (projectManager.tasks.Count != 0)
            {
                string earliestTimes = projectManager.FindEarliestTimes();

                Console.Write("Enter the filename to save the earliest times: ");
                string fileName = Console.ReadLine();
                fileName = GetFullFilePath(fileName);


                try
                {
                    File.WriteAllText(fileName, earliestTimes);
                    Console.WriteLine("Earliest times saved to file successfully.");
                    Console.WriteLine("Earliest times:");
                    Console.WriteLine(earliestTimes);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred while saving the file: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("No tasks.");
            }
        }



        private string GetFullFilePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName), "File name cannot be null or empty.");
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string fullFilePath = Path.Combine(currentDirectory, fileName);

            // Add .txt extension if not present
            if (Path.GetExtension(fullFilePath) != ".txt")
            {
                fullFilePath += ".txt";
            }

            return fullFilePath;
        }

    }
}
