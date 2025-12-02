using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeViewForm.Models;

public abstract class BaseViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}