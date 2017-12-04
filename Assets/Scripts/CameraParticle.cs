using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParticle : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

	public void SetParticlesVisible(bool visible)
    {
        _particles.gameObject.SetActive(visible);
    }
}
