using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoard : MonoBehaviour
{
    Animator animator;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 collPosition = coll.transform.position;

        if (collPosition.y < transform.position.y)
        {
            if (coll.otherCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
            {

                animator.SetBool("isUp", true);
            }
            else
            {
                animator.SetBool("isUp", false);
            }
        }
    }
}
