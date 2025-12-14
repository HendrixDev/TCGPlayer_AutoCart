
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TCGPlayer_AutoCart;


var deckList = DeckParser.ParseDecklist("C:\\Users\\m374l\\source\\repos\\TCGPlayer_AutoCart\\TCGPlayer_AutoCart\\decklist.txt");

//using IWebDriver driver = new ChromeDriver();

try
{
    // Navigate to the desired URL
    //driver.Navigate().GoToUrl("https://www.tcgplayer.com/");

    Thread.Sleep(5000); // Wait for 2 seconds to ensure the page loads

    //search bar
    //driver.FindElement(By.Id("autocomplete-input")).SendKeys("Cynthia's Gible DRI");
    //Thread.Sleep(1000); 
    //driver.FindElement(By.Id("autocomplete-input")).SendKeys(Keys.Enter);

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
    //Console.WriteLine("An error occurred: " + ex.Message);
}
finally
{
    // Close the browser session
    //driver.Quit();
}




