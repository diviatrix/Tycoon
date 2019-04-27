using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject buildEffectPrefab;
    public GameObject SellEffectPrefab;

    void OnEnable()
    {
        EventManager.OnBuild += PlayBuildEffect;
    }

    void OnDisable()
    {
        EventManager.OnBuild -= PlayBuildEffect;
    }

    void PlayBuildEffect(SceneObjectBehavior sob)
    {
        GameObject effect = Instantiate(buildEffectPrefab, sob.transform.position, Quaternion.identity);
        Destroy(effect, 2);
    }

    public void PlaySellEffect(Transform t)
    {
        GameObject effect = Instantiate(SellEffectPrefab, t.position, Quaternion.identity);
        Destroy(effect, 2);
    }
}
