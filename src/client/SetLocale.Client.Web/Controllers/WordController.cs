﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Controllers
{
    public class WordController : BaseController
    {
        private readonly IWordService _wordService;
        public WordController(IUserService userService, IFormsAuthenticationService formsAuthenticationService, IWordService wordService) : base(userService, formsAuthenticationService)
        {
            _wordService = wordService;
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToHome();
            }

            var entity = await _wordService.GetByKey(id);
            if (entity == null)
            {
                return RedirectToHome();
            }

            var model = WordModel.MapEntityToModel(entity);
            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> All()
        {
            var entities = await _wordService.GetAll();
            var model = new List<WordModel>();
            foreach (var entity in entities)
            {
                model.Add(WordModel.MapEntityToModel(entity));
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult NotTranslated()
        {
            //var model = _demoDataService.GetNotTranslatedKeys();
            //return View(model);
            return null;
        }


        [HttpGet]
        public ViewResult New()
        {
            var model = new WordModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(WordModel model)
        {
            if (!model.IsValidForNew())
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            model.CreatedBy = User.Identity.GetUserId();
            var key = await _wordService.Create(model);
            if (key == null)
            {
                model.Msg = "bir sorun oluştu, daha önce eklenmiş olabilir";
                return View(model);
            }

            return Redirect("/key/detail/" + key);
        }

        [HttpGet]
        public ViewResult Edit(string id, string lang = ConstHelper.tr)
        {
            //var model = _demoDataService.GetATranslation();
            //return View(model);
            return null;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TranslationModel model)
        {
            if (model.IsValid())
            {


                return Redirect("/tag/detail/" + model.Key);
            }

            return View(model);
        }
    }
}