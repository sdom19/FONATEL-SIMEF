﻿CREATE TABLE [dbo].[Anno] (
    [IdAnno] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre] VARCHAR (50) NOT NULL,
    [Estado] BIT          CONSTRAINT [DF_Anno_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Anno] PRIMARY KEY CLUSTERED ([IdAnno] ASC)
);
