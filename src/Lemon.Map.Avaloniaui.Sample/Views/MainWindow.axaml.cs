using Avalonia.Controls;

namespace Lemon.Map.Avaloniaui.Sample.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var handle = TryGetPlatformHandle();
            var winHandle = handle!.Handle;
        }
    }
}