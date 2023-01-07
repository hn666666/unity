using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank_Health : MonoBehaviour
{
    public int hp=100;
    public GameObject tankExplosion;
    public AudioClip tankExplosionAudio;
    public Slider hpSlider;
    private int hpTotal;
    // Start is called before the first frame update
    void Start()
    {
        hpTotal = hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool TankDamage(){
        if(hp<=0)
            return false;
        hp-=Random.Range(10,20);
        hpSlider.value = (float)hp/hpTotal;
        if(hp<=0){
            AudioSource.PlayClipAtPoint(tankExplosionAudio,transform.position);
            GameObject.Instantiate(tankExplosion, transform.position + Vector3.up, transform.rotation);
            GameObject.Destroy(this.gameObject);
            return true;
        }
        return false;
    }
}
