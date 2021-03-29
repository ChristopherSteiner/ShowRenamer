using ShowRenamer.ViewModels;
using System.Windows;

namespace ShowRenamer
{
    /// <summary>
    /// Interaction logic for TmdbSearch.xaml
    /// </summary>
    public partial class TmdbSearchDialog : Window
    {
        public ITmdbSearchViewModel SearchViewModel { get; }

        public TmdbSearchDialog(ITmdbSearchViewModel searchViewModel)
        {
            InitializeComponent();
            DataContext = searchViewModel;
            SearchViewModel = searchViewModel;
        }
    }
}
