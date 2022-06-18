using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public Renderer[] layers; //array of bg layers
    public float[] speedMultiplier; //array of different speedMultipliers for diff bg layers
    private float previousXPositionMario; //mario's previous x position
    private float previousXPositionCamera; //camera's previous x position
    public Transform mario;
    public Transform mainCamera;
    private float[] offset; //keep track of each bg layer offset value (x direction)

    // Start is called before the first frame update
    void Start()
    {
        offset = new float[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            offset[i] = 0.0f; //initially set all offset of all bg layers to 0
        }
        previousXPositionMario = mario.transform.position.x;
        previousXPositionCamera = mainCamera.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        // if camera has moved
        if (Mathf.Abs(previousXPositionCamera - mainCamera.transform.position.x) > 0.001f)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (offset[i] > 1.0f || offset[i] < -1.0f)
                    offset[i] = 0.0f; //reset offset
                float newOffset = mario.transform.position.x - previousXPositionMario;
                offset[i] = offset[i] + newOffset * speedMultiplier[i]; //this line changes each background offset differently based on the speedMultiplier
                layers[i].material.mainTextureOffset = new Vector2(offset[i], 0);
            }
        }
        //update previous pos
        previousXPositionMario = mario.transform.position.x;
        previousXPositionCamera = mainCamera.transform.position.x;
    }
}
