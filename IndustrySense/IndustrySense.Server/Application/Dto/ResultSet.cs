using System.Data;

namespace IndustrySense.Server.Application.Dto
{
    public class ResultSet<T>(IEnumerable<T> resultList, int resultCount, int pageIndex)
    {
        public IEnumerable<T> ResultList { get; set; } = resultList;
        public int ResultCount { get; set; } = resultCount;
        public int PageIndex { get; set; } = pageIndex;
        public int PageCount
        {
            get
            {
                if (ResultCount % 10 == 0)
                {
                    return ResultCount / 10;
                }
                else
                {
                    return ResultCount / 10 + 1;
                }
            }
        }
    }
}
