﻿using ConferenceDTO;
using FrontEnd.Infrastructure;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Pages.Admin
{
    public class CreateSpeakerModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public CreateSpeakerModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public SpeakerRequest Speaker { get; set; }

        public void OnGet()
        {
            var newSession = TempData.Get<Session>(TempDataKey.NewSession);
            var newImage = TempData.Get<ImageRequest>(TempDataKey.NewImage);

            if (newSession != null)
            {
                Speaker.Sessions.Add(newSession);
            }

            if (newImage != null)
            {
                Speaker.Image = newImage;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _apiClient.CreateSpeakerAsync(Speaker);

            return RedirectToPage("/Speakers");
        }

        public void OnPostRemoveAsync(int index)
        {
            var session = Speaker.Sessions.ElementAt(index);
            Speaker.Sessions.Remove(session);
        }
    }
}