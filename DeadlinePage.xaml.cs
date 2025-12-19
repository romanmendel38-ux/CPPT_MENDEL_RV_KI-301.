using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using lab4_18.Entity;
using lab4_18.Request;

namespace lab4_18.Pages;

public partial class DeadlinePage : Window, INotifyPropertyChanged
{
    private string _specialty;
    public ObservableCollection<Deadline> Deadlines { get; set; }

    public string Specialty
    {
        get { return _specialty; }
        set
        {
            _specialty = value;
            OnPropertyChanged();
        }
    }

    public DeadlinePage()
    {
        InitializeComponent();
        Deadlines = new ObservableCollection<Deadline>();
        DataContext = this;
    }
    
    private async void OnShowDeadlinesClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Deadlines.Clear();

            var deadlinesFromServer = await GetDeadlines.Send(_specialty);

            foreach (var deadline in deadlinesFromServer)
            {
                Deadlines.Add(deadline);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}