USE [bodb_CustomerLog]
GO
/****** Object:  StoredProcedure [dbo].[IS_SelLogPadding_All]    Script Date: 9/12/2017 5:28:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[IS_SelLogPadding_All]
	@Categoryid		INT,
	@Severity		NVARCHAR(32),
	@MachineId		INT			= 0,
	@IsProduction	BIT			= NULL,
	@IsMonitored	BIT			= NULL,
	@ToGMT			SMALLINT	= 7,
	@From			DATETIME,
	@To				DATETIME,
	@Page			INT,
	@Size			INT,
	@TotalRows		INT			= 0		OUTPUT
AS

/**  
	- Created: 20130909@Edward
	- Task : Get log
	- DB	: bodb_CustomerLog
	- Revisions:
		- 20150929@Kos: modify to enhance performance
		- 20151009@Kos: add machine ip
	- Param's Explanation	: 
 */

BEGIN
	SET NOCOUNT ON

	CREATE TABLE #TempTable (Id BIGINT, Num INT, CategoryID INT)
	CREATE clustered INDEX IDX_CategoryID on #TempTable (CategoryID)

	DECLARE @tsql NVARCHAR(MAX),@ParmDefinition NVARCHAR(500);  
	SET @tsql = N'INSERT INTO #TempTable(Id, Num, CategoryID)
				SELECT MAX(al.LogID)
					,COUNT(*)
					,acl.CategoryID
				FROM AppLog_Machine AS m WITH(NOLOCK)
					INNER JOIN AppLog_Log AS al WITH (NOLOCK) ON al.MachineId = m.MachineId
					INNER JOIN AppLog_CategoryLog AS acl WITH (NOLOCK) ON al.LogID = acl.LogID
				WHERE al.Timestamp BETWEEN @From AND @To '
	IF @Categoryid <> 0
		SET @tsql = @tsql + N' AND acl.CategoryID = @Categoryid'
	IF @Severity <> 'ALL'
		SET @tsql = @tsql + N' AND al.Severity = @Severity'
	IF @MachineId <> 0 
		SET @tsql = @tsql + N' AND al.MachineId = @MachineId'
	IF @IsMonitored IS NOT NULL 
		SET @tsql = @tsql + N' AND al.IsMonitored = @IsMonitored'
	IF @IsProduction IS NOT NULL
		SET @tsql = @tsql + N' AND m.IsProduction = @IsProduction'

	SET @tsql = @tsql + N' GROUP BY 
							al.Message,
							al.Severity,
							al.MachineId,
							acl.CategoryID,
							DATEADD(HOUR, DATEDIFF(HOUR, 0, al.Timestamp), 0)
							OPTION (recompile) '
 
	SET @ParmDefinition =  N'@From DATETIME, @To DATETIME,@Categoryid INT,@Severity NVARCHAR(32),@MachineId INT,@IsMonitored BIT,@IsProduction BIT';  
	EXECUTE sp_executesql @tsql, @ParmDefinition,  
						@From = @From,
						@To = @To,
						@Categoryid = @Categoryid,
						@Severity = @Severity,
						@MachineId = @MachineId,
						@IsMonitored = @IsMonitored,
						@IsProduction = @IsProduction;  
 
	SELECT @TotalRows = COUNT(*)
	FROM #TempTable 	

	SELECT TOP(@Size) * 
	FROM
	(
		SELECT 
			DATEADD(Hour, @ToGMT, a.Timestamp) AS Timestamp
			,a.EventID As C_EventID
			,a.LogID
			,a.Priority 
			,a.ProcessID 
			,a.ProcessName
			,a.Win32ThreadId
			,a.ThreadName
			,a.EventID
			,a.Severity
			,a.Title
			,a.AppDomainName
			,a.Message
			,a.FormattedMessage
			,ci.username AS MonitoringUser
			,a.IsMonitored
			,temp.Num AS NumMessage
			,c.CategoryID
			,c.CategoryName
			,m.MachineId
			,m.MachineName
			,m.MachineIP
			,m.IsProduction
			,ROW_NUMBER() OVER (ORDER BY a.Timestamp DESC) AS num
		FROM AppLog_Category AS c WITH (NOLOCK)
			INNER JOIN #TempTable AS temp ON  c.CategoryID = temp.CategoryID
			INNER JOIN AppLog_Log AS a WITH (NOLOCK) ON  a.LogID = temp.Id			 
			INNER JOIN AppLog_Machine AS m WITH (NOLOCK) ON a.MachineId = m.MachineId
			LEFT JOIN bodb02.dbo.CustInfo AS ci WITH (NOLOCK) ON ci.custid = a.MonitoringUserID
	) AS alias
	WHERE num > (@Page - 1) * @Size

	DROP TABLE #TempTable
END	 



GO
GRANT	VIEW DEFINITION	ON	IS_SelLogPadding_All	TO	vnteam
GRANT	EXECUTE	ON	IS_SelLogPadding_All	TO	bodbAdminNet
GO
