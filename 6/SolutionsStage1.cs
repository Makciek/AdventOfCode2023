namespace _6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

public class SolutionsStage1(string fileName) : SolutionBase(fileName)
{
    protected override string GetResult()
    {
        var raceGame = new RaceGame(this.Lines.ToArray());
        return raceGame.GetErrorMargin().ToString();
    }
}
