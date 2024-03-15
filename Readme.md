```cshap
static readonly HttpClient client = new HttpClient();

public static async Task GetCity(TextBox txtHome)
{
    var response = await GetResponseAsync("http://api.openweathermap.org/geo/1.0/direct?q=London&limit=1&appid=376a7ca78ae6108dfac01a9935a41af7");

    if (response.IsSuccessStatusCode)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonData = JObject.Parse(responseContent);
        txtHome.Text = responseContent;
    }else
    {
        Popup.WePopup pop = new Popup.WePopup();
        pop.Show();
    }
}

public static async Task<HttpResponseMessage> GetResponseAsync(string url)
{
    HttpResponseMessage response = await client.GetAsync(url);
    return response;
}
```
dsda
