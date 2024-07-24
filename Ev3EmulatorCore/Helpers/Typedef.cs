namespace Ev3EmulatorCore.Helpers
{
	public class Typedef<T>
	{
		#region Variables
		protected T m_value;
		#endregion

		#region Constructor/Destructor
		public Typedef()
		{
			this.m_value = default(T);
		}

		public Typedef(T value)
		{
			this.m_value = value;
		}
		#endregion

		#region Override Methods
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!object.ReferenceEquals(this.m_value.GetType(), obj.GetType()))
			{
				return false;
			}
			return this.m_value.Equals(obj);
		}

		public override int GetHashCode()
		{
			return this.m_value.GetHashCode();
		}

		public override string ToString()
		{
			return this.m_value.ToString();
		}
		#endregion

		#region Static Methods

		private static bool IsNumeric(object value)
		{
			return ((value is sbyte) || (value is byte) || (value is short) || (value is ushort) || (value is int) || (value is uint) || (value is long) || (value is ulong) || (value is float) || (value is double) || (value is decimal));
		}

		private static bool IsInteger(object value)
		{
			return ((value is sbyte) || (value is byte) || (value is short) || (value is ushort) || (value is int) || (value is uint) || (value is long) || (value is ulong));
		}

		private static bool IsFloatingPoint(object value)
		{
			return ((value is float) || (value is double) || (value is decimal));
		}

		private static decimal GetNumeric(object value)
		{
			if (!IsNumeric(value))
			{
				throw new InvalidOperationException();
			}
			return (decimal)Convert.ChangeType((object)value, TypeCode.Decimal);
		}

		private static long GetInteger(object value)
		{
			if (!IsInteger(value))
			{
				throw new InvalidOperationException();
			}
			return (long)Convert.ChangeType((object)value, TypeCode.Int64);
		}

		private static decimal GetFloatingPoint(object value)
		{
			if (!IsFloatingPoint(value))
			{
				throw new InvalidOperationException();
			}
			return (decimal)Convert.ChangeType((object)value, TypeCode.Decimal);
		}

		#region Convertional operators
		public static implicit operator Typedef<T>(T value)
		{
			return new Typedef<T>(value);
		}

		public static explicit operator T(Typedef<T> value)
		{
			return value.Value;
		}
		#endregion

		#region Unary operators  { +, -, !, ~, ++, --, true, false }
		public static T operator +(Typedef<T> operand)
		{
			T temp = operand.m_value;
			if (IsInteger(temp))
			{
				return (T)(object)(+GetInteger(temp));
			}
			else if (IsFloatingPoint(temp))
			{
				return (T)(object)(+GetFloatingPoint(temp));
			}

			return temp;
		}

		public static T operator -(Typedef<T> operand)
		{
			T temp = operand.m_value;
			if (IsInteger(temp))
			{
				return (T)(object)(-GetInteger(temp));
			}
			else if (IsFloatingPoint(temp))
			{
				return (T)(object)(-GetFloatingPoint(temp));
			}

			return temp;
		}

		public static T operator !(Typedef<T> operand)
		{
			T temp = operand.m_value;
			if (temp is bool)
			{
				return (T)(object)(!((bool)Convert.ChangeType((object)temp, TypeCode.Boolean)));
			}
			else if (IsNumeric(temp))
			{
				decimal val = (decimal)Convert.ChangeType((object)temp, TypeCode.Decimal);
				return (T)(object)((val != 0) ? 0 : val);
			}
			else
			{
				try
				{
					bool isNull = object.ReferenceEquals(((object)temp), null);
					return ((isNull) ? ((T)(object)null) : temp);
				}
				catch (Exception)
				{
					throw new InvalidOperationException();
				}
			}
		}

		public static T operator ~(Typedef<T> operand)
		{
			T temp = operand.m_value;
			if (IsInteger(temp))
			{
				return (T)(object)(~GetInteger(temp));
			}

			return temp;
		}

		public static bool operator true(Typedef<T> operand)
		{
			T temp = operand.m_value;

			if (temp is bool)
			{
				return (bool)Convert.ChangeType((object)temp, TypeCode.Boolean);
			}
			else if (temp is char)
			{
				return ((char)Convert.ChangeType((object)temp, TypeCode.Char) != '\0');
			}
			else if (temp is string)
			{
				return string.IsNullOrEmpty((string)(object)temp);
			}
			else if (IsInteger(temp))
			{
				return ((long)Convert.ChangeType((object)temp, TypeCode.Int64) > 0);
			}
			else if (IsFloatingPoint(temp))
			{
				return ((decimal)Convert.ChangeType((object)temp, TypeCode.Decimal) > 0);
			}
			else
			{
				try
				{
					return !object.ReferenceEquals(((object)temp), null);
				}
				catch (Exception)
				{
					throw new InvalidOperationException();
				}
			}
		}

		public static bool operator false(Typedef<T> operand)
		{
			if (operand)
			{
				return false;
			}
			return true;
		}
		#endregion

		#region Binary operators { +, -, *, /, %, &, |, ^, <<, >> }
		public static T operator +(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if ((tempLeft is string) && (tempRight is string))
			{
				return (T)(object)(((string)(object)tempLeft) + ((string)(object)tempRight));
			}
			else if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (T)(object)(GetNumeric((object)tempLeft) + GetNumeric((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator -(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (T)(object)(GetNumeric((object)tempLeft) - GetNumeric((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator *(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (T)(object)(GetNumeric((object)tempLeft) * GetNumeric((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator /(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (T)(object)(GetNumeric((object)tempLeft) / GetNumeric((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator %(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (T)(object)(GetNumeric((object)tempLeft) % GetNumeric((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator &(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsInteger(tempLeft) && IsInteger(tempRight))
			{
				return (T)(object)(GetInteger((object)tempLeft) & GetInteger((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator |(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsInteger(tempLeft) && IsInteger(tempRight))
			{
				return (T)(object)(GetInteger((object)tempLeft) | GetInteger((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator ^(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsInteger(tempLeft) && IsInteger(tempRight))
			{
				return (T)(object)(GetInteger((object)tempLeft) ^ GetInteger((object)tempRight));
			}

			return tempLeft;
		}

		public static T operator <<(Typedef<T> left, int right)
		{
			T tempLeft = left.m_value;

			if (IsInteger(tempLeft))
			{
				return (T)(object)(GetInteger((object)tempLeft) << right);
			}

			return tempLeft;
		}

		public static T operator >>(Typedef<T> left, int right)
		{
			T tempLeft = left.m_value;

			if (IsInteger(tempLeft))
			{
				return (T)(object)(GetInteger((object)tempLeft) >> right);
			}

			return tempLeft;
		}
		#endregion

		#region Comparison operators { ==, !=, <, >, <=, >= }
		public static bool operator ==(Typedef<T> left, Typedef<T> right)
		{
			//return value1.Value == value2.Value;
			return left.Equals(right);
		}

		public static bool operator !=(Typedef<T> left, Typedef<T> right)
		{
			//return left.Value != value2.Value;
			return !(left.Equals(right));
		}

		public static bool operator <(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (GetNumeric((object)tempLeft) < GetNumeric((object)tempRight));
			}

			return false;
		}

		public static bool operator >(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (GetNumeric((object)tempLeft) > GetNumeric((object)tempRight));
			}

			return false;
		}

		public static bool operator <=(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (GetNumeric((object)tempLeft) <= GetNumeric((object)tempRight));
			}

			return false;
		}

		public static bool operator >=(Typedef<T> left, Typedef<T> right)
		{
			T tempLeft = left.m_value;
			T tempRight = right.m_value;

			if (IsNumeric(tempLeft) && IsNumeric(tempRight))
			{
				return (GetNumeric((object)tempLeft) >= GetNumeric((object)tempRight));
			}

			return false;
		}
		#endregion

		#endregion

		#region Properties
		public virtual T Value
		{
			get { return this.m_value; }
			set { this.m_value = value; }
		}
		#endregion
	}
}
