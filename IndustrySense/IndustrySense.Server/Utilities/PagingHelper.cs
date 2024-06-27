using IndustrySense.Server.Dto;

namespace IndustrySense.Server.Utilities
{
    public static class PagingHelper
    {
        public static ResultSet<T> Page<T>(this IEnumerable<T> source, int pageIndex)
        {
            List<T> result;
            int pageSize = 10; // TODO 加到配置文件里面
            int resultCount = source.Count();
            if (pageIndex <= -1)
            {
                result = source.ToList();
            }
            else
            {
                result = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            return new ResultSet<T>(result, resultCount, pageIndex);
        }
    }
}
