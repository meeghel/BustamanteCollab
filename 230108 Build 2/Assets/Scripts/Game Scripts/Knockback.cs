using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float knockTime;
    [SerializeField] private string otherTag;
    //public float damage;

    public float OffsetY { get; private set; } = 0.3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Esta parte de pots se debe arreglar
        /*
        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<pot>().Smash();
        }
        */
        if (other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            Rigidbody2D hit = other.GetComponentInParent<Rigidbody2D>();
            SpriteRenderer colliderRenderer = other.GetComponentInParent<SpriteRenderer>();

            if (hit != null)
            {
                Vector3 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                difference.x = Mathf.RoundToInt(difference.x);
                difference.y = Mathf.RoundToInt(difference.y);
                hit.DOMove(hit.transform.position + difference, knockTime);
                //hit.AddForce(difference, ForceMode2D.Impulse);

                colliderRenderer.material.DOColor(Color.red, 1f)
                    .SetEase(Ease.Flash, 3, 0)
                    .OnComplete(() =>
                    colliderRenderer.material.color = Color.white);

                if (other.gameObject.CompareTag("enemyhealth") && other.isTrigger)
                {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime);
                }

                if (other.gameObject.CompareTag("Player") && other.isTrigger)
                {
                    hit.GetComponent<PlayerController>().state = PlayerState.Stagger;
                    hit.GetComponent<PlayerController>().SetPositionAndSnapToTile(hit.transform.position);
                    //Hold();
                }

                /*if (other.GetComponentInParent<PlayerMovement>().currentState != PlayerState.stagger)
                {
                    hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                    other.GetComponentInParent<PlayerMovement>().Knock(knockTime);
                }
                */
            }
        }
    }

    public void Hold()
    {
        StartCoroutine(HoldCo(knockTime));
    }

    private IEnumerator HoldCo(float knockTime)
    {
        this.GetComponentInParent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(knockTime);
        this.GetComponentInParent<Collider2D>().enabled = true;
    }
}