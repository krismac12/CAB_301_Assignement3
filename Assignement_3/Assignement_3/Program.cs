using Assignement_3;
using System;

class Program
{
    static void Main(string[] args)
    {
        ProjectManagementSystem projectManager = new ProjectManagementSystem();
        MenuHandler menuHandler = new MenuHandler(projectManager);

        menuHandler.RunMenu();
    }
}


