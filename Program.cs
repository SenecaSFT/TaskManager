namespace TaskManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskManager TM;

            Console.WriteLine("Hello, User!");
            Console.Write("Welcome to Task Manager!");
            Console.WriteLine("Verifying Data Files...");

            if (!DataManager.VerifyDataFiles())
            {
                Console.WriteLine("VERIFYING FILES FAILED!");
                Console.WriteLine("PERFORMING NEW CREATION >>");
                DataManager.CreateDataDirectories();
                DataManager.CreateDataFiles();
                User us = DataManager.CreateUser();
                DataManager.SaveUser(us);
                TM = new TaskManager(us);
            }
            else
            {
                Console.WriteLine("DATA VERIFIED SUCCESFULLY!");
                User us = DataManager.GetUser();
                TM = new TaskManager(us);
            }

            while (true)
            {
                TM.tasks = DataManager.GetTasks();
                TM.ShowTaskMenu();
            }
        }

    }
}