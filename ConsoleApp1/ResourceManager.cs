using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BenchmarkProject
{
    public class ResourceManager
    {
        public static async Task<bool> RunApplicationAsync(string filepath, CancellationToken cancellationToken)
        {
            JSONReader reader = new JSONReader();
            
            if (string.IsNullOrEmpty(filepath)) filepath = Constants.JSONFilePath;

            ProjectsContainer? projectsContainer = reader.ReadProjects(filepath);

            List<Task> projectTasks = new List<Task>();

            if (projectsContainer != null && projectsContainer.Projects != null)
            {
                Console.WriteLine("Projects in total: " + projectsContainer.Projects.Count);
                
                SemaphoreSlim globalSemaphore = new SemaphoreSlim(Constants.MaxGlobalThreads);
                
                Console.WriteLine("Starting...");

                foreach (var project in projectsContainer.Projects)
                {

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break; 
                    }
                    await globalSemaphore.WaitAsync();
                    var projectTask = Task.Run(() => ExecuteProject(project, globalSemaphore, cancellationToken));
                    projectTasks.Add(projectTask);
                }

                await Task.WhenAll(projectTasks);
                return true;
            }
            else
            {
                Console.WriteLine(Constants.JSONFilePath + " json file not found");
                return false;
            };
        }

        static void ExecuteProject(Project project, SemaphoreSlim globalSemaphore, CancellationToken cancellationToken)
        {
            SemaphoreSlim projectSemaphore = new SemaphoreSlim(project.MaxThreads);

            for (int i = 0; i < project.TryCount; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                projectSemaphore.Wait();
                Task.Run(() =>
                {
                    MemoryLoader.MemoryLoad(project.MemoryCount, project.TryCount, cancellationToken);

                    projectSemaphore.Release();
                });
            }

            globalSemaphore.Release();
        }

    }
}
