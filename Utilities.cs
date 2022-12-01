using System.Diagnostics;

namespace JLagut.WebdriverDownloader
{
    internal static class Utilities
    {
        private const string CHROMEDRIVER_LATEST_URL = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_";
        private const string CHROMEDRIVER_DOWNLOAD_URL = "https://chromedriver.storage.googleapis.com/{version}/chromedriver_win32.zip";

        private const string EDGEDRIVER_DOWNLOAD_URL = "https://msedgedriver.azureedge.net/{version}/edgedriver_win64.zip";

        /// <summary>
        /// Return the url to get the latest chromedriver url for Chrome version provided.
        /// </summary>
        /// <param name="version">Chrome version to look for a compatible driver</param>
        /// <returns></returns>
        internal static string GetChromeDriverLatestUrl(string version)
        {
            return CHROMEDRIVER_LATEST_URL + version;
        }

        /// <summary>
        /// Return ChromeDriver download url.
        /// </summary>
        /// <param name="version">full version number of Chrome</param>
        /// <returns></returns>
        internal static string GetChromeDriverDownloadUrl(string version)
        {
            return CHROMEDRIVER_DOWNLOAD_URL.Replace("{version}", version.ToString());
        }

        /// <summary>
        /// Return EdgeDriver download url.
        /// </summary>
        /// <param name="version">full version number of Edge</param>
        /// <returns></returns>
        internal static string GetEdgeDriverDownloadUrl(string version)
        {
            return EDGEDRIVER_DOWNLOAD_URL.Replace("{version}", version.ToString());
        }

        /// <summary>
        /// Get the path to the Google Chrome installation.
        /// </summary>
        /// <returns>a full path to chrome.exe or an empty string if the installation is not found</returns>
        internal static string GetChromePath()
        {
            string fromProgramFile = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            string fromProgramFile86 = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";

            if (File.Exists(fromProgramFile))
            {
                return fromProgramFile;
            }
            else if(File.Exists(fromProgramFile86))
            {
                return fromProgramFile86;
            }
            return "";
        }

        /// <summary>
        /// Get the path to the Microsoft Edge (Chromium) installation.
        /// </summary>
        /// <returns>a full path to msedge.exe or an empty string if the installation is not found</returns>
        internal static string GetEdgePath()
        {
            string defaultLocation = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
            if (File.Exists(defaultLocation))
            {
                return defaultLocation;
            }
            return "";
        }
    }
}
