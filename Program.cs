using System.Text;
using TreeLibrary.Trees;

namespace TreeLibrary;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.InputEncoding = new UTF8Encoding();
        Console.OutputEncoding = new UTF8Encoding();

        var sampleData = new List<IntClassTester> { new(1), new(2), new(3), new(4), new(5) };

        // Тест ArrayTree
        var arrayTree = new ArrayTree<IntClassTester>();
        TreeTester.RunTests(arrayTree, sampleData);
        TreeTester.RunTestsWithImmutable(arrayTree);

        // Тест LinkedTree
        var linkedTree = new LinkedTree<IntClassTester>();
        TreeTester.RunTests(linkedTree, sampleData);
        TreeTester.RunTestsWithImmutable(linkedTree);
    }

    private class IntClassTester : IComparable<IntClassTester>
    {
        private int Value { get; }

        public IntClassTester(int value)
        {
            Value = value;
        }

        public int CompareTo(IntClassTester? other)
        {
            if (other == null)
            {
                return -1;
            }
            
            return other.Value - Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}