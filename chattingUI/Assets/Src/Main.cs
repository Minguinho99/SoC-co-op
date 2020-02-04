using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sample;

public class Main : MonoBehaviour
{

    public Hero hero;
    public Namespace gameLauncher;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.tag == "ground")
                {
                    hero.Move(hit.point);
                    gameLauncher.Send(hit.point);
                }
            }
        }

    }
}
