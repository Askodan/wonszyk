using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public ParticleSystem LoadEffect;
    public ParticleSystem ShootEffect;
    public ParticleSystem ShootEffect_mirror;

    public void SetPosition(ItemOnMap place)
    {
        transform.position = place.transform.position;
        transform.rotation = place.transform.rotation;
        if (ShootEffect_mirror.transform.localPosition.z != -GameLogic.Instance.map.Size) {
            ShootEffect_mirror.transform.localPosition = new Vector3(0, 0, -GameLogic.Instance.map.Size);
        }
    }
    public void SetDuration(float duration)
    {
        var lm = LoadEffect.main;
        lm.duration = duration;
        var sm = ShootEffect.main;
        sm.duration = duration;
        var sm_m = ShootEffect_mirror.main;
        sm_m.duration = duration;
    }
    public void LoadLaser()
    {
        LoadEffect.Play();
    }
    public void ShootLaser()
    {
        ShootEffect.Play();
        ShootEffect_mirror.Play();
    }
}
