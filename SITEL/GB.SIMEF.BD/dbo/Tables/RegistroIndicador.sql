CREATE TABLE [dbo].[RegistroIndicador] (
    [idRegistroIndicador]   INT            NOT NULL,
    [idRegistro]            INT            NOT NULL,
    [idIndicador]           INT            NOT NULL,
    [CantidadFilas]         INT            NOT NULL,
    [ObservacionInformante] VARCHAR (2000) NULL,
    [ObservacionEncargado]  VARCHAR (2000) NULL,
    CONSTRAINT [PK_RegistroIndicador] PRIMARY KEY CLUSTERED ([idRegistroIndicador] ASC, [idIndicador] ASC),
    CONSTRAINT [FK_RegistroIndicador_Registro] FOREIGN KEY ([idRegistro]) REFERENCES [dbo].[Registro] ([idRegistro])
);

