Buscar los declarantes sin datos en Declaraciones_Unidades 

Tipo_Declaracion=921 --desplazado
(Persona tipo "D") -- declarante
id_unidad =32
Elegible = si,
Atender = Si o no definido
NO esta en Declaraciones_Unidades => ruv.Id is null o Id_EstadoUnidad is null o Estado_RUV is ( cualquiera de las tres condiciones)
radicados en IRD en un rango de fechas
RUVConsultaNoValorados

Traer los DatosBasicos de cada persona consultando el WS Vivanto por Documento ( Devuelve un Listad de DatosBasicos la misma persona varias veces)

Para cada registro revisar:
    si Hecho == Desplazamiento forzado
        si la fuente == RUV => 
            comparar que la "Numero_Declaracion" == "NUM_FUD_NUM_CASO"  || Fecha_Valoracion (IRD) <=  "F_VALORACION" ("F_VALORACION"  >= Fecha_Valoracion (IRD))    ==>
                si "Estado" == "Incluido" => Id_EstadoUnidad = 371
                si "Estado" == "No Incluido" => Id_EstadoUnidad = 372
                guarda registro si Id_estadoUnidad<>0;
                si la fechas de Declaracion o deplazamiento no coinciden hacer un log !!!! 
                pasar a la siguiente declaracion

        si la fuente == SIPOD => 
            comparar que la "Fecha_Declaracion" == "F_DECLARACION"  ==> &&   --"Fecha_Desplazamiento" =="FECHA_SINIESTRO" =>  
                si "ESTADO" == "Incluido" => Id_EstadoUnidad = 371
                si "ESTADO" == "No Incluido" => Id_EstadoUnidad = 372
                guarda registro si Id_estadoUnidad<>0;
                pasar a la siguiente declaracion

de lo contrario pasar al siguiente hecho