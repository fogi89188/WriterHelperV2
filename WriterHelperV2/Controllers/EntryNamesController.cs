using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Reflection;
using WriterHelperV2.Data;
using WriterHelperV2.Models;
using WriterHelperV2.Models.Domain;

namespace WriterHelperV2.Controllers
{
    public class EntryNamesController : Controller
    {
        private readonly WriterHelperDBContext writerHelperDBContext;

        public EntryNamesController(WriterHelperDBContext writerHelperDBContext)
        {
            this.writerHelperDBContext = writerHelperDBContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var entryNames = await writerHelperDBContext.EntryNames.ToListAsync();
            return View(entryNames);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEntryNameViewModel addEntryNameRequest)
        {
            var entryName = new EntryName()
            {
                Id = Guid.NewGuid(),
                Name = addEntryNameRequest.Name,
                Race = addEntryNameRequest.Race,
                Gender = addEntryNameRequest.Gender,
                FirstOrMiddleOrLastName = addEntryNameRequest.FirstOrMiddleOrLastName
            };

            await writerHelperDBContext.EntryNames.AddAsync(entryName);
            await writerHelperDBContext.SaveChangesAsync();

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var name = await writerHelperDBContext.EntryNames.FirstOrDefaultAsync(x => x.Id == id);

            if (name != null)
            {
                var viewModel = new UpdateEntryNameViewModel()
                {
                    Id = name.Id,
                    Name = name.Name,
                    Race = name.Race,
                    Gender = name.Gender,
                    FirstOrMiddleOrLastName = name.FirstOrMiddleOrLastName
                };
                return View(viewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEntryNameViewModel model)
        {
            var name = await writerHelperDBContext.EntryNames.FindAsync(model.Id);
            if (name != null)
            {
                name.Name = model.Name;
                name.Race = model.Race;
                name.FirstOrMiddleOrLastName = model.FirstOrMiddleOrLastName;
                name.Gender = model.Gender;

                await writerHelperDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEntryNameViewModel model)
        {
            var name = await writerHelperDBContext.EntryNames.FindAsync(model.Id);

            if (name != null)
            {
                writerHelperDBContext.EntryNames.Remove(name);
                await writerHelperDBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            //TODO add error page
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GenerateEntryName()
        {
            var generateName = new GenerateNameViewModel()
            {
                Gender = null,
                Race = null,
                Name = null
            };
            return View(generateName);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateEntryName(GenerateNameViewModel model)
        {
            string gender = model.Gender;
            string race = model.Race;

            var firstName = await writerHelperDBContext.EntryNames.OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync(x =>
                x.FirstOrMiddleOrLastName == $"First"
                && x.Race == $"{race}"
                && x.Gender == $"{gender}");
            var middleName = await writerHelperDBContext.EntryNames.OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync(x =>
                x.FirstOrMiddleOrLastName == $"Middle"
                && x.Race == $"{race}"
                && x.Gender == $"{gender}");
            var lastName = await writerHelperDBContext.EntryNames.OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync(x =>
                x.FirstOrMiddleOrLastName == $"Last"
                && x.Race == $"{race}"
                && x.Gender == $"{gender}");
            if (firstName != null && middleName != null && lastName != null)
            {
                var resultName = new GenerateNameViewModel
                {
                    Gender = gender,
                    Race = race,
                    Name = $"{firstName.Name} {middleName.Name} {lastName.Name}"
                };
                return View(resultName);
            }

            return NotFound();
        }
    }
}
