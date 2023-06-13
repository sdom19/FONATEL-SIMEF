CREATE TABLE [dbo].[AcumulacionFormula] (
    [IdAcumulacionFormula] INT          IDENTITY (1, 1) NOT NULL,
    [Acumulacion]          VARCHAR (25) NOT NULL,
    [Estado]               BIT          NOT NULL,
    CONSTRAINT [PK_AcumulacionFormula] PRIMARY KEY CLUSTERED ([IdAcumulacionFormula] ASC)
);

