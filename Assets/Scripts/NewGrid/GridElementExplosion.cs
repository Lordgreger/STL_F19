using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridElementExplosion : MonoBehaviour {
    public Animator animator;
    public float explosionTime;
    public GameObject pointPrefab;
    public RectTransform pointTarget;

    public void StartExplosion(float delay) {
        StartCoroutine(explode(delay));
    }

    IEnumerator explode(float delay) {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("Explode");
        Destroy(gameObject, explosionTime);

        if (pointPrefab != null && pointTarget != null)
            Instantiate(pointPrefab, transform.parent).GetComponent<PointController>().target = pointTarget;
    }
}