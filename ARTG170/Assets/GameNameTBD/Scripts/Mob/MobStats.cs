using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStats : MonoBehaviour
{
    [SerializeField] private float health = 3;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float flashDuration = 2;
    private Color normalColor;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        normalColor = transform.Find("body").GetComponent<SkinnedMeshRenderer>().material.color;
    }
    public void TakeDamage()
    {
        health -= 1;
        if (health == 0)
        {
            Destroy(gameObject);

            SceneChangeManager.Load(SceneChangeManager.Scene.GameOver);
        }
        Debug.Log("Got Hit!");
        StartCoroutine("Damaged");
    }
    IEnumerator Damaged()
    {
        for (float t = 0f; t < 1; t += Time.deltaTime / flashDuration)
        {
            _renderer.material.color = Color.Lerp(Color.red, normalColor, t);
            yield return null;
        }

    }
}
