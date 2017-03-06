using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;

namespace Core.Interface
{
    public interface IDevTestService
    {
        Task<IEnumerable<DevTestDto>> GetAllAsync();
        Task<DevTestDto> GetByIdAsync(int id);
        Task SaveAsync(DevTestDto dto);
        Task DeleteAsync(int id);
    }
}
