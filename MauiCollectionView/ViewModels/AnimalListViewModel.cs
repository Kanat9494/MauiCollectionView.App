
namespace MauiCollectionView.ViewModels;

public partial class AnimalListViewModel : ObservableObject
{
    private readonly AnimalService _animalService;
    public ObservableCollection<EntryDetails> AnimalList { get; set; } = new ObservableCollection<EntryDetails>();
    private List<EntryDetails> _allAnimals;
    private int _pageSize = 20;
    [ObservableProperty]
    private bool _isBusy;
    [ObservableProperty]
    private bool _isLoading;

    public AnimalListViewModel(AnimalService animalService)
    {
        _animalService = animalService;

        GetAnimalList();
    }

    private void GetAnimalList()
    {
        AnimalList.Clear();
        IsBusy = true;
        Task.Run(async () =>
        {
            _allAnimals = await _animalService.GetAnimalList();

            App.Current.Dispatcher.Dispatch(() =>
            {
                var recordsToBeAdded = _allAnimals.Take(_pageSize).ToList();

                foreach (var record in recordsToBeAdded) 
                    AnimalList.Add(record);

                IsBusy = false;
            });
        });
    }

    [ICommand]
    public async Task LoadMoreData()
    {
        if (IsLoading) return;

        if (_allAnimals?.Count > 0)
        {
            IsLoading = true;
            await Task.Delay(2000);
            var recordsToBeAdded = _allAnimals.Skip(AnimalList.Count).Take(_pageSize).ToList();
            foreach (var record in recordsToBeAdded)
                AnimalList.Add(record);

            IsLoading = false;
        }
    }
}
