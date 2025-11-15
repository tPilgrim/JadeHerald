using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Combat(bool state, int id);

    void JumpCheck(bool state);
}
