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
                        projectManager.ReadTasksFromFile(fileName);
                        Console.WriteLine("-------------------------------------");
                        break;

                    case "2":
                        enterTask();
                        break;

                    case "3":
                        // Remove task logic
                        break;

                    case "4":
                        // Change task completion time logic
                        break;

                    case "5":
                        // Save tasks to file logic
                        break;

                    case "6":
                        // Find task sequence logic
                        break;

                    case "7":
                        // Find earliest task completion times logic
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

        public void enterTask()
        {
            Console.Write("Enter Task ID: ");
            string taskID = Console.ReadLine();
            Console.Write("Enter Time Needed: ");
            int timeNeeded = int.Parse(Console.ReadLine());
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("1. Add Dependency");
                Console.WriteLine("2. Add Task");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayTasks();
                        string input2 = Console.ReadLine();
                        break;

                    case "2":
                        Console.WriteLine("\nAdded Task");
                        loop = false;
                        break;
                }
            }
            Console.WriteLine("-------------------------------------");
        }
        public void DisplayTasks()
        {
            Console.WriteLine("----- Task List -----");

            for (int i = 0; i < projectManager.tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {projectManager.tasks[i].TaskID}");
            }

            Console.WriteLine("---------------------");
        }
    }
}
