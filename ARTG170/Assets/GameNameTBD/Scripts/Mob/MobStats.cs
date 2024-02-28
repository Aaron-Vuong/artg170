using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStats : MonoBehaviour
{
    [SerializeField] private float health = 3;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] public GameObject doorway;
    public float damagedIndicatorTime = 2;
    [SerializeField] private AudioSource monsterSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;


    private void Start()
    {
        if (_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
    }
    public void TakeDamage()
    {
        health -= 1;
        
        monsterSound.PlayOneShot(damageSound);
        // Play particles.
        if (health == 0)
        {
            monsterSound.PlayOneShot(deathSound);
            doorway.SetActive(true);
            Destroy(gameObject);


           //SceneChangeManager.Load(SceneChangeManager.Scene.GameOver);
        }
        Debug.Log("Got Hit!");
        StartCoroutine("Damaged");
    }
    IEnumerator Damaged()
    {
        _particleSystem.Play();
        yield return new WaitForSeconds(damagedIndicatorTime);
        _particleSystem.Stop();
    }
}
