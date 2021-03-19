﻿using Hakoniwa.Core;
using Hakoniwa.Core.Simulation;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Hakoniwa.GUI
{
    public class SimStart : MonoBehaviour
    {
  
        public void OnButtonClick()
        {
            SimulationController simulatgor = SimulationController.Get();
            simulatgor.Start();
        }
    }
}
