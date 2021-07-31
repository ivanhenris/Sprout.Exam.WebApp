IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Employee') and NAME = 'BasePay')
BEGIN
      ALTER TABLE [dbo].Employee
	  DROP COLUMN BasePay
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.EmployeeType') and NAME = 'PayLabel')
BEGIN
      ALTER TABLE [dbo].EmployeeType
	  DROP COLUMN PayLabel
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.EmployeeType') and NAME = 'DayLabel')
BEGIN
      ALTER TABLE [dbo].EmployeeType
	  DROP COLUMN DayLabel
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.EmployeeType') and NAME = 'Tax')
BEGIN
      ALTER TABLE [dbo].EmployeeType
	  DROP COLUMN Tax
END
GO

TRUNCATE TABLE [Version]
GO
INSERT INTO [Version] VALUES ('v1.00')
GO 