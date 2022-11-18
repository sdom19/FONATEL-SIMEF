CREATE TABLE [dbo].[RelacionCategoriaId](
	[idRelacion] [int] NOT NULL,
	[idCategoriaId] [int] NOT NULL,
 CONSTRAINT [PK_RelacionCategoriaId] PRIMARY KEY CLUSTERED 
(
	[idRelacion] ASC,
	[idCategoriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RelacionCategoriaId]  WITH CHECK ADD  CONSTRAINT [FK_RelacionCategoriaId_RelacionCategoria] FOREIGN KEY([idRelacion])
REFERENCES [dbo].[RelacionCategoria] ([IdRelacionCategoria])
GO

ALTER TABLE [dbo].[RelacionCategoriaId] CHECK CONSTRAINT [FK_RelacionCategoriaId_RelacionCategoria]
GO