Lupina: ¡Hola! Hace rato que no te veía por aquí.
* [¡Hola!] Player: ¡Hola Lupina, buenas tardes! Sí, vengo llegando.
    ** -> Divert1
* [Me tengo que ir] Player: Sí, mucho tiempo... Perdón, me tengo que ir.
    ** [Continuar] ->EndPart
    
== Divert1 ==
Lupina: ¿Te puedo ayudar en algo?
* [Sobre la casa] Player: Fíjate que estoy tocando la puerta de la casa pero no me contestan... ¿sabes dónde pudieran estar?
    Lupina: ¿Estás seguro? Escuché ruidos hace un minuto... ¿Checaste por atrás?
    ** [Está cerrado] Player: Intenté entrar pero está cerrada la puerta y no tengo llave.
        Lupina: Te presto mi copia, la dejé atrás de casa de Nety. Sólo regrésamela por favor cuando termines porque no tengo otra.
        *** [Regresar] -> Divert1
* [Sobre Bustamante] Player: ¿Y qué novedades en el pueblo?
    Lupina: No muchas... el tiempo pasa lento aquí. ¡Por eso me encanta! Aunque ahora que lo mencionas sí he visto mucha gente nueva últimamente.
    ** [¿Gente nueva?] Player: ¿Gente nueva? ¿De buena o de mala pinta?
        Lupina: Pues de ambas, la verdad, pero mejor ni me meto. ¡Somos un pueblo mágico después de todo!
        *** [Regresar] -> Divert1
* [No, gracias] Player: No, gracias, estoy bien.
    ** [Continuar] ->EndPart
    
== EndPart==

-> END
