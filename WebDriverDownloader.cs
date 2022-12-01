using JLagut.WebdriverDownloader;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Remote;

namespace JLagut.WebDriverDownloader
{
    public class WebDriverDownloader
    {
        private static readonly string DEFAULT_STORAGE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WebDriverDownloader");

        public string StorageFolderPath { get; set; }

        public WebDriverDownloader()
        {
            this.StorageFolderPath = DEFAULT_STORAGE;
        }
        public WebDriverDownloader(string storageFolderPath)
        {
            if (string.IsNullOrWhiteSpace(storageFolderPath))
            {
                throw new ArgumentNullException(nameof(storageFolderPath));
            }
            this.StorageFolderPath = storageFolderPath;
        }

        /// <summary>
        /// Check if Google Chrome is installed on the computer and gets its driver.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="forceDownload">true to always download a driver from Google's website</param>
        /// <returns></returns>
        public async Task<ChromeDriver> GetChromeDriver(string[] arguments, bool forceDownload = false)
        {
            string chromePath = Utilities.GetChromePath();
            if (string.IsNullOrWhiteSpace(chromePath))
            {
                throw new Exception("Path to chrome.exe not found.");
            }

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(chromePath);
            string version = versionInfo.FileMajorPart + "." + versionInfo.FileMinorPart + "." + versionInfo.FileBuildPart;

            string localVersionDir = Path.Combine(StorageFolderPath, "ChromeDriver", version.ToString());
            string localVersionPath = Path.Combine(localVersionDir, "chromedriver.exe");

            if (forceDownload && Directory.Exists(localVersionDir))
            {
                Directory.Delete(localVersionDir, true);
            }

            if (!File.Exists(localVersionPath))
            {
                await DownloadChromeDriver(version, localVersionDir);
            }
            ChromeOptions options = new();
            foreach (string arg in arguments)
            {
                options.AddArgument(arg);
            }
            return new ChromeDriver(localVersionDir, options);
        }

        /// <summary>
        /// Check if Google Chrome is installed on the computer and gets its driver.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="forceDownload">true to always download a driver from Google's website</param>
        /// <returns></returns>
        public async Task<EdgeDriver> GetEdgeDriver(string[] arguments, bool forceDownload = false)
        {
            string edgePath = Utilities.GetEdgePath();
            if (string.IsNullOrWhiteSpace(edgePath))
            {
                throw new Exception("Path to msedge.exe not found.");
            }
            string version = FileVersionInfo.GetVersionInfo(edgePath).FileVersion?.ToString() ?? "";

            string localVersionDir = Path.Combine(StorageFolderPath, "EdgeDriver", version.ToString());
            string localVersionPath = Path.Combine(localVersionDir, "msedgedriver.exe");

            if (forceDownload && Directory.Exists(localVersionDir))
            {
                Directory.Delete(localVersionDir, true);
            }

            if (!File.Exists(localVersionPath))
            {
                await DownloadEdgeDriver(version, localVersionDir);
            }

            EdgeOptions options = new();
            foreach (string arg in arguments)
            {
                options.AddArgument(arg);
            }
            return new EdgeDriver(localVersionDir, options);
        }


        /// <summary>
        /// Check if Google Chrome is installed on the computer and gets its driver.
        /// If not, check if Edge is installed and gets its driver.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="forceDownload">true to always download a driver from Google's website</param>
        /// <returns></returns>
        public async Task<IWebDriver> GetAvailableBrowserWebDriver(string[] arguments, bool forceDownload = false)
        {
            try
            {
                return await GetChromeDriver(arguments, forceDownload);
            }
            catch (Exception)
            {
                // Do nothing, it means Chrome is not on the machine
            }

            try
            {
                return await GetEdgeDriver(arguments, forceDownload);
            }
            catch (Exception)
            {
                // Do nothing, it means Edge is not on the machine
            }

            throw new NotFoundException("Google Chrome and Microsoft Edge are not found on the machine");
        }

        /// <summary>
        /// Download chromedriver.exe for the provided chromeVersion and store it in the destinationDir.
        /// </summary>
        /// <param name="version">version of Google Chrome to automate</param>
        /// <param name="destinationDir">to store chromedriver.exe</param>
        public async Task DownloadChromeDriver(string version, string destinationDir)
        {
            using HttpClient client = new HttpClient();

            // Get the chromedriver compatible version for our Google chrome version
            using HttpResponseMessage versionResponse = await client.GetAsync(Utilities.GetChromeDriverLatestUrl(version));
            versionResponse.EnsureSuccessStatusCode();

            string driverVersion = await versionResponse.Content.ReadAsStringAsync();

            // Download chromedriver
            using HttpResponseMessage driverZipResponse = await client.GetAsync(Utilities.GetChromeDriverDownloadUrl(driverVersion));
            driverZipResponse.EnsureSuccessStatusCode();

            string zipFilePath = Path.Combine(StorageFolderPath, "chromedriver.zip");
            Directory.CreateDirectory(destinationDir);
            using FileStream zipFileCreate = File.Create(zipFilePath);

            using Stream driverZipSteam = await driverZipResponse.Content.ReadAsStreamAsync();
            driverZipSteam.Seek(0, SeekOrigin.Begin);
            driverZipSteam.CopyTo(zipFileCreate);
            driverZipSteam.Close();

            zipFileCreate.Close();

            ZipFile.ExtractToDirectory(zipFilePath, destinationDir);
            File.Delete(zipFilePath);
        }


        /// <summary>
        /// Download edgedriver.exe for the provided chromeVersion and store it in the destinationDir.
        /// </summary>
        /// <param name="version">version of Google Chrome to automate</param>
        /// <param name="destinationDir">to store chromedriver.exe</param>
        public async Task DownloadEdgeDriver(string version, string destinationDir)
        {
            using HttpClient client = new HttpClient();

            using HttpResponseMessage driverZipResponse = await client.GetAsync(Utilities.GetEdgeDriverDownloadUrl(version));
            driverZipResponse.EnsureSuccessStatusCode();

            string zipFilePath = Path.Combine(StorageFolderPath, "edgedriver.zip");
            Directory.CreateDirectory(destinationDir);
            using FileStream zipFileCreate = File.Create(zipFilePath);

            using Stream driverZipSteam = await driverZipResponse.Content.ReadAsStreamAsync();
            driverZipSteam.Seek(0, SeekOrigin.Begin);
            driverZipSteam.CopyTo(zipFileCreate);
            driverZipSteam.Close();

            zipFileCreate.Close();

            ZipFile.ExtractToDirectory(zipFilePath, destinationDir);
            File.Delete(zipFilePath);
        }

    }
}
