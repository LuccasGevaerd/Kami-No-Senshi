using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesativateByTime : MonoBehaviour {
    public bool destroy;
    public float time;
    float cloack;

	void Update () {
        cloack += Time.deltaTime;
        if(cloack >= time)
        {
            cloack = 0;
            if (!destroy) gameObject.SetActive(false); else Destroy(gameObject);
        }
	}
}
