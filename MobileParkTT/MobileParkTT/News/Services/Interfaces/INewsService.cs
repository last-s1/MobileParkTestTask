using NewsAPI.Constants;

namespace MobileParkTT.News;

/// <summary>
/// Описывает интерфейс взаимодействия с сервисом новостей
/// </summary>
internal interface INewsService<TNew> where TNew : class
{
    /// <summary>
    /// Получение статей по заданным параметрам
    /// </summary>
    /// <param name="keyWord">Ключевое слово</param>
    /// <param name="languages">Язык поиска</param>
    /// <param name="fromDate">Дата с которой начинается поиск</param>
    /// <returns>Перечисление статей</returns>
    public IEnumerable<TNew> GetArticles(string keyWord, DateTime fromDate, Languages languages);

    /// <summary>
    /// Выводит новости и необходимое по условию задачи слово
    /// </summary>
    /// <param name="keyWord">Ключевое слово</param>
    /// <param name="fromDate">Дата с которой начинается поиск</param>
    /// /// <param name="languages">Язык поиска</param>
    public void PrintNewsAndMagicWord(string keyWord = "космос", DateTime? fromDate = null, Languages? languages = null);
}
