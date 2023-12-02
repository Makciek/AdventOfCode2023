namespace Common;

public abstract class SolutionBase(string fileName)
{
    protected readonly List<string> Lines = File.ReadAllLines(fileName).ToList();

    public void Run()
    {
        var result = this.GetResult();
        Console.WriteLine($"Result: {result}");
        Console.ReadLine();
    }

    protected abstract string GetResult();
}