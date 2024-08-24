
using HospitalDataLayer.Infrastructure.DTOs.Visitor;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IVisitorData
    {
        Task<IEnumerable<VisitorDTO>> GetAllVisitorsAsync();
    }
}