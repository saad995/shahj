using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Text;

namespace shahj
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("YouTube Data API: Fetch Channel Info By Username");
            Console.WriteLine("============================");

            Console.WriteLine("Please enter api key: ");
            var apiKey = Console.ReadLine();

            Console.WriteLine("Please specify the username for which you need the details: ");
            var forUsername = Console.ReadLine();

            try
            {
                new Program().Run(apiKey, forUsername).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task Run(string apiKey, string forUsername)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
              ApiKey = apiKey,
              ApplicationName = this.GetType().ToString()
            });
        
            var channelsListRequest = youtubeService.Channels.List("snippet,statistics");
            channelsListRequest.ForUsername = forUsername;
        
            // Call the channels.list method to retrieve results matching the specified query term.
            var channelsListResponse = await channelsListRequest.ExecuteAsync();
            List<string> channels = new List<string>();
            StringBuilder strBuilder = new StringBuilder();
        
            if(channelsListResponse != null && channelsListResponse.Items != null)
            {
                // Add each result to the appropriate list, and then display the lists of channels.
                foreach (var searchResult in channelsListResponse.Items)
                {
                  switch (searchResult.Kind)
                  {        
                    case "youtube#channel":
                        var channelIdentifier = searchResult.Id;
                        var channelTitle = searchResult.Snippet.Title;
                        var channelSubscribers = searchResult.Statistics.SubscriberCount;
                        var channelViewCount = searchResult.Statistics.ViewCount;
                        strBuilder.Append(string.Format("INSERT INTO [dbo].[ChannelStats] (ChannelIdentifier, ChannelTitle, ChannelSubscribers, ChannelViewCount) VALUES ('{0}','{1}',{2},{3});",
                            channelIdentifier,
                            channelTitle,
                            channelSubscribers,
                            channelViewCount));
                        channels.Add(String.Format("{0} | {1} | {2}", channelTitle, channelSubscribers, channelViewCount));
                        break;
                  }
                }

                var sqlQuery = strBuilder.ToString();
                var connStr = "Server=127.0.0.1;Initial Catalog=ShahjTask;User ID=sa;Password=@Password1;";
                SqlConnection sql = new SqlConnection(connStr);
                sql.Open();

                using(SqlCommand cmd = new SqlCommand(sqlQuery, sql))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) updated");
                }

                Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            }
            else
            {
                Console.WriteLine("No record found!");
            }
        }
    }
}
