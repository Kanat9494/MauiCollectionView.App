namespace MauiCollectionView.Services;

public class AnimalService
{
    public async Task<List<EntryDetails>> GetAnimalList()
    {
        var returnResponse = new List<EntryDetails>();
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("https://api.publicapis.org/entries");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var deserializeContent = JsonConvert.DeserializeObject<MainResponse>(content);

                if (deserializeContent?.Entries?.Count > 0)
                    returnResponse = deserializeContent.Entries;
            }
        }
        return returnResponse;
    }

    public async Task<List<Item>> GenerateNewItems(int itemId)
    {
        List<Item> items = new List<Item>();
        int endOfLoop = itemId + 50;

        await Task.Run(() =>
        {
            for (; itemId < endOfLoop; itemId++)
            {
                var item = new Item();
                item.ItemId = itemId;
                item.Title = $"Title of {itemId}-th item";
                item.Description = $"This is the Description of {itemId}-th item";
                item.ImageUrl = $"https://picsum.photos/id/{itemId}/200/300";

                items.Add(item);
            }
        });

        return items;
    }
}
