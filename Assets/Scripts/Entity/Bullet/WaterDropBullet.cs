using UnityEngine;

namespace WarBall.Entity.Bullet
{
    public class WaterDropBullet : BulletBase
    {
        protected override void Update()
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
    }
}
