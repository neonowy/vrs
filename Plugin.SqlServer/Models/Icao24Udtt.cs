﻿// Copyright © 2017 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRadar.Interface.Database;

namespace VirtualRadar.Plugin.SqlServer.Models
{
    /// <summary>
    /// Represents the ICAO24 UDTT.
    /// </summary>
    class Icao24Udtt
    {
        /// <summary>
        /// Gets the pre-generated set of UDTT properties for the class.
        /// </summary>
        public static UdttProperty<Icao24Udtt>[] UdttProperties { get; private set; }

        /// <summary>
        /// Gets or sets the ICAO24 code.
        /// </summary>
        [Ordinal(0)]
        public string ModeS { get; set; }

        /// <summary>
        /// Creates a new object.
        /// </summary>
        public Icao24Udtt() : this(null)
        {
        }

        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <param name="modeS"></param>
        public Icao24Udtt(string modeS)
        {
            ModeS = modeS;
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Icao24Udtt()
        {
            UdttProperties = UdttProperty<Icao24Udtt>.GetUdttProperties();
        }
    }
}