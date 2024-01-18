using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Benchmark;

namespace BenchmarkProject
{
    public class ProjectsContainer
    {
        public List<Project>? Projects { get; set; }
    }
    public class JSONReader
    {
        public ProjectsContainer? ReadProjects(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<ProjectsContainer>(json);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: The file was not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }

        }
    }
}
