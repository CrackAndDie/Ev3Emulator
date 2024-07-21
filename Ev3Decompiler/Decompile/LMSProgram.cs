using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3DecompilerLib.Decompile
{
    /// <summary>
    /// Program class for LMS disassebly
    /// </summary>
    public class LMSProgram
    {
        public List<LMSObject> Objects = new List<LMSObject>();
        internal string fname;
        internal string version; internal Int16 num_objs; internal Int32 global_bytes;

        public int Globals { get { return global_bytes; } }

        public string DetailedToString(int printLevel = 1)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"// Disassembly of {this.fname}");
            sb.AppendLine($"//");
            sb.AppendLine($"// Byte code version: {this.version}");
            sb.AppendLine();

            for (int i = 0; i < this.global_bytes; i++)
            {
                if (printLevel >= 4)
                    sb.AppendLine($"DATA8 GLOBAL{i}");
            }
            for (int i = 0; i < this.num_objs; i++)
            {
                var obj = this.Objects[i];
                if (printLevel >= 2 || obj.Id == 1 && printLevel >= 1)
                    sb.AppendLine(obj.DetailedToString(printLevel));
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Program Object class for LMS disassebly
    /// </summary>
    public class LMSObject
    {
        public List<LMSStatement> Statements = new List<LMSStatement>();
        public lms2012.ObjectHeader header;
        public int Id { get; set; }
        public List<(lms2012.Callparam, int, int)> Arguments = new List<(lms2012.Callparam, int, int)>();
        public List<int> Locals = new List<int>();
        public int subcall_num_args;
        internal LMSProgram program;
        public LMSComment Comment { get; set; }

        public string display_type1;
        public LMSStatement NextLine(LMSStatement statement)
        {
            int idx = statement.LineIndex + 1;
            if (idx >= 0 && idx <= Statements.Count - 1) return Statements[idx];
            else return null;
        }
        public LMSStatement PreviousLine(LMSStatement statement)
        {
            int idx = statement.LineIndex - 1;
            if (idx >= 0 && idx <= Statements.Count - 1) return Statements[idx];
            else return null;
        }
        public int LineIndexOfOffset(long offset)
        {
            return Statements.FindLastIndex(elem => elem.Offset == offset);
        }

        public LMSStatement this[int index]
        {
            get { return Statements[index]; }
        }

        public override string ToString()
        {
            return $"OBJECT{Id} {display_type1} ({Statements.Count} lines)";
        }

        public string DetailedToString(int printLevel = 1)
        {
            StringBuilder sb = new StringBuilder();

            if (this.Comment != null) { sb.AppendLine($"# {this.Comment.ToString()}"); }
            sb.AppendLine($"{this.display_type1} OBJECT{this.Id}" + (this.header.is_block ? $" trigger_count:{header.trigger_count}" : null));
            sb.AppendLine("{");

            if (this.subcall_num_args > 0)
            {
                for (int i = 0; i < this.subcall_num_args; i++)
                {
                    (lms2012.Callparam type2, int arg_bytes, int string_size) = this.Arguments[i];
                    string string_size_str = (string_size > 0) ? $" {string_size}" : null;
                    sb.AppendLine($"\t{type2} LOCAL{arg_bytes}{string_size_str}");
                }
                sb.AppendLine();
            }
            if (this.Locals.Count > 0 && printLevel >= 4)
            {
                for (int i = 0; i < this.Locals.Count; i++)
                {
                    sb.AppendLine($"\tDATA8 LOCAL{this.Locals[i]}");
                }
                sb.AppendLine();
            }

            LMSComment lastcomment = null;
            foreach (var line in this.Statements)
            {
                if (lastcomment != line.Comment)
                {
                    if (line.LineIndex > 0) sb.AppendLine();
                    if (line.Comment != null) { sb.AppendLine($"\t# {line.Comment.ToString()}"); }
                    lastcomment = line.Comment;
                }
                if (printLevel >= 1) sb.Append($"{this.Id}_{line.Offset}:");
                sb.AppendLine("\t" + line.ToString());
            }

            sb.AppendLine("}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Line Statement class for LMS disassebly
    /// </summary>
    public class LMSStatement
    {
        public LMSObject Object;
        public long Offset;
        public lms2012.Op Operator;
        public List<LMSParam> Params = new List<LMSParam>();
        public LMSStatement(LMSObject obj)
        {
            Object = obj;
        }
        public override string ToString()
        {
            return ToString(true, true);
        }
        public string ToString(bool withPostfix, bool withHandle)
        {
            return $"{Operator.ToString()}({string.Join(",", Params.Select(elem => elem.ToString(withPostfix, withHandle)).ToArray())})";
        }
        public LMSStatement Next()
        {
            return Object.NextLine(this);
        }
        public LMSStatement Previous()
        {
            return Object.PreviousLine(this);
        }
        public int LineIndex
        {
            get
            {
                return m_LineIndex; // Object.Statements.IndexOf(this);
            }
            internal set { m_LineIndex = value; }
        }
        private int m_LineIndex = -1;

        public LMSComment Comment { get; set; }
    }

    /// <summary>
    /// Statement Parameter class for LMS disassebly
    /// </summary>
    [DebuggerDisplay("{data?.ToString()}")]
    public class LMSParam
    {
        public Object data;
        public string scope;
        public string handle;
        public string postfix;
        public long offsetdata;
        public override string ToString()
        {
            return ToString(true, true);
        }
        public string ToString(bool withPostfix, bool withHandle)
        {
            return
                (withHandle ? handle : null) +
                $"{scope}{data}"
                + (withPostfix ? postfix : null);
        }
        public string ToStringWOHandle()
        {
            return ToString(true, false);
        }
    }

    /// <summary>
    /// Comment class for LMS disassebly
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class LMSComment
    {
        public LMSComment(string text)
        {
            this.Text = text;
        }
        public string Text { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }

}
