using System.Linq.Expressions;
using Newtonsoft.Json;

namespace TaskManager
{
    class Program
    {
        
        static void Main(string[] args)
        {
            bool loop=true;
            string number;
            MainMenu mainMenu = new MainMenu();
            while (loop)
            {
                mainMenu.Menu();
                number = Console.ReadLine();
                mainMenu.MenuOperations(number,ref loop);
            }
        }
    }

    public class Task
    {
        public static int counter = 1;
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
        public Task(string title, string description)
        {
            ID = counter++;
            Title = title;
            Description = description;
            CreatedAt = DateTime.Now;
            IsCompleted = false;
        }
    }

    public class TaskOperations
    {

        public static List<Task> tasks = new List<Task>();
        public static string filePath = "task.json";
        public static void AddTask(Task task)
        {

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                tasks = JsonConvert.DeserializeObject<List<Task>>(json) ?? new List<Task>();
            }

            tasks.Add(task);
            string updatedJson = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
            Console.WriteLine("You have successfully added the task.");
        }
        public static void DeleteTask(int id)
        {
            string json = File.ReadAllText(filePath);
            tasks = JsonConvert.DeserializeObject<List<Task>>(json) ?? new List<Task>();
            Task taskToRemove=tasks.FirstOrDefault(t=>t.ID==id);

            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
                Console.WriteLine("You have successfully deleted the task.");
            }
            else { Console.WriteLine("Wrong ID."); }
            string updatedJson = JsonConvert.SerializeObject(tasks,Formatting.Indented);
            File.WriteAllText (filePath, updatedJson);
        }
        public static void UpdateTask(int id, string? titleUpdate, string? descriptionUpdate,bool @bool)
        {
            string json= File.ReadAllText(filePath);
            tasks=JsonConvert.DeserializeObject<List<Task>>(json) ?? new List<Task>();
            Task taskToUpdate = tasks.FirstOrDefault(t=>t.ID==id);

            if (taskToUpdate != null) 
            {
                if (titleUpdate != null) { taskToUpdate.Title = titleUpdate; }
                if (descriptionUpdate != null) {taskToUpdate.Description = descriptionUpdate; }
                taskToUpdate.IsCompleted =@bool;
                Console.WriteLine("You have successfully update the task.");
            }
            else { Console.WriteLine("Wrong ID."); }
        }
    }

    public class TaskList
    {
        private static readonly string filePath= "task.json";
        private static List<Task> tasks;
 
        public static void ListTasks()
        {
            string json = File.ReadAllText(filePath);
            tasks=JsonConvert.DeserializeObject<List<Task>>(json) ?? new List<Task>();
            foreach (Task task in tasks) 
            {
                Console.WriteLine($"ID={task.ID}, Title:{task.Title}, Description:{task.Description}.");
            }
        }
        public static void ListCompletedTasks()
        {
            string json = File.ReadAllText(filePath);
            tasks = JsonConvert.DeserializeObject<List<Task>>(json) ?? new List<Task>();
            tasks=tasks.Where(t=>t.IsCompleted==true).ToList();
            foreach (Task task in tasks)
            {
                Console.WriteLine($"ID={task.ID}, Title:{task.Title}, Description:{task.Description}.");
            }
        }
        public static void SortByDate()
        {
            string json = File.ReadAllText(filePath);
            tasks = JsonConvert.DeserializeObject<List<Task>>(json) ?? new List<Task>();
            var sortedTasks = tasks.OrderBy(t => t.CreatedAt).ToList();
            foreach(Task task in sortedTasks)
            {
                Console.WriteLine($"ID={task.ID}, Title:{task.Title}, Description:{task.Description}.");
            }
        }
    }

    public class MainMenu
    {

        int Id;
        string title;
        string description;
        string boolControl;
        bool @bool;
        bool loop;
        public void Menu()
        {
            Console.WriteLine("\nSelect an option:\n");
            Console.WriteLine("1 - Add New Task");
            Console.WriteLine("2 - List All Tasks");
            Console.WriteLine("3 - Update Task");
            Console.WriteLine("4 - Delete Task");
            Console.WriteLine("5 - List Completed Tasks");
            Console.WriteLine("6 - Sort Tasks by Date");
            Console.WriteLine("7 - Exit");
        }
        public void MenuOperations(string deger, ref bool loop)
        {
            switch (deger)
            {
                case "1":
                    Console.WriteLine("Enter the title of task.");
                    title = Console.ReadLine();
                    Console.WriteLine("Enter the description.");
                    description = Console.ReadLine();
                    Task task = new Task(title, description);
                    TaskOperations.AddTask(task);
                    break;
                case "2":
                    Console.WriteLine("\nYour task list:\n");
                    TaskList.ListTasks();
                    break;
                case "3":
                    Console.WriteLine("Enter the ID of task.");
                    Id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter the title change.");
                    title = Console.ReadLine();
                    Console.WriteLine("Enter the description change.");
                    description = Console.ReadLine();
                    Console.WriteLine("Did you complete the task.(Yes/No)");
                    boolControl = Console.ReadLine();
                    if(boolControl.ToLower()== "yes") { @bool = true; }
                    else { @bool = false; }
                    TaskOperations.UpdateTask(Id, title, description,@bool);
                    break;
                case "4":
                    Console.WriteLine("Enter the ID of task.");
                    Id = Convert.ToInt32(Console.ReadLine());
                    TaskOperations.DeleteTask(Id);
                    break;
                case "5":
                    Console.WriteLine("Your completed tasks list:\n");
                    TaskList.ListCompletedTasks();
                    break;
                case "6":
                    Console.WriteLine("Your list sorted by time");
                    TaskList.SortByDate();
                    break;
                case "7":
                    Console.WriteLine("Exiting the program...");
                    loop = false;
                    break;
                default:
                    Console.WriteLine("You entered an incorrect value.");
                    break;
            }
        }
    }
}