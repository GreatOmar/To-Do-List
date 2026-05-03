using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;

namespace To_Do_List
{
    class Program
    {
        static void Main(string[] args)
        {
            // اسم الملف الذي سيتم تخزين المهام فيه
            string filePath = "tasks_data.txt";
            List<TaskItem> tasks = LoadTasksFromFile(filePath);

            while (true)
            {
                Console.Clear();
                int width = Console.WindowWidth;

                // 1. Header (ASCII Art)
                Console.ForegroundColor = ConsoleColor.Cyan;
                string[] logo = {
                    @" _____            _              _      _     _   ",
                    @"|_   _|__      __| | ___        | |    (_)___| |_ ",
                    @"  | |/ _ \    / _` |/ _ \       | |    | / __| __|",
                    @"  | | (_) |  | (_| | (_) |      | |___ | \__ \ |_ ",
                    @"  |_|\___/    \__,_|\___/       |_____||_|___/\__|"
                };

                foreach (string line in logo) Console.WriteLine(CenterText(line, width));
                Console.WriteLine(CenterText(new string('=', Math.Min(width, 60)), width));

                // 2. Task Display
                Console.WriteLine("\n" + CenterText("--- CURRENT TASKS ---", width));

                if (tasks.Count == 0)
                {
                    Console.WriteLine(CenterText("Your list is currently empty.", width));
                }
                else
                {
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        string statusIcon = tasks[i].IsCompleted ? "[X]" : "[ ]";
                        string taskLine = $"{i + 1}. {statusIcon} {tasks[i].Title}";

                        if (tasks[i].IsCompleted) Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(CenterText(taskLine, width));
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                }

                // 3. Menu
                Console.WriteLine("\n" + CenterText(new string('-', Math.Min(width, 40)), width));
                Console.WriteLine(CenterText("1. Add | 2. Complete | 3. Delete | 4. Exit", width));
                Console.Write("\n" + CenterText("Choose an option: ", width - 15));

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write(CenterText("Enter task title: ", width - 15));
                    string title = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        tasks.Add(new TaskItem { Title = title });
                        SaveTasksToFile(filePath, tasks);
                    }
                }
                else if (choice == "2")
                {
                    Console.Write(CenterText("Task number to complete: ", width - 5));
                    if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
                    {
                        tasks[index - 1].IsCompleted = true;
                        SaveTasksToFile(filePath, tasks);
                    }
                }
                else if (choice == "3")
                {
                    Console.Write(CenterText("Task number to delete: ", width - 5));
                    if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
                    {
                        tasks.RemoveAt(index - 1);
                        SaveTasksToFile(filePath, tasks);
                    }
                }
                else if (choice == "4") break;
            }
        }

        // --- دالة الحفظ ---
        static void SaveTasksToFile(string path, List<TaskItem> taskList)
        {
            var lines = taskList.Select(t => $"{t.Title}|{t.IsCompleted}");
            File.WriteAllLines(path, lines);
        }

        // --- دالة القراءة ---
        static List<TaskItem> LoadTasksFromFile(string path)
        {
            var list = new List<TaskItem>();
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        list.Add(new TaskItem
                        {
                            Title = parts[0],
                            IsCompleted = bool.Parse(parts[1])
                        });
                    }
                }
            }
            return list;
        }

        // --- دالة التوسيط ---
        static string CenterText(string text, int width)
        {
            if (string.IsNullOrEmpty(text) || text.Length >= width) return text;
            int padding = (width - text.Length) / 2;
            return new string(' ', padding) + text;
        }
    }

    // تعريف الكائن
    class TaskItem
    {
        public string Title { get; set; } = "";
        public bool IsCompleted { get; set; } = false;
    }
}