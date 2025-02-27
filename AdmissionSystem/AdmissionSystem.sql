﻿USE [master]
GO
/****** Object:  Database [AdmissionSystem]    Script Date: 13.02.2025 16:45:17 ******/
CREATE DATABASE [AdmissionSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AdmissionSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TDG2022\MSSQL\DATA\AdmissionSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AdmissionSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TDG2022\MSSQL\DATA\AdmissionSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AdmissionSystem] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AdmissionSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AdmissionSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AdmissionSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AdmissionSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AdmissionSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AdmissionSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [AdmissionSystem] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [AdmissionSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AdmissionSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AdmissionSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AdmissionSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AdmissionSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AdmissionSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AdmissionSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AdmissionSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AdmissionSystem] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AdmissionSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AdmissionSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AdmissionSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AdmissionSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AdmissionSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AdmissionSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AdmissionSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AdmissionSystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AdmissionSystem] SET  MULTI_USER 
GO
ALTER DATABASE [AdmissionSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AdmissionSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AdmissionSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AdmissionSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AdmissionSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AdmissionSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AdmissionSystem] SET QUERY_STORE = ON
GO
ALTER DATABASE [AdmissionSystem] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [AdmissionSystem]
GO
/****** Object:  Table [dbo].[AdmissionStages]    Script Date: 13.02.2025 16:45:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdmissionStages](
	[StageID] [int] IDENTITY(1,1) NOT NULL,
	[StageName] [nvarchar](100) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Applicants]    Script Date: 13.02.2025 16:45:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applicants](
	[ApplicantID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[Gender] [nvarchar](10) NULL,
	[Address] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[Email] [nvarchar](100) NULL,
	[PassportNumber] [nvarchar](50) NULL,
	[RegistrationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Applications]    Script Date: 13.02.2025 16:45:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applications](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicantID] [int] NULL,
	[SpecializationID] [int] NULL,
	[SubmissionDate] [datetime] NULL,
	[Status] [nvarchar](255) NULL,
	[FacultyID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamResults]    Script Date: 13.02.2025 16:45:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamResults](
	[ResultID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NULL,
	[StageID] [int] NULL,
	[ExamScore] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Faculties]    Script Date: 13.02.2025 16:45:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faculties](
	[FacultyID] [int] IDENTITY(1,1) NOT NULL,
	[FacultyName] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[FacultyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Specializations]    Script Date: 13.02.2025 16:45:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specializations](
	[SpecializationID] [int] IDENTITY(1,1) NOT NULL,
	[SpecializationName] [nvarchar](255) NULL,
	[FacultyID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SpecializationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 13.02.2025 16:45:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](50) NULL,
	[LastLogin] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Applicants] ADD  DEFAULT (getdate()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[Applications] ADD  DEFAULT (getdate()) FOR [SubmissionDate]
GO
ALTER TABLE [dbo].[Applications] ADD  DEFAULT ('??????') FOR [Status]
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD FOREIGN KEY([ApplicantID])
REFERENCES [dbo].[Applicants] ([ApplicantID])
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD FOREIGN KEY([SpecializationID])
REFERENCES [dbo].[Specializations] ([SpecializationID])
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD  CONSTRAINT [FK_Applications_Faculties] FOREIGN KEY([FacultyID])
REFERENCES [dbo].[Faculties] ([FacultyID])
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Applications_Faculties]
GO
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [dbo].[AdmissionStages] ([StageID])
GO
ALTER TABLE [dbo].[Specializations]  WITH CHECK ADD FOREIGN KEY([FacultyID])
REFERENCES [dbo].[Faculties] ([FacultyID])
GO
USE [master]
GO
ALTER DATABASE [AdmissionSystem] SET  READ_WRITE 
GO
