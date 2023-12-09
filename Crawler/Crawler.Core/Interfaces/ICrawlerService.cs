using Crawler.Core.Models;

namespace Crawler.Core.Interfaces
{
    public interface ICrawlerService
    {
        Task<SearchResult> Search();
    }
}

