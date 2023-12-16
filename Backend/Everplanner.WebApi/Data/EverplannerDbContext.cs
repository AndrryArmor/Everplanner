using Azure;
using Everplanner.WebApi.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using Task = Everplanner.WebApi.Core.Task;

namespace Everplanner.WebApi.Data;

public class EverplannerDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Worker> Workers { get; set; }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsUserWithEmailAsync(string email)
    {
        return await Users.AnyAsync(u => u.Email == email);
    }

    public EverplannerDbContext(DbContextOptions<EverplannerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Email)
            .IsUnique(true);

        modelBuilder.Entity<User>().HasData(
            new User("Ачілов Андрій", "andrry1@gmail.com", "password") { Id = 1 });

        modelBuilder.Entity<Project>().HasData(
            new Project("Проєкт 1", 1) { Id = 1 });

        modelBuilder.Entity<Worker>().HasData(
            new Worker("Співробітник 1", 150, 15, 1) { Id = 1 },
            new Worker("Співробітник 2", 100, 10, 1) { Id = 2 },
            new Worker("Співробітник 3", 30, 3, 1) { Id = 3 },
            new Worker("Співробітник 4", 20, 2, 1) { Id = 4 });

        modelBuilder.Entity<Task>().HasData(
            new Task("Завдання 1", 5, 1) { Id = 1 },
            new Task("Завдання 2", 10, 1) { Id = 2 },
            new Task("Завдання 3", 2, 1) { Id = 3 },
            new Task("Завдання 4", 6, 1) { Id = 4 },
            new Task("Завдання 5", 8, 1) { Id = 5 },
            new Task("Завдання 6", 7, 1) { Id = 6 },
            new Task("Завдання 7", 1, 1) { Id = 7 },
            new Task("Завдання 8", 4, 1) { Id = 8 },
            new Task("Завдання 9", 2, 1) { Id = 9 },
            new Task("Завдання 10", 5, 1) { Id = 10 });

        modelBuilder.Entity<Task>()
            .HasMany(e => e.ChildTasks)
            .WithMany(e => e.ParentTasks)
            .UsingEntity("ParentTask_ChildTask",
                r => r.HasOne(typeof(Task))
                    .WithMany()
                    .HasForeignKey("ChildTaskId")
                    .HasPrincipalKey(nameof(Task.Id)),
                l => l.HasOne(typeof(Task))
                    .WithMany()
                    .HasForeignKey("ParentTaskId")
                    .HasPrincipalKey(nameof(Task.Id)),
                j =>
                {
                    j.HasKey("ParentTaskId", "ChildTaskId");
                    j.HasData(
                        new { ParentTaskId = 1, ChildTaskId = 5 },
                        new { ParentTaskId = 1, ChildTaskId = 6 },
                        new { ParentTaskId = 2, ChildTaskId = 6 },
                        new { ParentTaskId = 3, ChildTaskId = 6 },
                        new { ParentTaskId = 3, ChildTaskId = 7 },
                        new { ParentTaskId = 4, ChildTaskId = 7 },
                        new { ParentTaskId = 5, ChildTaskId = 8 },
                        new { ParentTaskId = 6, ChildTaskId = 9 },
                        new { ParentTaskId = 7, ChildTaskId = 9 },
                        new { ParentTaskId = 8, ChildTaskId = 10 },
                        new { ParentTaskId = 9, ChildTaskId = 10 });
                });

        modelBuilder.Entity<Task>()
            .HasMany(e => e.AvailableWorkers)
            .WithMany(e => e.SubmittedTasks)
            .UsingEntity("TaskWorker",
                r => r.HasOne(typeof(Worker))
                    .WithMany()
                    .HasForeignKey("WorkerId")
                    .HasPrincipalKey(nameof(Worker.Id)),
                l => l.HasOne(typeof(Task))
                    .WithMany()
                    .HasForeignKey("TaskId")
                    .HasPrincipalKey(nameof(Task.Id))
                    .OnDelete(DeleteBehavior.ClientCascade),
                j =>
                {
                    j.HasKey("TaskId", "WorkerId");
                    j.HasData(
                        new { TaskId = 1, WorkerId = 1 },
                        new { TaskId = 1, WorkerId = 2 },
                        new { TaskId = 1, WorkerId = 3 },
                        new { TaskId = 1, WorkerId = 4 },
                        new { TaskId = 2, WorkerId = 1 },
                        new { TaskId = 2, WorkerId = 2 },
                        new { TaskId = 2, WorkerId = 3 },
                        new { TaskId = 2, WorkerId = 4 },
                        new { TaskId = 3, WorkerId = 1 },
                        new { TaskId = 3, WorkerId = 2 },
                        new { TaskId = 3, WorkerId = 3 },
                        new { TaskId = 3, WorkerId = 4 },
                        new { TaskId = 4, WorkerId = 1 },
                        new { TaskId = 4, WorkerId = 2 },
                        new { TaskId = 4, WorkerId = 3 },
                        new { TaskId = 4, WorkerId = 4 },
                        new { TaskId = 5, WorkerId = 1 },
                        new { TaskId = 5, WorkerId = 2 },
                        new { TaskId = 5, WorkerId = 3 },
                        new { TaskId = 5, WorkerId = 4 },
                        new { TaskId = 6, WorkerId = 1 },
                        new { TaskId = 6, WorkerId = 2 },
                        new { TaskId = 6, WorkerId = 3 },
                        new { TaskId = 6, WorkerId = 4 },
                        new { TaskId = 7, WorkerId = 1 },
                        new { TaskId = 7, WorkerId = 2 },
                        new { TaskId = 7, WorkerId = 3 },
                        new { TaskId = 7, WorkerId = 4 },
                        new { TaskId = 8, WorkerId = 1 },
                        new { TaskId = 8, WorkerId = 2 },
                        new { TaskId = 8, WorkerId = 3 },
                        new { TaskId = 8, WorkerId = 4 },
                        new { TaskId = 9, WorkerId = 1 },
                        new { TaskId = 9, WorkerId = 2 },
                        new { TaskId = 9, WorkerId = 3 },
                        new { TaskId = 9, WorkerId = 4 },
                        new { TaskId = 10, WorkerId = 1 },
                        new { TaskId = 10, WorkerId = 2 },
                        new { TaskId = 10, WorkerId = 3 },
                        new { TaskId = 10, WorkerId = 4 });
                });
    }
}
