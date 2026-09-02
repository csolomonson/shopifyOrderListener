/*
    apply_all_migrations.sql  -- GENERATED, do not edit by hand.

    Migrations 001-004 concatenated for running in SSMS in one pass, e.g. after
    M1_ME has been restored from a backup (the SalesOrders schema lives inside
    M1_ME, so a restore removes it along with the ERP data).

    Every step is idempotent -- IF SCHEMA_ID(...) IS NULL / IF OBJECT_ID(...) IS
    NULL / IF COL_LENGTH(...) IS NULL -- so running this against a database that
    already has some or all of the schema is safe, including re-running it after
    a partial failure.

    Open in SSMS against the correct server and press F5. Each file carries its
    own USE [M1_ME] and its own transaction.

    Verify afterwards with the query at the bottom of this file.
*/

--------------------------------------------------------------------------------
-- 001_create_salesorders_schema.sql
--------------------------------------------------------------------------------
USE [M1_ME];
GO

-- The schema must be created in a batch of its own. SQL Server binds schema
-- names while COMPILING a batch, and deferred name resolution does not cover
-- them, so every "CREATE TABLE SalesOrders.*" below fails to compile with
-- "The specified schema name ... does not exist" if the CREATE SCHEMA shares
-- their batch -- the EXEC would only run afterwards, at execution time.
IF SCHEMA_ID(N'SalesOrders') IS NULL
    EXEC(N'CREATE SCHEMA [SalesOrders] AUTHORIZATION [dbo]');
GO

SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;
GO
BEGIN TRANSACTION;

IF OBJECT_ID(N'SalesOrders.SchemaVersions', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.SchemaVersions (
        VersionNumber int NOT NULL PRIMARY KEY,
        AppliedAt datetime2(3) NOT NULL CONSTRAINT DF_SalesOrdersSchemaVersions_AppliedAt DEFAULT SYSUTCDATETIME(),
        Description nvarchar(250) NOT NULL
    );
END;

IF OBJECT_ID(N'SalesOrders.Orders', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.Orders (
        OrderID uniqueidentifier NOT NULL PRIMARY KEY,
        ShopifyOrderID nvarchar(100) NOT NULL,
        LegacyOrderID nvarchar(40) NULL,
        OrderName nvarchar(40) NOT NULL,
        ShopifyUpdatedAt datetimeoffset(3) NOT NULL,
        SourceHash char(64) NOT NULL,
        CommittedHash char(64) NULL,
        State nvarchar(40) NOT NULL,
        Severity nvarchar(20) NOT NULL,
        ActionTitle nvarchar(250) NOT NULL,
        ActionDetail nvarchar(1000) NOT NULL,
        BlocksCommit bit NOT NULL,
        MatchRequiresReview bit NOT NULL CONSTRAINT DF_SalesOrdersOrders_MatchReview DEFAULT 0,
        M1OrganizationID nvarchar(30) NULL,
        M1LocationID nvarchar(30) NULL,
        M1ContactID nvarchar(30) NULL,
        M1SalesOrderID nvarchar(30) NULL,
        M1QuantityShipped decimal(18,5) NOT NULL CONSTRAINT DF_SalesOrdersOrders_QtyShipped DEFAULT 0,
        NormalizedJson nvarchar(max) NOT NULL,
        RawJson nvarchar(max) NOT NULL,
        CommittedAt datetime2(3) NULL,
        CreatedAt datetime2(3) NOT NULL,
        UpdatedAt datetime2(3) NOT NULL,
        RowVersion rowversion NOT NULL,
        CONSTRAINT UQ_SalesOrdersOrders_ShopifyOrder UNIQUE (ShopifyOrderID),
        CONSTRAINT CK_SalesOrdersOrders_NormalizedJson CHECK (ISJSON(NormalizedJson)=1),
        CONSTRAINT CK_SalesOrdersOrders_RawJson CHECK (ISJSON(RawJson)=1)
    );
    CREATE INDEX IX_SalesOrdersOrders_Queue ON SalesOrders.Orders (State, ShopifyUpdatedAt DESC);
    CREATE INDEX IX_SalesOrdersOrders_M1PO ON SalesOrders.Orders (M1SalesOrderID) WHERE M1SalesOrderID IS NOT NULL;
END;

IF OBJECT_ID(N'SalesOrders.OrderLines', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.OrderLines (
        OrderLineID uniqueidentifier NOT NULL PRIMARY KEY,
        OrderID uniqueidentifier NOT NULL,
        ShopifyLineID nvarchar(100) NOT NULL,
        LineNumber int NOT NULL,
        SKU nvarchar(80) NOT NULL,
        Description nvarchar(500) NOT NULL,
        Variant nvarchar(250) NOT NULL,
        OriginalQuantity int NOT NULL,
        CurrentQuantity int NOT NULL,
        UnitPrice decimal(19,4) NOT NULL,
        LineTotal decimal(19,4) NOT NULL,
        CONSTRAINT FK_SalesOrdersOrderLines_Order FOREIGN KEY (OrderID) REFERENCES SalesOrders.Orders(OrderID),
        CONSTRAINT UQ_SalesOrdersOrderLines_Shopify UNIQUE (OrderID,ShopifyLineID)
    );
END;

IF OBJECT_ID(N'SalesOrders.Refunds', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.Refunds (
        RefundID uniqueidentifier NOT NULL PRIMARY KEY,
        OrderID uniqueidentifier NOT NULL,
        ShopifyRefundID nvarchar(100) NOT NULL,
        ShopifyCreatedAt datetimeoffset(3) NULL,
        ShopifyUpdatedAt datetimeoffset(3) NULL,
        Amount decimal(19,4) NOT NULL,
        Note nvarchar(1000) NOT NULL,
        PayloadJson nvarchar(max) NOT NULL,
        CONSTRAINT FK_SalesOrdersRefunds_Order FOREIGN KEY (OrderID) REFERENCES SalesOrders.Orders(OrderID),
        CONSTRAINT UQ_SalesOrdersRefunds_Shopify UNIQUE (OrderID,ShopifyRefundID),
        CONSTRAINT CK_SalesOrdersRefunds_PayloadJson CHECK (ISJSON(PayloadJson)=1)
    );
END;

IF OBJECT_ID(N'SalesOrders.CustomerCandidates', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.CustomerCandidates (
        CandidateID uniqueidentifier NOT NULL PRIMARY KEY,
        OrderID uniqueidentifier NOT NULL,
        M1OrganizationID nvarchar(30) NOT NULL,
        M1LocationID nvarchar(30) NULL,
        M1ContactID nvarchar(30) NULL,
        Score int NOT NULL,
        MatchReasonsJson nvarchar(max) NOT NULL,
        Selected bit NOT NULL CONSTRAINT DF_SalesOrdersCandidates_Selected DEFAULT 0,
        CreatedAt datetime2(3) NOT NULL CONSTRAINT DF_SalesOrdersCandidates_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_SalesOrdersCandidates_Order FOREIGN KEY (OrderID) REFERENCES SalesOrders.Orders(OrderID),
        CONSTRAINT CK_SalesOrdersCandidates_Reasons CHECK (ISJSON(MatchReasonsJson)=1)
    );
    CREATE INDEX IX_SalesOrdersCandidates_OrderScore ON SalesOrders.CustomerCandidates (OrderID,Score DESC);
END;

IF OBJECT_ID(N'SalesOrders.Settings', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.Settings (
        SettingKey nvarchar(100) NOT NULL PRIMARY KEY,
        ValueJson nvarchar(max) NOT NULL,
        UpdatedAt datetime2(3) NOT NULL,
        UpdatedBy nvarchar(120) NOT NULL,
        CONSTRAINT CK_SalesOrdersSettings_ValueJson CHECK (ISJSON(ValueJson)=1)
    );
END;

IF OBJECT_ID(N'SalesOrders.SyncRuns', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.SyncRuns (
        SyncRunID uniqueidentifier NOT NULL PRIMARY KEY,
        StartedAt datetime2(3) NOT NULL,
        FinishedAt datetime2(3) NULL,
        Status nvarchar(20) NOT NULL,
        OrdersSeen int NOT NULL,
        OrdersChanged int NOT NULL,
        ErrorMessage nvarchar(2000) NOT NULL CONSTRAINT DF_SalesOrdersSyncRuns_Error DEFAULT N''
    );
END;

IF OBJECT_ID(N'SalesOrders.AuditEvents', N'U') IS NULL
BEGIN
    CREATE TABLE SalesOrders.AuditEvents (
        EventID uniqueidentifier NOT NULL PRIMARY KEY,
        OrderID uniqueidentifier NULL,
        EventType nvarchar(80) NOT NULL,
        Actor nvarchar(120) NOT NULL,
        DetailJson nvarchar(max) NOT NULL,
        CreatedAt datetime2(3) NOT NULL,
        CONSTRAINT FK_SalesOrdersAudit_Order FOREIGN KEY (OrderID) REFERENCES SalesOrders.Orders(OrderID),
        CONSTRAINT CK_SalesOrdersAudit_DetailJson CHECK (ISJSON(DetailJson)=1)
    );
    CREATE INDEX IX_SalesOrdersAudit_OrderCreated ON SalesOrders.AuditEvents (OrderID,CreatedAt DESC);
END;

IF NOT EXISTS (SELECT 1 FROM SalesOrders.SchemaVersions WHERE VersionNumber=1)
    INSERT SalesOrders.SchemaVersions (VersionNumber,Description) VALUES (1,N'Initial sales-order staging schema');

COMMIT TRANSACTION;
GO

--------------------------------------------------------------------------------
-- 002_matching_and_line_overrides.sql
--------------------------------------------------------------------------------
USE [M1_ME];
GO
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
IF COL_LENGTH(N'SalesOrders.Orders', N'M1BillingLocationID') IS NULL ALTER TABLE SalesOrders.Orders ADD M1BillingLocationID nvarchar(30) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'M1BillingContactID') IS NULL ALTER TABLE SalesOrders.Orders ADD M1BillingContactID nvarchar(30) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'MatchValidationJson') IS NULL ALTER TABLE SalesOrders.Orders ADD MatchValidationJson nvarchar(max) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'AddressOverrideJson') IS NULL ALTER TABLE SalesOrders.Orders ADD AddressOverrideJson nvarchar(max) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'LineOverridesJson') IS NULL ALTER TABLE SalesOrders.Orders ADD LineOverridesJson nvarchar(max) NULL;
IF NOT EXISTS (SELECT 1 FROM SalesOrders.SchemaVersions WHERE VersionNumber=2)
  INSERT SalesOrders.SchemaVersions (VersionNumber,Description) VALUES (2,N'Separate billing selections, address validation, and line overrides');
COMMIT TRANSACTION;
GO

--------------------------------------------------------------------------------
-- 003_precomputed_customer_resolution.sql
--------------------------------------------------------------------------------
USE [M1_ME];
GO
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;

IF COL_LENGTH(N'SalesOrders.Orders', N'CustomerResolutionJson') IS NULL
  ALTER TABLE SalesOrders.Orders ADD CustomerResolutionJson nvarchar(max) NULL;

IF COL_LENGTH(N'SalesOrders.Orders', N'CustomerResolutionAt') IS NULL
  ALTER TABLE SalesOrders.Orders ADD CustomerResolutionAt datetime2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM SalesOrders.SchemaVersions WHERE VersionNumber=3)
  INSERT SalesOrders.SchemaVersions (VersionNumber,Description)
  VALUES (3,N'Precomputed customer-resolution results for fast order review');

COMMIT TRANSACTION;
GO

--------------------------------------------------------------------------------
-- 004_precomputed_m1_preview.sql
--------------------------------------------------------------------------------
USE [M1_ME];
GO
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;

-- The commit preview is rebuilt from live M1 reads (customer validation, the
-- customer-PO lookup and the home currency). Storing the result alongside the
-- precomputed customer resolution keeps opening an order a pure SQL read, so a
-- cold M1 directory cache after a restart is never on the reviewer's path.

IF COL_LENGTH(N'SalesOrders.Orders', N'M1PreviewJson') IS NULL
  ALTER TABLE SalesOrders.Orders ADD M1PreviewJson nvarchar(max) NULL;

-- Hash of every input the preview was built from. A mismatch means the stored
-- preview is stale and must be rebuilt before it is served.
IF COL_LENGTH(N'SalesOrders.Orders', N'M1PreviewFingerprint') IS NULL
  ALTER TABLE SalesOrders.Orders ADD M1PreviewFingerprint char(64) NULL;

IF COL_LENGTH(N'SalesOrders.Orders', N'M1PreviewAt') IS NULL
  ALTER TABLE SalesOrders.Orders ADD M1PreviewAt datetime2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM SalesOrders.SchemaVersions WHERE VersionNumber=4)
  INSERT SalesOrders.SchemaVersions (VersionNumber,Description)
  VALUES (4,N'Precomputed M1 commit preview for instant order review');

COMMIT TRANSACTION;
GO

--------------------------------------------------------------------------------
-- Verification. Expect 8 tables and versions 1,2,3,4.
--------------------------------------------------------------------------------
USE [M1_ME];
GO

SELECT t.name AS TableName
FROM sys.tables AS t
WHERE SCHEMA_NAME(t.schema_id) = N'SalesOrders'
ORDER BY t.name;

SELECT VersionNumber, AppliedAt, Description
FROM SalesOrders.SchemaVersions
ORDER BY VersionNumber;
GO
