using NewsAPI.Constants;
using System.Text.RegularExpressions;

namespace MobileParkTT.News;

/// <summary>
/// Базовый класс для работы с сервисом новостей
/// </summary>
internal abstract class BaseNewsService<TNew> : INewsService<TNew>
    where TNew : class
{

    #region Fileds

    private static Regex _ruRegex = new Regex(@"\b[А-яЁё]{2,}\b", RegexOptions.IgnoreCase);

    private static Regex _enRegex = new Regex(@"\b[A-z]{2,}\b", RegexOptions.IgnoreCase);

    #endregion

    #region Constructor

    protected BaseNewsService()
    {
    }

    #endregion

    #region Methods

    #region Abstract

    public abstract IEnumerable<TNew> GetArticles(string keyWord, DateTime fromDate, Languages languages);

    public abstract void PrintNewsAndMagicWord(string keyWord = "космос", DateTime? fromDate = null, 
        Languages? languages = null);

    #endregion

    #region Protected

    /// <summary>
    /// Возвращает русский язык, если входное слово написана на руском, иначе - английский
    /// </summary>
    /// <param name="word">Входное слово</param>
    /// <returns>Язык слова</returns>
    protected Languages GetLanguagesByWord(string word)
        => word.Any(wordByte => wordByte > 127) ? Languages.RU : Languages.EN;

    /// <summary>
    /// Поиск слова с наибольшим кол-вом глассных
    /// </summary>
    /// <param name="content">Входной текст</param>
    /// <param name="languages">Язык текста</param>
    /// <returns></returns>
    protected string GetMagicWord(string content, Languages languages)
    {
        var vowels = languages == Languages.RU ?
            new char[] { 'а', 'я', 'у', 'ю', 'о', 'е', 'ё', 'э', 'и', 'ы' } :
            new char[] { 'a', 'e', 'i', 'o', 'u', 'y' };

        string word = "";

        var parts = languages == Languages.RU ? _ruRegex.Matches(content) : _enRegex.Matches(content);
        var mostVowels = 0;

        foreach (var part in parts.Select(p => p.Value))
        {
            var challengerWord = part;
            var numberOfVowels = 0;

            foreach (var symbol in challengerWord)
            {
                if (vowels.Contains(symbol))
                {
                    numberOfVowels++;
                }
                
            }

            if (mostVowels < numberOfVowels)
            {
                mostVowels = numberOfVowels;
                word = challengerWord;
            }
        }
        return (word);
    }

    #endregion

    #endregion
}
