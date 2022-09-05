CREATE TABLE [dbo].[Mes] (
    [idMes]  INT          IDENTITY (1, 1) NOT NULL,
    [Nombre] VARCHAR (50) NOT NULL,
    [Estado] BIT          CONSTRAINT [DF_Mes_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Mes] PRIMARY KEY CLUSTERED ([idMes] ASC)
);

