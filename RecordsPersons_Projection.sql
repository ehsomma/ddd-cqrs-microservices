﻿USE [master]
GO

/****** Object:  Database [RecordsPersons_Projection]    Script Date: 24/6/2023 08:42:25 ******/
CREATE DATABASE [RecordsPersons_Projection]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RecordsPersons_Projection', FILENAME = N'/var/opt/mssql/data/RecordsPersons_Projection.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RecordsPersons_Projection_log', FILENAME = N'/var/opt/mssql/data/RecordsPersons_Projection_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RecordsPersons_Projection] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RecordsPersons_Projection].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RecordsPersons_Projection] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET ARITHABORT OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RecordsPersons_Projection] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RecordsPersons_Projection] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RecordsPersons_Projection] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RecordsPersons_Projection] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET RECOVERY FULL 
GO
ALTER DATABASE [RecordsPersons_Projection] SET  MULTI_USER 
GO
ALTER DATABASE [RecordsPersons_Projection] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RecordsPersons_Projection] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RecordsPersons_Projection] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RecordsPersons_Projection] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RecordsPersons_Projection] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RecordsPersons_Projection] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RecordsPersons_Projection] SET QUERY_STORE = ON
GO
ALTER DATABASE [RecordsPersons_Projection] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [RecordsPersons_Projection]
GO

/****** Object:  Table [dbo].[Addresses]    Script Date: 24/6/2023 08:42:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[Id] [int] NOT NULL,
	[PersonId] [uniqueidentifier] NOT NULL,
	[StreetLine1] [nvarchar](60) NOT NULL,
	[StreetLine2] [nvarchar](60) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](60) NULL,
	[Lat] [decimal](8, 6) NULL,
	[Lng] [decimal](9, 6) NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PersonalAssets]    Script Date: 24/6/2023 08:42:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonalAssets](
	[Id] [int] NOT NULL,
	[PersonId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Value] [decimal](9, 2) NOT NULL,
 CONSTRAINT [PK_PersonalAssets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Persons]    Script Date: 24/6/2023 08:42:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persons](
	[Id] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](62) NOT NULL,
	[Email] [varchar](362) NULL,
	[Phone] [nvarchar](20) NULL,
	[Gender] [varchar](10) NOT NULL,
	[Birthdate] [datetime] NULL,
 CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PersonsSummary]    Script Date: 24/6/2023 08:42:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonsSummary](
	[Id] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](62) NOT NULL,
	[Email] [varchar](362) NULL,
	[Phone] [nvarchar](20) NULL,
	[Gender] [varchar](10) NOT NULL,
	[Birthdate] [datetime] NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](60) NULL,
	[Lat] [decimal](8, 6) NULL,
	[Lng] [decimal](9, 6) NULL,
	[PersonalAssetsCount] [decimal](9, 2) NOT NULL,
	[PersonalAssetsBalance] [decimal](9, 2) NOT NULL,
 CONSTRAINT [PK_PersonsSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([Id])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_Persons]
GO
ALTER TABLE [dbo].[PersonalAssets]  WITH CHECK ADD  CONSTRAINT [FK_PersonalAssets_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([Id])
GO
ALTER TABLE [dbo].[PersonalAssets] CHECK CONSTRAINT [FK_PersonalAssets_Persons]
GO
USE [master]
GO
ALTER DATABASE [RecordsPersons_Projection] SET  READ_WRITE 
GO


/****** Object:  Table [dbo].[Countries]    Script Date: 22/9/2023 22:05:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Countries](
	[IataCode] [nvarchar](2) NOT NULL,
	[Name] [nvarchar](120) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[IataCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO