# Chromedriver Downloader

Sick of having to update your chromedriver each time there is a new version of Google Chrome?
This lib is for you.

## 🎯 Goal
- Download a supported chromedriver for your version of Google Chrome

## ⚙ Features
- Detect your installed Google Chrome version and download the latest chromedriver supporting it.
- Allow you to force the download of a new driver.
- Support ChromeOptions.

## 📗 How to use it?
Simply call the function **GetDriver()** to get your ChromeDriver.
Bellow is a sample code using it:
```C#
Console.WriteLine("Chromedriver tester started");
ChromeOptions options = new ChromeOptions();
options.AddArgument("--log-level=3"); // Fatal errors only
ChromeDriver driver = await ChromedriverDownloader.GetDriver(options);
Console.WriteLine("Google Chrome should be opened. Press a key to quit.");
Console.ReadLine();
driver.Quit();
```