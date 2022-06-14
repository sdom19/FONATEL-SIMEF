
-- =============================================
-- Author:		Ing Edgar Cordero
-- Create date:     20170110
-- Description:	Funcion que se agrega para considerrar algunos casos que la plantilla no esta considerando en relacion a los formatos de los numeros
-- =============================================
CREATE FUNCTION [dbo].[getFormatoNumerico]
(
-- Add the parameters for the function here
@strNumero VARCHAR(500)
)
RETURNS VARCHAR(500)
AS
     BEGIN
         --ELIMINAMOS CARACTERES ESPECIALES Y ESPACIOS
         SET @strNumero = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(@strNumero)), CHAR(9), ''), CHAR(10), ''), CHAR(13), ''), CHAR(8), ''), CHAR(160), '');
		 SET @strNumero = REPLACE(@strNumero,'-','0')
         SELECT @strNumero = CASE
                                 WHEN CHARINDEX(',', @strNumero) = 0 -- sin comas
                                      AND CHARINDEX('.', @strNumero) > 0 -- con puntos
                                      AND (LEN(@strNumero)-LEN(REPLACE(@strNumero, '.', '')))/LEN('.') > 1 --pero mas de dos puntos 
                                 THEN REPLACE(@strNumero, '.', '')--entonces reemplazamos todos los puntos por vacios
                                 WHEN CHARINDEX(',', @strNumero) = 0 -- sin comas
                                      AND CHARINDEX('.', @strNumero) > 0 -- con puntos
                                      AND (LEN(@strNumero)-LEN(REPLACE(@strNumero, '.', '')))/LEN('.') = 1 -- pero con un punto
                                 THEN @strNumero --REPLACE(@strNumero, '.', ',')--  lo dejamos como esta
                                 WHEN CHARINDEX(',', @strNumero) > 0 -- con comas
                                      AND CHARINDEX('.', @strNumero) = 0 -- sin puntos
                                      AND (LEN(@strNumero)-LEN(REPLACE(@strNumero, ',', '')))/LEN(',') > 1 --pero mas de dos comas 
                                 THEN REPLACE(@strNumero, ',', '')--entonces reemplazamos todas las comas por vacios
                                 WHEN CHARINDEX(',', @strNumero) > 0 -- con comas
                                      AND CHARINDEX('.', @strNumero) = 0 -- sin puntos
                                      AND (LEN(@strNumero)-LEN(REPLACE(@strNumero, ',', '')))/LEN(',') > 1 --pero mas de dos comas 
                                 THEN REPLACE(@strNumero, ',', '')--entonces reemplazamos todas las comas por vacios
                                 WHEN CHARINDEX(',', @strNumero) > 0 -- con comas
                                      AND CHARINDEX('.', @strNumero) = 0 -- sin puntos
                                      AND (LEN(@strNumero)-LEN(REPLACE(@strNumero, ',', '')))/LEN(',') = 1 --pero una coma
                                 THEN REPLACE(@strNumero, ',', '.')--entonces reemplazamos coma por punto
                                 WHEN CHARINDEX(',', @strNumero) > 0 -- con comas
                                      AND CHARINDEX('.', @strNumero) > 0 -- con puntos
                                      AND (LEN(@strNumero)-LEN(REPLACE(@strNumero, ',', '')))/LEN(',') = 1 --pero una coma  123.456,456
                                 THEN REPLACE(REPLACE(@strNumero, '.', ''), ',', '.')--entonces reemplazamos todos las comas  por vacios y mantenemos el punto
                                 WHEN CHARINDEX(',', @strNumero) > 0 -- con comas
                                      AND CHARINDEX('.', @strNumero) > 0 -- con puntos
                                      AND (LEN(@strNumero)-LEN(REPLACE(@strNumero, '.', '')))/LEN('.') = 1 --pero un punto 123,456.5456
                                 THEN REPLACE(@strNumero, ',', '')--reemplazamos las comas por vacios 
                                 ELSE @strNumero
                             END;
         RETURN @strNumero;
     END;