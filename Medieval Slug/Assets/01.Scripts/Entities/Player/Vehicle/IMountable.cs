using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMountalbe
{
    bool IsMounted { get; }
    void Mount(PlayerController playerController);
    void Dismount(bool expleded);
}
