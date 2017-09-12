USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[Acc_AppLog_MonitorLogUpd]    Script Date: 9/12/2017 6:12:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jason.Ly
-- Create date: 2013-08-08
-- Description:	Update monitored status of a log
-- =============================================
ALTER PROCEDURE [dbo].[Acc_AppLog_MonitorLogUpd]
	@LogIds				VARCHAR(MAX), 
	@MonitoringUserId	INT,
	@IsMonitored		BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AppLog_Log]
	SET [IsMonitored] = @IsMonitored,
	    [MonitoringUserId] = @MonitoringUserId
	WHERE [LogId] IN (SELECT val FROM dbo.StrSplit(@LogIds,';'))
END

/****** Object:  StoredProcedure [dbo].[IS_SelLogPadding_All]    Script Date: 9/6/2013 9:15:20 AM ******/
SET ANSI_NULLS ON



GO
GRANT	VIEW DEFINITION	ON	Acc_AppLog_MonitorLogUpd	TO	vnteam
GRANT	EXECUTE	ON	Acc_AppLog_MonitorLogUpd	TO	bodbAdminNet
GO
