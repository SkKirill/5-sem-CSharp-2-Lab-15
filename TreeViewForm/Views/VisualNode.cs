    namespace TreeViewForm.Views;

    public class VisualNode<T>
    {
        public double SubtreeWidth { get; set; }
        public T Value { get; set; }
        public VisualNode<T>? Left { get; set; }
        public VisualNode<T>? Right { get; set; }
    }