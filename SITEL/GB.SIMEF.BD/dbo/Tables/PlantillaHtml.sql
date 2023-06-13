CREATE TABLE [dbo].[PlantillaHtml] (
    [IdPlantillaHtml] INT           NOT NULL,
    [Html]            TEXT          NOT NULL,
    [Descripcion]     VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_PlantillasHtml] PRIMARY KEY CLUSTERED ([IdPlantillaHtml] ASC)
);

