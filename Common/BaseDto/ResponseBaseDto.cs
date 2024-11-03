namespace Common.BaseDto
{
    public class ResponseBaseDto
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class ResponseBaseDto<T> : ResponseBaseDto
    {
        public T Data { get; set; }

    }
}
