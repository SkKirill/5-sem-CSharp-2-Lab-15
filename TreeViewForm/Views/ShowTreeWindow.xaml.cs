using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AvlTreeLibrary.Trees;
using TreeLibrary.Trees;

namespace TreeViewForm.Views;

public partial class ShowTreeWindow : Window
{
    private readonly ITree<string> _treeString;
    private readonly ITree<int> _treeInt;
    
    
    public ShowTreeWindow(ITree<string> treeString = null, ITree<int> treeInt = null)
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            if (treeString != null)
            {
                var root = GetRoot(treeString);
                DrawTree(root, 400, 40);
            }
            if (treeInt != null)
            {
                var root = GetRoot(treeInt);
                DrawTree(root, 400, 40);
            }
        };
    }
    
    private VisualNode<T>? GetRoot<T>(ITree<T> tree)
        where T : IComparable<T>
    {
        if (tree is LinkedTree<T> linked)
        {
            var rootField = typeof(LinkedTree<T>).GetField("_root",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var root = (Node<T>?)rootField?.GetValue(linked);
            return ConvertLinked(root);
        }

        if (tree is ArrayTree<T> arr)
            return ConvertArray(arr);

        return null;
    }
    
    private VisualNode<T>? ConvertLinked<T>(Node<T>? node)
    {
        if (node == null) return null;

        return new VisualNode<T>
        {
            Value = node.Value,
            Left = ConvertLinked(node.Left),
            Right = ConvertLinked(node.Right)
        };
    }
    
    private VisualNode<T>? ConvertArray<T>(ArrayTree<T> tree)
        where T : IComparable<T> 
    {
        return BuildArrayNode(tree, 0);
    }

    private VisualNode<T>? BuildArrayNode<T>(ArrayTree<T> tree, int index)
        where T : IComparable<T>
    {
        var arrField = typeof(ArrayTree<T>).GetField("_array", 
            BindingFlags.NonPublic | BindingFlags.Instance);

        var arr = (T?[])arrField!.GetValue(tree);

        if (index >= arr.Length || arr[index] == null)
            return null;

        return new VisualNode<T>
        {
            Value = arr[index]!,
            Left  = BuildArrayNode(tree, index * 2 + 1),
            Right = BuildArrayNode(tree, index * 2 + 2)
        };
    }
    
    private const double NodeRadius = 20;
    private const double HorizontalSpacing = 40;
    private const double VerticalSpacing = 60;

    private void DrawTree<T>(VisualNode<T>? node, double x, double y)
    {
        if (node == null) return;

        // Рисуем окружность
        var circle = new Ellipse
        {
            Width = NodeRadius * 2,
            Height = NodeRadius * 2,
            Stroke = Brushes.Black,
            Fill = Brushes.LightYellow,
            StrokeThickness = 2
        };
        Canvas.SetLeft(circle, x - NodeRadius);
        Canvas.SetTop(circle, y - NodeRadius);
        TreeCanvas.Children.Add(circle);

        // Подпись
        var text = new TextBlock
        {
            Text = node.Value!.ToString(),
            FontSize = 14
        };
        Canvas.SetLeft(text, x - 10);
        Canvas.SetTop(text, y - 10);
        TreeCanvas.Children.Add(text);

        // Линии -> потомки
        if (node.Left != null)
        {
            DrawLine(x, y, x - HorizontalSpacing, y + VerticalSpacing);
            DrawTree(node.Left, x - HorizontalSpacing, y + VerticalSpacing);
        }

        if (node.Right != null)
        {
            DrawLine(x, y, x + HorizontalSpacing, y + VerticalSpacing);
            DrawTree(node.Right, x + HorizontalSpacing, y + VerticalSpacing);
        }
    }

    private void DrawLine(double x1, double y1, double x2, double y2)
    {
        var line = new System.Windows.Shapes.Line
        {
            X1 = x1,
            Y1 = y1,
            X2 = x2,
            Y2 = y2,
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        TreeCanvas.Children.Add(line);
    }
}