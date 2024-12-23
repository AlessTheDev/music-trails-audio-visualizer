using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralFluxToggle : MonoBehaviour
{
    [SerializeField] private KeyCode key;

    [SerializeField] private GameObject fluxDetails;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            fluxDetails.SetActive(!fluxDetails.activeSelf);
        }
    }
}
