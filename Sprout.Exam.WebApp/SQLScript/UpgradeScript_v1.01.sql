IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Employee') and NAME = 'BasePay')
BEGIN
      ALTER TABLE [dbo].Employee
	  ADD BasePay decimal(18,2) NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.EmployeeType') and NAME = 'PayLabel')
BEGIN
      ALTER TABLE [dbo].EmployeeType
	  ADD PayLabel NVARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.EmployeeType') and NAME = 'DayLabel')
BEGIN
      ALTER TABLE [dbo].EmployeeType
	  ADD DayLabel NVARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.EmployeeType') and NAME = 'Tax')
BEGIN
      ALTER TABLE [dbo].EmployeeType
	  ADD Tax decimal(18,2) NULL
END
GO

IF NOT EXISTS (SELECT * FROM SYS.COLUMNS WHERE OBJECT_ID = OBJECT_ID(N'Version'))
BEGIN
	CREATE TABLE dbo.Version
	(
		[Version] NVARCHAR(100) NULL
	)
END
GO

UPDATE dbo.EmployeeType
SET PayLabel = 'Salary', DayLabel = 'Absent Days', Tax = 12
WHERE TypeName = 'Regular'

UPDATE dbo.EmployeeType
SET PayLabel = 'Rate Per Day', DayLabel = 'Worked Days', Tax = 0
WHERE TypeName = 'Contractual'

UPDATE Employee
SET BasePay = 0
WHERE BasePay is null

TRUNCATE TABLE [Version]
GO
INSERT INTO [Version] VALUES ('v1.01')
GO 