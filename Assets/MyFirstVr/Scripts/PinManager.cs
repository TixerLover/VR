using UnityEngine;
using System.Collections;

public class PinManager : MonoBehaviour
{
    private PinBehavior[] allPins;
    private int fallenPinsCount = 0;

    private void Start()
    {
        allPins = GetComponentsInChildren<PinBehavior>();
    }

    public void ReportFallenPin()
    {
        fallenPinsCount++;
        if (fallenPinsCount >= allPins.Length)
        {
            StartCoroutine(RespawnAllPinsAfterDelay(2f));
        }
    }

    private IEnumerator RespawnAllPinsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (PinBehavior pin in allPins)
        {
            pin.ResetPin();
        }

        fallenPinsCount = 0;
    }
}