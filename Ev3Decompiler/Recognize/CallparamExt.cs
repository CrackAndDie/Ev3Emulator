using EV3DecompilerLib.Decompile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EV3DecompilerLib.Decompile.lms2012;

namespace EV3DecompilerLib.Recognize
{
    public class CallparamExt
    {
        internal lms2012.Callparam? callparam { get; set; }
        internal bool IsArrayType { get; set; }
        internal bool IsInput
        {
            get
            {
                return (((int)callparam & (int)DataDirection.IN) == (int)DataDirection.IN);
            }
        }
        public CallparamExt(Callparam cp_in, bool isArray_in = false)
        {
            callparam = cp_in;
            IsArrayType = isArray_in;
        }
        public static implicit operator CallparamExt(Callparam input)
        {
            return new CallparamExt(input);
        }

        public CallparamExt SetDirection(bool isInput)
        {
            if (isInput)
                callparam = (Callparam)((int)callparam & (int)~DataDirection.OUT | (int)DataDirection.IN);
            else
                callparam = (Callparam)((int)callparam & (int)~DataDirection.IN | (int)DataDirection.OUT);

            return this;
        }

        public string GetOutboundType()
        {
            if (callparam.HasValue && ((int)callparam.Value & (int)DataFormat.DATAS) == (int)DataFormat.DATAS)
            {
                return Pattern.BLOCK_DataType_Text;
            }
            else if (callparam.HasValue && ((int)callparam.Value & (int)DataFormat.DATAF) == (int)DataFormat.DATAF)
            {
                return Pattern.BLOCK_DataType_Numeric + (IsArrayType ? Pattern.BLOCK_DataType_ArrayPostFix : null);
            }
            else if (callparam.HasValue && ((int)callparam.Value & (int)DataFormat.DATA8) == (int)DataFormat.DATA8)
            {
                return Pattern.BLOCK_DataType_Boolean + (IsArrayType ? Pattern.BLOCK_DataType_ArrayPostFix : null);
            }
            return string.Empty;
        }

        public override string ToString()
        {
            return callparam.ToString() + (IsArrayType ? "Arr" : null);
        }
    }
}
