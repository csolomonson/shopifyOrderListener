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
