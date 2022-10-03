using System;
using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementController : MonoBehaviour {
  [Header("Movement Controller Settings")]
  public float acceleration  = 10.0f;
  public float braking       = 10.0f;
  public float maxSpeed      = 4.0f;

  [Networked]
  [HideInInspector]
  public bool IsGrounded { get; set; }

  [Networked]
  [HideInInspector]
  public Vector3 Velocity { get; set; }

  private TileManager tileManager;

  public CharacterController Controller { get; private set; }

  protected void Awake() {
    tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
  }

  private void CacheController() {
    if (Controller == null) {
      Controller = GetComponent<CharacterController>();

      Assert.Check(Controller != null, $"An object with {nameof(MovementController)} must also have a {nameof(CharacterController)} component.");
    }
  }

  /// <summary>
  /// Basic implementation of a character controller's movement function based on an intended direction.
  /// <param name="direction">Intended movement direction, subject to movement query, acceleration and max speed values.</param>
  /// </summary>
  public virtual void Move(Vector3 direction) {
    var deltaTime    = Time.deltaTime;
    var previousPos  = transform.position;
    var moveVelocity = Velocity;

    direction = direction.normalized;

    var horizontalVel = default(Vector3);
    var verticalVel = default(Vector3);

    horizontalVel.x = moveVelocity.x;
    verticalVel.y = moveVelocity.y;

    var currentBraking = braking;
    var currentAcceleration = acceleration;

    // check for terrain changes to acceleration and braking
    Tile currentTile = tileManager.GetTileData(transform.position);

    // friction will reduce acceleration but increase breaking
    if (currentTile.friction > 0) 
    {
      currentAcceleration -= currentTile.friction;
    } 
    else if (currentTile.friction < 0) 
    {
      currentBraking += currentTile.friction;
    }

    // apply current acceleration and braking
    if (direction == default) {
      horizontalVel = Vector3.Lerp(horizontalVel, default, currentBraking * deltaTime);
      verticalVel = Vector3.Lerp(verticalVel, default, currentBraking * deltaTime);
    } else {
      horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * currentAcceleration * deltaTime, maxSpeed);
      verticalVel = Vector3.ClampMagnitude(verticalVel + direction * currentAcceleration * deltaTime, maxSpeed);
    }

    // move the character to the new position based on movement properties 
    moveVelocity.x = horizontalVel.x;
    moveVelocity.y = verticalVel.y;

    transform.position += (moveVelocity * deltaTime);

    Velocity = (transform.position - previousPos); // may need to add deltaTime

    // check for gap tiles
    if (IsGrounded && currentTile.isGap) 
    {
      // take damage
      
      // disappear

      // respawn near hole
      var celPosition = tileManager.GetMap().WorldToCell(transform.position);
      celPosition.x -= 1;
      celPosition.y -= 1;
      transform.position = tileManager.GetMap().CellToWorld(celPosition);
    }

    // IsGrounded = Controller.isGrounded;
  }

  public void UpdateMovementProperties(PartScriptableObject part)
  {
    acceleration = part.acceleration;
    braking = part.braking;
    maxSpeed = part.maxSpeed;
  }
}