using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
  public const byte MOUSEBUTTON1 = 0x01;
  public const byte MOUSEBUTTON2 = 0x02;

  public Vector3 direction;
  public Vector3 previousDirection;
  public Vector3 mousePos;
  public byte buttons;
  public bool activatedMobilityPart;
}