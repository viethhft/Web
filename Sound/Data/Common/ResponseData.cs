namespace Data.Common
{
    public class ResponseData<T>
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
    }
}