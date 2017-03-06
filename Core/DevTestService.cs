using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dto;
using Core.Interface;
using Entities;
using Entities.Models;
using Repositories.Interface;
using System.Data.Entity;
using System;

namespace Core
{
    public class DevTestService : IDevTestService
    {
        private readonly IUnitOfWork<DBEntities> _unitOfWork;
        private readonly IRepositoryBase<DevTest,  DBEntities> _devTestRepository;

        public DevTestService(IUnitOfWork<DBEntities> unitOfWork, IRepositoryBase<DevTest, DBEntities> devTestRepository)
        {
            _unitOfWork = unitOfWork;
            _devTestRepository = devTestRepository;
        }

        public async Task<IEnumerable<DevTestDto>> GetAllAsync()
        {
            var data = await _devTestRepository.All().Select(a => new DevTestDto
            {
                ID = a.ID,
                CampaignName = a.CampaignName,
                Clicks = a.Clicks,
                Date = a.Date,
                Conversions = a.Conversions,
                Impressions = a.Impressions,
                AffiliateName = a.AffiliateName
            }).ToListAsync();

            return data;
        }

        public async Task<DevTestDto> GetByIdAsync(int id)
        {
            var data = await _devTestRepository.GetByIDAsync(id);

            var dto = new DevTestDto
            {
                ID = data.ID,
                CampaignName = data.CampaignName,
                Clicks = data.Clicks,
                Date = data.Date,
                Conversions = data.Conversions,
                Impressions = data.Impressions,
                AffiliateName = data.AffiliateName
            };

            return dto;
        }        

        public async Task SaveAsync(DevTestDto dto)
        {
            var devTest = new DevTest
            {
                ID = dto.ID,
                CampaignName = dto.CampaignName,
                Clicks = dto.Clicks,
                Date = dto.Date == null ? (DateTime?)null : Convert.ToDateTime(dto.Date),
                Conversions = dto.Conversions,
                Impressions = dto.Impressions,
                AffiliateName = dto.AffiliateName
            };
            if (devTest.ID == 0)
            {
                await _devTestRepository.InsertAsync(devTest);
            }
            else
            {
                await _devTestRepository.UpdateAsync(devTest);
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _devTestRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
