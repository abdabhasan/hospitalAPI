using HospitalDataLayer.Infrastructure.DTOs.Visitor;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsVisitor
    {

        public clsVisitor(IVisitorData visitorData)
        {
            _visitorData = visitorData;
        }


        private readonly IVisitorData _visitorData;
        public async Task<IEnumerable<VisitorDTO>> GetAllVisitorsAsync()
        {
            return await _visitorData.GetAllVisitorsAsync();
        }
    }
}