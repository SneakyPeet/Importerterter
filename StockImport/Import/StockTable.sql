USE [Stocks]
GO

/****** Object:  Table [dbo].[Stocks]    Script Date: 2016-02-17 07:55:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Stocks](
	[StockId] [varchar](100) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Open] [decimal](16, 2) NOT NULL,
	[High] [decimal](16, 2) NOT NULL,
	[Low] [decimal](16, 2) NOT NULL,
	[Close] [decimal](16, 2) NOT NULL,
	[Volume] [decimal](16, 2) NOT NULL,
	[AdjClose] [decimal](16, 2) NOT NULL,
	[Sma12] [decimal](16, 2) NOT NULL,
	[Sma20] [decimal](16, 2) NOT NULL,
	[Sma50] [decimal](16, 2) NOT NULL,
	[Ema12] [decimal](16, 2) NOT NULL,
	[Ema20] [decimal](16, 2) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


