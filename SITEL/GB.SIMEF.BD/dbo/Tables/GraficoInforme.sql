CREATE TABLE [dbo].[GraficoInforme] (
    [IdGraficoInforme] INT          NOT NULL,
    [Nombre]           VARCHAR (30) NOT NULL,
    [Estado]           BIT          CONSTRAINT [DF_GraficoInforme_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GraficoInforme] PRIMARY KEY CLUSTERED ([IdGraficoInforme] ASC)
);

