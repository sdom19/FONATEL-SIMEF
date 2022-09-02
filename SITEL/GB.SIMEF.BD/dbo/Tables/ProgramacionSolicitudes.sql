CREATE TABLE [dbo].[ProgramacionSolicitudes] (
    [IdProgamacion] INT  NOT NULL,
    [idSolicitud]   INT  NOT NULL,
    [idFecuencia]   INT  NOT NULL,
    [Fecha]         DATE NULL,
    [NumeroDia]     INT  NULL,
    [Mes]           INT  NULL,
    [Repeticiones]  INT  NULL,
    [Estado]        BIT  NULL,
    CONSTRAINT [PK_ProgramacionSolicitudes] PRIMARY KEY CLUSTERED ([IdProgamacion] ASC),
    CONSTRAINT [FK_ProgramacionSolicitudes_FrecuenciaEnvio] FOREIGN KEY ([idFecuencia]) REFERENCES [dbo].[FrecuenciaEnvio] ([idFrecuencia]),
    CONSTRAINT [FK_ProgramacionSolicitudes_Solicitud] FOREIGN KEY ([idSolicitud]) REFERENCES [dbo].[Solicitud] ([idSolicitud])
);

