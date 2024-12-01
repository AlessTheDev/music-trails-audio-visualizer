using System;
using TMPro;
using UnityEngine;

namespace Testers
{
    [RequireComponent(typeof(TMP_InputField))]
    public class FPSSetter : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TMP_InputField>().onSubmit.AddListener((newValue) =>
            {
                Debug.Log("Changing targetFrameRate to " + newValue);
                Application.targetFrameRate = int.Parse(newValue);
            });
        }
    }
}