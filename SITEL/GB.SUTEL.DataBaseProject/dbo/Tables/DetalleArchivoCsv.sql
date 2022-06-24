CREATE TABLE [dbo].[DetalleArchivoCsv] (
    [IdDetalleArchivoCsv] BIGINT   IDENTITY (1, 1) NOT NULL,
    [IdArchivoCsv]        INT      NOT NULL,
    [Tiempo]              DATETIME NOT NULL,
    [Nivel]               MONEY    NOT NULL,
    [Frecuencia]          BIGINT   NULL,
    CONSTRAINT [PK_DetalleArchivoCsv] PRIMARY KEY CLUSTERED ([IdDetalleArchivoCsv] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DetalleArchivoCsv_ArchivoCsv] FOREIGN KEY ([IdArchivoCsv]) REFERENCES [dbo].[ArchivoCsv] ([IdArchivoCsv])
);

