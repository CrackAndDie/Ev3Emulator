using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ev3LowLevelLib.Other
{
	internal class MeanFilter
	{
		private int _power;
		private float[] _values;
		public MeanFilter(int power = 3) 
		{
			_power = power;
			_values = new float[power];
		}

		public float Update(float value)
		{
			var tmpArr = new float[_power];
			Array.Copy(_values, 0, tmpArr, 1, _power - 1);
			tmpArr[0] = value;
			_values = tmpArr;
			return _values.Sum() / _power;
		}
	}
}
