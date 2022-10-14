using Fusion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
  [Header("Projectiles")]
  private WeaponController _wc;
  [SerializeField] private Projectile _prefabProjectile;
  private Timer delay = new Timer();

  [Header("Movement")]
  private MovementController _mc;
  private Vector3 _forward;
  private Vector3 _direction;
  private Vector3 _prevDirection;
  private Timer boostChargeTime = new Timer();
  private Timer boostTime = new Timer();
  private bool activatedMobilityPart = false;

  [Header("Targeting")]
  private TargetingController _tc;

  [Header("Parts")]
  private PartSlots _parts;

  [Header("Health")]
  private Health _health;

  [Header("Inputs")]
  private NetworkInputData data;
  private bool _mouseButton0;
  private bool _mouseButton1;

  [Header("Colliders")]
  [SerializeField] private GameObject _collider;
  private Rigidbody2D _rb;

  [Header("Rendering")]
  public Sprite[] sprites;
  private SpriteRenderer _renderer;
  [SerializeField] private Sprite[] _playerOneSprites;
  [SerializeField] private Sprite[] _playerTwoSprites;
  [SerializeField] private Sprite[] _playerThreeSprites;
  [SerializeField] private Sprite[] _playerFourSprites;
  private List<Sprite[]> _playerSprites = new List<Sprite[]>();
  [SerializeField] private GameObject weaponObject;
  [SerializeField] private GameObject mobilityObject;
  [SerializeField] private GameObject targetingObject;

  private void Awake()
  {
    _mc = GetComponent<MovementController>();
    _wc = GetComponent<WeaponController>();
    _tc = GetComponent<TargetingController>();
    _rb = GetComponent<Rigidbody2D>();
    _parts = GetComponent<PartSlots>();
    _health = GetComponent<Health>();
    _renderer = GetComponent<SpriteRenderer>();
    _forward = transform.forward;
    _playerSprites.Add(_playerOneSprites);
    _playerSprites.Add(_playerTwoSprites);
    _playerSprites.Add(_playerThreeSprites);
    _playerSprites.Add(_playerFourSprites);
  }

  void Start() 
  {
    WeaponController.onShoot += Recoil;
    PartBehavior.onCollect += Collect;
    PartSlots.onPartChange += OnPartChange;
    TargetRange.foundTarget += FoundTarget;
    Health.onHealthGone += OnHealthGone;

    // check number of players to determine your robot color
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    if(players.Length > 0 && players.Length < 4)
    {
      sprites = _playerSprites[players.Length - 1];
    }
 
    _renderer.sprite = sprites[0];

    if (_parts.weaponSlot) 
    {
      _wc.UpdateWeaponProperties(_parts.weaponSlot);
    }

    if (_parts.mobilitySlot)
    {
      _mc.UpdateMovementProperties(_parts.mobilitySlot);
    }

    if (_parts.targetingSlot)
    {
      _tc.UpdateTargetingProperties(_parts.targetingSlot);
    }

    UpdatePartSprites();
  }

  void Recoil(Vector3 recoilDirection, float recoilAmount, GameObject shooter) 
  {
    if (shooter.Equals(gameObject)) 
    {
      _rb.AddForce(recoilDirection * recoilAmount);
    }
  }

  void Collect(PartScriptableObject newPart, GameObject collector) 
  {
    if (collector.Equals(gameObject)) 
    {
      _wc.UpdateWeaponProperties(newPart);
      _mc.UpdateMovementProperties(newPart);
      _tc.UpdateTargetingProperties(newPart);
      UpdatePartSprites();
    }
  }

  void OnHealthGone(GameObject go)
  {
    if (go.Equals(gameObject))
    {
      WeaponController.onShoot -= Recoil;
      PartBehavior.onCollect -= Collect;
      PartSlots.onPartChange -= OnPartChange;
      TargetRange.foundTarget -= FoundTarget;
      Health.onHealthGone -= OnHealthGone;
    }
  }

  void OnPartChange(GameObject robot) 
  {
    if (robot.Equals(gameObject))
    {
      _wc.UpdateWeaponProperties(_parts.weaponSlot);
      _mc.UpdateMovementProperties(_parts.mobilitySlot);
      _tc.UpdateTargetingProperties(_parts.targetingSlot);
      UpdatePartSprites();
    }
  }

  void FoundTarget(GameObject target, GameObject self) 
  {
    if (self.Equals(gameObject))
    {
      Debug.Log("FoundTarget");
      Debug.Log(target);
    }
  }

  private void CheckForBoost(NetworkInputData data)
  {
      // only activate the boost if the player has a part with this ability and if it has been recharged or unused
      if (activatedMobilityPart && _parts.mobilitySlot && _parts.mobilitySlot.hasBoost && boostChargeTime.ExpiredOrNotRunning())  
      {
          boostChargeTime = Timer.CreateFromSeconds(_parts.mobilitySlot.boostRechargeTime + 0.5f);
          boostTime = Timer.CreateFromSeconds(0.5f);
          SoundManager.instance.Play("Boost");
      }

      // increase acceleration and top speed for duration of boost
      if (boostTime.isRunning)
      {
          _mc.acceleration += _parts.mobilitySlot.boostPower;
          _mc.maxSpeed += _parts.mobilitySlot.boostPower;
      }

      // boost has finished
      if(boostTime.Expired())
      {
        boostTime = Timer.None;
      }
   
      // boost has recharged
      if(boostChargeTime.Expired())
      {
        boostChargeTime = Timer.None;
      }
  }

  void Update() 
  {
        delay.Update();
        boostChargeTime.Update();
        boostTime.Update();

        data = new NetworkInputData();

        RotateWeapon();
  }

  public void FixedUpdate()
  {
      data.direction.Normalize();
      
      if (!data.direction.Equals(Vector3.zero)) 
      {
        _prevDirection = data.direction;
      }

      // boost powerup
      CheckForBoost(data);

      _mc.Move(_direction.normalized);

      if (data.direction.sqrMagnitude > 0)
        _forward = data.direction;
  }

  void UpdatePartSprites()
  {
    if (_parts.weaponSlot)
    {
      weaponObject.GetComponent<SpriteRenderer>().sprite = _parts.weaponSlot.inUseImage;
    }

    if(_parts.mobilitySlot)
    {
      mobilityObject.GetComponent<SpriteRenderer>().sprite = _parts.mobilitySlot.inUseImage;
    }

    if (_parts.targetingSlot)
    {
      targetingObject.GetComponent<SpriteRenderer>().sprite = _parts.targetingSlot.inUseImage;
    }

    // used in case any parts have full sprite sheet to use instead of a single sprite 
    UpdatePartSpriteSheet(0);
  }

  void UpdatePartSpriteSheet(int index) 
  {
    if (_parts.mobilitySlot && _parts.mobilitySlot.spriteSheet.Length > 0)
    {
      mobilityObject.GetComponent<SpriteRenderer>().sprite = _parts.mobilitySlot.spriteSheet[index];
    } 

    if (_parts.targetingSlot && _parts.targetingSlot.spriteSheet.Length > 0)
    {
      targetingObject.GetComponent<SpriteRenderer>().sprite = _parts.targetingSlot.spriteSheet[index];
    } 
  }

  void RotateWeapon()
  {
    Vector3 dir = _wc.GetAimDirection();
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    weaponObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
  }

  public void OnShoot(InputAction.CallbackContext context) 
  {
    // _rb.AddForce(direction);
    _wc.Shoot(context);
    Debug.Log("Shoot called");

  }

 
 Vector3 ClosestDirection(Vector3 v) 
 {
    Vector3[] compass = new Vector3[6] {Vector3.left, Vector3.right, Vector3.forward, Vector3.back, Vector3.up, Vector3.down};
    var maxDot = -Mathf.Infinity;
    var ret = Vector3.zero;
     
     foreach(Vector3 dir in compass) 
     { 
        var t = Vector3.Dot(v, dir);
        if (t > maxDot) 
        {
          ret = dir;
          maxDot = t;
        }
     }
 
     return ret;
 }

  public void onMove(InputAction.CallbackContext context)
  {
    Vector2 newDirection = context.ReadValue<Vector2>();
    _prevDirection = new Vector3(_direction.x,_direction.y,_direction.z);
    _direction = new Vector3(newDirection.x, newDirection.y, 0f);

    if (_direction.Equals(Vector3.zero)) return;

    // 0 = down
    // 1 = up
    // 2 = left
    // 3 = right

    if (ClosestDirection(_direction).Equals(Vector3.up)) 
    {
        _renderer.sprite = sprites[1];
        UpdatePartSpriteSheet(1);
    }

    if (ClosestDirection(_direction).Equals(Vector3.down)) 
    {
        _renderer.sprite = sprites[0];
        UpdatePartSpriteSheet(0);
    }

    if (ClosestDirection(_direction).Equals(Vector3.left)) 
    {
        _renderer.sprite = sprites[2];
        UpdatePartSpriteSheet(2);
    }

    if (ClosestDirection(_direction).Equals(Vector3.right)) 
    {
        _renderer.sprite = sprites[3];
        UpdatePartSpriteSheet(3);
    }

  }
}