using HospitalDataLayer.Infrastructure.DTOs.Visitor;

namespace HospitalBusinessLayer.Core.Interfaces
{
    public interface IVisitor
    {
        Task<IEnumerable<VisitorDTO>> GetAllVisitorsAsync();
        Task<bool> DeleteVisitorByIdAsync(int visitorId);
        Task<IEnumerable<VisitorDTO>> GetVisitorByNameAsync(string visitorName);
    }
}