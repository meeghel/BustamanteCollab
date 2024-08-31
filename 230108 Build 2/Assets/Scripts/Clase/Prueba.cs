using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Prueba", menuName = "Prueba/Create new Prueba")]

public class Prueba : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;

    [SerializeField] AnimatorController animatorController;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public AnimatorController AnimatorController
    {
        get { return animatorController; }
    }
}
