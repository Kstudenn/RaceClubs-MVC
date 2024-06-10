using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Data.Repository.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Services.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IClubRepository _clubRepository;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _photoService = photoService;
            _clubRepository = clubRepository;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        // Just create page so can be no async
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);
                // This or automapper
                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = _clubRepository.GetByIdAsync(id);
            if (club == null)
                return View("Error");
            //Automapper lub to
            var clubVM = new EditClubViewModel
            {
                Title = club.Result.Title,
                Description = club.Result.Description,
                Address = club.Result.Address,
                AddressId = club.Result.AddressId,
                URL = club.Result.Image,
                ClubCategory = club.Result.ClubCategory
            };
            return View(clubVM);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }
            //Notracking beacuse there will be another to track
            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Address = clubVM.Address,
                    AddressId = clubVM.AddressId,
                    Image = photoResult.Url.ToString(),
                };
                _clubRepository.Update(club);
                return RedirectToAction("Index");

            }
            else
            {
                return View(clubVM);
            }

        }
    }
}
