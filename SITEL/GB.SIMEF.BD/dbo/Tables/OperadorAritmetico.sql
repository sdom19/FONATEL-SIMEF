﻿CREATE TABLE [dbo].[OperadorAritmetico] (
    [IdOperadorAritmetico] INT           NOT NULL,
    [Nombre]               VARCHAR (300) NOT NULL,
    [Operador]             CHAR (3)      NOT NULL,
    [Tipo]                 INT           NULL,
    [Estado]               BIT           NOT NULL,
    CONSTRAINT [PK_OperadorReglaValidacion] PRIMARY KEY CLUSTERED ([IdOperadorAritmetico] ASC)
);
