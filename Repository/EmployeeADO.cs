using EmpMgmtApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
//using Microsoft.Data.SqlClient;



    public class EmployeeADO
    {
    private readonly string _connectionString;
    public EmployeeADO(string connectionString)
    {
        _connectionString = connectionString;
    }


    List<Employee> employees = new List<Employee>();

        string ConnectionString = @"data source=.\MSSQLSERVER02; database=Employee_Project; integrated security=SSPI";

        public List<Employee> ViewEmployees()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string viewQuery = "SELECT Employee.EmpID, Employee.EmpName, Employee.Salary, Department.DeptName FROM Employee INNER JOIN Department ON Employee.DeptID=Department.DeptID";
                    SqlCommand viewCmd = new SqlCommand(viewQuery, connection);
                    using (SqlDataReader reader = viewCmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                EmpId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Salary = reader.GetDecimal(2),
                                DeptName = reader.GetString(3)
                            };

                            employees.Add(employee);
                        }
                        foreach (Employee employee in employees)
                        {
                            Console.WriteLine($" ID : {employee.EmpId} Name: {employee.Name}, Salary: {employee.Salary}, Dept: {employee.DeptName}");
                        }
                    }
                    return employees;

                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                return employees;
                }

            }
        }
    public Employee GetEmployeeByID(int id)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            Employee employee = new Employee();
            employee.EmpId = id;
            try
            {
                connection.Open();
                string viewQuery = "SELECT Employee.EmpID, Employee.EmpName, Employee.Salary, Department.DeptName FROM Employee INNER JOIN Department ON Employee.DeptID=Department.DeptID WHERE EmpID = @empID ";
                SqlCommand viewCmd = new SqlCommand(viewQuery, connection);
                viewCmd.Parameters.AddWithValue("@empID", employee.EmpId);
                using (SqlDataReader reader = viewCmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Employee employee1 = new Employee
                        {
                            EmpId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Salary = reader.GetDecimal(2),
                            DeptName = reader.GetString(3)
                        };
                        employee = employee1;
                    }
                    return employee;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (employee);
            }

        }

    }

        public int GetDeptID(Employee emp)
        {
            int deptId =0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string getDeptIDQuery = "Select DeptID from Department where DeptName = @deptName";
                    SqlCommand viewCmd = new SqlCommand(getDeptIDQuery, connection);
                    viewCmd.Parameters.AddWithValue("@deptName", emp.DeptName);

                    connection.Open(); 
                    Object result=viewCmd.ExecuteScalar();
                    if (result != null)
                    {
                      deptId = Convert.ToInt32(result);
                    }
                    return deptId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return deptId;
                }
            }

        }
        public void AddEmployee(Employee emp)
        {
            int deptID = GetDeptID(emp);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string AddEmpQuery = "INSERT INTO Employee VALUES (@Name,@Salary,@DeptID)";
                    SqlCommand InsertCmd = new SqlCommand(AddEmpQuery, connection);
                    InsertCmd.Parameters.AddWithValue("@Name", emp.Name);
                    InsertCmd.Parameters.AddWithValue("@Salary", emp.Salary);
                    InsertCmd.Parameters.AddWithValue("@DeptID", deptID);
                    connection.Open();
                    InsertCmd.ExecuteNonQuery();
                    Console.WriteLine("Employee Added Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        public void UpdateEmployee(Employee emp)
        {

            int? deptID = null;
        if (emp.Salary==0)
        {
            emp.Salary = null;
        }
            if (!string.IsNullOrEmpty(emp.DeptName) && !(string.Equals(emp.DeptName,"string")) )
            {
                deptID = GetDeptID(emp);
            }
            string query = @"
        UPDATE Employee
        SET
            EmpName = COALESCE(@EmpName, EmpName),
            Salary = COALESCE(@Salary, Salary),
            DeptID = COALESCE(@DeptID, DeptID)

        WHERE
            EmpID = @EmpID;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@EmpID", emp.EmpId);
                
                // Using a dictionary to manage parameters
                var parameters = new Dictionary<string, object>
        {
            { "@EmpName", string.Equals(emp.Name,"String") ? DBNull.Value : (object)(emp.Name)},
            { "@Salary", emp.Salary.HasValue ? (object)emp.Salary.Value : DBNull.Value },
            { "@DeptID", deptID.HasValue ? (object)deptID.Value : DBNull.Value }
        };

                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                try
                {
                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Employee details updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No employee found with the specified ID.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        public void DeleteEmployee(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string deleteQuery = "DELETE FROM Employee WHERE EmpID = @empID";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection);
                    deleteCmd.Parameters.AddWithValue("@empID",id);
                    connection.Open();
                    deleteCmd.ExecuteNonQuery();
                    Console.WriteLine("Employee details deleted successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }



