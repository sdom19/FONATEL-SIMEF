CREATE PROCEDURE pa_GetCriteriosXIndicador
	 @IdIndicador varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
select distinct 
cri.IdCriterio,
cri.IdIndicador,
cri.IdDireccion,
case  when (select IdCriterio from ParamFormulas where FromArray like '%'+cri.IdCriterio+'%') IS NULL then '0' 
 else
  (select IdCriterio from ParamFormulas where FromArray like '%'+cri.IdCriterio+'%')
 end
as criteriosCkech,
cri.DescCriterio,
ISNULL(prm.FormulaCumplimiento,'') as FormulaCumplimiento,
ISNULL(prm.FormulaPorcentaje,'') as FormulaPorcentaje,
ISNULL(prm.Criterios,'') as Criterios,
ISNULL(prm.FromArray,'') as FromArray,
ISNULL(prm.ArrayIf,'') as ArrayIf,
ISNULL(prm.ArrayVerdadero,'') as ArrayVerdadero,
ISNULL(prm.ArrayFalso,'') as ArrayFalso,
ISNULL(prm.Usuario,'') as Usuario,
ISNULL(prm.FechaUltimaActualizacion,'') as FechaUltimaActualizacion
from
 Criterio cri 
  left join ParamFormulas prm on cri.IdIndicador = prm.IdIndicador
 where cri.IdIndicador = @IdIndicador

END