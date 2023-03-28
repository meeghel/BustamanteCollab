¡Hola! Hace rato que no te veía por aquí.
* [¡Hola!] ¡Hola, buenas tardes! Sí, vengo llegando.
    ** -> Divert1
* [Me tengo que ir] Sí, mucho tiempo... Perdón, me tengo que ir.
    ** [Continuar] ->EndPart
    
== Divert1 ==
¿Te puedo ayudar en algo?
* [Sobre la casa] Fíjate que estoy tocando la puerta de la casa pero no me contestan... ¿sabes dónde pudieran estar?
    ¿Estás seguro? Escuché ruidos hace un minuto... ¿Checaste por atrás?
    ** [Está cerrado] Intenté entrar pero está cerrada la puerta y no tengo llave.
        Te presto mi copia, la dejé atrás de casa de Nety. Sólo regrésamela por favor cuando termines porque no tengo otra.
        *** [Regresar] -> Divert1
* [Sobre Bustamante] ¿Y qué novedades en el pueblo?
    No muchas... el tiempo pasa lento aquí. ¡Por eso me encanta! Aunque ahora que lo mencionas sí he visto mucha gente nueva últimamente.
    ** [¿Gente nueva?] ¿Gente nueva? ¿De buena o de mala pinta?
        Pues de ambas, la verdad, pero mejor ni me meto. ¡Somos un pueblo mágico después de todo!
        *** [Regresar] -> Divert1
* [No, gracias] No, gracias, estoy bien.
    ** [Continuar] ->EndPart
    
== EndPart==

-> END
