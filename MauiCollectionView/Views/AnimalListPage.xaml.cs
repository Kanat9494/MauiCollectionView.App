namespace MauiCollectionView.Views;

public partial class AnimalListPage : ContentPage
{
	public AnimalListPage(AnimalListViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}
}