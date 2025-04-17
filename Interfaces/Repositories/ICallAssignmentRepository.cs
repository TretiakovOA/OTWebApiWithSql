using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Repositories
{
    public interface ICallAssignmentRepository
    {
        void AddAssignment(int callId, int operatorId);
        void RemoveAssignment(int callId, int operatorId);
    }
}
