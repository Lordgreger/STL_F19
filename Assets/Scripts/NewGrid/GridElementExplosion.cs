using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElementExplosion : MonoBehaviour {
    public Animator animator;
    public float explosionTime;

    private void Start() {
        StartCoroutine(explode(1f));
    }

    IEnumerator explode(float delay) {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("Explode");

        Destroy(gameObject, explosionTime);
    }
}