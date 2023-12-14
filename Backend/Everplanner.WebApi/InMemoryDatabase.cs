using Everplanner.WebApi.Core;
using Task = Everplanner.WebApi.Core.Task;

namespace Everplanner.WebApi;

public static class InMemoryDatabase
{
    static InMemoryDatabase()
    {
        var workers = new List<Worker>()
        {
            new(0, "Співробітник 1", 150, 15),
            new(1, "Співробітник 2", 100, 10),
            new(2, "Співробітник 3", 30, 3),
            new(3, "Співробітник 4", 20, 2),
        };

        var task10 = new Task(10, "Завдання 10", 5);
        task10.AvailableWorkers.AddRange(workers);

        var task9 = new Task(9, "Завдання 9", 2);
        task9.ChildTasks.AddRange(new List<Task> { task10 });
        task9.AvailableWorkers.AddRange(workers);

        var task8 = new Task(8, "Завдання 8", 4);
        task8.ChildTasks.AddRange(new List<Task> { task10 });
        task8.AvailableWorkers.AddRange(workers);

        var task7 = new Task(7, "Завдання 7", 1);
        task7.ChildTasks.AddRange(new List<Task> { task9 });
        task7.AvailableWorkers.AddRange(workers);
        
        var task6 = new Task(6, "Завдання 6", 7);
        task6.ChildTasks.AddRange(new List<Task>{ task9 });
        task6.AvailableWorkers.AddRange(workers);
        
        var task5 = new Task(5, "Завдання 5", 8);
        task5.ChildTasks.AddRange(new List<Task> { task8 });
        task5.AvailableWorkers.AddRange(workers);
        
        var task4 = new Task(4, "Завдання 4", 6);
        task4.ChildTasks.AddRange(new List<Task> { task7 });
        task4.AvailableWorkers.AddRange(workers);
        
        var task3 = new Task(3, "Завдання 3", 2);
        task3.ChildTasks.AddRange(new List<Task> { task6, task7 });
        task3.AvailableWorkers.AddRange(workers);

        var task2 = new Task(2, "Завдання 2", 10);
        task2.ChildTasks.AddRange(new List<Task> { task6 });
        task2.AvailableWorkers.AddRange(workers);

        var task1 = new Task(1, "Завдання 1", 5);
        task1.ChildTasks.AddRange(new List<Task> { task5, task6 });
        task1.AvailableWorkers.AddRange(workers);
        
        var tasks = new List<Task> { task1, task2, task3, task4, task5, task6, task7, task8, task9, task10 };
        var projects = new List<Project>()
        {
            new(0, "Проєкт 1", tasks, workers)
        };
        Users.Add(new User(0, "Ачілов Андрій", "andrry1@gmail.com", "password", projects));
    }

    public static List<User> Users { get; } = new List<User>();
}
