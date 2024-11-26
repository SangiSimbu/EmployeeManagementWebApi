namespace EmpMgmtApp
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        //public int Age { get; set; }
        public decimal? Salary { get; set; }
        public string DeptName { get; set; }

        public override string ToString()
        {
            return $" ID : {EmpId} Name: {Name}, Salary: {Salary}, Dept: {DeptName}";
        }
    }

}
