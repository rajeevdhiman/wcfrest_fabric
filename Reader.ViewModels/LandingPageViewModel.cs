using FabricWCF.Common.Objects;
using Reader.ServiceClient.RESTful;
using Reader.ViewModels.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace Reader.ViewModels
{
    public class LandingPageViewModel : ViewModelBase
    {
        private ICommand _navigateCommand;
        private ICommand _updateFeedCommand;
        private ICommand _searchCommand;

        public string DocumentUri
        {
            get { return GetValue(() => DocumentUri); }
            set { SetValue(() => DocumentUri, value); }
        }
        public string StatusText
        {
            get { return GetValue(() => StatusText); }
            set { SetValue(() => StatusText, value); }
        }

        public bool IsBusy
        {
            get { return GetValue(() => IsBusy); }
            set { SetValue(() => IsBusy, value); }
        }
        public bool IsNavigated
        {
            get { return GetValue(() => IsNavigated); }
            set { SetValue(() => IsNavigated, value); }
        }

        public bool AutoRefresh
        {
            get { return GetValue(() => AutoRefresh); }
            set { SetValue(() => AutoRefresh, value); }
        }

        public List<FeedItem> Items
        {
            get { return GetValue(() => Items); }
            set { SetValue(() => Items, value); }
        }

        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand<string>((uri) => NavigateToUriProcess(uri)));

        public NewsCategory SelectedCategory
        {
            get { return GetValue(() => SelectedCategory); }
            set { SetValue(() => SelectedCategory, value); }
        }

        public LocaleInfo SelectedLocale
        {
            get { return GetValue(() => SelectedLocale); }
            set { SetValue(() => SelectedLocale, value); }
        }
        public ICommand UpdateFeedCommand => _updateFeedCommand ?? (_updateFeedCommand = new RelayCommand<NewsCategory>(LoadItems, (c) => this.SelectedLocale != null));

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new RelayCommand<string>(SearchItems, (c) => string.IsNullOrWhiteSpace(c) == false && this.SelectedLocale != null));

        public async void LoadItems(NewsCategory cat = NewsCategory.Headlines)
        {
            var client = new NewsClient();
            var loc = this.SelectedLocale;
            IsBusy = true;
            if (loc != null)
            {
                StatusText = "Updating News Content...";
                Items = await client.GetFeed(cat, loc.LangCode, loc.RegionCode).ConfigureAwait(false);
                this.SelectedCategory = cat;
            }
            IsBusy = false;
            StatusText = "Ready";
        }
        public async void SearchItems(string searchText)
        {
            var client = new NewsClient();
            var loc = this.SelectedLocale;
            IsBusy = true;
            if (loc != null)
            {
                StatusText = $"Searching for {searchText}...";
                Items = await client.Search(searchText, loc.LangCode, loc.RegionCode).ConfigureAwait(false);
            }
            IsBusy = false;
            StatusText = "Ready";
        }
        private void NavigateToUriProcess(string resourceUri)
        {
            this.IsBusy = true;
            this.IsNavigated = true;
            this.DocumentUri = resourceUri;
        }
    }
}