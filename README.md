# Webdriver Downloader

Sick of having to update your webdriver each time there is a new version of your favorite web browser?
This lib is for you.

## 🎯 Goal
- Download a supported driver for your web browser version.

## ⚙ Features
- Detect your installed browser version and download the latest driver supporting it.
- Allow you to force the download of a new driver.
- Support driver options.

## ✔️ Supported web browsers
| Browser        | Supported |
| -------------- | --------- |
| Google Chrome  | ✔️        |
| Microsoft Edge | ✔️        |

## 📗 How to use it?
The lib has 3 primary functions:
- GetChromeDriver: download (if necessary) and return the chromedriver for your version of Google Chrome.
- GetEdgeDriver: download (if necessary) and return the msedgedriver for your version of Microsoft Edge.
- GetAvailableBrowserWebDriver: call GetChromeDriver first, then GetEdgeDriver is an issue occured with GetChromeDriver.

Additionally, there are two functions to download the drivers:
- DownloadEdgeDriver
- DownloadChromeDriver
They are not needed if you use the primary functions above.


Bellow is a sample code getting using GetAvailableBrowserWebDriver:
```C#
	Console.WriteLine("WebDriver downloader started");
	string[] arguments = new[] { "--log-level=3" };

	WebDriverDownloader wdd = new WebDriverDownloader();
	IWebDriver driver = await wdd.GetAvailableBrowserWebDriver(arguments);

	Console.WriteLine("Google Chrome or Microsoft Edge should be opened. Press a key to quit.");
	Console.ReadKey();
	driver.Quit();
```