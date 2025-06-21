using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.API
{
    internal class CubeSolverBridge : MonoBehaviour
    {
        public string FindSolution(string state)
        {
            // TODO: replace with the real DLL call
            return "U' R2 F B R";
        }
    }
}
