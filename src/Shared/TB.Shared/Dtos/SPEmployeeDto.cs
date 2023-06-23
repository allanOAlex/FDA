namespace TB.Shared.Dtos
{
    public class SPEmployeeDto
    {
        public SPEmployeeDto(int oldSalary, int id, string? name, int salary)
        {
            OldSalary = oldSalary;
            Id = id;
            Name = name;
            Salary = salary;
        }

        public int OldSalary { get; set; }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Salary { get; set; }
    }
}
