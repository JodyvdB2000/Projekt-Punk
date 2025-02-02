using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xShake = Random.Range(-1, 1) * magnitude;
            float yShake = Random.Range(-1, 1) * magnitude;

            Debug.Log(xShake + ", " + yShake + ": " + elapsed);

            transform.localPosition = new Vector3(xShake, yShake, originPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originPos;
    }
}
