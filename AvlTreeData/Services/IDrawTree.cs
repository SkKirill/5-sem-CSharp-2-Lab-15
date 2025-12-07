using AvlTreeData.Views;

namespace AvlTreeData.Services;

public interface IDrawTree<T>
{
    VisualNode<T>? ConvertToVisualNode();
}