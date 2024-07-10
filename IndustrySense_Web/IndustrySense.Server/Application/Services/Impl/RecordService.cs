using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Dao;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Application.Services.Impl
{
    public class RecordService : IRecordService
    {
        private readonly IRecordDao _recordDao;

        public RecordService(IRecordDao recordDao)
        {
            _recordDao = recordDao;
        }

        public void AddRecord(Record record)
        {
            _recordDao.Insert(record);
        }

        public void RemoveRecordById(int id)
        {
            _recordDao.Delete(x => x.RecordId == id);
        }

        public Record? GetRecordById(int id)
        {
            return _recordDao.Select(x => x.RecordId == id);
        }

        public ResultSet<Record> GetRecords(int pageIndex)
        {
            return _recordDao.SelectFilteredList(x => true, pageIndex);
        }

        public void UpdateRecord(int id, Record updatedRecord)
        {
            _recordDao.Update(
                x => x.RecordId == id,
                record =>
                {
                    record.Timestamp = updatedRecord.Timestamp;
                    record.Content = updatedRecord.Content;
                    record.DeviceId = updatedRecord.DeviceId;
                }
            );
        }
    }
}
