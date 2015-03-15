namespace JapaneseCrossword
{
    public interface ICrosswordSolverCore
    {
        SolutionStatus Solve(Crossword crossword);
    }
}