using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour
{
    public GameObject Red;
    public GameObject Green;
    public GameObject Yellow;
    private float GreenTime = 15;
    private float RedTime = 10;
    public bool Pos1 = true;
    public bool Pos2 = false;
    public bool Pos3 = false;
    public bool Pos4 = false;

    void Start() //Four pos
    {
        if (Pos1)
        {
            StartCoroutine(Pos_1());
        }
        if (Pos2)
        {
            StartCoroutine(Pos_2());
        }
        if (Pos3)
        {
            StartCoroutine(Pos_3());
        }
        if (Pos4)
        {
            StartCoroutine(Pos_4());
        }
    }

    IEnumerator Pos_1()
    {
        Red.SetActive(false); //turn off red light
        Yellow.SetActive(false); //turn off yellow light
        Green.SetActive(true); //turn on green light

        yield return new WaitForSeconds(GreenTime); //wait for GreenTime = 15s

        Green.SetActive(false); //flashing green light
        yield return new WaitForSeconds(1); //1 second interval
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Green.SetActive(false);
        yield return new WaitForSeconds(1);
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Yellow.SetActive(true); //yellow on
        yield return new WaitForSeconds(1);
        Red.SetActive(true); //red on

        Yellow.SetActive(true); //green off; yellow red on
        Green.SetActive(false);
        Red.SetActive(true);

        Yellow.SetActive(false);//yellow off; 
        Green.SetActive(false);
        yield return new WaitForSeconds(30); //wait for 30s

        Start(); //back to init pos1
    }

    IEnumerator Pos_2()
    {
        Red.SetActive(true);
        Yellow.SetActive(false);
        Green.SetActive(false);
        yield return new WaitForSeconds(7.5f);

        Red.SetActive(true);
        Yellow.SetActive(true);
        Green.SetActive(false);
        Red.SetActive(false);
        Yellow.SetActive(false);
        Green.SetActive(true);
        yield return new WaitForSeconds(GreenTime);

        Green.SetActive(false);
        yield return new WaitForSeconds(1);
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Green.SetActive(false);
        yield return new WaitForSeconds(1);
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Yellow.SetActive(true);
        yield return new WaitForSeconds(1);
        Red.SetActive(true);

        Yellow.SetActive(false);
        Green.SetActive(false);
        yield return new WaitForSeconds(22.5f);
        Start();
    }

    IEnumerator Pos_3()
    {
        Red.SetActive(true);
        Yellow.SetActive(false);
        Green.SetActive(false);
        yield return new WaitForSeconds(15);
        Red.SetActive(true);
        Yellow.SetActive(true);
        Green.SetActive(false);
        Red.SetActive(false);
        Yellow.SetActive(false);
        Green.SetActive(true);
        yield return new WaitForSeconds(GreenTime);

        Green.SetActive(false);
        yield return new WaitForSeconds(1);
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Green.SetActive(false);
        yield return new WaitForSeconds(1);
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Yellow.SetActive(true);
        yield return new WaitForSeconds(1);
        Red.SetActive(true);

        Yellow.SetActive(false);
        Green.SetActive(false);
        yield return new WaitForSeconds(15);
        Start();
    }

    IEnumerator Pos_4()
    {
        Red.SetActive(true);
        Yellow.SetActive(false);
        Green.SetActive(false);
        yield return new WaitForSeconds(22.5f);

        Red.SetActive(true);
        Yellow.SetActive(true);
        Green.SetActive(false);
        Red.SetActive(false);
        Yellow.SetActive(false);
        Green.SetActive(true);
        yield return new WaitForSeconds(GreenTime);

        Green.SetActive(false);
        yield return new WaitForSeconds(1);
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Green.SetActive(false);
        yield return new WaitForSeconds(1);
        Green.SetActive(true);
        yield return new WaitForSeconds(1);
        Yellow.SetActive(true);
        yield return new WaitForSeconds(1);
        Red.SetActive(true);

        Yellow.SetActive(false);
        Green.SetActive(false);
        yield return new WaitForSeconds(7.5f);
        Start();
    }
}