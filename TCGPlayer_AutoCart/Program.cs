
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TCGPlayer_AutoCart;

var deckPath = Path.Combine("Files", "decklist.txt");
var setPath = Path.Combine("Files", "setlist.txt");

var deckList = DeckParser.ParseDecklist(deckPath);
var setList = SetParser.ParseSetList(setPath);


using IWebDriver driver = new ChromeDriver();

try
{
    // Navigate to the desired URL
    driver.Navigate().GoToUrl("https://www.tcgplayer.com/");

    Thread.Sleep(2000); // Wait for 5 seconds to ensure the page loads


    foreach (var card in deckList)
    {
        Card currentCard = card.card;

        if (string.IsNullOrEmpty(currentCard.SetCode))
        {
            Console.WriteLine($"Skipping card '{currentCard.Name}', could not find.");
            continue;
        }

        if (setList.TryGetValue(currentCard.SetCode, out var setName))
        {
            currentCard.SetName = setName;
        }

        if (string.IsNullOrEmpty(currentCard.SetName))
        {
            Console.WriteLine($"Skipping card '{currentCard.Name}', could not find set name for code '{currentCard.SetCode}'.");
            continue;
        }

        string cardToSearch = currentCard.Name + " " + currentCard.SetName;

        var searchBox = driver.FindElement(By.Id("autocomplete-input"));

        searchBox.SendKeys(cardToSearch);
        Thread.Sleep(1000);
        searchBox.SendKeys(Keys.Enter);
        Thread.Sleep(1000);
        searchBox.SendKeys(Keys.Control + "a");
        Thread.Sleep(500);
        searchBox.SendKeys(Keys.Delete);
        Thread.Sleep(1000);
    }

    

    //click search button
    //driver.FindElement(By.Id("search-button")).Click();

    //hit enter key to search


    Console.ReadKey();
    //Console.WriteLine("Page Title: " + driver.Title);
    //Console.WriteLine("Current URL: " + driver.Url);

    // Optional: Keep the browser open for a few seconds to see the result
    //Thread.Sleep(5000);
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred: " + ex.Message);
}
finally
{
    // Close the browser session
    //driver.Quit();
}




