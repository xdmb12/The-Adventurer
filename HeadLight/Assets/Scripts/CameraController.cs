using UnityEngine;

public class CameraController : MonoBehaviour
{
   public Transform player;

   public float leftBorder;
   public float rightBorder;

   private void Update()
   {
      float playerCameraBorders = Mathf.Clamp(player.position.x, leftBorder, rightBorder);
      transform.position = new Vector3( playerCameraBorders, transform.position.y, transform.position.z);
   }
}
