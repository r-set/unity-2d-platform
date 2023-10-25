using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int pointsForCoinPickup = 100;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.otherCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            FindObjectOfType<GameScore>().AddToScore(pointsForCoinPickup);
            Destroy(gameObject);
        }
    }
}
