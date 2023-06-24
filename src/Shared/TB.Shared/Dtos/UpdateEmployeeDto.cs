namespace TB.Shared.Dtos
{
    public record UpdateEmployeeDto : Dto
    {
        public string? Name { get; set; }
        public int OldSalary { get; set; }
        public int Salary { get; set; }
    }
}
