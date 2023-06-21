using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Google
{
    public record GoogleSheetResponse : Response
    {
        public string? DataUrl { get; set; }
        public DateTime Date { get; set; }
        public string? Asset { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal CloseAdjusted { get; set; }
        public decimal SplitCoefficient { get; set; }
    }
}
