namespace TreeLibrary;

public class IntClassTester : IComparable<IntClassTester>
{

    private int Value { get; }

    public IntClassTester(int value)
    {
        Value = value;
    }
    public IntClassTester()
    {
        Value = Random.Shared.Next();
    }

    public int CompareTo(IntClassTester? other)
    {
        if (other == null)
        {
            return -1;
        }

        var r = other.Value - Value;
        return r;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}