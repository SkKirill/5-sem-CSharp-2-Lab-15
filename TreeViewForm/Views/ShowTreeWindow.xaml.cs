using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AvlTreeData.Trees;
using AvlTreeData.Views;

namespace TreeViewForm.Views;

public partial class ShowTreeWindow : Window
{
    public ShowTreeWindow(ITree<string>? treeString = null, ITree<int>? treeInt = null)
    {
        InitializeComponent();
        
        if (treeString is not null && !treeString.IsEmpty)
        {
            var root = treeString.ConvertToVisualNode();
            MeasureSubtree(root);

            var centerX = root!.SubtreeWidth / 2 + 40;
            TreeCanvas.Width = root.SubtreeWidth + 80;

            DrawTree(root, centerX, 40);
        }

        if (treeInt is not null && !treeInt.IsEmpty)
        {
            var root = treeInt.ConvertToVisualNode();
            MeasureSubtree(root);

            var centerX = root!.SubtreeWidth / 2 + 40;
            TreeCanvas.Width = root.SubtreeWidth + 80;

            DrawTree(root, centerX, 40);
        }
    }

    // -----------------------------------------------------------
    // Измерение ширины поддерева
    // -----------------------------------------------------------

    private double MeasureSubtree<T>(VisualNode<T>? node)
    {
        if (node == null) return 0;

        // размер текста
        var txt = node.Value!.ToString();
        var tb = new TextBlock { Text = txt, FontSize = 14 };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var textWidth = tb.DesiredSize.Width;

        var radius = Math.Max(20, textWidth / 2 + 10);
        var nodeWidth = radius * 2 + 40; // запас по бокам

        // рекурсивно считаем ширины детей
        var left = MeasureSubtree(node.Left);
        var right = MeasureSubtree(node.Right);

        var total = Math.Max(nodeWidth, left + right);
        node.SubtreeWidth = total;

        return total;
    }

    // -----------------------------------------------------------
    // Рисование
    // -----------------------------------------------------------

    private void DrawTree<T>(VisualNode<T>? node, double x, double y)
    {
        if (node == null) return;

        var textValue = node.Value!.ToString();

        var tb = new TextBlock { Text = textValue, FontSize = 14 };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var textWidth = tb.DesiredSize.Width;

        var radius = Math.Max(20, textWidth / 2 + 10);

        // Рендер узла (овал)
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

        var nextY = y + radius + 60;

        var hasLeft = node.Left != null;
        var hasRight = node.Right != null;

        // --- ЕДИНСТВЕННЫЙ РЕБЁНОК: смещаем в сторону ---
        const double singleOffset = 20;

        // Левый ребёнок
        if (hasLeft)
        {
            double leftX;

            if (!hasRight) // только ЛЕВЫЙ ребёнок
                leftX = x - singleOffset;
            else // оба ребёнка
                leftX = x - (node.Right?.SubtreeWidth ?? 0) / 2 - singleOffset*2;

            DrawLine(x, y + radius, leftX, nextY - radius);
            DrawTree(node.Left, leftX, nextY);
        }

        // Правый ребёнок
        if (hasRight)
        {
            double rightX;

            if (!hasLeft) // только ПРАВЫЙ ребёнок
                rightX = x + singleOffset;
            else // оба ребёнка
                rightX = x + (node.Left?.SubtreeWidth ?? 0) / 2 + singleOffset*2;

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