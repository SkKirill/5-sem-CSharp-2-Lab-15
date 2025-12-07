using System.Windows;
using System.Windows.Input;
using AvlTreeData.Trees;
using AvlTreeLibrary.Trees;
using TreeViewForm.Views;

namespace TreeViewForm.Models;

public class MainWindowViewModel : BaseViewModel
{
    // ——————————————————————————————
    //     ХРАНИМ ТРИ ДЕРЕВА
    // ——————————————————————————————
    private ITree<int> _intTreeLinked = new LinkedTree<int>();
    private ITree<int> _intTreeArray = new ArrayTree<int>();

    private ITree<string> _stringTreeLinked = new LinkedTree<string>();
    private ITree<string> _stringTreeArray = new ArrayTree<string>();

    // ——————————————————————————————
    //     ВЫБОР ИЗ COMBOBOX
    // ——————————————————————————————
    public string TextBoxValue { get; set; }

    public List<string> ComboBoxItems { get; set; } =
    [
        "int",
        "string"
    ];

    public List<string> ComboBoxTreesItems { get; set; } =
    [
        "linked",
        "array",
        "immutable"
    ];

    public string SelectTypeTree { get; set; } = "int";
    public string SelectedTrees { get; set; } = "linked";

    // ——————————————————————————————
    // ВСПОМОГАТЕЛЬНЫЙ МЕТОД: выбрать нужное дерево
    // ——————————————————————————————
    private ITree<T> GetTree<T>() where T : IComparable<T>
    {
        if (typeof(T) == typeof(int))
        {
            return SelectedTrees switch
            {
                "linked" => (ITree<T>)_intTreeLinked,
                "array" => (ITree<T>)_intTreeArray,
                "immutable" => new UnmutableTree<T>((ITree<T>)_intTreeLinked)
            };
        }

        if (typeof(T) == typeof(string))
        {
            return SelectedTrees switch
            {
                "linked" => (ITree<T>)_stringTreeLinked,
                "array" => (ITree<T>)_stringTreeArray,
                "immutable" => new UnmutableTree<T>((ITree<T>)_stringTreeArray)
            };
        }

        throw new Exception("Неподдерживаемый тип");
    }


    // ——————————————————————————————
    //              КОМАНДЫ
    // ——————————————————————————————
    public ICommand AddCommand => new RelayCommand(_ => Add());
    public ICommand DeleteCommand => new RelayCommand(_ => Delete());
    public ICommand ContainsCommand => new RelayCommand(_ => Contains());
    public ICommand ExistsCommand => new RelayCommand(_ => Exists());
    public ICommand CheckForAllCommand => new RelayCommand(_ => CheckForAll());
    public ICommand FindAllCommand => new RelayCommand(_ => FindAll());
    public ICommand ForEachCommand => new RelayCommand(_ => ForEach());
    public ICommand ShowCommand => new RelayCommand(_ => Show());

    // ——————————————————————————————
    //         ОБРАБОТКА ДАННЫХ
    // ——————————————————————————————
    private void Add()
    {
        try
        {
            if (SelectTypeTree == "int")
            {
                if (int.TryParse(TextBoxValue, out int v))
                    GetTree<int>().Add(v);
                else
                {
                    MessageBox.Show("Введите число");
                }
            }
            else
            {
                GetTree<string>().Add(TextBoxValue);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void Delete()
    {
        try
        {
            if (SelectTypeTree == "int")
            {
                if (int.TryParse(TextBoxValue, out int v))
                    GetTree<int>().Remove(v);
                else
                {
                    MessageBox.Show("Введите число");
                }
            }
            else
            {
                GetTree<string>().Remove(TextBoxValue);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void Contains()
    {
        try
        {
            bool result;

            if (SelectTypeTree == "int")
            {
                if (!int.TryParse(TextBoxValue, out int v))
                {
                    MessageBox.Show("Введите число");
                    return;
                }

                result = GetTree<int>().Contains(v);
            }
            else
            {
                result = GetTree<string>().Contains(TextBoxValue);
            }

            MessageBox.Show(result ? "Есть" : "Нет");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void Exists()
    {
        // пример: элементы > введённого значения
        try
        {
            if (SelectTypeTree == "int")
            {
                if (!int.TryParse(TextBoxValue, out var v))
                {
                    MessageBox.Show("Введите число");
                    return;
                }

                var result = GetTree<int>().Any(x => x > v);

                MessageBox.Show(result ? "Да, есть" : "Нет");
            }
            else
            {
                var tree = GetTree<string>();
                var result = tree.Any(x => x.Contains(TextBoxValue));
                MessageBox.Show(result ? "Есть" : "Нет");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void CheckForAll()
    {
        try
        {
            if (SelectTypeTree == "int")
            {
                if (!int.TryParse(TextBoxValue, out int v))
                {
                    MessageBox.Show("Введите число");
                    return;
                }

                var result = GetTree<int>().All(x => x > v);
                MessageBox.Show(result ? "Все подходят" : "Не все");
            }
            else
            {
                var tree = GetTree<string>();
                var result = tree.All(x => x.Contains(TextBoxValue));
                MessageBox.Show(result ? "Все подходят" : "Не все");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void FindAll()
    {
        try
        {
            if (SelectTypeTree == "int")
            {
                if (!int.TryParse(TextBoxValue, out int v))
                {
                    MessageBox.Show("Введите число");
                    return;
                }

                var result = GetTree<int>().Where(x => x > v).ToList();
                MessageBox.Show("Найдено: " + string.Join(", ", result));
            }
            else
            {
                var result = GetTree<string>().Where(x => x.Contains(TextBoxValue)).ToList();
                MessageBox.Show("Найдено: " + string.Join(", ", result));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void ForEach()
    {
        try
        {
            if (SelectTypeTree == "int")
            {
                foreach (var x in GetTree<int>())
                    Console.WriteLine(x);
            }
            else
            {
                foreach (var x in GetTree<string>())
                    Console.WriteLine(x);
            }

            MessageBox.Show("Готово, смотрите консоль.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void Show()
    {
        try
        {
            if (SelectTypeTree == "int")
            {
                var window = new ShowTreeWindow(null, GetTree<int>());
                window.ShowDialog();
            }
            else
            {
                var window = new ShowTreeWindow(GetTree<string>());
                window.ShowDialog();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}