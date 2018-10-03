using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDamageBehaviour : MonoBehaviour {
    public GameObject[] particles;
    public int partivleSelector;
    public string hitAnimationName;
    public float damage;
    public GameObject father;
    public float force;
    public float forceUp;
    public bool special;
    public AudioSource audioSource;
    public AudioClip audioclip;
    public float timefloat = 0f;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player 1" || col.gameObject.tag == "Player 2")
        {
            audioSource.PlayOneShot(audioclip);
            GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            Cam.SendMessage("ShakeCamera",0.1f);
            col.SendMessage("Hit", damage);
            if(!special) Instantiate(particles[partivleSelector], transform.position, transform.rotation);
            else Instantiate(particles[partivleSelector], col.transform.position, col.transform.rotation);
            col.GetComponent<Animator>().SetTrigger(hitAnimationName);
            col.GetComponent<Rigidbody>().AddForce(force * father.transform.localScale.x, 0, 0);
            col.SendMessage("Jump", forceUp);
            Time.timeScale = timefloat;
        }
    }
}
