CREATE TABLE [dbo].[FuenteIndicador] (
    [idFuenteIndicador] INT           NOT NULL,
    [FuenteIndicador]   VARCHAR (200) NULL,
    [Estado]            BIT           NULL,
    CONSTRAINT [PK_FuenteIndicador] PRIMARY KEY CLUSTERED ([idFuenteIndicador] ASC)
);

