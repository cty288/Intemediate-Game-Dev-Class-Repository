using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public interface IDamageable
    {
        public void DealDamage(int damage);

        public int Health { get; }
        public int MaxHealth { get; }
    }
}