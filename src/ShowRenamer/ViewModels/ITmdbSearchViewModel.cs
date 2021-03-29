using ShowRenamer.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ShowRenamer.ViewModels
{
    public interface ITmdbSearchViewModel : INotifyPropertyChanged
    {
        public bool? DialogResult { get; set; }

        public string SearchQuery { get; set; }

        public TmdbSearchShowResultModel SelectedResult { get; set; }

        public ObservableCollection<TmdbSearchShowResultModel> SearchResults { get; set; }

        public ICommand SearchCommand { get; }

        public ICommand ConfirmSelectionCommand { get; }

        public void ResetDialog();
    }
}
