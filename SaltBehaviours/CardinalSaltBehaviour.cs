using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BasicMod;
using Newtonsoft.Json;

namespace MoreSalts
{
    class CardinalSaltBehaviour : SaltBehaviour
    {
        private Vector2 dir { get; set; }
        private float rate { get; set; }

        public CardinalSaltBehaviour(Vector2 _dir, float _rate)
        {
            dir = _dir;
            rate = _rate;
        }

        public override void OnCauldronDissolve()
        {
            Debug.Log(dir);
        }
    }
}
