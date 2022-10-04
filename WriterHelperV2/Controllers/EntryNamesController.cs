using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            return RedirectToAction("Add");
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
            //TODO add error page
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEntryNameViewModel model)
        {
            var name = await writerHelperDBContext.EntryNames.FindAsync(model.Id);
            if(name != null)
            {
                name.Name = model.Name;
                name.Race = model.Race;
                name.FirstOrMiddleOrLastName = model.FirstOrMiddleOrLastName;
                name.Gender = model.Gender;

                await writerHelperDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //TODO add error page
            return RedirectToAction("Index");
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
    }
}
