USE [master]
GO
/****** Object:  Database [WorkplaceReservation]    Script Date: 2/25/2024 5:20:02 PM ******/
CREATE DATABASE [WorkplaceReservation]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WorkplaceReservation', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\WorkplaceReservation.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WorkplaceReservation_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\WorkplaceReservation_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [WorkplaceReservation] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WorkplaceReservation].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WorkplaceReservation] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET ARITHABORT OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WorkplaceReservation] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WorkplaceReservation] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WorkplaceReservation] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WorkplaceReservation] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET RECOVERY FULL 
GO
ALTER DATABASE [WorkplaceReservation] SET  MULTI_USER 
GO
ALTER DATABASE [WorkplaceReservation] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WorkplaceReservation] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WorkplaceReservation] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WorkplaceReservation] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WorkplaceReservation] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WorkplaceReservation] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'WorkplaceReservation', N'ON'
GO
ALTER DATABASE [WorkplaceReservation] SET QUERY_STORE = OFF
GO
USE [WorkplaceReservation]
GO
/****** Object:  Table [dbo].[ClassRoomReservations]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassRoomReservations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompaniesRoomId] [bigint] NULL,
	[SubscriptionsId] [bigint] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CountOfDay] [int] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_ClassRoomReservations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompainesAttachmenets]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompainesAttachmenets](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NULL,
	[Name] [nvarchar](250) NULL,
	[Path] [nvarchar](250) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_CompainesAttachmenets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompainesRoomAttachments]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompainesRoomAttachments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyRoomId] [bigint] NULL,
	[Name] [nvarchar](250) NULL,
	[Path] [nvarchar](250) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_CompainesRoomAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[OwnerName] [nvarchar](400) NULL,
	[OwnerPhone] [nvarchar](50) NULL,
	[LocationDescriptions] [nvarchar](500) NULL,
	[LocationLink] [nvarchar](500) NULL,
	[FloorCount] [int] NULL,
	[ClassRoomCount] [int] NULL,
	[MeetingRoomCount] [int] NULL,
	[TraningRoomCount] [int] NULL,
	[PrivateRoomCount] [int] NULL,
	[OfficeCount] [int] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompaniesRooms]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompaniesRooms](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NULL,
	[Type] [smallint] NULL,
	[Discriptions] [nvarchar](max) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_CompaniesRooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompaniesSchedule]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompaniesSchedule](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NULL,
	[Day] [smallint] NULL,
	[From] [datetime] NULL,
	[To] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_CompaniesSchedule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactUs]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactUs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Phone] [nvarchar](50) NULL,
	[Mesaage] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_ContactUs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Offers]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompaniesRoomId] [bigint] NULL,
	[Name] [nvarchar](50) NULL,
	[Descriptions] [nvarchar](max) NULL,
	[Target] [smallint] NULL,
	[LessLenthType] [smallint] NULL,
	[MaxLenth] [smallint] NULL,
	[Price] [int] NULL,
	[BookingValue] [int] NULL,
	[InitialPaymentPrice] [int] NULL,
	[LastPaymentBefore] [smallint] NULL,
	[AcceptedBy] [bigint] NULL,
	[AcceptedOn] [datetime] NULL,
	[RejectedOn] [datetime] NULL,
	[RejectedBy] [bigint] NULL,
	[RejectedResone] [nvarchar](max) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_Offers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethods]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethods](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[EnglishName] [nvarchar](50) NULL,
	[AcountNumber] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_PaymentMethods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriptions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[OfferId] [bigint] NULL,
	[Start] [datetime] NULL,
	[End] [datetime] NULL,
	[PaiedValue] [int] NULL,
	[RemindValue] [int] NULL,
	[LastPaymentDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Level] [smallint] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_SubscriptionsRequest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Operations] [nvarchar](50) NULL,
	[Descriptions] [nvarchar](max) NULL,
	[Controller] [nvarchar](250) NULL,
	[OldObject] [nvarchar](max) NULL,
	[NewObject] [nvarchar](max) NULL,
	[ItemId] [bigint] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](400) NULL,
	[LoginName] [nvarchar](50) NULL,
	[Email] [nvarchar](70) NULL,
	[Password] [nvarchar](250) NULL,
	[UserType] [smallint] NULL,
	[Image] [nvarchar](500) NULL,
	[Phone] [nvarchar](12) NULL,
	[Gender] [smallint] NULL,
	[LoginTryAttemptDate] [datetime] NULL,
	[LoginTryAttempts] [smallint] NULL,
	[LastLoginOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Level] [smallint] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSuspends]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSuspends](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[Resone] [nvarchar](max) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_UserSuspends] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[Value] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WalletPurchases]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WalletPurchases](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[WalletId] [bigint] NULL,
	[SubscriptionsId] [bigint] NULL,
	[Value] [int] NULL,
	[CoursePrice] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_WalletPurchases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WalletTransactions]    Script Date: 2/25/2024 5:20:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WalletTransactions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[WalletId] [bigint] NULL,
	[PaymentMethodId] [bigint] NULL,
	[Value] [int] NULL,
	[ProcessType] [smallint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[Status] [smallint] NULL,
 CONSTRAINT [PK_WalletTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [LoginName], [Email], [Password], [UserType], [Image], [Phone], [Gender], [LoginTryAttemptDate], [LoginTryAttempts], [LastLoginOn], [CreatedBy], [CreatedOn], [Level], [Status]) VALUES (1, N'Ahmed', N'Ahmed', N'ahmedtareckb@gmail.com', N's9s29j8nmkIZkPQ0y1LP+WM0SmcSntd/D/xz+fwfRqu8v9TOnDmYXpe8xuB2k8JjD39xDYMwtgobpCV8ToDiq8aDbVkswA==', 1, NULL, N'0911111517', 1, NULL, 0, CAST(N'2024-02-24T23:11:48.480' AS DateTime), NULL, NULL, 1, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UserType_1]  DEFAULT ((2)) FOR [UserType]
GO
ALTER TABLE [dbo].[ClassRoomReservations]  WITH CHECK ADD  CONSTRAINT [FK_ClassRoomReservations_CompaniesRooms] FOREIGN KEY([CompaniesRoomId])
REFERENCES [dbo].[CompaniesRooms] ([Id])
GO
ALTER TABLE [dbo].[ClassRoomReservations] CHECK CONSTRAINT [FK_ClassRoomReservations_CompaniesRooms]
GO
ALTER TABLE [dbo].[ClassRoomReservations]  WITH CHECK ADD  CONSTRAINT [FK_ClassRoomReservations_Subscriptions] FOREIGN KEY([SubscriptionsId])
REFERENCES [dbo].[Subscriptions] ([Id])
GO
ALTER TABLE [dbo].[ClassRoomReservations] CHECK CONSTRAINT [FK_ClassRoomReservations_Subscriptions]
GO
ALTER TABLE [dbo].[CompainesAttachmenets]  WITH CHECK ADD  CONSTRAINT [FK_CompainesAttachmenets_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[CompainesAttachmenets] CHECK CONSTRAINT [FK_CompainesAttachmenets_Companies]
GO
ALTER TABLE [dbo].[CompainesRoomAttachments]  WITH CHECK ADD  CONSTRAINT [FK_CompainesRoomAttachments_CompaniesRooms] FOREIGN KEY([CompanyRoomId])
REFERENCES [dbo].[CompaniesRooms] ([Id])
GO
ALTER TABLE [dbo].[CompainesRoomAttachments] CHECK CONSTRAINT [FK_CompainesRoomAttachments_CompaniesRooms]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Users]
GO
ALTER TABLE [dbo].[CompaniesRooms]  WITH CHECK ADD  CONSTRAINT [FK_CompaniesRooms_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[CompaniesRooms] CHECK CONSTRAINT [FK_CompaniesRooms_Companies]
GO
ALTER TABLE [dbo].[CompaniesSchedule]  WITH CHECK ADD  CONSTRAINT [FK_CompaniesSchedule_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[CompaniesSchedule] CHECK CONSTRAINT [FK_CompaniesSchedule_Companies]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionsRequest_Offers] FOREIGN KEY([OfferId])
REFERENCES [dbo].[Offers] ([Id])
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_SubscriptionsRequest_Offers]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionsRequest_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_SubscriptionsRequest_Users]
GO
ALTER TABLE [dbo].[UserSuspends]  WITH CHECK ADD  CONSTRAINT [FK_UserSuspends_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserSuspends] CHECK CONSTRAINT [FK_UserSuspends_Users]
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Wallet] CHECK CONSTRAINT [FK_Wallet_Users]
GO
ALTER TABLE [dbo].[WalletPurchases]  WITH CHECK ADD  CONSTRAINT [FK_WalletPurchases_Subscriptions] FOREIGN KEY([SubscriptionsId])
REFERENCES [dbo].[Subscriptions] ([Id])
GO
ALTER TABLE [dbo].[WalletPurchases] CHECK CONSTRAINT [FK_WalletPurchases_Subscriptions]
GO
ALTER TABLE [dbo].[WalletPurchases]  WITH CHECK ADD  CONSTRAINT [FK_WalletPurchases_Wallet] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
ALTER TABLE [dbo].[WalletPurchases] CHECK CONSTRAINT [FK_WalletPurchases_Wallet]
GO
ALTER TABLE [dbo].[WalletTransactions]  WITH CHECK ADD  CONSTRAINT [FK_WalletTransactions_PaymentMethods] FOREIGN KEY([PaymentMethodId])
REFERENCES [dbo].[PaymentMethods] ([Id])
GO
ALTER TABLE [dbo].[WalletTransactions] CHECK CONSTRAINT [FK_WalletTransactions_PaymentMethods]
GO
ALTER TABLE [dbo].[WalletTransactions]  WITH CHECK ADD  CONSTRAINT [FK_WalletTransactions_Wallet] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
ALTER TABLE [dbo].[WalletTransactions] CHECK CONSTRAINT [FK_WalletTransactions_Wallet]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClassRoomReservations', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompainesAttachmenets', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompainesRoomAttachments', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Companies', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompaniesRooms', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Studnet
2-Copmanies
3-both' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Offers', @level2type=N'COLUMN',@level2name=N'Target'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Days
2-Month
3-Years' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Offers', @level2type=N'COLUMN',@level2name=N'LessLenthType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Offers', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active 2-not active 9-delete ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentMethods', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Subscriptions', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Add
2-Edit
3-Delete
4-ChangeStatus
5-Other' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Transactions', @level2type=N'COLUMN',@level2name=N'Operations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-admin
2- Company
3-finder




' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Request Join
2-Accepted
3-Rejected' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-active
2-not active
3-stopped
4-admin
9-delete
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserSuspends', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-recharge
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WalletTransactions', @level2type=N'COLUMN',@level2name=N'ProcessType'
GO
USE [master]
GO
ALTER DATABASE [WorkplaceReservation] SET  READ_WRITE 
GO
