
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Globoma.Services;

namespace Globoma.Controller
{
    public class ConferenceController: Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IConferenceService service;
        public ConferenceController(IConferenceService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Conference Overview";
            return View(await service.GetAll());
        }

        public IActionResult Add()
        {
            ViewBag.Title = "Add Conference";
            return View(new ConferenceModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ConferenceModel model)
        {
            if (ModelState.IsValid)
                await service.Add(model);

            return RedirectToAction("Index");
        }

    }
}
