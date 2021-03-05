using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    float speed = 0f;
    float movePower = 0.07f;
    Rigidbody rb;

    AudioSource audio_;

    [SerializeField]
    AudioClip puresentsound;

    [SerializeField]
    AudioClip damagesound;

    [SerializeField]
    GameObject santaobj;

    [SerializeField]
    GameObject canvastext;

    float timecount = 0;


    bool startbutton = false;

    public float TurnPerSecond;//旋回力を決める変数(deg/s)

    int presentcount;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        audio_ = GetComponent<AudioSource>();

        GameObject.FindWithTag("puresent");

    }

    private void FixedUpdate()
    {
        timecount += Time.deltaTime;


        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (timecount > 2)
            {
                startbutton = true;
                Destroy(canvastext);
            }
        }

        if (this.gameObject.transform.position.z > 128f)
        {

            if (presentcount >= 20)
            {
                SceneManager.LoadScene("END_1");
            }
            else if (presentcount >= 15)
            {
                SceneManager.LoadScene("END_2");
            }
            else if (presentcount <= 14)
            {
                SceneManager.LoadScene("END_3");
            }
        }


        if (startbutton)
        {
            Accel();
        }

        if(OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            Right();
        }


        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            Left();
        }


        //speed -= 0.5f;

        if (speed < 0)
        {
            speed = 0f;
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "puresent")
        {
            Destroy(collision.gameObject);

            audio_.PlayOneShot(puresentsound);

            presentcount++;

            santaobj.GetComponent<Animator>().SetBool("go", true);
            StartCoroutine("Coroutine1");
        }


        if(collision.gameObject.tag == "enemy")
        {
            audio_.PlayOneShot(damagesound);

            santaobj.GetComponent<Animator>().SetBool("damage",true);

            StartCoroutine("Coroutine2");
        }


    }

    IEnumerator Coroutine1()
    {
        yield return new WaitForSeconds(0.7f);
        santaobj.GetComponent<Animator>().SetBool("go", false);
    }

    IEnumerator Coroutine2()
    {
        yield return new WaitForSeconds(0.7f);
        santaobj.GetComponent<Animator>().SetBool("damage", false);
    }



    void Accel()
    {
        if (speed < 4.2)
        {
            speed += 1f;
        }
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
    }

    void Right()
    {

        Vector3 temp = new Vector3(transform.position.x + movePower, transform.position.y, transform.position.z);
        transform.position = temp;
    }

    public void Left()
    {
        Vector3 temp = new Vector3(transform.position.x - movePower, transform.position.y, transform.position.z);
        transform.position = temp;
    }

}

