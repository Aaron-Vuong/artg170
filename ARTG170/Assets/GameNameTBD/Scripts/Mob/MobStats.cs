using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStats : MonoBehaviour
{
    [SerializeField] private float health = 3;
    private ParticleSystem _particleSystem;
    public float damagedIndicatorTime = 2;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    public void TakeDamage()
    {
        health -= 1;
        // Play particles.
        if (health == 0)
        {
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
