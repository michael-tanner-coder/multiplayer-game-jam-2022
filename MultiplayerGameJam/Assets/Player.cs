using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
  [SerializeField] private Ball _prefabBall;

  [Networked] private TickTimer delay { get; set; }

  private NetworkCharacterControllerPrototype _cc;
  private Vector3 _forward;
  private Vector3 _prevDirection;

  private void Awake()
  {
    _cc = GetComponent<NetworkCharacterControllerPrototype>();
    _forward = transform.forward;
  }

  public override void FixedUpdateNetwork()
  {
    if (GetInput(out NetworkInputData data))
    {
      data.direction.Normalize();
      transform.position += 5 * data.direction * Runner.DeltaTime;
      
      if (!data.direction.Equals(Vector3.zero)) 
      {
        _prevDirection = data.direction;
      }

      if (data.direction.sqrMagnitude > 0)
        _forward = data.direction;

      if (delay.ExpiredOrNotRunning(Runner))
      {
        if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
        {
          delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
          Runner.Spawn(_prefabBall,
          transform.position, Quaternion.identity,
          Object.InputAuthority, (runner, o) =>
          {
            // Initialize the Ball before synchronizing it
            Ball ball = o.GetComponent<Ball>();
            ball.Init();
            ball.SetDirection(_prevDirection);
          });
        }
      }
    }
  }
}