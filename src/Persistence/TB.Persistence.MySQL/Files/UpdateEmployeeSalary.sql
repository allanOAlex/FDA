CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateEmployeeSalary`(
    IN empID INT,
    IN newSalary INT,
    OUT oldSalary INT
)
BEGIN
    
-- Get the old salary of the employee
SELECT Salary INTO oldSalary FROM Employees WHERE Id = empID;
    
-- Update the employee's salary
UPDATE Employees SET Salary = newSalary WHERE Id = empID;

END