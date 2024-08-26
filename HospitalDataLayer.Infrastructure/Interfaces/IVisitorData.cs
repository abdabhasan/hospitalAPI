
using HospitalDataLayer.Infrastructure.DTOs.Visitor;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IVisitorData
    {
        Task<IEnumerable<VisitorDTO>> GetAllVisitorsAsync();
        Task<bool> DeleteVisitorByIdAsync(int visitorId);
        Task<IEnumerable<VisitorDTO>> GetVisitorByNameAsync(string visitorName);
    }
}