----------------------------------------------------------------------------------------------------
------------------------------------- !!! DO NOT MODIFY !!! ----------------------------------------
----------------------------------------------------------------------------------------------------
-- Request Approval and Directions from Bhagya,Nimesh,Sayuru if modification needed to the file. --
----------------------------------------------------------------------------------------------------
--  DATE    USER        DESCRIPTION  
--  ------  ----------  ----------------------------------------------------------------------------
--  
--  281203  NimeshE     Created the file for versioning.
----------------------------------------------------------------------------------------------------

USE [master]
GO
/****** Object:  Database [platnedpass]    Script Date: 12/3/2024 6:45:48 PM ******/
CREATE DATABASE [platnedpass]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'platnedpass', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\platnedpass.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'platnedpass_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\platnedpass_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [platnedpass] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [platnedpass].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [platnedpass] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [platnedpass] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [platnedpass] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [platnedpass] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [platnedpass] SET ARITHABORT OFF 
GO
ALTER DATABASE [platnedpass] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [platnedpass] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [platnedpass] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [platnedpass] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [platnedpass] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [platnedpass] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [platnedpass] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [platnedpass] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [platnedpass] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [platnedpass] SET  DISABLE_BROKER 
GO
ALTER DATABASE [platnedpass] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [platnedpass] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [platnedpass] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [platnedpass] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [platnedpass] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [platnedpass] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [platnedpass] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [platnedpass] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [platnedpass] SET  MULTI_USER 
GO
ALTER DATABASE [platnedpass] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [platnedpass] SET DB_CHAINING OFF 
GO
ALTER DATABASE [platnedpass] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [platnedpass] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [platnedpass] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [platnedpass] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [platnedpass] SET QUERY_STORE = ON
GO
ALTER DATABASE [platnedpass] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [platnedpass]
GO
/****** Object:  User [platnedpassuser]    Script Date: 12/3/2024 6:45:51 PM ******/
CREATE USER [platnedpassuser] FOR LOGIN [platnedpassuser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [ProcedureExecutor]    Script Date: 12/3/2024 6:45:52 PM ******/
CREATE ROLE [ProcedureExecutor]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [platnedpassuser]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [platnedpassuser]
GO
ALTER ROLE [db_datareader] ADD MEMBER [platnedpassuser]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [platnedpassuser]
GO
/****** Object:  Table [dbo].[PASS_COMPANY_CONTACT_TAB]    Script Date: 12/3/2024 6:45:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PASS_COMPANY_CONTACT_TAB](
	[COMPANY_ID] [varchar](6) NULL,
	[USER_ID] [varchar](50) NULL,
	[COMPANY_CONTACT_TITLE] [varchar](100) NULL,
	[COMPANY_CONTACT_NUMBER] [varchar](50) NULL,
	[COMPANY_CONTACT_EMAIL] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PASS_COMPANY_TAB]    Script Date: 12/3/2024 6:45:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PASS_COMPANY_TAB](
	[COMPANY_ID] [varchar](6) NOT NULL,
	[COMPANY_NAME] [varchar](200) NOT NULL,
	[COMPANY_ADDRESS] [varchar](1000) NULL,
	[LICENSE_LIMIT] [int] NOT NULL,
	[LICENSE_CONSUMED] [int] NULL,
	[COMPANY_TYPE] [varchar](50) NULL,
	[CREATED_BY] [varchar](20) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](20) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROWSTATE] [varchar](20) NULL,
 CONSTRAINT [PK_PASS_COMPANY_TAB] PRIMARY KEY CLUSTERED 
(
	[COMPANY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PASS_USER_APP_LOG_TAB]    Script Date: 12/3/2024 6:45:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PASS_USER_APP_LOG_TAB](
	[COMPANY_ID] [varchar](6) NOT NULL,
	[USER_ID] [varchar](50) NOT NULL,
	[LOG_LINE_NUMBER] [int] NOT NULL,
	[LOG_DATE] [datetime] NOT NULL,
	[LOG_TYPE] [varchar](15) NOT NULL,
	[LOG_DESCRIPTION] [nvarchar](max) NULL,
	[PLATNED_REMARKS] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PASS_USERS_PER_COMPANY_TAB]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PASS_USERS_PER_COMPANY_TAB](
	[COMPANY_ID] [varchar](6) NULL,
	[USER_ID] [varchar](50) NOT NULL,
	[USER_NAME] [varchar](200) NULL,
	[PASSWORD] [varchar](200) NULL,
	[USER_EMAIL] [varchar](100) NULL,
	[LICENSE_KEY] [varchar](20) NULL,
	[VALID_FROM] [date] NULL,
	[VALID_TO] [date] NULL,
	[CREATED_BY] [varchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[USER_ROLE] [nchar](20) NULL,
	[ROWSTATE] [varchar](20) NULL,
 CONSTRAINT [PK_PASS_USERS_PER_COMPANY_TAB] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PASS_COMPANY_CONTACT_TAB]  WITH CHECK ADD  CONSTRAINT [FK_PASS_COMPANY_CONTACT_TAB_PASS_COMPANY_TAB] FOREIGN KEY([COMPANY_ID])
REFERENCES [dbo].[PASS_COMPANY_TAB] ([COMPANY_ID])
GO
ALTER TABLE [dbo].[PASS_COMPANY_CONTACT_TAB] CHECK CONSTRAINT [FK_PASS_COMPANY_CONTACT_TAB_PASS_COMPANY_TAB]
GO
ALTER TABLE [dbo].[PASS_COMPANY_CONTACT_TAB]  WITH CHECK ADD  CONSTRAINT [FK_PASS_COMPANY_CONTACT_TAB_PASS_USERS_PER_COMPANY_TAB] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[PASS_USERS_PER_COMPANY_TAB] ([USER_ID])
GO
ALTER TABLE [dbo].[PASS_COMPANY_CONTACT_TAB] CHECK CONSTRAINT [FK_PASS_COMPANY_CONTACT_TAB_PASS_USERS_PER_COMPANY_TAB]
GO
ALTER TABLE [dbo].[PASS_USERS_PER_COMPANY_TAB]  WITH CHECK ADD  CONSTRAINT [FK_PASS_USERS_PER_COMPANY_TAB_PASS_COMPANY_TAB] FOREIGN KEY([COMPANY_ID])
REFERENCES [dbo].[PASS_COMPANY_TAB] ([COMPANY_ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PASS_USERS_PER_COMPANY_TAB] CHECK CONSTRAINT [FK_PASS_USERS_PER_COMPANY_TAB_PASS_COMPANY_TAB]
GO
/****** Object:  Table [dbo].[PASS_USER_ROLE_ACCESS_TAB]    Script Date: 12/6/2024 5:12:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PASS_USER_ROLE_ACCESS_TAB](
	[APP_FUNCTION] [varchar](100) NOT NULL,
	[APP_FUNCTION_DESC] [varchar](2000) NOT NULL,
	[USER_ROLE] [nchar](20) NOT NULL,
    [CREATE_ALLOWED] [varchar](5) NOT NULL,
	[READ_ALLOWED] [varchar](5) NOT NULL,    
	[UPDATE_ALLOWED] [varchar](5) NOT NULL,
	[DELETE_ALLOWED] [varchar](5) NOT NULL,
	[CREATED_BY] [varchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
 CONSTRAINT [PK_PASS_USER_ROLE_ACCESS_TAB_1] PRIMARY KEY CLUSTERED 
(
	[APP_FUNCTION] ASC,
	[USER_ROLE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spDeletePassAccessControl]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDeletePassAccessControl]
    @APP_FUNCTION VARCHAR(100),
    @USER_ROLE VARCHAR(20)
AS
BEGIN
    DELETE FROM dbo.PASS_USER_ROLE_ACCESS_TAB 
    WHERE APP_FUNCTION = @APP_FUNCTION
    AND USER_ROLE = @USER_ROLE;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeletePassCompany]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDeletePassCompany]
    @COMPANY_ID VARCHAR(10),@MODIFIED_BY VARCHAR(100) = '',@ROWSTATE VARCHAR(10) = ''
AS
BEGIN
    DELETE FROM dbo.PASS_COMPANY_TAB WHERE COMPANY_ID = @COMPANY_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeletePassCompanyContact]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDeletePassCompanyContact]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    DELETE FROM PASS_COMPANY_CONTACT_TAB WHERE COMPANY_ID = @COMPANY_ID AND USER_ID = @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeletePassUserAppLogs]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDeletePassUserAppLogs]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    DELETE FROM PASS_USER_APP_LOG_TAB WHERE COMPANY_ID = @COMPANY_ID AND USER_ID = @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeleteUsersPerCompany]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDeleteUsersPerCompany]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    DELETE FROM PASS_USERS_PER_COMPANY_TAB WHERE COMPANY_ID = @COMPANY_ID AND USER_ID= @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditPassAccessControl]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEditPassAccessControl]
    @APP_FUNCTION VARCHAR(100),
    @APP_FUNCTION_DESC VARCHAR(2000),
    @USER_ROLE VARCHAR(20),
    @READ_ALLOWED VARCHAR(5),
    @CREATE_ALLOWED VARCHAR(5),
    @UPDATE_ALLOWED VARCHAR(5),
    @DELETE_ALLOWED VARCHAR(5),
    @MODIFIED_BY VARCHAR(20)
AS
BEGIN
    UPDATE dbo.PASS_USER_ROLE_ACCESS_TAB
    SET APP_FUNCTION = @APP_FUNCTION, APP_FUNCTION_DESC = @APP_FUNCTION_DESC, USER_ROLE = @USER_ROLE, READ_ALLOWED = @READ_ALLOWED, 
        CREATE_ALLOWED = @CREATE_ALLOWED, UPDATE_ALLOWED = @UPDATE_ALLOWED, DELETE_ALLOWED = @DELETE_ALLOWED, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE()
    WHERE APP_FUNCTION = @APP_FUNCTION
    AND USER_ROLE = @USER_ROLE;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditPassCompany]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEditPassCompany]
    @COMPANY_ID VARCHAR(10),
    @COMPANY_NAME VARCHAR(200),
    @COMPANY_ADDRESS VARCHAR(1000),
    @LICENSE_LIMIT INT,
    @LICENSE_CONSUMED INT,
    @COMPANY_TYPE VARCHAR(50),
    @MODIFIED_BY VARCHAR(20),
    @ROWSTATE VARCHAR(20)
AS
BEGIN
    UPDATE dbo.PASS_COMPANY_TAB
    SET COMPANY_NAME = @COMPANY_NAME, COMPANY_ADDRESS = @COMPANY_ADDRESS, LICENSE_LIMIT = @LICENSE_LIMIT, LICENSE_CONSUMED = @LICENSE_CONSUMED, 
        COMPANY_TYPE = @COMPANY_TYPE, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE(), ROWSTATE = @ROWSTATE
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditPassCompanyContact]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEditPassCompanyContact]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COMPANY_CONTACT_TITLE VARCHAR(100),
    @COMPANY_CONTACT_NUMBER VARCHAR(50),
    @COMPANY_CONTACT_EMAIL VARCHAR(100)
AS
BEGIN
    UPDATE PASS_COMPANY_CONTACT_TAB
    SET USER_ID = @USER_ID, COMPANY_CONTACT_TITLE = @COMPANY_CONTACT_TITLE,
        COMPANY_CONTACT_NUMBER = @COMPANY_CONTACT_NUMBER, COMPANY_CONTACT_EMAIL = @COMPANY_CONTACT_EMAIL
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditPassUserAppLogs]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEditPassUserAppLogs]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @LOG_LINE_NUMBER INT,
    @LOG_DATE DATETIME,
    @LOG_TYPE VARCHAR(15),
    @LOG_DESCRIPTION NVARCHAR(MAX),
    @PLATNED_REMARKS NVARCHAR(MAX)
AS
BEGIN
    UPDATE PASS_USER_APP_LOG_TAB
    SET LOG_LINE_NUMBER = @LOG_LINE_NUMBER, LOG_DATE = @LOG_DATE, LOG_TYPE = @LOG_TYPE, LOG_DESCRIPTION = @LOG_DESCRIPTION, 
        PLATNED_REMARKS = @PLATNED_REMARKS
    WHERE COMPANY_ID = @COMPANY_ID AND USER_ID = @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditUserPassword]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEditUserPassword]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
	@PASSWORD VARCHAR(200),
    @USER_EMAIL VARCHAR(100)
AS
BEGIN
    UPDATE PASS_USERS_PER_COMPANY_TAB
    SET PASSWORD = @PASSWORD
    WHERE COMPANY_ID = @COMPANY_ID AND USER_ID= @USER_ID AND USER_EMAIL= @USER_EMAIL;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditUsersPerCompany]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spEditUsersPerCompany]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @USER_NAME VARCHAR(200),
    @USER_EMAIL VARCHAR(100),
    @VALID_FROM DATETIME,
    @VALID_TO DATETIME,
    @MODIFIED_BY VARCHAR(50),
    @USER_ROLE NCHAR(20),
    @ROWSTATE VARCHAR(20)
AS
BEGIN
    UPDATE PASS_USERS_PER_COMPANY_TAB
    SET USER_NAME = @USER_NAME, USER_EMAIL = @USER_EMAIL,
        VALID_FROM = @VALID_FROM, VALID_TO = @VALID_TO, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE(), USER_ROLE = @USER_ROLE, ROWSTATE = @ROWSTATE
    WHERE COMPANY_ID = @COMPANY_ID AND USER_ID= @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetAccessData]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetAccessData]
AS
BEGIN
    SELECT * FROM PASS_USER_ROLE_ACCESS_TAB
    ORDER BY APP_FUNCTION, USER_ROLE;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetAccessPerFunction]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetAccessPerFunction]
    @APP_FUNCTION VARCHAR(200),
    @USER_ROLE VARCHAR(20)
AS
BEGIN
    SELECT * FROM PASS_USER_ROLE_ACCESS_TAB
    WHERE APP_FUNCTION = @APP_FUNCTION
    AND USER_ROLE = @USER_ROLE;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetCompanyPassUserAppLogs]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetCompanyPassUserAppLogs]
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    SELECT * FROM PASS_USER_APP_LOG_TAB
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetLoginUser]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGetLoginUser]
    @USER_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    WHERE USER_ID= @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPassCompanies]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetPassCompanies]
AS
BEGIN
    SELECT * 
    FROM dbo.PASS_COMPANY_TAB;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPassCompany]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetPassCompany]
    @COMPANY_ID VARCHAR(10)
AS
BEGIN
    SELECT * 
    FROM dbo.PASS_COMPANY_TAB
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPassCompanyContact]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetPassCompanyContact]
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    SELECT * FROM PASS_COMPANY_CONTACT_TAB
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPassCompanyContacts]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetPassCompanyContacts]
AS
BEGIN
    SELECT * FROM PASS_COMPANY_CONTACT_TAB
    ORDER BY COMPANY_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPassUserAppLogs]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetPassUserAppLogs]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_USER_APP_LOG_TAB
    WHERE COMPANY_ID = @COMPANY_ID AND USER_ID = @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPassUsers]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetPassUsers]
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    ORDER BY USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetUserPerCompany]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetUserPerCompany]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID= @USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetUsersperCompany]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGetUsersperCompany]
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    WHERE COMPANY_ID = @COMPANY_ID
    ORDER BY USER_ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spSavePassAccessControl]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spSavePassAccessControl]
    @APP_FUNCTION VARCHAR(100),
    @APP_FUNCTION_DESC VARCHAR(2000),
    @USER_ROLE VARCHAR(20),
    @READ_ALLOWED VARCHAR(5),
    @CREATE_ALLOWED VARCHAR(5),
    @UPDATE_ALLOWED VARCHAR(5),
    @DELETE_ALLOWED VARCHAR(5),
    @CREATED_BY VARCHAR(50)
AS
BEGIN
    INSERT INTO dbo.PASS_USER_ROLE_ACCESS_TAB(APP_FUNCTION, APP_FUNCTION_DESC, USER_ROLE, READ_ALLOWED, CREATE_ALLOWED, UPDATE_ALLOWED, DELETE_ALLOWED, CREATED_BY, CREATED_DATE)
    VALUES(@APP_FUNCTION, @APP_FUNCTION_DESC, @USER_ROLE, @READ_ALLOWED, @CREATE_ALLOWED, @UPDATE_ALLOWED, @DELETE_ALLOWED, @CREATED_BY, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spSavePassCompany]    Script Date: 12/6/2024 7:16:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spSavePassCompany]
    @COMPANY_ID VARCHAR(10),
    @COMPANY_NAME VARCHAR(200),
    @COMPANY_ADDRESS VARCHAR(1000),
    @LICENSE_LIMIT INT,   
    @COMPANY_TYPE VARCHAR(50),
    @CREATED_BY VARCHAR(20),
    @ROWSTATE VARCHAR(20)
AS
BEGIN
    INSERT INTO dbo.PASS_COMPANY_TAB(COMPANY_ID, COMPANY_NAME, COMPANY_ADDRESS, LICENSE_LIMIT,  COMPANY_TYPE, CREATED_BY, CREATED_DATE, ROWSTATE)
    VALUES(@COMPANY_ID, @COMPANY_NAME, @COMPANY_ADDRESS, @LICENSE_LIMIT,  @COMPANY_TYPE, @CREATED_BY, GETDATE(), @ROWSTATE);

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spSavePassCompanyContact]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spSavePassCompanyContact]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COMPANY_CONTACT_TITLE VARCHAR(100),
    @COMPANY_CONTACT_NUMBER VARCHAR(50),
    @COMPANY_CONTACT_EMAIL VARCHAR(100)
AS
BEGIN
    INSERT INTO PASS_COMPANY_CONTACT_TAB(COMPANY_ID, USER_ID, COMPANY_CONTACT_TITLE, COMPANY_CONTACT_NUMBER, COMPANY_CONTACT_EMAIL)
    VALUES(@COMPANY_ID, @USER_ID, @COMPANY_CONTACT_TITLE, @COMPANY_CONTACT_NUMBER, @COMPANY_CONTACT_EMAIL);
END;
GO
/****** Object:  StoredProcedure [dbo].[spSavePassUserAppLogs]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spSavePassUserAppLogs]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @LOG_LINE_NUMBER INT,
    @LOG_DATE DATETIME,
    @LOG_TYPE VARCHAR(15),
    @LOG_DESCRIPTION NVARCHAR(MAX),
    @PLATNED_REMARKS NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO PASS_USER_APP_LOG_TAB(COMPANY_ID, USER_ID, LOG_LINE_NUMBER, LOG_DATE, LOG_TYPE, LOG_DESCRIPTION, PLATNED_REMARKS)
    VALUES(@COMPANY_ID, @USER_ID, @LOG_LINE_NUMBER, @LOG_DATE, @LOG_TYPE, @LOG_DESCRIPTION, @PLATNED_REMARKS);
END;
GO
/****** Object:  StoredProcedure [dbo].[spSaveUsersPerCompany]    Script Date: 12/3/2024 6:45:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spSaveUsersPerCompany]
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @USER_NAME VARCHAR(200),
    @PASSWORD VARCHAR(200),
    @USER_EMAIL VARCHAR(100),
    @LICENSE_KEY VARCHAR(20),
    @VALID_FROM DATETIME,
    @VALID_TO DATETIME,
    @CREATED_BY VARCHAR(50),
    @USER_ROLE NCHAR(20),
    @ROWSTATE VARCHAR(20)
AS
BEGIN
    INSERT INTO PASS_USERS_PER_COMPANY_TAB(COMPANY_ID, USER_ID, USER_NAME, PASSWORD, USER_EMAIL, LICENSE_KEY, VALID_FROM, VALID_TO, CREATED_BY, 
                                           CREATED_DATE, USER_ROLE, ROWSTATE)
    VALUES(@COMPANY_ID, @USER_ID, @USER_NAME, @PASSWORD, @USER_EMAIL, @LICENSE_KEY, @VALID_FROM, @VALID_TO, @CREATED_BY, GETDATE(), @USER_ROLE, @ROWSTATE);
END;
GO

------------------------------------ DEFAULT VALUES - BASIC DATA ------------------------------------
USE [master]
GO
INSERT INTO [dbo].[PASS_COMPANY_TAB]
           ([COMPANY_ID]
           ,[COMPANY_NAME]
           ,[COMPANY_ADDRESS]
           ,[LICENSE_LIMIT]
           ,[LICENSE_CONSUMED]
           ,[COMPANY_TYPE]
           ,[CREATED_BY]
           ,[CREATED_DATE]
           ,[MODIFIED_BY]
           ,[MODIFIED_DATE]
           ,[ROWSTATE])
     VALUES
           ('PLTPVT'
           ,'Platned (Pvt) Ltd'
           ,'Maharagama, Sri Lanka'
           ,'10000'
           ,NULL
           ,'Internal'
           ,'PLATNEDPASS'
           ,'2024-12-03 13:46:56.747'
           ,NULL
           ,NULL
           ,'ACTIVE')
GO

INSERT INTO [dbo].[PASS_USERS_PER_COMPANY_TAB]
           ([COMPANY_ID]
           ,[USER_ID]
           ,[USER_NAME]
           ,[PASSWORD]
           ,[USER_EMAIL]
           ,[LICENSE_KEY]
           ,[VALID_FROM]
           ,[VALID_TO]
           ,[CREATED_BY]
           ,[CREATED_DATE]
           ,[MODIFIED_BY]
           ,[MODIFIED_DATE]
           ,[USER_ROLE]
           ,[ROWSTATE])
     VALUES
           ('PLTPVT'
           ,'PLATNEDPASS'
           ,'Platned Pass User'
           ,'$2a$11$u8E5BK6mq3mmnJJhYq8AxuYIYA9z2kCLGwjj4SCEyaGviSMe4WF86'
           ,'mahara@platned.com'
           ,'ABC'
           ,'2024-12-03'
           ,'9999-12-03'
           ,'PLATNEDPASS'
           ,'2024-12-03 13:48:55.030'
           ,NULL
           ,NULL
           ,'Super Admin'
           ,'ACTIVE')
GO

INSERT INTO [dbo].[PASS_COMPANY_CONTACT_TAB]
           ([COMPANY_ID]
           ,[USER_ID]
           ,[COMPANY_CONTACT_TITLE]
           ,[COMPANY_CONTACT_NUMBER]
           ,[COMPANY_CONTACT_EMAIL])
     VALUES
           ('PLTPVT'
           ,'PLATNEDPASS'
           ,'Platned Pass App Owner'
           ,'+94112183634'
           ,'mahara@platned.com')
GO

INSERT INTO [dbo].[PASS_USER_ROLE_ACCESS_TAB]
           ([APP_FUNCTION],
            [APP_FUNCTION_DESC],
            [USER_ROLE],
            [READ_ALLOWED],
            [CREATE_ALLOWED],
            [UPDATE_ALLOWED],
            [DELETE_ALLOWED],
            [CREATED_BY],
            [CREATED_DATE],
            [MODIFIED_BY],
            [MODIFIED_DATE])
VALUES
    ('BTN_NEW_USER', 'NEW USER functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_NEW_USER', 'NEW USER functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_NEW_USER', 'NEW USER functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null),
	('PGE_READ_USER', 'READ USER Details page functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('PGE_READ_USER', 'READ USER Details page functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('PGE_READ_USER', 'READ USER Details page functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null),
	('BTN_EDIT_USER', 'EDIT USER Details functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_EDIT_USER', 'EDIT USER Details functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_EDIT_USER', 'EDIT USER Details functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null),
	('BTN_DELETE_USER', 'DELETE USER functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_DELETE_USER', 'DELETE USER functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_DELETE_USER', 'DELETE USER functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_NEW_COMPANY', 'NEW USER functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_NEW_COMPANY', 'NEW USER functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_NEW_COMPANY', 'NEW USER functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null),
	('PGE_READ_COMPANY', 'READ USER Details functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('PGE_READ_COMPANY', 'READ USER Details functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('PGE_READ_COMPANY', 'READ USER Details functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null),
	('BTN_EDIT_COMPANY', 'EDIT USER Details functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_EDIT_COMPANY', 'EDIT USER Details functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_EDIT_COMPANY', 'EDIT USER Details functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null),
	('BTN_DELETE_COMPANY', 'DELETE USER functionality', 'Super Admin', 'True', 'True', 'True', 'True', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_DELETE_COMPANY', 'DELETE USER functionality', 'User Admin', 'True', 'True', 'True', 'False', 'PLATNEDPASS', GETDATE(), null, null),
    ('BTN_DELETE_COMPANY', 'DELETE USER functionality', 'User', 'False', 'True', 'False', 'False', 'PLATNEDPASS', GETDATE(), null, null);