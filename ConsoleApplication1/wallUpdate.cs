using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using System.ComponentModel;

namespace wallUpdate
{
    class wallUpdate
    {
        static String extension = "";
        static String [] SubReddit;
        static int timeWait;
        static bool NSFW;
        static void Main(string[] args)
        {
            Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly().Location);
            
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.Idle;
            StreamReader sr = new StreamReader(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("WallUpdate.exe","config"));
            SubReddit = new Scanner(sr.ReadLine(), ";").LineArray();
            foreach (String value in SubReddit)
                Console.WriteLine(value + ",");
            timeWait = (int)(Convert.ToDouble(sr.ReadLine()) * 60 * 1000);
            if (!Convert.ToBoolean( sr.ReadLine())) 
                return;
            Console.WriteLine(timeWait);
            NSFW = Convert.ToBoolean(sr.ReadLine());
            sr.Close();
            Console.WriteLine(NSFW.ToString());
            string img = "";
            do
            {
                img = findImgSrc();
                if ((!(img.EndsWith(".jpg") || img.EndsWith(".png") || img.EndsWith(".jpeg"))) && img.Contains("imgur"))
                    img += ".jpeg";
            } while (!(img.EndsWith(".jpg") || img.EndsWith(".png") || img.EndsWith(".jpeg")));
            downloadFiles(img);
            while (true)
            {
                System.Threading.Thread.Sleep(timeWait);
                try
                {
                    do
                    {
                        img = findImgSrc();
                        if ((!(img.EndsWith(".jpg") || img.EndsWith(".png") || img.EndsWith(".jpeg"))) && img.Contains("imgur"))
                            img += ".jpeg";
                    } while (!(img.EndsWith(".jpg") || img.EndsWith(".png") || img.EndsWith(".jpeg")));
                    downloadFiles(img);
                }
                catch (Exception e) { Console.WriteLine("Exception Caught: Most likely web based");  }
            }
        }

        private static void downloadFiles(string imageSrc)
        {
            try
            {
                if (imageSrc.EndsWith(".jpg"))
                    extension = ".jpeg";
                if (imageSrc.EndsWith(".png"))
                    extension = ".png";
                if (imageSrc.EndsWith(".jpeg"))
                    extension = ".jpeg";

                String writepath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("WallUpdate.exe","Walls/wall") + extension;
                Console.WriteLine(writepath);
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                // Specify a progress notification handler.
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                Uri src = new Uri(imageSrc);
                webClient.DownloadFileAsync(src, writepath);
            }
            catch (Exception e) {  }
        }
        private static String findImgSrc()
        {
            String[] subdom = { "www", "api" };
            Console.WriteLine("Finding Image src....");
            try
            {       
                    Random r = new Random();
                    string subdomain = subdom[r.Next(0, 2)];
                    string subreddit = SubReddit[r.Next(0, SubReddit.Length)];
                    Console.WriteLine(subdomain + "..." + subreddit);
                    String imageSrc = "";
                    String jsonString = "";
                    WebClient wc = new WebClient();
                    string link = "http://" + subdomain + ".reddit.com/r/" + subreddit + "/random.json";
                    Console.WriteLine(link);
                    jsonString = wc.DownloadString(link);
                    wc = null;
                    JArray arr = JArray.Parse(jsonString);
                    JObject obj = (JObject)arr[0];
                    obj = (JObject)obj.GetValue("data");
                    arr = (JArray)obj.GetValue("children");
                    obj = (JObject)arr[0];
                    obj = (JObject)obj.GetValue("data");
                    string notSafe = ((String)obj.GetValue("over_18")).Trim();
                    if (notSafe==("True"))
                    {
                        Console.WriteLine("This link is NSFW");
                        if (NSFW)
                            imageSrc = (String)obj.GetValue("url");
                        else
                            imageSrc = "NSFW NO!";
                    }
                    else
                    {
                        imageSrc = (String)obj.GetValue("url");
                    }

                    Console.WriteLine(imageSrc);
                    return imageSrc;
                

            }
            catch (Exception Fe) { Console.WriteLine(Fe.Message);  return "Page Doesnt Exist"; }
        }
        private static void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {

                String writepath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("WallUpdate.exe", "Walls/wall") + extension;
                WallSetter ws = new WallSetter();
                ws.SetWallpaper(writepath);
                Console.WriteLine();
                Console.WriteLine("Saved!");
            }
            catch (Exception Ce) { Console.WriteLine(Ce.Message);  }
        }
        private static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                Console.Write("|");
            }
            catch (Exception Pe)
            {
                Console.WriteLine(Pe.Message);
                
            }
        }
    }
}


