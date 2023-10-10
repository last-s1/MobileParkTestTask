using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using System.Configuration;

namespace MobileParkTT.News;

internal class NewsApiService : BaseNewsService<Article>
{
    private string _apiKey {  get; }

    public NewsApiService(string? apiKey = null) : base()
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            var configuredApiKey = ConfigurationManager.AppSettings["NewsApi"];

            if (configuredApiKey == null) 
            {
                throw new ArgumentException("Configuration with key 'NewsApi' not found");
            }
        
            _apiKey = configuredApiKey;
        }
        else
        {
            _apiKey = apiKey;
        }
    }

    public override IEnumerable<Article> GetArticles(string keyWord, DateTime fromDate, Languages languages)
    {
        var newsApiClient = new NewsApiClient(_apiKey);

        var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
        {
            Q = keyWord,
            Language = languages,
            From = fromDate,
        });

        if (articlesResponse.Status == Statuses.Ok) 
        {
            return articlesResponse.Articles;
        }
        else
        {
            Console.WriteLine($"Error during gettings articles:\n{articlesResponse.Error.Message}");
        }

        return new List<Article>();
    }

    public override void PrintNewsAndMagicWord(string keyWord = "космос", DateTime? fromDate = null, Languages? languages = null)
    {
        var fromDateSearch = fromDate ?? DateTime.Now.AddDays(-1).Date;
        var languagesSearch = languages ?? GetLanguagesByWord(keyWord);

        var articles = GetArticles(keyWord, fromDateSearch, languagesSearch);

        if(articles?.Count() == 0)
        {
            Console.WriteLine("Articles not found");
        }

        foreach (var article in articles) 
        {
            var magicWord = GetMagicWord(article.Content, languagesSearch);
            Console.WriteLine($"News: {article.Content}\nMagicWord: {magicWord}");
            Console.WriteLine("=============");
        }
    }
}
