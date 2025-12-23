
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
    driver.Navigate().GoToUrl("https://www.tcgplayer.com/");

    Thread.Sleep(2000);

    foreach (var card in deckList)
    {
        Card currentCard = card.card;
        int quantityToAddToCart = card.quantity;

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

        //find the node that matches the current cards card num
        string leftPadded = currentCard.CardNum.ToString().PadLeft(3, '0');
        string cardNumberToMatch = "#" + leftPadded;
        ClickSpanContaining(cardNumberToMatch);
        Thread.Sleep(2000);

        //TODO: write logic to add card to cart based on quantity

        Thread.Sleep(1000);
    }


    Console.WriteLine("Program has finished, press any key to exit...");
    Console.ReadKey();
    //Console.WriteLine("Page Title: " + driver.Title);
    //Console.WriteLine("Current URL: " + driver.Url);
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred: " + ex.Message);
}
finally
{
    // Close the browser session
    driver.Quit();
}

void ClickSpanContaining(string cardNumToMatch)
{
    var xpath = $"//span[contains(text(), '{cardNumToMatch}')]";
    var element = driver.FindElement(By.XPath(xpath));
    element.Click();
}



