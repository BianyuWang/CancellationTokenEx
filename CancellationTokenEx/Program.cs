using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenEx
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string url = "https://www.google.com";
            CancellationTokenSource cts = new CancellationTokenSource();
            //canel request after 5 seconds
            cts.CancelAfter(5000); // pass milliseconds

            // download google 100 times, and cancel download after 5 seconds, if download is not finished.
            try
            {
                await DownloadAsync(url, 100, cts.Token);
                //await DownloadAsync(url, 100);
            }
            catch (Exception e)
            {
                if (cts.IsCancellationRequested)
                { Console.WriteLine("request is cancelled"); }
              
            }


        }
        static async Task DownloadAsync(string URL, int repeat, CancellationToken cancellationToken=default)
        {
            // to clean unmanaged resources, HttpClient has dispose method.
            using (HttpClient client = new HttpClient())
            {
                for (var i = 0; i < repeat; i++)
                {
                    string htmlText = await client.GetStringAsync(URL);
                    Console.WriteLine($"{DateTime.Now}:{htmlText}");

                    #region Using cancellationToken way one
                    // #way 1: check cancellationToken manually 

                    //if (cancellationToken.IsCancellationRequested)
                    //{
                    //    // if cancelled, do something
                    //    Console.WriteLine("Resquest is cancelled");
                    //    break;
                    //}
                    #endregion

                    //#way 2: throw an exception, and manage exception outside of function.
                    cancellationToken.ThrowIfCancellationRequested();
                }
            
            
            }
        }
    }
}
