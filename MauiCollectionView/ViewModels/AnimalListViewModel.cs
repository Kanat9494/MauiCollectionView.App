using ObservableObject = Microsoft.Toolkit.Mvvm.ComponentModel.ObservableObject;

namespace MauiCollectionView.ViewModels;

public partial class AnimalListViewModel : ObservableObject
{
    private readonly AnimalService _animalService;
    public ObservableRangeCollection<EntryDetails> AnimalList { get; set; } = new ObservableRangeCollection<EntryDetails>();
    private List<EntryDetails> _allAnimals;
    private int _pageSize = 20;
    [ObservableProperty]
    private bool _isBusy;
    [ObservableProperty]
    private bool _isLoading;
    public ObservableCollection<Item> AllItems { get; set; } = new ObservableCollection<Item>();

    public AnimalListViewModel(AnimalService animalService)
    {
        _animalService = animalService;

        GetItems();
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
                AnimalList.ReplaceRange(_allAnimals.Take(_pageSize).ToList());

                IsBusy = false;
            });
        });
    }

    public void GetItems()
    {
        AllItems.Clear();
        List<Item> items;

        Task.Run(async () =>
        {
            items = await _animalService.GenerateNewItems(1);

            foreach(var item in items) 
                AllItems.Add(item);
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
            AnimalList.AddRange(_allAnimals.Skip(AnimalList.Count).Take(_pageSize).ToList());

            IsLoading = false;
        }
    }
}
