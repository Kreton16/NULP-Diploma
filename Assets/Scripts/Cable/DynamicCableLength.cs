using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

public class DynamicCableLength : MonoBehaviour
{
    [Header("Положення штекерів")]
    public Transform startPlug;
    public Transform endPlug;
    [Header("Коефіцієнт прогину")]
    public float sagCoefficient = 1.15f;
    public float minimumSlack = 0.05f;

    public float CalculateLength()
    {
        float distance = Vector3.Distance(startPlug.position, endPlug.position);
        float calculatedLength = (distance * sagCoefficient) + minimumSlack;
        return calculatedLength;
    }
}
