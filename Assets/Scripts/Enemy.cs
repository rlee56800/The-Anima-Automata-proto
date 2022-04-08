using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health; // current health
    public GameObject target; // self
    public float maxHealth = 20;
    public float dmgDealt = 3; // player dmg
    public bool canHit = true; // abe to be hit; not in inivisbiclity frames
    public float iFrames; // invincinility frames

    // materials
    public Material stMat; //standard material
    public Material dmgMat; // mat when damaged
    public Material deathMat; // change mat when died; mostly for testing

    public Material shieldHitmat; // tetsubg

    public char[] dfa = new char[3];
    // a = attack
    // b = block
    public float dfaIndex = 0;
    public bool canPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("entered");
        if(canPlay && canHit)
        {
            if(collision.gameObject.CompareTag("Weapon"))
            {
                //Debug.Log("damaging");
                //canHit = false;
                //TakeDamage(dmgDealt);
                StartCoroutine(Invincibility(dmgMat));
                dfaIndex = DFACheck('a', dfaIndex);
            } else if (collision.gameObject.CompareTag("Shield"))
            {
                //canHit = false;
                StartCoroutine(Invincibility(shieldHitmat));
                dfaIndex = DFACheck('b', dfaIndex);
            }
        }

        if (canPlay && dfaIndex >= dfa.Length)
        {
            canHit = false;
            canPlay = false;
            target.GetComponent<MeshRenderer>().material = deathMat;
            Debug.Log("DEAD");
            Debug.Log("YOUR'E WINNER!");
        }
    }

    private void TakeDamage(float amtDmg)
    {
        //health -= amtDmg;
        Debug.Log(health);
        if(health <= 0)
        {
            canHit = false;
            target.GetComponent<MeshRenderer>().material = deathMat;
            Debug.Log("DEAD");
        } else
        {
            StartCoroutine(Invincibility(dmgMat));
        }
    }

    public IEnumerator Invincibility(Material mat)
    {
        //bool isStandard = true;

        //for (float i = 0; i<=3; i++)
        //{
        //    target.GetComponent<MeshRenderer>().material = isStandard ? dmgMat : stMat;
        //    isStandard = !isStandard;
        //    yield return new WaitForSeconds(iFrames);
        //}
        canHit = false;

        target.GetComponent<MeshRenderer>().material = mat;
        yield return new WaitForSecondsRealtime(iFrames);
        target.GetComponent<MeshRenderer>().material = stMat;
        yield return new WaitForSeconds(iFrames);
        target.GetComponent<MeshRenderer>().material = mat;
        yield return new WaitForSeconds(iFrames);
        target.GetComponent<MeshRenderer>().material = stMat;
        yield return new WaitForSeconds(iFrames);

        canHit = true;

    }

    public float DFACheck(char action, float index)
    {
        if(action.Equals(dfa[(int)index]))
        {
            return index + 1;
        } 
        else
        {
            return 0;
        }
    }

}
