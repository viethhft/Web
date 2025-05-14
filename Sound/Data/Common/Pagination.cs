namespace Data.Common
{
    public class Pagination<T>
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Data { get; set; }
    }
}