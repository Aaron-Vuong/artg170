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
    private Animator mAnimator;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        if (_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
    }
    public void TakeDamage()
    {
        health -= 1;
        mAnimator.SetTrigger("Damage");
        // Play particles.
        if (health == 0)
        {
            monsterSound.PlayOneShot(deathSound);
            doorway.SetActive(true);
            StartCoroutine("Dead");


           //SceneChangeManager.Load(SceneChangeManager.Scene.GameOver);
        }
        Debug.Log("Got Hit!");
        StartCoroutine("Damaged");
    }
    IEnumerator Damaged()
    {
       
        monsterSound.PlayOneShot(damageSound);
        _particleSystem.Play();
        yield return new WaitForSeconds(damagedIndicatorTime);
        _particleSystem.Stop();
    }
    IEnumerator Dead()
    {
       
        yield return new WaitForSeconds(.05f);
        Destroy(gameObject);
    }
   
}
