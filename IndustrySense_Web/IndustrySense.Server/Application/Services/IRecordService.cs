using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Application.Services
{
    public interface IRecordService
    {
        void AddRecord(Record record);
        Record? GetRecordById(int id);
        ResultSet<Record> GetRecords(int pageIndex);
        void RemoveRecordById(int id);
        void UpdateRecord(int id, Record updatedRecord);
    }
}