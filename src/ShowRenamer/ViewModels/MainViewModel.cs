using GalaSoft.MvvmLight.Command;
using ShowRenamer.Models;
using ShowRenamer.Services.File;
using ShowRenamer.Services.Tmdb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ShowRenamer.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        private string rootPath;
        private int progress;
        private int showId;
        private string showName;
        private RegexFilterModel identifierRegex;
        private string targetFileNamePattern;
        private string filenameFilter;
        private bool includeSubfolders;
        private bool copyToMainfolder;
        private BrowseFilterModel selectedBrowseFilter;
        private readonly IFileService fileService;
        private readonly ITmdbService tmdbService;
        private readonly ITmdbSearchViewModel searchViewModel;

        public MainViewModel(
            IFileService fileService,
            ITmdbService tmdbService,
            ITmdbSearchViewModel searchViewModel)
        {
            this.fileService = fileService;
            this.tmdbService = tmdbService;
            this.searchViewModel = searchViewModel;
            SelectedBrowseFilter = BrowseFilters.First();

            TargetFileNamePattern = "S{Season}E{Episode} - {Name}.{Extension}";
        }

        public int Progress
        {
            get => progress;
            set => SetField(ref progress, value);
        }

        public string RootPath
        {
            get => rootPath;
            set => SetField(ref rootPath, value);
        }

        public int ShowId
        {
            get => showId;
            set
            {
                SetField(ref showId, value);
                Preview();
            }
        }

        public string ShowName
        {
            get => showName;
            set => SetField(ref showName, value);
        }

        public string FilenameFilter
        {
            get => filenameFilter;
            set
            {
                SetField(ref filenameFilter, value);
                Preview();
            }
        }

        public RegexFilterModel IdentifierRegex
        {
            get => identifierRegex;
            set
            {
                if (value?.Regex?.Contains("{Name}") ?? false)
                {
                    SetField(ref identifierRegex, null);
                }
                else
                {
                    SetField(ref identifierRegex, value);
                }
                Preview();
            }
        }

        public string TargetFileNamePattern
        {
            get => targetFileNamePattern;
            set
            {
                SetField(ref targetFileNamePattern, value);
                Preview();
            }
        }

        public bool IncludeSubfolders
        {
            get => includeSubfolders;
            set => SetField(ref includeSubfolders, value);
        }

        public bool CopyToMainfolder
        {
            get => copyToMainfolder;
            set
            {
                SetField(ref copyToMainfolder, value);
                Preview();
            }
        }

        public ObservableCollection<FileModel> Files { get; set; } = new ObservableCollection<FileModel>();

        public ObservableCollection<BrowseFilterModel> BrowseFilters { get; set; } = new ObservableCollection<BrowseFilterModel>
            {
                new BrowseFilterModel { Name = "All files", Pattern = "*" },
                new BrowseFilterModel { Name = "mkv", Pattern = "*.mkv" },
                new BrowseFilterModel { Name = "mp4", Pattern = "*.mp4" },
                new BrowseFilterModel { Name = "avi", Pattern = "*.avi" }
            };

        public ObservableCollection<RegexFilterModel> PreDefinedIdentifierRegexes { get; set; } = new ObservableCollection<RegexFilterModel>
        {
            new RegexFilterModel { Name = "S01E01", Regex = "(?:.*)[Ss](?<Season>\\d+)[Ee](?<Episode>\\d+)(?:.*)\\.(?<Extension>.+)" },
            new RegexFilterModel { Name = "Season01E01", Regex = "(?:.*)(?:Season|season)(?<Season>\\d+)(?:\\.?)[Ee](?<Episode>\\d+)(?:.*)\\.(?<Extension>.+)" },
            new RegexFilterModel { Name = "101", Regex = "(?:[-_]+)(?<Season>\\d{1})(?<Episode>\\d{2})(?:[-_]*)(?:.*)\\.(?<Extension>.+)" },
            new RegexFilterModel { Name = "1001 (Season 10 Episode 01)", Regex = "(?:[-_]+)(?<Season>\\d{2})(?<Episode>\\d{2})(?:[-_]*)(?:.*)\\.(?<Extension>.+)" },
            new RegexFilterModel { Name = "E01", Regex = "(?:\\.?)[Ee](?<Episode>\\d+)(?:.*)\\.(?<Extension>.+)" },
            new RegexFilterModel { Name = "Ep.01", Regex = "(?:\\.?)[EePp\\.](?<Episode>\\d+)(?:.*)\\.(?<Extension>.+)" },
        };

        public ObservableCollection<string> PreviewItems { get; set; } = new ObservableCollection<string>();

        public BrowseFilterModel SelectedBrowseFilter
        {
            get => selectedBrowseFilter;
            set
            {
                SetField(ref selectedBrowseFilter, value);
            }
        }

        public ICommand BrowseCommand => new RelayCommand(Browse);

        public ICommand OpenSearchDialogCommand => new RelayCommand(Search);

        public ICommand ApplyCommand => new RelayCommand(Apply);

        public ICommand PreviewCommand => new RelayCommand(Preview);

        private void Browse()
        {
            try
            {
                Files.Clear();
                string pattern = SelectedBrowseFilter.Pattern;
                if (!string.IsNullOrWhiteSpace(FilenameFilter))
                {
                    pattern = pattern.Replace("*", FilenameFilter);
                }

                fileService.GetFiles(RootPath, pattern, IncludeSubfolders).ToList().ForEach(file => Files.Add(new FileModel
                {
                    Name = fileService.GetFileName(file),
                    Path = file,
                    RootPath = RootPath
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Search()
        {
            searchViewModel.ResetDialog();
            var tmdbSearchDialog = new TmdbSearchDialog(searchViewModel);

            if (tmdbSearchDialog.ShowDialog() ?? false)
            {
                ShowId = searchViewModel.SelectedResult.Id;
                ShowName = searchViewModel.SelectedResult.Name;
            }
        }

        private void Apply()
        {
            if ((Files?.Any() ?? false)
                    && ShowId != 0
                    && !string.IsNullOrWhiteSpace(IdentifierRegex?.Regex)
                    && !string.IsNullOrWhiteSpace(TargetFileNamePattern))
            {
                Progress = 0;
                int counter = 0;
                Regex regex = new Regex(IdentifierRegex?.Regex);
                IEnumerable<TmdbEpisodeResultModel> episodes = tmdbService.GetAllEpisodes(ShowId)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                foreach (FileModel file in Files)
                {
                    fileService.RenameFile(file, ApplyNewFileName(file.Name, regex, episodes), copyToMainfolder);
                    counter++;
                    Progress = counter / Files.Count * 100;
                }
            }
        }

        private void Preview()
        {
            if ((Files?.Any() ?? false)
                    && ShowId != 0
                    && !string.IsNullOrWhiteSpace(IdentifierRegex?.Regex)
                    && !string.IsNullOrWhiteSpace(TargetFileNamePattern))
            {
                PreviewItems.Clear();
                Regex regex = new Regex(IdentifierRegex.Regex);
                IEnumerable<TmdbEpisodeResultModel> episodes = tmdbService.GetAllEpisodes(ShowId)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
                foreach (FileModel file in Files)
                {
                    PreviewItems.Add(ApplyNewFileName(file.Name, regex, episodes));
                }
            }
        }

        private string ApplyNewFileName(string originalFileName, Regex regex, IEnumerable<TmdbEpisodeResultModel> episodes)
        {
            try
            {
                Regex patternRegex = new Regex("(\\{\\w*\\})");
                Match match = regex.Match(originalFileName);
                string fileName = patternRegex.Replace(TargetFileNamePattern, (Match paramMatch) =>
                {
                    string paramName = paramMatch.Value[1..^1];
                    if (match.Groups.ContainsKey(paramName))
                    {
                        return match.Groups[paramName].Value;
                    }
                    else if (paramName == "Name")
                    {
                        string episodeName = null;
                        int? season = 1;
                        int? episode = default;
                        if (match.Groups.TryGetValue("Season", out Group seasonGroup) && int.TryParse(seasonGroup.Value, out int seasonNumber))
                        {
                            season = seasonNumber;
                        }
                        if (match.Groups.TryGetValue("Episode", out Group episodeGroup) && int.TryParse(episodeGroup.Value, out int episodeNumber))
                        {
                            episode = episodeNumber;
                        }
                        if (season.HasValue && episode.HasValue)
                        {
                            episodeName = episodes.FirstOrDefault(ep => ep.Number == episode.Value && ep.SeasonNumber == season.Value)?.Name;
                        }

                        return episodeName;
                    }
                    else
                    {
                        return paramName;
                    }
                });

                return fileService.RemoveInvalidCharacters(fileName);
            }
            catch
            {
                return originalFileName;
            }
        }
    }
}
