using TreeLibrary.Trees;

namespace TreeLibrary;

internal static class Program
{
    public static void Main(string[] args)
    {
        var sampleData = new List<int> { 1, 2, 3, 4, 5 };

        // Тест ArrayTree
        var arrayTree = new ArrayTree<int>();
        TreeTester.RunTests(arrayTree, sampleData);
        TreeTester.RunTestsWithImmutable(arrayTree);

        // Тест LinkedTree
        var linkedTree = new LinkedTree<int>();
        TreeTester.RunTests(linkedTree, sampleData);
        TreeTester.RunTestsWithImmutable(linkedTree);
    }
}