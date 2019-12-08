using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour {

    public RectTransform rect;
    public float speed;
    public RectTransform target;

    private void Update() {
        moveTowards(target.position);
    }

    void moveTowards(Vector3 target) {
        Vector3 dist = target - rect.position;

        if (dist.magnitude < 10f) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = dist.normalized;

        rect.position = rect.position + (dir * speed * Time.deltaTime); 
    }

}
