using UnityEngine;
using Fusion;

public class PhysxBall : NetworkBehaviour
{
  [Networked] private TickTimer life { get; set; }

  public void Init(Vector3 direction)
  {
    life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    GetComponent<Rigidbody>().velocity = direction;
  }

  public override void FixedUpdateNetwork()
  {
    if(life.Expired(Runner))
      Runner.Despawn(Object);
  }
}