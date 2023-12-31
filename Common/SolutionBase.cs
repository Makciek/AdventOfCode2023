﻿namespace Common;

using System.Diagnostics;

public abstract class SolutionBase(string fileName)
{
    protected readonly List<string> Lines = File.ReadAllLines(fileName).ToList();

    public void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();
        var result = this.GetResult();
        sw.Stop();
        Console.WriteLine($"Result: {result} in {sw.ElapsedMilliseconds}ms");
        Console.ReadLine();
    }

    public async Task RunAsync()
    {
        Stopwatch sw = Stopwatch.StartNew();
        var result = await this.GetResultAsync();
        sw.Stop();
        Console.WriteLine($"Result: {result} in {sw.ElapsedMilliseconds}ms");
        Console.ReadLine();
    }

    protected virtual string GetResult() => string.Empty;

    protected virtual async Task<string> GetResultAsync() => string.Empty;
}