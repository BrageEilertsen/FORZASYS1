using System.Collections.Generic;
using System.Threading.Tasks;
using FORZASYS1.Models;

namespace FORZASYS1.Interfaces
{
    public interface IElasticsearchService
    {
        Task<List<Video>> SearchHighlights(string searchTerm);
        string ConstructBlobUrl(string filename);
    }
}