
----------------------------------------------------------------------------------------------------
------------------------------------- !!! DO NOT MODIFY !!! ----------------------------------------
----------------------------------------------------------------------------------------------------
-- Request Approval and Directions from Bhagya,Nimesh,Sayuru if modification needed to the file. --
----------------------------------------------------------------------------------------------------
--  DATE    USER        DESCRIPTION  
--  ------  ----------  ----------------------------------------------------------------------------
--  
--  281124  NimeshE     Created the file for versioning, initial script created by Bhagya.
----------------------------------------------------------------------------------------------------

USE [platnedpass]
GO

---------------------------- Company ----------------------------
-----------------------------------------------------------------

CREATE OR ALTER PROCEDURE spGetPassCompanies
AS
BEGIN
    SELECT * 
    FROM dbo.PASS_COMPANY_TAB;
END;
GO

CREATE OR ALTER PROCEDURE spGetPassCompany
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    SELECT * 
    FROM dbo.PASS_COMPANY_TAB
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO

CREATE OR ALTER PROCEDURE spSavePassCompany
    @COMPANY_ID VARCHAR(6),
    @COMPANY_NAME VARCHAR(200),
    @COMPANY_ADDRESS VARCHAR(1000),
    @LICENSE_LIMIT INT,
    @LICENSE_CONSUMED INT,
    @COMPANY_TYPE VARCHAR(50),
    @CREATED_BY VARCHAR(50),
    @ROWSTATE VARCHAR(20)
AS
BEGIN
    INSERT INTO dbo.PASS_COMPANY_TAB(COMPANY_ID, COMPANY_NAME, COMPANY_ADDRESS, LICENSE_LIMIT, LICENSE_CONSUMED, COMPANY_TYPE, CREATED_BY, CREATED_DATE, ROWSTATE)
    VALUES(@COMPANY_ID, @COMPANY_NAME, @COMPANY_ADDRESS, @LICENSE_LIMIT, @LICENSE_CONSUMED, @COMPANY_TYPE, @CREATED_BY, GETDATE(), @ROWSTATE);

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE OR ALTER PROCEDURE spEditPassCompany
    @COMPANY_ID VARCHAR(6),
    @COMPANY_NAME VARCHAR(200),
    @COMPANY_ADDRESS VARCHAR(1000),
    @LICENSE_LIMIT INT,
    @LICENSE_CONSUMED INT,
    @COMPANY_TYPE VARCHAR(50),
    @MODIFIED_BY VARCHAR(50),
    @ROWSTATE VARCHAR(20)
AS
BEGIN
    UPDATE dbo.PASS_COMPANY_TAB
    SET COMPANY_NAME = @COMPANY_NAME, COMPANY_ADDRESS = @COMPANY_ADDRESS, LICENSE_LIMIT = @LICENSE_LIMIT, LICENSE_CONSUMED = @LICENSE_CONSUMED, 
        COMPANY_TYPE = @COMPANY_TYPE, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE(), ROWSTATE = @ROWSTATE
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO

CREATE OR ALTER PROCEDURE spDeletePassCompany
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    DELETE FROM dbo.PASS_COMPANY_TAB WHERE COMPANY_ID = @COMPANY_ID;
END;
GO

------------------------ Company Contact ------------------------
-----------------------------------------------------------------

CREATE OR ALTER PROCEDURE spGetPassCompanyContacts
AS
BEGIN
    SELECT * FROM PASS_COMPANY_CONTACT_TAB
    ORDER BY COMPANY_ID;
END;
GO

CREATE OR ALTER PROCEDURE spGetPassCompanyContact
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    SELECT * FROM PASS_COMPANY_CONTACT_TAB
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO

CREATE OR ALTER PROCEDURE spSavePassCompanyContact
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

CREATE OR ALTER PROCEDURE spEditPassCompanyContact
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

CREATE OR ALTER PROCEDURE spDeletePassCompanyContact
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    DELETE FROM PASS_COMPANY_CONTACT_TAB WHERE COMPANY_ID = @COMPANY_ID AND USER_ID = @USER_ID;
END;
GO

--------------------------- User Logs ---------------------------
-----------------------------------------------------------------

CREATE OR ALTER PROCEDURE spGetCompanyPassUserAppLogs
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    SELECT * FROM PASS_USER_APP_LOG_TAB
    WHERE COMPANY_ID = @COMPANY_ID;
END;
GO

CREATE OR ALTER PROCEDURE spGetPassUserAppLogs
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_USER_APP_LOG_TAB
    WHERE COMPANY_ID = @COMPANY_ID AND USER_ID = @USER_ID;
END;
GO

CREATE OR ALTER PROCEDURE spSavePassUserAppLogs
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

CREATE OR ALTER PROCEDURE spEditPassUserAppLogs
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

CREATE OR ALTER PROCEDURE spDeletePassUserAppLogs
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    DELETE FROM PASS_USER_APP_LOG_TAB WHERE COMPANY_ID = @COMPANY_ID AND USER_ID = @USER_ID;
END;
GO

------------------------- Company Users -------------------------
-----------------------------------------------------------------

CREATE OR ALTER PROCEDURE spGetUsersperCompany
    @COMPANY_ID VARCHAR(6)
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    WHERE COMPANY_ID = @COMPANY_ID
    ORDER BY USER_ID;
END;
GO

CREATE OR ALTER PROCEDURE spGetUserPerCompany
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID= @USER_ID;
END;
GO

CREATE OR ALTER PROCEDURE spGetLoginUser
    @USER_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    WHERE USER_ID= @USER_ID;
END;
GO

CREATE OR ALTER PROCEDURE spSaveUsersPerCompany
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

CREATE OR ALTER PROCEDURE spEditUsersPerCompany
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

CREATE OR ALTER PROCEDURE spEditUserPassword
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

CREATE OR ALTER PROCEDURE spDeleteUsersPerCompany
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    DELETE FROM PASS_USERS_PER_COMPANY_TAB WHERE COMPANY_ID = @COMPANY_ID AND USER_ID= @USER_ID;
END;
GO

CREATE OR ALTER PROCEDURE spGetPassUsers
AS
BEGIN
    SELECT * FROM PASS_USERS_PER_COMPANY_TAB
    ORDER BY USER_ID;
END;
GO

-------------------------- Role Access --------------------------
-----------------------------------------------------------------

CREATE OR ALTER PROCEDURE spGetAccessData
AS
BEGIN
    SELECT * FROM PASS_USER_ROLE_ACCESS_TAB
    ORDER BY APP_FUNCTION, USER_ROLE;
END;
GO

CREATE OR ALTER PROCEDURE spGetAccessPerFunction
    @APP_FUNCTION VARCHAR(200),
    @USER_ROLE VARCHAR(20)
AS
BEGIN
    SELECT * FROM PASS_USER_ROLE_ACCESS_TAB
    WHERE APP_FUNCTION = @APP_FUNCTION
    AND USER_ROLE = @USER_ROLE;
END;
GO

CREATE OR ALTER PROCEDURE spSavePassAccessControl
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

CREATE OR ALTER PROCEDURE spEditPassAccessControl
    @APP_FUNCTION VARCHAR(100),
    @APP_FUNCTION_DESC VARCHAR(2000),
    @USER_ROLE VARCHAR(20),
    @READ_ALLOWED VARCHAR(5),
    @CREATE_ALLOWED VARCHAR(5),
    @UPDATE_ALLOWED VARCHAR(5),
    @DELETE_ALLOWED VARCHAR(5),
    @MODIFIED_BY VARCHAR(50)
AS
BEGIN
    UPDATE dbo.PASS_USER_ROLE_ACCESS_TAB
    SET APP_FUNCTION = @APP_FUNCTION, APP_FUNCTION_DESC = @APP_FUNCTION_DESC, USER_ROLE = @USER_ROLE, READ_ALLOWED = @READ_ALLOWED, 
        CREATE_ALLOWED = @CREATE_ALLOWED, UPDATE_ALLOWED = @UPDATE_ALLOWED, DELETE_ALLOWED = @DELETE_ALLOWED, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE()
    WHERE APP_FUNCTION = @APP_FUNCTION
    AND USER_ROLE = @USER_ROLE;
END;
GO

CREATE OR ALTER PROCEDURE spDeletePassAccessControl
    @APP_FUNCTION VARCHAR(100),
    @USER_ROLE VARCHAR(20)
AS
BEGIN
    DELETE FROM dbo.PASS_USER_ROLE_ACCESS_TAB 
    WHERE APP_FUNCTION = @APP_FUNCTION
    AND USER_ROLE = @USER_ROLE;
END;
GO

-------------------------- JSON Collection --------------------------
-----------------------------------------------------------------

CREATE OR ALTER PROCEDURE spGetJsonCollections
AS
BEGIN
    SELECT * FROM PASS_JSON_COLLECTION_TAB
    ORDER BY COMPANY_ID, USER_ID, CREATED_DATE;
END;
GO

CREATE OR ALTER PROCEDURE spGetJsonCollectionPerUser
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_JSON_COLLECTION_TAB
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND ROWSTATE = 'Active';
END;
GO

CREATE OR ALTER PROCEDURE spSaveJsonCollection
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
    @COLLECTION_NAME VARCHAR(500),
    @CREATED_BY VARCHAR(50)
AS
BEGIN
    INSERT INTO dbo.PASS_JSON_COLLECTION_TAB(COMPANY_ID, USER_ID, COLLECTION_ID, COLLECTION_NAME, CREATED_BY, CREATED_DATE, ROWSTATE)
    VALUES(@COMPANY_ID, @USER_ID, @COLLECTION_ID, @COLLECTION_NAME, @CREATED_BY, GETDATE(), 'Active');

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE OR ALTER PROCEDURE spEditJsonCollection
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
    @COLLECTION_NAME VARCHAR(500),
    @MODIFIED_BY VARCHAR(50)
AS
BEGIN
    UPDATE dbo.PASS_JSON_COLLECTION_TAB
    SET COLLECTION_NAME = @COLLECTION_NAME, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE()
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID;
END;
GO

CREATE OR ALTER PROCEDURE spDeleteJsonCollection
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
    @MODIFIED_BY VARCHAR(50)
AS
BEGIN
    UPDATE dbo.PASS_JSON_COLLECTION_TAB
    SET MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE(), ROWSTATE = 'Obselete'
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID;

    UPDATE dbo.PASS_JSON_FILE_TAB
    SET MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE(), ROWSTATE = 'Obselete'
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID;
END;
GO

CREATE OR ALTER PROCEDURE spShareJsonCollection
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
    @MODIFIED_BY VARCHAR(50)
AS
BEGIN
    UPDATE dbo.PASS_JSON_COLLECTION_TAB
    SET USER_ID = @USER_ID, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE()
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID;

    UPDATE dbo.PASS_JSON_FILE_TAB
    SET USER_ID = @USER_ID, COLLECTION_ID = @COLLECTION_ID, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE()
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID;
END;
GO

-------------------------- JSON File --------------------------
-----------------------------------------------------------------

CREATE OR ALTER PROCEDURE spGetJsonFiles
AS
BEGIN
    SELECT * FROM PASS_JSON_FILE_TAB
    ORDER BY COMPANY_ID, USER_ID, CREATED_DATE;
END;
GO

CREATE OR ALTER PROCEDURE spGetJsonFilePerUserPerCollection
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50)
AS
BEGIN
    SELECT * FROM PASS_JSON_FILE_TAB
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID;
END;
GO

CREATE OR ALTER PROCEDURE spSaveJsonFile
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
    @FILE_ID VARCHAR(50),
    @FILE_NAME VARCHAR(500),
    @FILE_CONTENT VARCHAR(MAX),
    @CREATED_BY VARCHAR(50)
AS
BEGIN
    INSERT INTO dbo.PASS_JSON_FILE_TAB(COMPANY_ID, USER_ID, COLLECTION_ID, FILE_ID, FILE_NAME, FILE_CONTENT, CREATED_BY, CREATED_DATE)
    VALUES(@COMPANY_ID, @USER_ID, @COLLECTION_ID, @FILE_ID, @FILE_NAME, @FILE_CONTENT, @CREATED_BY, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE OR ALTER PROCEDURE spEditJsonFile
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
    @FILE_ID VARCHAR(50),
    @FILE_NAME VARCHAR(500),
    @FILE_CONTENT VARCHAR(MAX),
    @MODIFIED_BY VARCHAR(50)
AS
BEGIN
    UPDATE dbo.PASS_JSON_FILE_TAB
    SET FILE_NAME = @FILE_NAME, FILE_CONTENT = @FILE_CONTENT, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE()
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID
    AND FILE_ID = @FILE_ID;
END;
GO

CREATE OR ALTER PROCEDURE spDeleteJsonFile
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
    @FILE_ID VARCHAR(50)
AS
BEGIN
    DELETE FROM dbo.PASS_JSON_FILE_TAB 
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID
    AND FILE_ID = @FILE_ID;
END;
GO

CREATE OR ALTER PROCEDURE spShareJsonFile
    @COMPANY_ID VARCHAR(6),
    @USER_ID VARCHAR(50),
    @COLLECTION_ID VARCHAR(50),
	@FILE_ID VARCHAR(50),
    @COLLECTION_NAME VARCHAR(500),
    @MODIFIED_BY VARCHAR(50)
AS
BEGIN
    UPDATE dbo.PASS_JSON_FILE_TAB
    SET USER_ID = @USER_ID, COLLECTION_ID = @COLLECTION_ID, MODIFIED_BY = @MODIFIED_BY, MODIFIED_DATE = GETDATE()
    WHERE COMPANY_ID = @COMPANY_ID
    AND USER_ID = @USER_ID
    AND COLLECTION_ID = @COLLECTION_ID
    AND FILE_ID = @FILE_ID;
END;
GO