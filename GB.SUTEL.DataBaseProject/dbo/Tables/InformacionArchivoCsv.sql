CREATE TABLE [dbo].[InformacionArchivoCsv] (
    [IdInformacionArchivoCsv] INT           IDENTITY (1, 1) NOT NULL,
    [IdArchivoCsv]            INT           NOT NULL,
    [InformacionArchivoCsv]   VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_InformacionArchivoCsv] PRIMARY KEY CLUSTERED ([IdInformacionArchivoCsv] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_InformacionArchivoCsv_ArchivoCsv] FOREIGN KEY ([IdArchivoCsv]) REFERENCES [dbo].[ArchivoCsv] ([IdArchivoCsv])
);

