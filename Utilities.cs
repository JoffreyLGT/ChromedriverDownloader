using System.Diagnostics;

namespace Chromedriver_Downloader
{
    internal static class Utilities
    {
        private const string CHROMEDRIVER_LATEST_URL = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_";
        private const string CHROMEDRIVER_DOWNLOAD_URL = "https://chromedriver.storage.googleapis.com/{version}/chromedriver_win32.zip";

        /// <summary>
        /// Return the url to get the latest chromedriver url for Chrome version provided.
        /// </summary>
        /// <param name="version">Chrome version to look for a compatible driver</param>
        /// <returns></returns>
        internal static string GetChromedriverLatestUrl(string version)
        {
            return CHROMEDRIVER_LATEST_URL + version;
        }

        internal static string GetChromedriverDownloadUrl(string version)
        {
            return CHROMEDRIVER_DOWNLOAD_URL.Replace("{version}", version.ToString());
        }

        /// <summary>
        /// Get the path to the Google Chrome installation.
        /// </summary>
        /// <returns>a full path to chrome.exe</returns>
        /// <exception cref="FileNotFoundException">when chrome.exe is not found in C:\Program Files\Google\Chrome\Application\ or C:\Program Files (x86)\Google\Chrome\Application\</exception>
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
            throw new FileNotFoundException("Chrome installation not found");
        }

        /// <summary>
        /// Return the major part of Google Chrome version.
        /// </summary>
        /// <param name="chromePath">to chrome.exe</param>
        /// <returns>chrome major version number</returns>
        /// <example>for example, 107 for 107.0.3.432</example>
        internal static int GetChromeVersion(string chromePath)
        {
            return FileVersionInfo.GetVersionInfo(chromePath).FileMajorPart;
        }
    }
}
