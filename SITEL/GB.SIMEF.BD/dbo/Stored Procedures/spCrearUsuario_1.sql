
CREATE procedure [dbo].[spCrearUsuario]
	  @idFuente int
     ,@Correo varchar(60)
     ,@Nombre varchar(60)
     ,@Contrasena varchar(128)
	 ,@Estado bit
AS

Declare @idusuario int=0;
declare @idRol int =39;

	BEGIN TRAN;
		MERGE SITELP.dbo.Usuario AS TARGET
		USING (VALUES( @Correo, @Nombre,@Contrasena))AS SOURCE 
					 (  Correo,  Nombre, Contrasena)
		ON upper(TARGET.AccesoUsuario)=upper(SOURCE.Correo)
		WHEN NOT MATCHED THEN
		INSERT  
			(
				IdOperador,AccesoUsuario,NombreUsuario,Contrasena,CorreoUsuario,UsuarioInterno
			    ,Activo,Borrado,Mercado,Calidad,FONATEL,FechaRegistro
			)
		VALUES
			(
				NULL,@Correo,@Nombre,@Contrasena,@Correo,0,@Estado,0,0,0,1,GETDATE()
		    )
			WHEN MATCHED THEN
			UPDATE SET 
			Activo=@Estado;

commit tran;

select  @idusuario= IdUsuario from  SITELP.dbo.Usuario
where upper(AccesoUsuario)=upper(CorreoUsuario) and upper(CorreoUsuario)=upper(@Correo)


delete from SITELP.dbo.FuenteUsuario
where IdUsuario=@idusuario;

delete from SITELP.dbo.RolUsuario
where IdUsuario=@idusuario;



insert into SITELP.dbo.FuenteUsuario(IdUsuario,IdFuente)
VALUES(@idusuario,@idFuente)


INSERT INTO SITELP.dbo.RolUsuario(IdRol,IdUsuario)
VALUES(@idRol,@idusuario)


SELECT idDetalleFuente,a.idFuente,NombreDestinatario
      ,CorreoElectronico,a.Estado
  FROM dbo.DetalleFuentesRegistro a
  inner join FuentesRegistro b
  on b.idFuente=a.idFuente
  where b.idFuente=@idFuente;