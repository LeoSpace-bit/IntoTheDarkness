using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code
{
    public interface IDamageable
    {
        void Damage(int value, UnityEngine.Vector3 from);
    }
}
