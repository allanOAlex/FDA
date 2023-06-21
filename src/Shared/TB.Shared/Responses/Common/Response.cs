namespace TB.Shared.Responses.Common
{
    public record Response
    {
        public int Id { get; set; }
        public bool Successful { get; set; }
        public string? Message { get; set; }
    }
}
