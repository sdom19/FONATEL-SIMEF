CREATE TABLE [dbo].[FuenteIndicador] (
    [IdFuenteIndicador] INT           IDENTITY (1, 1) NOT NULL,
    [Fuente]            VARCHAR (200) NOT NULL,
    [Estado]            BIT           NOT NULL,
    CONSTRAINT [PK_FuenteIndicador] PRIMARY KEY CLUSTERED ([IdFuenteIndicador] ASC)
);

