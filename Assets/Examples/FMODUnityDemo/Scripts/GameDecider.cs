using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Runtime.InteropServices;

public class GameDecider : MonoBehaviour
{
    public GameObject gameoverScreenGroup;

    private void Start()
    {
        gameoverScreenGroup.SetActive(false);
    }

    private void Update()
    {

	}
}
