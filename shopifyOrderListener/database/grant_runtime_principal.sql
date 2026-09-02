/*
    grant_runtime_principal.sql

    Grants the application's SQL login the rights it needs on M1_ME.SalesOrders.

    Run AFTER the schema exists (apply_all_migrations.sql), as a login that can
    alter users -- sysadmin or db_owner. The app's own account cannot run this,
    and cannot run the migrations either: creating the schema and tables needs
    rights this deliberately does not grant.

    Set @Login below to whichever account the app connects as. That is the value
    of SALES_ORDER_DB_USERNAME, which may be a dedicated login or one shared with
    another app (for example cost_app_access, also used by the cost calculator).

    Plain T-SQL -- no SQLCMD mode needed.
*/

USE [M1_ME];
GO

DECLARE @Login sysname = N'cost_app_access';   -- <<< SALES_ORDER_DB_USERNAME
DECLARE @sql nvarchar(max);

IF SUSER_ID(@Login) IS NULL
BEGIN
    RAISERROR(N'Server login %s does not exist. Create it before granting.', 16, 1, @Login);
    RETURN;
END;

IF SCHEMA_ID(N'SalesOrders') IS NULL
BEGIN
    RAISERROR(N'The SalesOrders schema does not exist yet. Run apply_all_migrations.sql first.', 16, 1);
    RETURN;
END;

-- Restoring a database replaces its users with the ones in the backup, carrying
-- their old SIDs. A user of the right name can therefore exist and still
-- authorise nothing, and CREATE USER fails against it. Re-map instead.
IF DATABASE_PRINCIPAL_ID(@Login) IS NULL
BEGIN
    SET @sql = N'CREATE USER ' + QUOTENAME(@Login) + N' FOR LOGIN ' + QUOTENAME(@Login);
    EXEC sp_executesql @sql;
END
ELSE
BEGIN
    SET @sql = N'ALTER USER ' + QUOTENAME(@Login) + N' WITH LOGIN = ' + QUOTENAME(@Login);
    EXEC sp_executesql @sql;
END;

-- The app only ever reads and writes its own schema. It reaches M1 itself over
-- the Public API (HTTP), never through this connection, so it needs nothing on
-- the ERP tables for this app's sake.
SET @sql = N'GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::SalesOrders TO ' + QUOTENAME(@Login);
EXEC sp_executesql @sql;

-- A schema-level DENY CONTROL includes every subordinate permission and would
-- override the four DML grants above. Remove a deny left by an older version of
-- this script, then deny only the schema-management permissions themselves.
SET @sql = N'REVOKE CONTROL ON SCHEMA::SalesOrders TO ' + QUOTENAME(@Login);
EXEC sp_executesql @sql;

SET @sql = N'DENY ALTER, TAKE OWNERSHIP ON SCHEMA::SalesOrders TO ' + QUOTENAME(@Login);
EXEC sp_executesql @sql;
GO

-- Verify what the account ended up with.
DECLARE @Login sysname = N'cost_app_access';   -- <<< keep in sync with above
SELECT
    @Login                                        AS LoginName,
    USER_NAME(DATABASE_PRINCIPAL_ID(@Login))      AS DatabaseUser,
    CASE WHEN EXISTS (SELECT 1
                      FROM sys.database_principals dp
                      JOIN sys.server_principals  sp ON dp.sid = sp.sid
                      WHERE dp.name = @Login)
         THEN 'mapped' ELSE 'ORPHANED' END        AS SidStatus;

SELECT dpr.permission_name, dpr.state_desc
FROM sys.database_permissions AS dpr
JOIN sys.database_principals  AS dp ON dpr.grantee_principal_id = dp.principal_id
WHERE dp.name = N'cost_app_access'
  AND dpr.class_desc = 'SCHEMA'
  AND dpr.major_id = SCHEMA_ID(N'SalesOrders')
ORDER BY dpr.state_desc, dpr.permission_name;
GO
