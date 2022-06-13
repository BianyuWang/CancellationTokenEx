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

            // way 1, 

            //await Download1Async(url, 100, cts.Token);

            ////way 2
            //try
            //{
            //    await Download2Async(url, 100, cts.Token);
            //    //await DownloadAsync(url, 100);
            //}
            //catch (Exception e)
            //{
            //    if (cts.IsCancellationRequested)
            //    { Console.WriteLine("request is cancelled"); }

            //}

            ////way 3 pass cancellationToken to GetAsync

            //try
            //{
            //    await Download3Async(url, 100, cts.Token);
            //    //await DownloadAsync(url, 100);
            //}
            //catch (Exception e)
            //{
            //    if (cts.IsCancellationRequested)
            //    { Console.WriteLine("request is cancelled"); }

            //}

            //way 4 manually stop progran

            CancellationTokenSource cts4 = new CancellationTokenSource();
            CancellationToken cToken = cts4.Token;
            Download3Async(url, 200, cToken);
            while (Console.ReadLine() != "q")
            { //if user not enter 'q' to quite program, do something
            }
            cts4.Cancel();
            Console.ReadLine();
        }
        static async Task Download1Async(string URL, int repeat, CancellationToken cancellationToken=default)
        {
            // to clean unmanaged resources, HttpClient has dispose method.
            using (HttpClient client = new HttpClient())
            {
                for (var i = 0; i < repeat; i++)
                {
                    string htmlText = await client.GetStringAsync(URL);
                    Console.WriteLine($"{DateTime.Now}:{htmlText}");

                
                    // #way 1: check cancellationToken manually 

                    if (cancellationToken.IsCancellationRequested)
                    {
                        // if cancelled, do something
                        Console.WriteLine("Resquest is cancelled");
                        break;
                    }
                  

                   
                }
            
            
            }
        }

        static async Task Download2Async(string URL, int repeat, CancellationToken cancellationToken = default)
        {
            // to clean unmanaged resources, HttpClient has dispose method.
            using (HttpClient client = new HttpClient())
            {
                for (var i = 0; i < repeat; i++)
                {
                    string htmlText = await client.GetStringAsync(URL);
                    Console.WriteLine($"{DateTime.Now}:{htmlText}");

                 
                    //#way 2: throw an exception, and manage exception outside of function.
                    cancellationToken.ThrowIfCancellationRequested();
                }


            }
        }

        static async Task Download3Async(string URL, int repeat, CancellationToken cancellationToken = default)
        {
            // to clean unmanaged resources, HttpClient has dispose method.
            using (HttpClient client = new HttpClient())
            {
                for (var i = 0; i < repeat; i++)
                {
                    //pass cancellationtoken to getAsync
                    var resp = await client.GetAsync(URL, cancellationToken);
                    string htmlText = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine($"{DateTime.Now}:{htmlText}");


                }


            }
        }
    }
}
