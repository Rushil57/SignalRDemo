using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Interface;
using System.Linq;
using SignalRDemo.Models;
using Core.Dto;
using System.Collections.Generic;

namespace SignalRDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDevTestService _devTestService;

        public HomeController(IDevTestService devTestService)
        {
            _devTestService = devTestService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAll()
        {
            var model = new List<DevTestModels>();

            try
            {
                var data = await _devTestService.GetAllAsync();

                model = data.Select(a => new DevTestModels
                {
                    ID = a.ID,
                    CampaignName = a.CampaignName,
                    Date = a.Date == null ? "" : a.Date.Value.ToString("yyyy/MM/dd"),
                    Clicks = a.Clicks,
                    Conversions = a.Conversions,
                    Impressions = a.Impressions,
                    AffiliateName = a.AffiliateName
                }).ToList();
                
            }
            catch (Exception)
            {                
                
            }
            return PartialView("_DevTestList", model);
            
        }

        public async Task<ActionResult> Edit(int id)
        {
            var data = await _devTestService.GetByIdAsync(id);

            var model = new DevTestModels
            {
                ID = data.ID,
                CampaignName = data.CampaignName,
                Date = data.Date == null ? "" : data.Date.Value.ToString("yyyy/MM/dd"),
                Clicks = data.Clicks,
                Conversions = data.Conversions,
                Impressions = data.Impressions,
                AffiliateName = data.AffiliateName
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Save(DevTestModels model)
        {
            var result = true;
            try
            {
                var dto = new DevTestDto
                {
                    ID = model.ID,
                    CampaignName = model.CampaignName,
                    Date = model.Date == null ? (DateTime?)null : Convert.ToDateTime(model.Date),
                    Clicks = model.Clicks,
                    Conversions = model.Conversions,
                    Impressions = model.Impressions,
                    AffiliateName = model.AffiliateName
                };
                await _devTestService.SaveAsync(dto);

                EmployeeHub.NotifyCurrentEmployeeInformationToAllClients();
            }
            catch (Exception ex)
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int id)
        {
            bool result = true;
            try
            {
                await _devTestService.DeleteAsync(id);
                EmployeeHub.NotifyCurrentEmployeeInformationToAllClients();
            }
            catch (Exception)
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
