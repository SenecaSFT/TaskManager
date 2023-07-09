using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class TaskManager
    {
        public List<Task> tasks = new List<Task>();
        public List<Task> completedTasks = new List<Task>();
        public Dictionary<int, string> taskDictName = new Dictionary<int, string>();
        public Dictionary<int, string> taskDictDesc = new Dictionary<int, string>();
        public Dictionary<int, string> taskCompletedDictName = new Dictionary<int, string>();
        public Dictionary<int, string> taskCompletedDictDesc = new Dictionary<int, string>();
        public User User { get; set; }
        public TaskManager(User user)
        {
            this.User = user;
            tasks = new List<Task>();
        }
        public void ShowTaskMenu()
        {
            Dictionary<int, Action> menu = new Dictionary<int, Action>
            {
                {0, ExitProgram },
                {1,  ViewTasks},
                //{2, ViewCompletedTasks },
                //{3, CompleteTask },
                {4, AddNewTask },
                {5, RemoveTask }
            };

            Console.WriteLine("*MENU*");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. View Tasks");
            //Console.WriteLine("2. View Completed Tasks");
            //Console.WriteLine("3. Complete Task");
            Console.WriteLine("4. Add New Task");
            Console.WriteLine("5. Remove Task");
            Console.Write("> ");

            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Writer.WriteErr("ERR: INPUT IS NULL");
                ShowTaskMenu();
            }
            else if (!int.TryParse(input, out int menuSelection)) 
            {
                Writer.WriteErr("ERR: BAD INPUT");
                ShowTaskMenu();
            }
            else
            {
                if (!menu.ContainsKey(menuSelection))
                {
                    Writer.WriteErr("ERR: OPTION DOES NOT EXIST!");
                    ShowTaskMenu();
                }
                else
                {
                    Action selectedOption = menu[menuSelection];
                    selectedOption.Invoke();
                }
            }

        }
        public static void ExitProgram()
        {
            Environment.Exit(0);
        }
        public void ViewTasks() 
        {
            if (!DataManager.VerifyTaskData())
            {
                Console.WriteLine("There are no Tasks!");
                Console.WriteLine("ENTER TO RETURN");
                Console.Write("> ");
                Console.ReadLine();
                ShowTaskMenu();
            }

            this.tasks = DataManager.GetTasks();
            // the dictionaries MUST be purged evertime function is called and repopulated.
            // yes, perhaps its slow, but its consistent and works

            this.taskDictName.Clear();
            this.taskDictDesc.Clear();


            Console.WriteLine($"Here are your tasks, {this.User.Firstname} {this.User.Lastname}");

            int iterator = 1;
            foreach (var item in this.tasks)
            {
                Console.WriteLine($"{iterator}. {item.Name}");
                this.taskDictName.Add(iterator, item.Name);
                this.taskDictDesc.Add(iterator, item.Description);
                iterator++;
            }

            Console.WriteLine("Select task to view description, or 0 for return: ");
            Console.Write("> ");


            if (!int.TryParse(Console.ReadLine(), out int userTaskSelection))
            {
                Writer.WriteErr("ERR: BAD INPUT");
                ViewTasks();
            }
            else
            {
                if (userTaskSelection == 0)
                {
                    // call save every iteration
                    Console.Write("Saving Task...");
                    DataManager.SaveTasks(this.tasks);

                    return; // this is valid! oh i love you c#
                }
                else
                {
                    try
                    {
                        string userTaskName = this.taskDictName[userTaskSelection];
                        string userTaskDesc = this.taskDictDesc[userTaskSelection];
                        Console.Clear();
                        Console.WriteLine("===");
                        Console.WriteLine($"{userTaskName}");
                        Console.WriteLine("===");
                        Console.WriteLine("Description:");
                        Console.WriteLine(userTaskDesc);
                        Console.WriteLine();

                        Console.Write("Saving Task...");
                        DataManager.SaveTasks(this.tasks);
                    }
                    catch (Exception)
                    {
                        Writer.WriteErr("ERR: COULD NOT FIND TASK!");
                    }
                }
            }
        }
        public void AddNewTask() 
        {
            string? taskName = null;
            string? taskDesc = null;

            Console.WriteLine("*ADD NEW TASK*");
            Console.WriteLine("Enter Task Name: ");
            Console.Write("> ");
            taskName = Console.ReadLine();

            Console.WriteLine("Enter Task Description: ");
            Console.Write("> ");
            taskDesc = Console.ReadLine();

            if (string.IsNullOrEmpty(taskName) || string.IsNullOrEmpty(taskDesc)) 
            {
                Writer.WriteErr("ERR: INPUT IS NULL");
                AddNewTask();
            }
            else
            {
                Task tk = new Task(taskName, taskDesc);
                this.tasks.Add(tk); // should be compatable with view tasks
                Console.WriteLine("Task Added Succesfully!");

                DataManager.SaveTasks(this.tasks);
            }
        }
        public void RemoveTask() 
        {
            if (!DataManager.VerifyTaskData())
            {
                Console.WriteLine("There are no Tasks!");
                Console.WriteLine("ENTER TO RETURN");
                Console.Write("> ");
                Console.ReadLine();
                ShowTaskMenu();
            }

            string? taskName = null;

            Console.WriteLine("*REMOVE TASK*");
            Console.WriteLine("Enter Task Name: ");
            Console.Write("> ");
            taskName = Console.ReadLine();

            if (string.IsNullOrEmpty(taskName))
            {
                Writer.WriteErr("ERR: INPUT IS NULL");
                RemoveTask();
            }
            else
            {
                Task tk = this.tasks.FirstOrDefault(x => x.Name.Equals(taskName.Trim()));
                try
                {
                    this.tasks.Remove(tk); // should be compatable with view tasks
                    DataManager.RemoveTask(tk);
                    Console.WriteLine("Task Removed Succesfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Writer.WriteErr("TASK DOES NOT EXIST");
                    ShowTaskMenu();
                }
            }
        }
    }
}
