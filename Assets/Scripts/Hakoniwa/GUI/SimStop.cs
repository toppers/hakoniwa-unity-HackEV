﻿using Hakoniwa.Core;
using Hakoniwa.Core.Simulation;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Hakoniwa.GUI
{
    public class SimStop : MonoBehaviour
    {
  
        public void OnButtonClick()
        {
            SimulationController simulator = SimulationController.Get();
            simulator.Stop();
        }
    }
}
