using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTestDummy : MonoBehaviour
{
    [SerializeField] private GameObject childPrefab;
    [SerializeField] private GameObject activeChild;

    [SerializeField] private float timeToReset;
    private bool hasChecked;

    // Start is called before the first frame update
    void Start()
    {
        hasChecked = false;
        ResetDummy();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeChild == null && !hasChecked)
        {
            hasChecked = true;
            Invoke(nameof(ResetDummy), timeToReset);
        }
    }

    public void ResetDummy()
    {
        activeChild = Instantiate(childPrefab, transform.position, Quaternion.identity);

        hasChecked = false;
    }
}
