USE [master]
GO
/****** Object:  Database [LCMR_DB]    Script Date: 2025/4/10 23:58:13 ******/
CREATE DATABASE [LCMR_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LCMR_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LCMR_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LCMR_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LCMR_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LCMR_DB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LCMR_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LCMR_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LCMR_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LCMR_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LCMR_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LCMR_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [LCMR_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LCMR_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LCMR_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LCMR_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LCMR_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LCMR_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LCMR_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LCMR_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LCMR_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LCMR_DB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LCMR_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LCMR_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LCMR_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LCMR_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LCMR_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LCMR_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LCMR_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LCMR_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [LCMR_DB] SET  MULTI_USER 
GO
ALTER DATABASE [LCMR_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LCMR_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LCMR_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LCMR_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LCMR_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LCMR_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LCMR_DB', N'ON'
GO
ALTER DATABASE [LCMR_DB] SET QUERY_STORE = ON
GO
ALTER DATABASE [LCMR_DB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LCMR_DB]
GO
/****** Object:  Table [dbo].[Revenue]    Script Date: 2025/4/10 23:58:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Revenue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReportDate] [nvarchar](7) NOT NULL,
	[DataYYYMM] [nvarchar](6) NOT NULL,
	[CompanyCode] [nvarchar](10) NOT NULL,
	[CompanyName] [nvarchar](100) NOT NULL,
	[Industry] [nvarchar](50) NOT NULL,
	[CurrentRevenue] [bigint] NULL,
	[LastMonthRevenue] [bigint] NULL,
	[LastYearSameMonthRevenue] [bigint] NULL,
	[MonthlyChangePercentage] [decimal](18, 8) NULL,
	[YearlyChangePercentage] [decimal](18, 8) NULL,
	[CumulativeCurrentRevenue] [bigint] NULL,
	[CumulativeLastYearRevenue] [bigint] NULL,
	[CumulativeChangePercentage] [decimal](18, 8) NULL,
	[Remark] [nvarchar](500) NULL,
 CONSTRAINT [PK__Revenue__3214EC07678823DC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Revenue_CompanyCode]    Script Date: 2025/4/10 23:58:13 ******/
CREATE NONCLUSTERED INDEX [IX_Revenue_CompanyCode] ON [dbo].[Revenue]
(
	[CompanyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Revenue_DataYYYMM]    Script Date: 2025/4/10 23:58:13 ******/
CREATE NONCLUSTERED INDEX [IX_Revenue_DataYYYMM] ON [dbo].[Revenue]
(
	[DataYYYMM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Revenue_DataYYYMM_CompanyCode]    Script Date: 2025/4/10 23:58:13 ******/
CREATE NONCLUSTERED INDEX [IX_Revenue_DataYYYMM_CompanyCode] ON [dbo].[Revenue]
(
	[DataYYYMM] ASC,
	[CompanyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRevenue]    Script Date: 2025/4/10 23:58:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetRevenue]
    @DataYYYMM NVARCHAR(6) = NULL,
    @CompanyCode NVARCHAR(10) = NULL
AS
BEGIN
    SELECT 
        Id,
        ReportDate,
        DataYYYMM,
        CompanyCode,
        CompanyName,
        Industry,
        CurrentRevenue,
        LastMonthRevenue,
        LastYearSameMonthRevenue,
        MonthlyChangePercentage,
        YearlyChangePercentage,
        CumulativeCurrentRevenue,
        CumulativeLastYearRevenue,
        CumulativeChangePercentage,
        Remark
    FROM Revenue
    WHERE (@DataYYYMM IS NULL OR DataYYYMM = @DataYYYMM)
      AND (@CompanyCode IS NULL OR CompanyCode = @CompanyCode)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertRevenue]    Script Date: 2025/4/10 23:58:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertRevenue]
    @ReportDate NVARCHAR(8),
    @DataYYYMM NVARCHAR(6),
    @CompanyCode NVARCHAR(10),
    @CompanyName NVARCHAR(100),
    @Industry NVARCHAR(50),
    @CurrentRevenue BIGINT,
    @LastMonthRevenue BIGINT,
    @LastYearSameMonthRevenue BIGINT,
    @MonthlyChangePercentage DECIMAL(18,8),
    @YearlyChangePercentage DECIMAL(18,8),
    @CumulativeCurrentRevenue BIGINT,
    @CumulativeLastYearRevenue BIGINT,
    @CumulativeChangePercentage DECIMAL(18,8),
    @Remark NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO Revenue AS target
    USING (SELECT 
                @ReportDate AS ReportDate,
                @DataYYYMM AS DataYYYMM,
                @CompanyCode AS CompanyCode,
                @CompanyName AS CompanyName,
                @Industry AS Industry,
                @CurrentRevenue AS CurrentRevenue,
                @LastMonthRevenue AS LastMonthRevenue,
                @LastYearSameMonthRevenue AS LastYearSameMonthRevenue,
                @MonthlyChangePercentage AS MonthlyChangePercentage,
                @YearlyChangePercentage AS YearlyChangePercentage,
                @CumulativeCurrentRevenue AS CumulativeCurrentRevenue,
                @CumulativeLastYearRevenue AS CumulativeLastYearRevenue,
                @CumulativeChangePercentage AS CumulativeChangePercentage,
                @Remark AS Remark
          ) AS source
    ON target.DataYYYMM = source.DataYYYMM
       AND target.CompanyCode = source.CompanyCode
    WHEN MATCHED THEN
        UPDATE SET
            ReportDate = source.ReportDate,
            CompanyName = source.CompanyName,
            Industry = source.Industry,
            CurrentRevenue = source.CurrentRevenue,
            LastMonthRevenue = source.LastMonthRevenue,
            LastYearSameMonthRevenue = source.LastYearSameMonthRevenue,
            MonthlyChangePercentage = source.MonthlyChangePercentage,
            YearlyChangePercentage = source.YearlyChangePercentage,
            CumulativeCurrentRevenue = source.CumulativeCurrentRevenue,
            CumulativeLastYearRevenue = source.CumulativeLastYearRevenue,
            CumulativeChangePercentage = source.CumulativeChangePercentage,
            Remark = source.Remark
    WHEN NOT MATCHED THEN
        INSERT (
            ReportDate,
            DataYYYMM,
            CompanyCode,
            CompanyName,
            Industry,
            CurrentRevenue,
            LastMonthRevenue,
            LastYearSameMonthRevenue,
            MonthlyChangePercentage,
            YearlyChangePercentage,
            CumulativeCurrentRevenue,
            CumulativeLastYearRevenue,
            CumulativeChangePercentage,
            Remark
        )
        VALUES (
            source.ReportDate,
            source.DataYYYMM,
            source.CompanyCode,
            source.CompanyName,
            source.Industry,
            source.CurrentRevenue,
            source.LastMonthRevenue,
            source.LastYearSameMonthRevenue,
            source.MonthlyChangePercentage,
            source.YearlyChangePercentage,
            source.CumulativeCurrentRevenue,
            source.CumulativeLastYearRevenue,
            source.CumulativeChangePercentage,
            source.Remark
        );

END
GO
USE [master]
GO
ALTER DATABASE [LCMR_DB] SET  READ_WRITE 
GO
