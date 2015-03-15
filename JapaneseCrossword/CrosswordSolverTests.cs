using System.IO;
using NUnit.Framework;

namespace JapaneseCrossword
{
    [TestFixture]
    public class CrosswordSolverTests
    {
	    private FileCrosswordSolver[] solvers;

        [TestFixtureSetUp]
        public void SetUp()
        {
	        solvers = new [] {
		        new FileCrosswordSolver(new CrosswordSolverCore()),
		        new FileCrosswordSolver(new ParallelCrosswordSolverCore())
	        };
        }

        [Test]
        public void InputFileNotFound([Values (0, 1)] int solverNumber)
        {
	        var solver = solvers[solverNumber];
            var solutionStatus = solver.Solve(Path.GetRandomFileName(), Path.GetRandomFileName());
            Assert.AreEqual(SolutionStatus.BadInputFilePath, solutionStatus);
        }

       [Test]
        public void IncorrectOutputFile([Values (0, 1)] int solverNumber)
        {
			var solver = solvers[solverNumber];
            var inputFilePath = @"TestFiles\SampleInput.txt";
            var outputFilePath = "///.&*#";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.BadOutputFilePath, solutionStatus);
        }

        [Test]
        public void IncorrectCrossword([Values (0, 1)] int solverNumber)
        {
			var solver = solvers[solverNumber];
            var inputFilePath = @"TestFiles\IncorrectCrossword.txt";
            var outputFilePath = Path.GetRandomFileName();
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.IncorrectCrossword, solutionStatus);
        }

        [Test]
        public void Simplest([Values (0, 1)] int solverNumber)
        {
			var solver = solvers[solverNumber];
            var inputFilePath = @"TestFiles\SampleInput.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\SampleInput.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.Solved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void Car([Values (0, 1)] int solverNumber)
        {
			var solver = solvers[solverNumber];
            var inputFilePath = @"TestFiles\Car.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Car.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.Solved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void Flower([Values (0, 1)] int solverNumber)
        {
			var solver = solvers[solverNumber];
            var inputFilePath = @"TestFiles\Flower.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Flower.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.Solved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void Winter([Values (0, 1)] int solverNumber)
        {
			var solver = solvers[solverNumber];
            var inputFilePath = @"TestFiles\Winter.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Winter.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.PartiallySolved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }
    }
}