using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Program>();
    }

    World w;

    [GlobalSetup]
    public void Setup()
    {
        w = new World(a => a, a => a, 1, 1);
        w.Init();
        w.BuildForTest(1, 1);
    }

    [Benchmark]
    public void Struct()
    {
        w.Process(0.1f);
    }
}