USE [Discovery2LocationServices]
GO
/****** Object:  Table [dbo].[CruisedDiscovery]    Script Date: 6/9/2017 12:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CruisedDiscovery](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Day] [nvarchar](50) NULL,
	[Date] [datetime] NOT NULL,
	[CruiseName] [nvarchar](50) NULL,
	[TUIDiscovery] [nvarchar](50) NULL,
	[Arrival] [nvarchar](50) NULL,
	[Departure] [nvarchar](50) NULL,
	[TimeDiff] [int] NULL
) ON [PRIMARY]

GO
GO
/****** Object:  Table [dbo].[DataFiles]    Script Date: 6/9/2017 12:55:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[DateOfFile] [datetime] NOT NULL DEFAULT ('1900-01-01T00:00:00.000'),
	[IsInSftp] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.DataFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterestLocations]    Script Date: 6/9/2017 12:55:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterestLocations](
	[AreaOfInterestId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.InterestLocations] PRIMARY KEY CLUSTERED 
(
	[AreaOfInterestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationIndicators]    Script Date: 6/9/2017 12:55:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationIndicators](
	[LoctionIndicatorId] [int] IDENTITY(1,1) NOT NULL,
	[AreaOfInterestId] [int] NOT NULL,
	[LoctionIndicator] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.LocationIndicators] PRIMARY KEY CLUSTERED 
(
	[LoctionIndicatorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NeighBourAreas]    Script Date: 6/9/2017 12:55:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NeighBourAreas](
	[NeighbourAreaId] [int] IDENTITY(1,1) NOT NULL,
	[AreaCode] [nvarchar](max) NULL,
	[AreaId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.NeighBourAreas] PRIMARY KEY CLUSTERED 
(
	[NeighbourAreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Parameters]    Script Date: 6/9/2017 12:55:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Parameters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LocationIndicators]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LocationIndicators_dbo.InterestLocations_AreaOfInterestId] FOREIGN KEY([AreaOfInterestId])
REFERENCES [dbo].[InterestLocations] ([AreaOfInterestId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LocationIndicators] CHECK CONSTRAINT [FK_dbo.LocationIndicators_dbo.InterestLocations_AreaOfInterestId]
GO
ALTER TABLE [dbo].[NeighBourAreas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.NeighBourAreas_dbo.InterestLocations_AreaId] FOREIGN KEY([AreaId])
REFERENCES [dbo].[InterestLocations] ([AreaOfInterestId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NeighBourAreas] CHECK CONSTRAINT [FK_dbo.NeighBourAreas_dbo.InterestLocations_AreaId]
GO

/*Insert the default data*/
SET IDENTITY_INSERT [dbo].[Parameters] ON 
INSERT [dbo].[Parameters] ([Id], [Name], [Value]) VALUES (1, N'DeltaTime', N'30')
INSERT [dbo].[Parameters] ([Id], [Name], [Value]) VALUES (2, N'RemotePath', N'/home/airloc8user/sftp/discovery2/')
INSERT [dbo].[Parameters] ([Id], [Name], [Value]) VALUES (3, N'WindowConvDwellTime', N'40')
INSERT [dbo].[Parameters] ([Id], [Name], [Value]) VALUES (4, N'WindowConvLengthTime', N'10')
SET IDENTITY_INSERT [dbo].[Parameters] OFF

GO
SET IDENTITY_INSERT [dbo].[InterestLocations] ON
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (1, N'Live Casino')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (2, N'Atrium')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (3, N'LiveRoom')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (4, N'47DegRestaurant')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (5, N'Venue Lounges')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (6, N'Shops')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (7, N'Destination Services')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (8, N'Photo Gallery')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (9, N'Future Cruise & Internet')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (10, N'Deck 9 Bar')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (11, N'Movies by Moonlight')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (12, N'The Glass House')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (13, N'Snack Shop')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (14, N'Spa')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (15, N'Deck 10 bar')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (16, N'Bar Eleven')
INSERT [dbo].[InterestLocations] ([AreaOfInterestId], [Name]) VALUES (17, N'Deck 11 restaurants')
SET IDENTITY_INSERT [dbo].[InterestLocations] OFF

SET IDENTITY_INSERT [dbo].[LocationIndicators] ON
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (1, 1, N'POO0004LIVERMCAS')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (2, 2, N'POO0004ATRIUM')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (3, 3, N'POO0004LIVECAS')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (4, 4, N'POO000447DEGP')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (5, 4, N'POO000447DEGSB')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (6, 4, N'PQQ00547DEGGALL')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (7, 4, N'PQQ00547DEG')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (8, 5, N'POO0005VEN2NDLNG')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (9, 5, N'POO0005VENMIDLNG')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (10, 5, N'POO0005VENCONLNG')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (11, 5, N'POO0005VENENT')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (12, 6, N'BFFWSHOP')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (13, 6, N'BPERFSHOP')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (14, 6, N'BSOUVSHOP')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (15, 6, N'BGJSHOP')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (16, 6, N'BLSSHOP')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (17, 7, N'DESTSRV')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (18, 8, N'PHOTOGAL')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (19, 9, N'POO0008D8FUTCRSINT')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (20, 10, N'PVV0009DBAR9')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (21, 11, N'POO0009MOV9')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (22, 12, N'PQQ0009GLASS9')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (23, 13, N'PQQ0009SNACKSH9')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (24, 14, N'POO0009SPA9')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (25, 15, N'PVV0010DBAR10')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (26, 16, N'PVV0011BARELEV')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (27, 17, N'PVV0011KORALA')
INSERT [dbo].[LocationIndicators] ([LoctionIndicatorId], [AreaOfInterestId], [LoctionIndicator]) VALUES (28, 17, N'PVV0011STEAKSURF')
SET IDENTITY_INSERT [dbo].[LocationIndicators] OFF
Go
/*Start of the Stored procedures*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[sp_DelteLocationsDataAsPerDate](
@Id int
)
AS
BEGIN
 Set NOCOUNT ON 
 DECLARE @intErrorCode INT,@Counter int,@row_number int,@AreaId int,@AreaName varchar(50),@tblName Varchar(50),@Dsql nvarchar(Max),@Date varchar(10),@fileName varchar(1000)
BEGIN TRY
  BEGIN TRAN
 select @Date=Cast(DateOfFile as date),@FileName=FileName from dbo.DataFiles where Id=@id
 select @tblName='Data_'+Format(Convert(DateTime,@date),'yyMM')
 --select @Date,@tblName,@FileName
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tblName)
	BEGIN
	   set @Dsql ='Delete from'+' '+ @tblName+' '+ 'where Cast(myDate as date)='''+@Date+''''
       SELECT @intErrorCode = @@ERROR
       exec(@Dsql)
	 END
    Delete from DataFiles where Id=@id
    COMMIT 
END TRY
BEGIN CATCH
		SELECT ERROR_MESSAGE()
		IF @@TRANCOUNT>0
			ROLLBACK
END CATCH
END
GO

/****** Object:  StoredProcedure [dbo].[sp_GetNumberOfVisit]    Script Date: 12/13/2016 11:50:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[sp_GetNumberOfVisit](
    /* Input Parameters */
	@ShipStartDateTime datetime,
    @ShipLastDateTime datetime,
    @Area int,
	@DwellTime int
	)
	AS
	Begin
	Set NoCount ON
    /* Variable Declaration */
	DECLARE @MACAddress char(17), @visitTime Time, @startTime Time, @timeDiff int,@EventTime varchar(10),@lasttime varchar(10), @Nexttime Time,@diffStartLast int,@AreaIdentifier varchar(200),@noOfConnections int,@noOfReturns int,@averageDwellTime decimal(10,2), @totalDwellTime int, @dwellTimeCounter int,@visitBounseCounter2Min int,@visitBounseCounter5Min int,@visitBounsePercentage2Min int,@visitBounsePercentage5Min int,@resultHourVisit varchar,@resultHourPassersBy varchar,@maxVisitHour varchar(20),@maxPassersByHour varchar(20),@CruiseName varchar(100),@DeltaTime int,@noofVisitBusiest int,@noofVisitPassersBy int,@TimeZoneDiff int,@UTCStartDateTimestring varchar(30),@UTCLastDateTimestring varchar(30),@countMac int,@countVisitTime int,@i int,@DSql nvarchar(MAX),@j int
	Declare @AvVisitFreq decimal(10,2),@AreaName varchar(100),@tblName varchar(100),@dynamicSQL1 nvarchar(Max),@dynamicSQL nvarchar(MAX),@dynamicSQL2 nvarchar(MAX)
	DECLARE @noOfVisits int, @totalNoOfRepeatVisits int, @noOfPassersBy int, @noOfNewVisits int,@EndSequenceDateTime DateTime,@startDateTime DateTime,@lastDateTime DateTime,@visitDateTime DateTime,@EndCalcDateTime DateTime,@tblNameNext varchar(20),@NeighBourCode Varchar(100),@entryDateTime DateTime,@entryDateTimeNeighBour DateTime,@WindowConverstionCalcCounter int,@entryStartDateTimeNeighBour DateTime,@entryEndDateTimeNeighBour DateTime,@entryLastDateTimeNeighBour datetime
	Declare @WindowConvDwellTime int,@WindowConvLengthTime int
    Declare @UtcStartDateTimeNextString varchar(100),@UtcLastDateTimeNextString varchar(100)
	set @noOfNewVisits = 0 --for all MACAddresses
	set @totalNoOfRepeatVisits = 0
	set @noOfPassersBy = 0
	set @AvVisitFreq=0
	set @dwellTimeCounter=0
	set @totalDwellTime=0
	set @noOfConnections=0
	set @visitBounseCounter2Min=0--this is for number of VisitsBounce more than 2 min
	set @visitBounseCounter5Min=0--this is for number od VisitBounce more than 5 min
	set @tblName='Area'+Convert(varchar(20),@Area)
	set @WindowConverstionCalcCounter=0
	
	CREATE table #TempVisitHour
     (
		VisitHour int
	 )

	 CREATE table #TempPassersByHour
	 (
	   VisitHour int
	 )

	 CREATE table #tmpCalcArea
	  (
	   Id int Identity(1,1),
	   MacAddress varchar(20),
	   myDate Date,
	   myTime time,
	   myDateTime datetime,
	   AreaId varchar(20)
	  )

	  CREATE table #tmpCalcNeighArea
	  (
	   Id int Identity(1,1),
	   MacAddress varchar(20),
	   myDateTime datetime,
	   AreaId varchar(100)
	  )

	  CREATE table #MacAddress
      (
	   Id int identity(1,1),
	   MacAddress varchar(20)
	  )


    --Convert UTCStartDateTime,UTCLastDateTime to SheepStartDateTime and SheepLastDateTime as per the TimeZoneDiff
   SET @TimeZoneDiff=(select CruisedDis.TimeDiff  from dbo.CruisedDiscovery as CruisedDis where CAST(CruisedDis.Date as date)=CAST(@ShipStartDateTime as Date)) 
   --print @TimeZoneDiff
   SET @WindowConvDwellTime=(Select Value from dbo.Parameters where Name='WindowConvDwellTime')
   SET @WindowConvLengthTime=(select Value from dbo.Parameters where Name='WindowConvLengthTime')

   set @WindowConvDwellTime=-(@WindowConvDwellTime)
   set @WindowConvLengthTime=-(@WindowConvLengthTime)

   if (@TimeZoneDiff Is Null)
   Begin
    set @TimeZoneDiff=0
   End

    SET @UTCStartDateTimestring=(SELECT CONVERT(varchar(23),CONVERT(DATETIME,(SELECT DATEADD(hour,-(@TimeZoneDiff),@ShipStartDateTime)),100),121));
   SET @UTCLastDateTimestring=(SELECT CONVERT(varchar(23),CONVERT(DATETIME,(SELECT DATEADD(hour,-(@TimeZoneDiff),@shipLastDateTime)),100),121));
   print @UTCStartDateTimestring
   print @UTCLastDateTimestring
   
 if(Month(Convert(datetime,@UTCStartDateTimestring))<>Month(Convert(datetime,@UTCLastDateTimestring)) or Year(Convert(datetime,@UTCStartDateTimestring))<>Year(Convert(datetime,@UTCLastDateTimestring)))
 Begin
  set @tblName='Data_'+Format(Convert(datetime,@UTCStartDateTimestring),'yyMM')
  set @tblNameNext='Data_'+Format(Convert(datetime,@UTCLastDateTimestring),'yyMM')
  IF(EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@tblName) and EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@tblNameNext))
	Begin 
	--print 'both exist'
	   --SET @DSql='Insert into #tmpCalcArea(MacAddress,myDate,myTime,myDateTime,AreaId) Select MacAddress,myDate,myTime,myDateTime From'+' '+ @tblName+','+@tblNameNext +' '+'where AreaId in(select LoctionIndicator from dbo.LocationIndicators where AreaOfInterestId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myTime asc'
	   ----print @DSql
	   --SET @dynamicSQL='INSERT into #tmpCalcNeighArea(MacAddress,myDateTime,AreaId) select MacAddress,myDateTime,AreaId from'+' '+@tblName+','+@tblNameNext +' '+'where AreaId in(select AreaCode from dbo.NeighBourAreas where AreaId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myTime asc'
	   
	   Set @UtcStartDateTimeNextString=Convert(varchar(100),Convert(date,@UTCStartDateTimestring))+' '+'23:59:00'
	   Set @UtcLastDateTimeNextString=Convert(varchar(100),Convert(date,@UTCLastDateTimestring))+' '+'00:00:00'
	   --print @UtcStartDateTimeNextString
	   --print @UtcLastDateTimeNextString
	  SET @DSql='Insert into #tmpCalcArea(MacAddress,myDate,myTime,myDateTime,AreaId)'+' '+'(Select MacAddress,myDate,myTime,myDateTime,AreaId from'+' '+ @tblName+' '+'Where AreaId in(select LoctionIndicator from dbo.LocationIndicators where AreaOfInterestId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UtcStartDateTimeNextString+''''+')'+' '+'Union all (Select MacAddress,myDate,myTime,myDateTime,AreaId from'+' '+ @tblNameNext+' '+'where AreaId in(select LoctionIndicator from dbo.LocationIndicators where AreaOfInterestId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UtcLastDateTimeNextString+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+')'
	  --print @DSql
	  SET @dynamicSQL='Insert into #tmpCalcNeighArea(MacAddress,myDateTime,AreaId)'+' '+'(Select MacAddress,myDateTime,AreaId from'+' '+ @tblName+' '+'Where AreaId in(select AreaCode from dbo.NeighBourAreas where AreaId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UtcStartDateTimeNextString+''''+')'+' '+'Union all (Select MacAddress,myDateTime,AreaId from'+' '+ @tblNameNext+' '+'where AreaId in(select AreaCode from dbo.NeighBourAreas where AreaId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UtcLastDateTimeNextString+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+')'
	  --print @dynamicSQL
	END
  else if EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@tblName)
	Begin
	--print 'tblname Exist'
	 SET @DSql='Insert into #tmpCalcArea(MacAddress,myDate,myTime,myDateTime,AreaId) Select MacAddress,myDate,myTime,myDateTime,AreaId From'+' '+@tblName +' '+'where AreaId in(select LoctionIndicator from dbo.LocationIndicators where AreaOfInterestId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myDateTime asc'
	 --print @dynamicSQL
	 SET @dynamicSQL='INSERT into #tmpCalcNeighArea(MacAddress,myDateTime,AreaId) select MacAddress,myDateTime,AreaId from'+' '+@tblName+' '+'where AreaId in(select AreaCode from dbo.NeighBourAreas where AreaId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myTime asc'
	--store the Calculation specific data in a temporary table
	End
   else if EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@tblNameNext)
	Begin
	print 'tblnaext Exist'
	 SET @DSql='Insert into #tmpCalcArea(MacAddress,myDate,myTime,myDateTime,AreaId) Select MacAddress,myDate,myTime,myDateTime,AreaId From'+' '+ @tblNameNext +' '+'where AreaId in(select LoctionIndicator from dbo.LocationIndicators where AreaOfInterestId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myDateTime asc'
	 --print @dynamicSQL
	 SET @dynamicSQL='INSERT into #tmpCalcNeighArea(MacAddress,myDateTime,AreaId) select MacAddress,myDateTime,AreaId from'+' '+@tblNameNext+' '+'where AreaId in(select AreaCode from dbo.NeighBourAreas where AreaId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myTime asc'
	--store the Calculation specific data in a temporary table
	End
End
else
Begin
 set @tblName='Data_'+Format(Convert(datetime,@UTCStartDateTimestring),'yyMM')
 if EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@tblName)
	Begin
	 SET @DSql='Insert into #tmpCalcArea(MacAddress,myDate,myTime,myDateTime,AreaId) Select MacAddress,myDate,myTime,myDateTime,AreaId From'+' '+ @tblName +' '+'where AreaId in(select LoctionIndicator from dbo.LocationIndicators where AreaOfInterestId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myDateTime asc'
	 --print @dynamicSQL
	 SET @dynamicSQL='INSERT into #tmpCalcNeighArea(MacAddress,myDateTime,AreaId) select MacAddress,myDateTime,AreaId from'+' '+@tblName+' '+'where AreaId in(select AreaCode from dbo.NeighBourAreas where AreaId='''+Convert(varchar(50),@Area)+''')'+' '+'and myDateTime>='''+@UTCStartDateTimestring+''''+'and myDateTime<='''+@UTCLastDateTimestring+''''+'order by myTime asc'
	--store the Calculation specific data in a temporary table
	End
End

   --print @DSql
   EXEC sp_executesql @DSql
   EXEC sp_executesql @dynamicSQL
	
	set @DeltaTime=(select parame.Value from dbo.Parameters as parame where parame.Name='DeltaTime')
	--print @DeltaTime
    set @dynamicSQL = 'Insert into #MacAddress(MacAddress) Select distinct MacAddress From #tmpCalcArea'
    -- Execute dynamic sql
    exec sp_executesql @dynamicSQL
	select @countMac=COUNT(*) from #MacAddress
	set @i=1
	WHILE(@i<=@countMac)
	BEGIN   --While Loop Start Here for looping each MacAddress
	    select @MACAddress=MacAddress from #MacAddress where Id=@i;
		--print @MACAddress
		
		set @noOfConnections = @noOfConnections + 1;
		set @noOfVisits = 0;-- this is the number of visits for this MACAddress only

	   Create table #VisitTime
	    (
	    Id int identity(1,1),
	    VisitTime Time,
		VisitDateTime DateTime ,
		NextVistTime DateTime,
		TimeDiff int
		)

	   set @dynamicSQL1 = 'Insert into #VisitTime(VisitTime,VisitDateTime,NextVistTime,TimeDiff) Select myTime,myDateTime,LAG(myDateTime) OVER (ORDER BY myDateTime),DATEDIFF(MINUTE,LAG(myDateTime) OVER (ORDER BY myDateTime),myDateTime) From #tmpCalcArea where MacAddress='''+@MACAddress+''''+'Order by myDateTime Asc'
		-- Execute dynamic sql
		exec sp_executesql @dynamicSQL1

		select @countVisitTime=COUNT(*) from #VisitTime

	    set @j=1
	    select @startDateTime=VisitDateTime from #VisitTime where Id=@j
		select @lastDateTime = VisitDateTime from #VisitTime where Id=@countVisitTime

		while(@j<=@countVisitTime)
		Begin
		     select @timeDiff=TimeDiff from #VisitTime where Id=@j
			 if(@timeDiff>=@DeltaTime)
			 Begin
			    select @EndSequenceDateTime=VisitDateTime from #VisitTime where Id=(@j-1)
			    set @timeDiff=(select DATEDIFF(MINUTE,@startDateTime,@EndSequenceDateTime))
			   
				if(@timeDiff>=@DwellTime)
				Begin
					set @entryLastDateTimeNeighBour=(SELECT DATEADD(MINUTE,@WindowConvDwellTime,@startDateTime))
					set @entryDateTimeNeighBour=(SELECT DATEADD(MINUTE,@WindowConvLengthTime,@startDateTime))
		
					if((select count(*) from #tmpCalcNeighArea where MacAddress = @MACAddress and myDateTime>=@entryDateTimeNeighBour and myDateTime<=@entryLastDateTimeNeighBour)>0)
					Begin
					  -- select 'MacAddress'+CONVERT(varchar(50),@MACAddress)
					   --select * from #VisitTime where VisitDateTime>=@startDateTime and VisitDateTime<=@EndSequenceDateTime		   
					   set @WindowConverstionCalcCounter=@WindowConverstionCalcCounter+1
					   --select * from #tmpCalcNeighArea where MacAddress = @MACAddress and myDateTime>=@entryDateTimeNeighBour and myDateTime<=@entryLastDateTimeNeighBour order by myDateTime asc
					End				
				 set @noOfVisits=@noOfVisits+1
				End
				Begin
				 set @noofPassersBy=@noofPassersBy+1
				End
				select @startDateTime=VisitDateTime from #VisitTime where Id=@j
			 End
			 set @j=@j+1
		End

		set @timeDiff=(select DATEDIFF(MINUTE,@startDateTime,@LastDateTime))
		if(@timeDiff>@DwellTime)
		Begin
		   set @noOfVisits=@noOfVisits+1			
			set @entryLastDateTimeNeighBour=(SELECT DATEADD(MINUTE,@WindowConvDwellTime,@startDateTime))
			set @entryDateTimeNeighBour=(SELECT DATEADD(MINUTE,@WindowConvLengthTime,@startDateTime))
		
			if((select count(*) from #tmpCalcNeighArea where MacAddress = @MACAddress and myDateTime>=@entryDateTimeNeighBour and myDateTime<=@entryLastDateTimeNeighBour)>0)
			 Begin
				 -- select 'MacAddress'+CONVERT(varchar(50),@MACAddress)
				 --select * from #VisitTime where VisitDateTime>=@startDateTime and VisitDateTime<=@EndSequenceDateTime		   
				 set @WindowConverstionCalcCounter=@WindowConverstionCalcCounter+1
				 --select * from #tmpCalcNeighArea where MacAddress = @MACAddress and myDateTime>=@entryDateTimeNeighBour and myDateTime<=@entryLastDateTimeNeighBour order by myDateTime asc
			 End		
		End
		Else
		Begin
		  set @noofPassersBy=@noofPassersBy+1
		End

		if(@noOfVisits>0)
		Begin
 		     
		--Store the each hour from vist in #TempVisitHour table for calculating Bussiest Hour
		   if(DATEPART(HOUR,@startDateTime) = DATEPART(HOUR,@LastDateTime))
			  Begin--if hour of start time and last time same then we conside only one
				  Insert into #TempVisitHour select (DATEPART(HOUR,@startDateTime))
			  End
			else 
			  Begin--if hour of start time and last time different then we need to consider both hour
				  Insert into #TempVisitHour select (DATEPART(HOUR,@startDateTime))
				  Insert into #TempVisitHour select (DATEPART(HOUR,@LastDateTime))
			  End 
	   --select * from #VisitTime
	   --select  Convert(varchar(10),@startDateTime)
	   --select Convert(varchar(10),@LastDateTime)
	   --select Convert(varchar(10),@timeDiff)
   	   set @totalDwellTime=@totalDwellTime+@timeDiff  
	    set @dwellTimeCounter = @dwellTimeCounter + 1 
			if(@timeDiff<(@DwellTime+2))
			 Begin
			 set @visitBounseCounter2Min=@visitBounseCounter2Min+1
			 End
			if(@timeDiff<=(@DwellTime+5))
		     Begin
			 set @visitBounseCounter5Min=@visitBounseCounter5Min+1
			 End
		End
		if(@noofPassersBy>0)
		Begin
		--Store the each hour from vist in #TempPassersByHour table for calculating Bussiest PassersBy
	       if(DATEPART(HOUR,@startDateTime) = DATEPART(HOUR,@LastDateTime))
			 Begin--if hour of start time and last time same then we conside only one
			     Insert into #TempPassersByHour select (DATEPART(HOUR,@startDateTime))
		     End
			 else--if hour of start time and last time different then we need to consider both hour
		     Begin
				 Insert into  #TempPassersByHour select (DATEPART(HOUR,@startDateTime))
				 Insert into  #TempPassersByHour select (DATEPART(HOUR,@LastDateTime))
			  End
		End
	   Drop table #VisitTime
		if(@noOfVisits > 0)
		BEGIN
		  set @noOfNewVisits = @noOfNewVisits + 1;
		  set @totalNoOfRepeatVisits = @totalNoOfRepeatVisits + (@noOfVisits - 1)
		END
 select @i=@i+1 
END 

 if(@dwellTimeCounter > 0)
	BEGIN
		--print @dwellTimeCounter
		set @averageDwellTime =CAST(@totalDwellTime AS float) / CAST(@dwellTimeCounter AS float)
	END
	else
    BEGIN
		set @averageDwellTime = 0.0
	END
	if(@noOfNewVisits > 0)
    BEGIN
		set @AvVisitFreq = CAST((@noOfNewVisits + @totalNoOfRepeatVisits) As float) / CAST(@noOfNewVisits as float)
	END
	else
	BEGIN
		set @AvVisitFreq = 0.0;
	END

	IF(@visitBounseCounter2Min>0 and (@noOfNewVisits+@totalNoOfRepeatVisits)>0)
	BEGIN
	   set @visitBounsePercentage2Min=(CAST(@visitBounseCounter2Min  as float)/CAST((@noOfNewVisits + @totalNoOfRepeatVisits) AS FLOAT))*100
	END
	ELSE
    BEGIN
	   SET @visitBounsePercentage2Min=0;
	END
	if(@visitBounseCounter5Min>0 and (@noOfNewVisits+@totalNoOfRepeatVisits)>0)
	BEGIN
	SET @visitBounsePercentage5Min=(CAST(@visitBounseCounter5Min as FLOAT)/CAST((@noOfNewVisits + @totalNoOfRepeatVisits) AS FLOAT))*100;
	END
	ELSE
	BEGIN
	  set @visitBounsePercentage5Min=0;
	END  
	SET @CruiseName=(SELECT TUIDiscovery FROM dbo.CruisedDiscovery WHERE CAST(Date AS date)= Cast(@ShipStartDateTime as Date))
	if(@CruiseName is null)
	Begin
	set @CruiseName='NA'
	End
	
	--Get the Maximum Hour Visit column by grouping as per VisitHour in descending order and fetch the top 1
    SELECT @maxVisitHour=tblTest.VisitHour,@noofVisitBusiest=tblTest.NumberOfCount FROM (SELECT TOP(1) VisitHour,Count(*) as NumberOfCount from #TempVisitHour group by VisitHour order by NumberOfCount desc) as tblTest
	--Get the Maximum Hour Visit column by grouping as per VisitHour and counting also and Fetch the top 1
	SELECT @maxPassersByHour=tblTest.VisitHour,@noofVisitPassersBy=tblTest.VisitHour FROM (SELECT TOP(1) VisitHour,Count(*) as NumberOfCount from #TempPassersByHour group by VisitHour order by NumberOfCount desc) as tblTest
	IF(@maxPassersByHour IS NULL OR @maxVisitHour IS NULL)
	BEGIN
		set @maxVisitHour='NA'
		set @maxPassersByHour='NA'
		--set @WindowConverstionCalcCounter='NA'
	End
	ELSE
	BEGIN
	SET @maxVisitHour=@maxVisitHour+@TimeZoneDiff
	SET @maxPassersByHour=@maxPassersByHour+@TimeZoneDiff
	print 'maxpassersBy'+Convert(varchar(50),@maxPassersByHour)
	print 'maxVisitHour'+Convert(varchar(50),@maxVisitHour)
	   --if the maxVisitHour and maxPassersByHour greater than 24 after adding TimeZone Difference then change mod the result with 24 then we get the 
	   IF(Cast(@maxVisitHour as INT) > 24 OR Cast(@maxPassersByHour as INT) > 24)
		BEGIN
		   SET @maxVisitHour=(SELECT (Cast(@maxVisitHour as INT) % 24))
		   SET @maxPassersByHour=(SELECT (Cast(@maxPassersByHour as INT) % 24))
		END
		--if the maxVisitHour have negative value then add the value in 24 hour to get the result i.e if @maxVisitHour=-2 then 24-2=22
	    IF(Cast(@maxVisitHour as INT) < 0)
		BEGIN
		   SET @maxVisitHour=(SELECT (24 + Cast(@maxVisitHour as INT)))
		END
		--if the maxPassersByHour have negative value then add the value in 24 hour to get the result 
	    IF(Cast(@maxPassersByHour as INT) < 0)
		BEGIN
		 SET @maxPassersByHour=(SELECT (24 + Cast(@maxPassersByHour as INT)))
		END
	End
   SELECT NoOfConnection=@noOfConnections,NoOfNewVisits=@noOfNewVisits,NoOfPassersBy=@noOfPassersBy,NoOfReturns=@totalNoOfRepeatVisits,AvgDwelltime=@averageDwellTime,AvgFrequency=@AvVisitFreq,VisitBounce2Min=@visitBounsePercentage2Min,visitBounce5Min=@visitBounsePercentage5Min,BusiestVisitHour=@maxVisitHour,BusiestVisitPassersBy=@maxPassersByHour,CruiseName=@CruiseName,WindowConversion=@WindowConverstionCalcCounter
drop table #TempVisitHour
drop table #TempPassersByHour
drop table #tmpCalcArea
drop table #MacAddress  
End
GO
/****** Object:  StoredProcedure [dbo].[sp_Importfile]    Script Date: 12/20/2016 11:13:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Importfile]
(
@filepath nvarchar(Max),
@filename varchar(50),
@date datetime
)
AS
BEGIN
  SET NOCOUNT ON
	DECLARE @bulkinsert NVARCHAR(2000), @intErrorCode int, @return_value int
	DECLARE @Counter int, @row_number int, @AreaId varchar(200), @AreaName varchar(100)
	declare @AreaIdentifier varchar(1000), @tblName varchar(200), @SQLString nvarchar(Max),@SQLInsertString nvarchar(Max)

	SET NOCOUNT ON
	BEGIN TRY
		BEGIN TRAN

		select @tblName='Data_'+Format(@date,'yyMM')
		--print @tblName

		set @return_value = 1

		----Create the temporary table for Importing all the data from csv file
		CREATE TABLE #temptbl(
		 [MacAddress] [varchar](100) NULL,
		 [assettype] [varchar](100) NULL,
		 [LastUpdateTime] [varchar](100) NULL,
		 [UserName] [varchar](100) NULL,
		 [Area] [varchar](50) NULL,
		 [Site] [varchar](50) NULL,
		 [building] [varchar](50) NULL,
		 [Floor] [varchar](50) NULL,
		 [X] [varchar](20) NULL,
		 [Y] [varchar](20) NULL
		) 
	
		--Importing the data to Temporary table from csv file using BulkInsert
		SET @bulkinsert = 
		   N'BULK INSERT #temptbl FROM ''' + 
		   @filepath + 
		   N''' WITH (FIRSTROW = 1, FIELDTERMINATOR = '','', ROWTERMINATOR = ''0x0a'',KEEPNULLS)'

		EXEC sp_executesql @bulkinsert

	   -- select * from #temptbl

		IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tblName)
		BEGIN
			set @SQLString = 'Create Table ' + @tblName + '('
			set @SQLString = @SQLString + '[MacAddress] [varchar](100) NULL,[AreaId] [varchar](50) NULL,[myDate] [date] NULL,[myTime] [time](1) NULL,[myDateTime] [datetime] NULL,[Y] [varchar](10) NULL,[X] [varchar](10) NULL,[Id] [int] IDENTITY(1,1) NOT NULL,CONSTRAINT'+' '+'[PK_'+@tblName+']' 
			set @SQLString = @SQLString + 'PRIMARY KEY CLUSTERED([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])ON [PRIMARY]'
			print @SQLString
		    Execute sp_executesql @SQLString
			set @SQLString='CREATE NONCLUSTERED INDEX [<NonCluster_Index, sysname,>] ON'+' '+@tblName+' '+'( [myDateTime] ASC ) INCLUDE ([MacAddress],[AreaId],[myDate],[myTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)'
			print @SQLString
			Execute sp_executesql @SQLString
        END

		set @SQLString = 'INSERT INTO' + ' ' + @tblName + '(MacAddress,AreaId,myDate,myTime,myDateTime,X,Y)' + ' ' + 'SELECT MacAddress,Area,CONVERT(VARCHAR(25),CONVERT(date, LastUpdateTime), 121) as myDate,CONVERT(VARCHAR(20),CONVERT(time, LastUpdateTime), 121) as myTime,LastUpdateTime,X,Y from #temptbl' 
		--print @SQLString
		Execute sp_executesql @SQLString
	

		--Update the FileName inside the FilesUploaded tabl
		IF EXISTS (SELECT 1 FROM dbo.DataFiles WHERE DateOfFile=Convert(date,@date) and IsInSftp='true') 
		BEGIN
		   UPDATE dbo.DataFiles set IsInSftp ='false' where DateOfFile=Convert(date,@date)
		END
		ELSE
		BEGIN
		  INSERT into dbo.DataFiles(FileName,DateOfFile,IsInSftp) values(@filename,@date,'false')
		END
		--commit the statement
	  COMMIT 
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE()
		IF @@TRANCOUNT>0
			ROLLBACK
		set @return_value = -1
	END CATCH
	drop table #temptbl
	return @return_value
	SET NOCOUNT OFF
END

/****** Object:  StoredProcedure [dbo].[sp_GetDate]    Script Date: 12/20/2016 11:13:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_GetDate]
As 
Begin
Create table #TempDate
(
  date varchar(10),
  status varchar(20)
)
Insert into #TempDate Select Convert(date,DateOfFile),'full' from dbo.DataFiles where IsInSftp=0
select * from  #TempDate
Drop table #TempDate
End

/****** Object:  StoredProcedure [dbo].[Sp_GetAllArea]    Script Date: 12/20/2016 11:13:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure Sp_GetAllArea
As
Begin
select AreaOfInterestId,Name from dbo.InterestLocations
End