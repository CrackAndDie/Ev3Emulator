﻿using Ev3Core.Ccom.Interfaces;
using Ev3Core.Cmemory.Interfaces;
using Ev3Core.Cui;
using Ev3Core.Cui.Interfaces;
using Ev3Core.Interfaces;
using Ev3Core.Lms2012;
using Ev3Core.Lms2012.Interfaces;

namespace Ev3Core
{
    public static class GH // Global Holder
    {
        public static IEv3System Ev3System { get; set; }

        // lms2012
        public readonly static ILms Lms = new Lms();
        public readonly static GLOBALS VMInstance = new GLOBALS();

        // c_memory
        public readonly static IMemory Memory = null;

        // c_com
        public readonly static ICom Com = null;

        // c_ui
        public readonly static ITerminal Terminal = new Terminal();
        public readonly static ILcd Lcd = new Lcd();
		public readonly static UI_GLOBALS UiInstance = new UI_GLOBALS();
	}
}
