namespace MauiCollectionView.ViewModels;

public class AnimalListViewModel : ObservableObject
{
    private readonly AnimalService _animalService;
    public ObservableCollection<EntryDetails> AnimalList { get; set; } = new ObservableCollection<EntryDetails>();
    private List<EntryDetails> _allAnimals;
    private int _pageSize = 20;
    public AnimalListViewModel(AnimalService animalService)
    {
        _animalService = animalService;

        GetAnimalList();
    }

    private void GetAnimalList()
    {
        AnimalList.Clear();
        Task.Run(async () =>
        {
            _allAnimals = await _animalService.GetAnimalList();

            App.Current.Dispatcher.Dispatch(() =>
            {
                var recordsToBeAdded = _allAnimals.Take(_pageSize).ToList();

                foreach (var record in recordsToBeAdded) 
                    AnimalList.Add(record); 
            });
        });
    }
}
