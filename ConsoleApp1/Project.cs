using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkProject
{
    public class Project
    {
        public int MemoryCount { get; set; }
        public int AppTimeout { get; set; }
        public int TryCount { get; set; }
        public int MaxThreads { get; set; }
    }
}
