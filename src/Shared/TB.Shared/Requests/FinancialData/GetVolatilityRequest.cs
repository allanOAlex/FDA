namespace TB.Shared.Requests.FinancialData
{
    public record GetVolatilityRequest
    {
        //public IQueryable<decimal>? Returns { get; set; } 
        public dynamic? Returns { get; set; } 
    }
}
