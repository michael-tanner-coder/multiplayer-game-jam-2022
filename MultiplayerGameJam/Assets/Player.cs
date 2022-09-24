using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
  [SerializeField] private Ball _prefabBall;
  [SerializeField] private PhysxBall _prefabPhysxBall;

  [Networked] private TickTimer delay { get; set; }

  private NetworkCharacterControllerPrototype _cc;
  private Vector3 _forward;
  private Vector3 _prevDirection;

  [Networked(OnChanged = nameof(OnBallSpawned))]
  public NetworkBool spawned { get; set; }

  private SpriteRenderer _renderer;
  SpriteRenderer renderer
  {
    get
    {
      if(_renderer==null)
        _renderer = GetComponentInChildren<SpriteRenderer>();
      return _renderer;
    }
  }

  private Text _messages;
  [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
  public void RPC_SendMessage(string message, RpcInfo info = default)
  {
    if (_messages == null)
      _messages = FindObjectOfType<Text>();
    if(info.IsInvokeLocal)
      message = $"You said: {message}\n";
    else
      message = $"Some other player said: {message}\n";
    _messages.text += message;
  }

  public override void Render()
  {
    renderer.color = Color.Lerp(renderer.color, Color.blue, Time.deltaTime );
  }

  public static void OnBallSpawned(Changed<Player> changed)
  {
    changed.Behaviour.renderer.color = Color.white;
  }

  private void Awake()
  {
    _cc = GetComponent<NetworkCharacterControllerPrototype>();
    _forward = transform.forward;
  }

  private void Update()
  {
    if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
    {
      RPC_SendMessage("Wassup!");
    }
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
        else if ((data.buttons & NetworkInputData.MOUSEBUTTON2) != 0)
        {
          delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
          Runner.Spawn(_prefabPhysxBall,
          transform.position,
          Quaternion.identity,
          Object.InputAuthority,
          (runner, o) =>
          {
            o.GetComponent<PhysxBall>().Init( 10*_prevDirection );
          });
          spawned = !spawned;
        }
      }
    }
  }
}