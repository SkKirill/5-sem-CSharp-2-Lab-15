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
                MeasureSubtree(root);

                double centerX = root.SubtreeWidth / 2 + 40;
                TreeCanvas.Width = root.SubtreeWidth + 80;

                DrawTree(root, centerX, 40);
            }

            if (treeInt != null)
            {
                var root = GetRoot(treeInt);
                MeasureSubtree(root);

                double centerX = root.SubtreeWidth / 2 + 40;
                TreeCanvas.Width = root.SubtreeWidth + 80;

                DrawTree(root, centerX, 40);
            }
        };
    }

    // -----------------------------------------------------------
    // Получение корня дерева
    // -----------------------------------------------------------

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
            Left = BuildArrayNode(tree, index * 2 + 1),
            Right = BuildArrayNode(tree, index * 2 + 2)
        };
    }

    // -----------------------------------------------------------
    // Измерение ширины поддерева
    // -----------------------------------------------------------

    private double MeasureSubtree<T>(VisualNode<T>? node)
    {
        if (node == null) return 0;

        // размер текста
        string txt = node.Value!.ToString();
        var tb = new TextBlock { Text = txt, FontSize = 14 };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        double textWidth = tb.DesiredSize.Width;

        double radius = Math.Max(20, textWidth / 2 + 10);
        double nodeWidth = radius * 2 + 40; // запас по бокам

        // рекурсивно считаем ширины детей
        double left = MeasureSubtree(node.Left);
        double right = MeasureSubtree(node.Right);

        double total = Math.Max(nodeWidth, left + right);
        node.SubtreeWidth = total;

        return total;
    }

    // -----------------------------------------------------------
    // Рисование
    // -----------------------------------------------------------

    private void DrawTree<T>(VisualNode<T>? node, double x, double y)
    {
        if (node == null) return;

        string textValue = node.Value!.ToString();

        var tb = new TextBlock { Text = textValue, FontSize = 14 };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        double textWidth = tb.DesiredSize.Width;

        double radius = Math.Max(20, textWidth / 2 + 10);

        // Овал
        var ellipse = new Ellipse
        {
            Width = radius * 2,
            Height = radius * 2,
            Stroke = Brushes.Black,
            Fill = Brushes.LightYellow,
            StrokeThickness = 2
        };
        Canvas.SetLeft(ellipse, x - radius);
        Canvas.SetTop(ellipse, y - radius);
        TreeCanvas.Children.Add(ellipse);

        // Текст
        var txt = new TextBlock { Text = textValue, FontSize = 14 };
        Canvas.SetLeft(txt, x - textWidth / 2);
        Canvas.SetTop(txt, y - 10);
        TreeCanvas.Children.Add(txt);

        // Расстояние по вертикали
        double nextY = y + radius + 60;

        // Левый ребенок
        if (node.Left != null)
        {
            double rightWidth = node.Right?.SubtreeWidth ?? 0;
            double leftX = x - rightWidth / 2;

            DrawLine(x, y + radius, leftX, nextY - radius);
            DrawTree(node.Left, leftX, nextY);
        }

        // Правый ребенок
        if (node.Right != null)
        {
            double leftWidth = node.Left?.SubtreeWidth ?? 0;
            double rightX = x + leftWidth / 2;

            DrawLine(x, y + radius, rightX, nextY - radius);
            DrawTree(node.Right, rightX, nextY);
        }
    }

    private void DrawLine(double x1, double y1, double x2, double y2)
    {
        var line = new Line
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
