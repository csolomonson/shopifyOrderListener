/*
    repair_m1_orphaned_users.sql

    Fixes the login failures that follow restoring M1_ME from a backup.

    WHY THE "VERSION MISMATCH" MESSAGE IS MISLEADING
    ------------------------------------------------
    M1.Core ServerManager.GetDatasetProperties connects to master and runs a
    cross-database read:

        select xadUniqueID,xadDescription,xadVersion from M1_ME.dbo.DatasetProperties

    If that read fails it retries the legacy column names, and if that also fails
    it falls back to DoesDatabaseExist and returns null when the login cannot see
    the database. A permission failure therefore returns null, never a version.

    M1DatabaseCollection.CheckVersion starts with `bool result = false` and only
    sets it true inside `if (datasetProperties != null)`. On null it returns false
    without ever comparing a version, and the caller throws the fixed string:

        "The version of database M1_ME does not match the current version of the
         application."

    So this message means "could not read DatasetProperties", which after a restore
    is almost always a permissions problem, not a schema version problem.

    WHAT A RESTORE ACTUALLY BREAKS
    ------------------------------
    Server LOGINS live in master and survive. DATABASE USERS live in the database
    and are replaced by the ones in the backup, carrying the SIDs from whenever it
    was taken. Any user whose SID no longer matches its login is orphaned: it
    exists, so CREATE USER fails, but it authorises nothing.

    Run as sysadmin. STEP 1 is read-only -- start there.
*/

--------------------------------------------------------------------------------
-- STEP 1 (read-only): which users in M1_ME did the restore orphan?
--------------------------------------------------------------------------------
USE [M1_ME];
GO

SELECT
    dp.name                AS DatabaseUser,
    dp.type_desc           AS UserType,
    CASE WHEN sp.sid IS NULL THEN 'ORPHANED - no matching server login'
         ELSE 'OK - mapped to login ' + sp.name END AS Status
FROM sys.database_principals AS dp
LEFT JOIN sys.server_principals AS sp
       ON dp.sid = sp.sid
WHERE dp.type IN ('S', 'U', 'G')          -- SQL user, Windows user, Windows group
  AND dp.principal_id > 4                 -- skip dbo/guest/sys/INFORMATION_SCHEMA
  AND dp.authentication_type <> 0         -- skip users without logins by design
ORDER BY Status DESC, dp.name;
GO

-- Which of those names still exist as server logins (so they can be re-mapped)?
SELECT sp.name AS ServerLogin, sp.type_desc, sp.is_disabled
FROM sys.server_principals AS sp
WHERE sp.type IN ('S', 'U', 'G')
  AND sp.name NOT LIKE '##%'
  AND sp.name NOT LIKE 'NT %'
ORDER BY sp.name;
GO

--------------------------------------------------------------------------------
-- STEP 2: re-map each orphan. Substitute the real login name; repeat per account.
-- This is the fix for the M1 API service account, which is whichever login M1's
-- API is configured to connect with -- STEP 1 will show it as ORPHANED.
--------------------------------------------------------------------------------
-- ALTER USER [<login_name>] WITH LOGIN = [<login_name>];
-- GO

--------------------------------------------------------------------------------
-- STEP 3 (read-only): prove the fix from the API account's point of view.
-- Run this while impersonating the account M1's API uses. If it returns a row,
-- CheckVersion will succeed and the "version mismatch" error goes away.
--------------------------------------------------------------------------------
-- EXECUTE AS LOGIN = '<login_name>';
--     SELECT xadUniqueID, xadDescription, xadVersion FROM M1_ME.dbo.DatasetProperties;
-- REVERT;
-- GO

--------------------------------------------------------------------------------
-- Notes
--
-- * GetDatasetProperties connects to master first, so the account also needs
--   CONNECT on master (the public role normally provides this).
-- * If STEP 1 shows the account as OK but STEP 3 still fails, the user exists and
--   is mapped but lacks SELECT on the table:
--       GRANT SELECT ON OBJECT::dbo.DatasetProperties TO [<login_name>];
-- * A genuine version mismatch is still possible. STEP 3 distinguishes them: if it
--   returns a row and M1 still refuses, compare that xadVersion against the
--   installed M1 build -- then, and only then, is a database upgrade the answer.
--------------------------------------------------------------------------------
