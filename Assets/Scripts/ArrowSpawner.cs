using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    [SerializeField] private int direction = 0;

    [SerializeField] private float frequency = 1;

    [SerializeField] private Arrow arrow;

    private bool coroutineStarted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!coroutineStarted)
        {
            StartCoroutine(SpawnArrow());
            coroutineStarted = true;
        }
    }

    IEnumerator SpawnArrow()
    {
        yield return new WaitForSeconds(frequency);
        arrow.GetComponent<Arrow>().speed = speed;
        arrow.GetComponent<Arrow>().chosenDirection = (Arrow.Direction)direction;
        Instantiate(arrow, transform.position, Quaternion.identity);
        coroutineStarted = false;
    }
}
