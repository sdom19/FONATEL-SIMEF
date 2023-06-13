 -- =============================================
-- Author:		MIchael Hernández Cordero
-- Create date: 18/04/2023
-- Description:	Procedimiento almecenado para Diccionario de datos
-- ============================================
CREATE PROCEDURE [dbo].[pa_DiccionarioDato]
AS

SELECT 
	sh.name Esquema,
	a.name [tabla],
	b.name [columna], 
	c.name [tipo], 
	CASE
		WHEN c.name = 'numeric' OR  c.name = 'decimal' OR c.name = 'float'  THEN b.precision
		ELSE null
	END [Precision], 
	b.max_length, 
	CASE 
		WHEN b.is_nullable = 0 THEN 'NO'
		ELSE 'SI'
	END [Permite Nulls],
	CASE 
		WHEN b.is_identity = 0 THEN 'NO'
		ELSE 'SI'
	END [Es Autonumerico],	
	ep.value [Descripcion],
	f.ForeignKey, 
	f.ReferenceTableName, 
	f.ReferenceColumnName
	 
FROM sys.tables a   
	INNER JOIN sys.columns b ON a.object_id= b.object_id 
	INNER JOIN sys.systypes c ON b.system_type_id= c.xtype 
	INNER JOIN sys.objects d ON a.object_id= d.object_id 
	INNER JOIN sys.schemas sh ON sh.schema_id=a.schema_id
	LEFT JOIN sys.extended_properties ep ON d.object_id = ep.major_id AND b.column_Id = ep.minor_id
	LEFT JOIN (SELECT 
				f.name AS ForeignKey,
				OBJECT_NAME(f.parent_object_id) AS TableName,
				COL_NAME(fc.parent_object_id,fc.parent_column_id) AS ColumnName,
				OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
				COL_NAME(fc.referenced_object_id,fc.referenced_column_id) AS ReferenceColumnName
				FROM sys.foreign_keys AS f
				INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id) 	f ON f.TableName =a.name AND f.ColumnName =b.name
WHERE a.name <> 'sysdiagrams' 
ORDER BY  sh.name, sh.schema_id