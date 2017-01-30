using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nebula.Domain;
using Nebula.Domain.Entities;
using Nebula.Domain.ViewModels.Admin.Geo;
using System.Data.Entity;

namespace Nebula.Services.Services.Admin
{
    public class AdminGeoVideoService
    {
        private readonly GeneralRepository _generalRepository;

        public AdminGeoVideoService(GeneralRepository generalRepository)
        {
            _generalRepository = generalRepository;
        }

        public GeoVideoViewModel GetViewModel(byte videoCategory, int workId, int id = 0)
        {
            var model = new GeoVideoViewModel
            {
                Category = videoCategory
            };

            if (videoCategory == 1)
            {
                var work = _generalRepository.GeoAuthorWorkPart.Set().Include(a=>a.AuthorWork).First(a=>a.Id==workId);
                model.WorkTitle = work.AuthorWork.Title;
                model.PartName = work.PartName;
                model.AuthorWorkPartId = workId;
            }
            else
            {
                var subTag = _generalRepository.GeoGrammarSubTag.Set().Include(a => a.GeoGrammarTag).First(a => a.Id == workId);
                model.TagName = subTag.GeoGrammarTag.TagName;
                model.SubTagName = subTag.TagName;
                model.TagId = workId;
            }

            if (id == 0)
                return model;

            var video = _generalRepository.GeoVideo.Set().First(a => a.Id == id);

            model.Title = video.Name;
            model.FileLink = video.FileLink;

            return model;
        }

        public void SaveGeoQuestion(GeoVideoViewModel model)
        {
            if (model.Id == 0)
                CreateGeoVideo(model);
            else
                UpdateGeoVideo(model);
        }

        public void CreateGeoVideo(GeoVideoViewModel model)
        {
            var video = new GeoVideo
            {
                FileLink = model.FileLink,
                Name = model.Title,
                GeoAuthorWorkPartId = model.AuthorWorkPartId,
                GeoGrammarSubTagId = model.TagId
            };

            _generalRepository.GeoVideo.Save(video);
        }

        public void UpdateGeoVideo(GeoVideoViewModel model)
        {
            var video = _generalRepository.GeoVideo.Fetch(model.Id);

            video.Name = model.Title;
            video.FileLink = model.FileLink;
            video.GeoAuthorWorkPartId = model.AuthorWorkPartId;
            video.GeoGrammarSubTagId = model.TagId;

            _generalRepository.GeoVideo.Save(video);
        }
    }
}
