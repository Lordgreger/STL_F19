using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsCreator : MonoBehaviour {

    public GameObject beamPrefab;

    public void createBeams(float[] parameters, GameGrid gridTo) {
        for (int i = 0; i < parameters.Length; i += 4) {
            Vector2 from = new Vector2(parameters[i], parameters[i + 1]);
            Vector2 to = gridTo.GetElementRealPos((int)parameters[i + 2], (int)parameters[i + 3]);
            print(to);
            print(from);
            Vector3[] pos = new Vector3[2] { from, to };
            GameObject go = Instantiate(beamPrefab, this.transform);
            LineRenderer lr = go.GetComponent<LineRenderer>();
            lr.SetPositions(pos);
            ParticleSystem ps = go.GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule sm = ps.shape;
            sm.position = to / 8;
            print(sm.position);
            Destroy(go, 1f);
        }
    }

}
