

CREATE TABLE [dbo].[Purchase_tb](
	[Purchase_Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[per_Product_Cost] [decimal](18, 4) NOT NULL,
	[Date] [date] NULL,
 CONSTRAINT [PK_Stock_tb] PRIMARY KEY CLUSTERED 
(
	[Purchase_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


