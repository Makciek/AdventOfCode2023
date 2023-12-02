namespace Common;

public abstract class SolutionBase
{
    protected readonly List<string> Lines;

    public SolutionBase(string fileName)
    {
        this.Lines = File.ReadAllLines(fileName).ToList();
    }

    public void Run()
    {
        var result = this.GetResult();
        Console.WriteLine($"Result: {result}");
        Console.ReadLine();
    }

    protected abstract string GetResult();
}