using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : PowerUp
{
    public FloatValue heartContainers;
    public FloatValue playerHealth;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.isTrigger)
        {
            // TODO esto aumenta 1 contenedor
            heartContainers.RuntimeValue += 1;
            // TODO esto aumenta vida a la cantidad de contenedores en el float value
            playerHealth.RuntimeValue = heartContainers.RuntimeValue * 2;
            // TODO esto aumenta vida en el Player Health
            GenericHealth temp = other.GetComponent<GenericHealth>();
            if (temp)
            {
                temp.FullHeal();
            }
            // TODO esto avisa a la UI de aumentar el contenedor
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }

}
