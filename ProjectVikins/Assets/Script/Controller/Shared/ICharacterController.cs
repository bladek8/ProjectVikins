using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller.Shared
{
    public interface ICharacterController<TViewModel>
        where TViewModel : class
    {
        int GetDamage();
        
        Vector3 PositionCenterAttack(Vector3 colSize, Transform transform);
    }
}
