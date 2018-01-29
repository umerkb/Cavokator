﻿//
// CAVOKATOR APP
// Website: https://github.com/Manuito83/Cavokator
// License GNU General Public License v3.0
// Manuel Ortega, 2018
//

using System.Collections.Generic;

namespace Cavokator
{
    class NotamContainer
    {
        public bool ConnectionError;

        public List<bool> NotamQ = new List<bool>();
        public List<bool> NotamD = new List<bool>();

        public List<string> NotamID = new List<string>();
        public List<string> NotamFreeText = new List<string>();
        public List<string> NotamRaw = new List<string>();

        public List<float> Latitude = new List<float>();
        public List<float> Longitude = new List<float>();
        public List<int> Radius = new List<int>();
    }
}