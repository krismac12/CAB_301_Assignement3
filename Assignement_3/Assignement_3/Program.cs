using Assignement_3;
using System;

class Program
{
    static void Main(string[] args)
    {
        ProjectManagementSystem projectManager = new ProjectManagementSystem();
        projectManager.ReadTasksFromFile("C:\\desk\\code\\Cab301\\CAB_301_Assignement3\\Assignement_3\\Assignement_3\\Test Text\\Test1.txt");

        string output = projectManager.FindEarliestTimes();
        Console.WriteLine(output);
    }
}