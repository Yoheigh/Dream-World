using UnityEngine;

// ColliderEnter 됐을 때 작동하는 함수
public abstract class CollisionObject : MonoBehaviour
{
    // 플레이어와 OnCollisionEnter 했을 경우 작동하는 함수
    public abstract void CollisionWithPlayer(PlayerController _player);

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            var player = col.gameObject.GetComponent<PlayerController>();
            CollisionWithPlayer(player);
        }
    }
}