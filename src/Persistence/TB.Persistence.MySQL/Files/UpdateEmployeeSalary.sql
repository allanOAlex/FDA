CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateEmployeeSalary`(
    IN empID INT,
    IN newSalary INT,
    OUT oldSalary INT
)
BEGIN
    -- Declare a variable to store the old salary
    DECLARE oldSalaryTemp INT;
    
    -- Get the old salary of the employee
SELECT 
    Salary
INTO oldSalaryTemp FROM
    Employees
WHERE
    Id = empID;
    
    -- Update the employee's salary
UPDATE Employees 
SET 
    Salary = newSalary
WHERE
    Id = empID;
    
    -- Set the old salary value in the OUT parameter
    SET oldSalary = oldSalaryTemp;
    SELECT oldSalary AS OldSalary;
END