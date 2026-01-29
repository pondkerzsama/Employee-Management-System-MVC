using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Models;

public class Employee
{
    [Key]
    public int EmployeeID { get; set; }

    [Required]
    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    [Range(0, 999999)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Salary { get; set; }

    public int DeptID { get; set; }

}