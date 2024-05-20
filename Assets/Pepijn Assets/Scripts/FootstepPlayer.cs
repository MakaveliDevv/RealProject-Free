using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioSource footsteps;

    public void PlayFootsteps()
    {
        footsteps.Play();
    }
    public void StopFootsteps()
    {
        footsteps.Stop();
    }
}
