using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    public Animator animator;
    public string LayerName;
    // Start is called before the first frame update
    void Start()
    {
       int index=  animator.GetLayerIndex(LayerName);
        print(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
