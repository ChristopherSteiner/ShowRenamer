using ShowRenamer.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ShowRenamer.ViewModels
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        public string RootPath { get; set; }

        public int ShowId { get; set; }

        public string ShowName { get; set; }

        public string FilenameFilter { get; set; }

        public string IdentifierRegex { get; set; }

        public string TargetFileNamePattern { get; set; }

        public bool IncludeSubfolders { get; set; }

        public bool CopyToMainfolder { get; set; }

        public ObservableCollection<FileModel> Files { get; set; }

        ObservableCollection<BrowseFilterModel> BrowseFilters { get; set; }

        ObservableCollection<string> PreDefinedIdentifierRegexes { get; set; }

        BrowseFilterModel SelectedBrowseFilter { get; set; }

        public ICommand BrowseCommand { get; }

        public ICommand OpenSearchDialogCommand { get; }

        public ICommand ApplyCommand { get; }
    }
}
