using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
  [Header("Projectiles")]
  [SerializeField] private Projectile _prefabProjectile;
  [Networked] private TickTimer delay { get; set; }

  [Header("Movement")]
  private MovementController _mc;
  private NetworkCharacterControllerPrototype _cc;
  private Vector3 _forward;
  private Vector3 _prevDirection;
  [Networked] private TickTimer boostChargeTime {get; set; }
  [Networked] private TickTimer boostTime {get; set; }

  [Header("Parts")]
  private PartSlots _parts;

  [Networked(OnChanged = nameof(OnProjectileSpawned))]
  public NetworkBool spawned { get; set; }

  private Health _health;

  [SerializeField] private GameObject _collider;

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
  }

  public static void OnProjectileSpawned(Changed<Player> changed)
  {
    // changed.Behaviour.renderer.color = Color.white;
  }

  private void Awake()
  {
    _mc = GetComponent<MovementController>();
    _cc = GetComponent<NetworkCharacterControllerPrototype>();
    _parts = GetComponent<PartSlots>();
    _health = GetComponent<Health>();
    _forward = transform.forward;
  }

  private void Update()
  {
    if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
    {
      RPC_SendMessage("Wassup!");
    }
  }

  private void CheckForBoost(NetworkInputData data)
  {
      // only activate the boost if the player has a part with this ability and if it has been recharged or unused
      if (data.activatedMobilityPart && _parts.mobilitySlot && _parts.mobilitySlot.hasBoost && boostChargeTime.ExpiredOrNotRunning(Runner))  
      {
          boostChargeTime = TickTimer.CreateFromSeconds(Runner, _parts.mobilitySlot.boostRechargeTime + 0.5f);
          boostTime = TickTimer.CreateFromSeconds(Runner, 0.5f);
      }

      // increase acceleration and top speed for duration of boost
      if (boostTime.IsRunning)
      {
          _cc.acceleration += _parts.mobilitySlot.boostPower;
          _cc.maxSpeed += _parts.mobilitySlot.boostPower;
      }

      // boost has finished
      if(boostTime.Expired(Runner))
      {
        boostTime = TickTimer.None;
      }
   
      // boost has recharged
      if(boostChargeTime.Expired(Runner))
      {
        boostChargeTime = TickTimer.None;
      }
  }

  public override void FixedUpdateNetwork()
  {
    if (GetInput(out NetworkInputData data))
    {
      data.direction.Normalize();

      if (_parts.mobilitySlot)
      {
        _cc.UpdateMovementProperties(_parts.mobilitySlot);
      }
      
      if (!data.direction.Equals(Vector3.zero)) 
      {
        _prevDirection = data.direction;
      }

      // boost powerup
      CheckForBoost(data);

      _cc.Move(data.direction);

      if (data.direction.sqrMagnitude > 0)
        _forward = data.direction;

      if (delay.ExpiredOrNotRunning(Runner))
      {
        if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
        {
          // get vector for projectile based on current aiming direction
          Vector3 mousePosition = Camera.main.ScreenToWorldPoint(data.mousePos);
          mousePosition.z = transform.position.z;
          Vector3 aimDirection = mousePosition - transform.position;

          // start delay timer to prevent continous shooting
          delay = TickTimer.CreateFromSeconds(Runner, 0.5f);

          // spawn the projectile with a normalized direction
          Runner.Spawn(_prefabProjectile,
          transform.position, Quaternion.identity,
          Object.InputAuthority, (runner, o) =>
          {
            // Initialize the Projectile before synchronizing it
            Projectile projectile = o.GetComponent<Projectile>();
            projectile.shooter = _collider;
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), _collider.GetComponent<Collider2D>(), true);
            projectile.SetDirection(aimDirection.normalized);
            projectile.Init();
          });
        }
      }
    }
  }
}