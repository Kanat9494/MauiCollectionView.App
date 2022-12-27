using System.Net;

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
                item.ImageUrl = $"http://127.0.0.1/1.jpg";

                items.Add(item);
            }
        });

        return items;
    }

    private async Task DownloadImage(string imageUrl)
    {
        await Task.Run(() =>
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(imageUrl, "1.png");
            }
        });
    }

    private async Image TestImage(string imageUrl)
    {
        using (WebClient client = new WebClient())
        {
            using (Stream stream = client.OpenRead(imageUrl))
            {
                return Image.FromStream(stream);
            };
        }
    }
}
