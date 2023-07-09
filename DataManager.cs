using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal static class DataManager
    {
        private static string DataDirectory = @$"../../../DATA/";
        private static string DataUserDirectory = @$"../../../DATA/USER/";
        private static string DataTaskDirectory = $@"../../../DATA/TASKS/";
        private static string DataUserFile = @$"../../../DATA/USER/USER.txt";
        private static string DataTaskFile = @$"../../../DATA/TASKS/TASKS.txt";

        public static bool VerifyDataFiles()
        {
            if (!Directory.Exists(DataDirectory) || !Directory.Exists(DataUserDirectory) || !Directory.Exists(DataTaskDirectory))
            {
                return false;
            }
            else if (!new DirectoryInfo(DataDirectory).Exists)
            { 
                return false;
            }
            else if (!new DirectoryInfo(DataUserDirectory).Exists || !new DirectoryInfo(DataTaskDirectory).Exists)
            {
                return false;
            }
            else if (!new FileInfo(DataUserFile).Exists || !new FileInfo(DataTaskFile).Exists)
            {
                return false;
            }
            else if (new FileInfo(DataUserFile).Length == 0)  //|| new FileInfo(DataTaskFile).Length == 0
            {
                return false;
            }
            else { return true; }
        }
        public static void CreateDataDirectories()
        {
            try
            {
                Directory.CreateDirectory(DataDirectory);
                Directory.CreateDirectory(DataUserDirectory);
                Directory.CreateDirectory(DataTaskDirectory);
            }
            catch (Exception)
            {
                Writer.WriteErr("ERR: FAILED TO CREATE DIRECTORIES");
                Environment.Exit(1);
            }
        }
        public static void CreateDataFiles()
        {
            try
            {
                using StreamWriter sw = File.CreateText(DataUserFile);
                using StreamWriter sw2 = File.CreateText(DataTaskFile);
            }
            catch (Exception)
            {
                Writer.WriteErr("ERR: FAILED TO CREATE FILES!");
                Environment.Exit(1);
            }
        }
        public static void PurgeDataFiles()
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }
        public static User CreateUser()
        {
            string? userFirstname;
            string? userLastname;
            string? userPassword;

            Console.WriteLine("Hello new User!");

            Console.WriteLine("Enter Firstname: ");
            Console.Write("> ");
            userFirstname = Console.ReadLine();

            Console.WriteLine("Enter Lastname: ");
            Console.Write("> ");
            userLastname = Console.ReadLine();

            Console.WriteLine("Enter Passwrod: ");
            Console.Write("> ");
            userPassword = Console.ReadLine();

            if (string.IsNullOrEmpty(userFirstname) || string.IsNullOrEmpty(userLastname) || string.IsNullOrEmpty(userPassword))
            {
                Writer.WriteErr("ERR: ONE OR MORE INPUT IS NULL!");
                CreateUser();
            }

            Console.WriteLine("User Created Succesfully!");
            return new User(userFirstname, userLastname, userPassword);
        }
        public static bool VerifyTaskData()
        {
            if (new FileInfo(DataTaskFile).Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void SaveUser(User user)
        {
            using (StreamWriter sw = new StreamWriter(DataUserFile, true))
            {
                sw.WriteLine(user.Firstname);
                sw.WriteLine(user.Lastname);
                sw.WriteLine(user.Password);
                sw.WriteLine("/");
            }
        }
        public static User GetUser()
        {
            using (StreamReader sr = new StreamReader(DataUserFile))
            {
                string? userFirstname = sr.ReadLine();
                string? userLastname = sr.ReadLine();
                string? userPassword = sr.ReadLine();

                if (string.IsNullOrEmpty(userFirstname) || string.IsNullOrEmpty(userLastname) || string.IsNullOrEmpty(userPassword)) 
                {
                    Writer.WriteErr("ERR: ERROR IN USER DATA");
                    Environment.Exit(1);
                }
                return new User(userFirstname, userLastname, userPassword);
            }
        }
        public static void SaveTasks(List<Task> taskList)
        {
            using (StreamWriter sw = new StreamWriter(DataTaskFile)) 
            {
                foreach (var task in taskList)
                {
                    sw.WriteLine($"NAME:{task.Name}");
                    sw.WriteLine($"DESC:{task.Description}");
                    sw.WriteLine("/");
                }
            }
        }
        public static List<Task> GetTasks()
        {
            List<Task> taskList = new List<Task>();

            using (StreamReader sr = new StreamReader(DataTaskFile)) 
            {
                List<string> taskNames = new List<string>();
                List<string> taskDescs = new List<string>();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string taskName;
                    string taskDesc;
                    if (line.Trim().Contains("NAME:"))    
                    {
                        taskName = line.Substring(5);
                        taskNames.Add(taskName);
                    }
                    else if (line.Trim().Contains("DESC:"))
                    {
                        taskDesc = line.Substring(5);
                        taskDescs.Add(taskDesc);
                    }
                    else
                    {
                    }
                }

                if (taskNames.Count != taskDescs.Count) 
                {
                    Writer.WriteErr("ERR: ");
                }
                else
                {
                    for (int i = 0; i < taskNames.Count; i++) 
                    {
                        Task tk = new Task(taskNames[i], taskDescs[i]);
                        taskList.Add(tk);
                    }
                }
            }
            return taskList;
        }
        public static void RemoveTask(Task tk)
        {
            // create temp file
            string DataTaskTempFile = $@"../../../DATA/TASKS/TASKS-TEMP.TXT";

            using (StreamReader sr = new StreamReader(DataTaskFile))
            using (StreamWriter sw = new StreamWriter(DataTaskTempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Trim().Contains(tk.Name))
                    {
                        sw.WriteLine(line);
                    }
                }
            }

            File.Delete(DataTaskFile);
            File.Move(DataTaskTempFile, DataTaskFile);  
        }

    }
}
