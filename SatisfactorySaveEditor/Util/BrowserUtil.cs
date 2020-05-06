using System;
using System.Diagnostics;

namespace SatisfactorySaveEditor.Util
{
    public static class BrowserUtil
    {
        public static void OpenBrowser(string url)
        {
            bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            Trace.Assert(result, "Can we not launch programs with this method");

            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
            {
                CreateNoWindow = true
            });
        }
    }
}
