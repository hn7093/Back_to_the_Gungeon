using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SystemManager.Instance.EventManager.OpenEventPage(PageType.ANGEL_PAGE).ContinueWith(task =>
            {
                Debug.Log(task.Result);
            });
        }
    }
}
