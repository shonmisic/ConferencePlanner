using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public SearchModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public string Term { get; set; }
        public List<object> SearchResults { get; set; }

        public async Task OnGetAsync(string term)
        {
            Term = term;
            var results = await _apiClient.SearchAsync(term);
            SearchResults = results.Select(r =>
                            {
                                switch (r.Type)
                                {
                                    case SearchResultType.Session:
                                        return r.Value.ToObject<SessionResponse>();
                                    case SearchResultType.Speaker:
                                        return r.Value.ToObject<SpeakerResponse>();
                                    default:
                                        return (object)r;
                                }
                            })
                            .ToList();
        }
    }
}