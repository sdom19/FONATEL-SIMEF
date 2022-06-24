CREATE TABLE [dbo].[ParamFormulas] (
    [IdParamFormulas]          INT            IDENTITY (1, 1) NOT NULL,
    [IdServicio]               INT            NULL,
    [IdIndicador]              NVARCHAR (50)  NULL,
    [FormulaPorcentaje]        VARCHAR (MAX)  NULL,
    [FormulaCumplimiento]      VARCHAR (MAX)  NULL,
    [Criterios]                VARCHAR (MAX)  NULL,
    [FromArray]                VARCHAR (MAX)  NULL,
    [FechaUltimaActualizacion] DATETIME       NULL,
    [Usuario]                  VARCHAR (30)   NULL,
    [Periodicidad]             NVARCHAR (10)  CONSTRAINT [DF__ParamForm__Perio__1CDC41A7] DEFAULT ('TRIMESTRAL') NULL,
    [ArrayIf]                  VARCHAR (1000) NULL,
    [ArrayVerdadero]           VARCHAR (1000) NULL,
    [ArrayFalso]               VARCHAR (1000) NULL,
    CONSTRAINT [PK__ParamFor__A2CDDD98A3541AF7] PRIMARY KEY CLUSTERED ([IdParamFormulas] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [ch_Periodicidad] CHECK ([Periodicidad]='TRIMESTRAL' OR [Periodicidad]='ANUAL'),
    CONSTRAINT [FK__ParamForm__IdInd__1DD065E0] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador]),
    CONSTRAINT [FK__ParamForm__IdSer__1EC48A19] FOREIGN KEY ([IdServicio]) REFERENCES [dbo].[Servicio] ([IdServicio])
);

