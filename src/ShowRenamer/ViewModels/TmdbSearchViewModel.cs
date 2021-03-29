using GalaSoft.MvvmLight.Command;
using ShowRenamer.Models;
using ShowRenamer.Services.Tmdb;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ShowRenamer.ViewModels
{
    public class TmdbSearchViewModel : BaseViewModel, ITmdbSearchViewModel
    {
        private string searchQuery;
        private bool? dialogResult;
        private TmdbSearchShowResultModel selectedResult;
        private readonly ITmdbService tmdbService;

        public TmdbSearchViewModel(ITmdbService tmdbService)
        {
            this.tmdbService = tmdbService;
        }

        public string SearchQuery
        {
            get => searchQuery;
            set => SetField(ref searchQuery, value);
        }

        public bool? DialogResult
        {
            get => dialogResult;
            set => SetField(ref dialogResult, value);
        }

        public TmdbSearchShowResultModel SelectedResult
        {
            get => selectedResult;
            set => SetField(ref selectedResult, value);
        }


        public ObservableCollection<TmdbSearchShowResultModel> SearchResults { get; set; } = new ObservableCollection<TmdbSearchShowResultModel>();


        // Todo AsyncCommand
        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand ConfirmSelectionCommand => new RelayCommand(ConfirmSelection);

        private void Search()
        {
            SearchResults.Clear();

            TmdbPagedSearchShowResultModel searchResult = tmdbService.SearchTvShowAsync(searchQuery)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            searchResult.SearchResults.ToList().ForEach(result => SearchResults.Add(result));
        }

        private void ConfirmSelection()
        {
            if (selectedResult == null)
            {
                MessageBox.Show("Select a TV Show");
            }
            else
            {
                DialogResult = true;
            }
        }

        public void ResetDialog()
        {
            dialogResult = null;
        }
    }
}
