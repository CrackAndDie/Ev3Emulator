using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace EV3DecompilerLib.Decompile
{
    /// <summary>
    /// Disassembler for Mindstorms EV3 rbf files
    /// Code is based on the lmsdisasm.py source from David Lechner, lms-hacker-tools
    /// </summary>
    public class Decompiler
    {
        public static int DEBUG_PRINT_LEVEL = 0;

        public LMSProgram BuildByteCodeForFile(string fname)
        {
            using (FileStream fs = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var size = new FileInfo(fname).Length;
                return BuildByteCodeForFile(fs, fname);
            }
        }

        public LMSProgram BuildByteCodeForData(byte[] data, string fname)
        {
            using (MemoryStream memory = new MemoryStream(data))
            {
                return BuildByteCodeForFile(memory, fname);
            }
        }

        public LMSProgram BuildByteCodeForFile(Stream fs, string fname)
        {
            long size = fs.Length;
            LMSProgram program = new LMSProgram();
            //    version, num_objs, global_bytes = parse_program_header(args.input, file_size)
            //(version, num_objs, global_bytes) = parse_program_header(rbffile));
            (program.version, program.num_objs, program.global_bytes) = parse_program_header(fs, size);
            program.fname = fname;

            if (DEBUG_PRINT_LEVEL >= 3) Console.WriteLine($"// Disassembly of {program.fname}");
            if (DEBUG_PRINT_LEVEL >= 3) Console.WriteLine($"//");
            if (DEBUG_PRINT_LEVEL >= 3) Console.WriteLine($"// Byte code version: {program.version}");
            if (DEBUG_PRINT_LEVEL >= 3) Console.WriteLine();
            for (int i = 0; i < program.global_bytes; i++)
            {
                if (DEBUG_PRINT_LEVEL >= 4) Console.WriteLine($"DATA8 GLOBAL{i}");
            }
            for (int i = 0; i < program.num_objs; i++)
            {
                if (DEBUG_PRINT_LEVEL >= 2) Console.WriteLine();
                var obj = parse_object(fs, i + 1);
                obj.program = program;
                program.Objects.Add(obj);
            }

            if (DEBUG_PRINT_LEVEL >= 2) Console.WriteLine();
            return program;
        }

        LMSObject parse_object(Stream fs, int id)
        {
            LMSObject obj = new LMSObject();

            var header = parse_object_header(fs);
            obj.header = header;
            obj.Id = id;
            var save_position = fs.Position;
            fs.Seek(header.offset, SeekOrigin.Begin);
            var num_args = 0;
            var arg_bytes = 0;
            string type1;
            if (header.is_vmthread)
            {
                type1 = "vmthread";
            }
            else if (header.is_subcall)
            {
                type1 = "subcall";
                num_args = (byte)(fs.ReadByte());
                obj.subcall_num_args = num_args;
            }
            else if (header.is_block)
            {
                type1 = "block";
                //obj.block_trigger_count = header.trigger_count;
            }
            else
            {
                throw new Exception("Unknown object type");
            }
            obj.display_type1 = type1;
            if (DEBUG_PRINT_LEVEL >= 2 || id == 1 && DEBUG_PRINT_LEVEL >= 1) Console.WriteLine($"{obj.display_type1} OBJECT{id}");
            if (DEBUG_PRINT_LEVEL >= 2 || id == 1 && DEBUG_PRINT_LEVEL >= 1) Console.WriteLine("{");

            if (num_args > 0)
            {
                for (int i = 0; i < num_args; i++)
                {
                    var type2 = (lms2012.Callparam)fs.ReadByte();
                    var format = lms2012.CallParam2DataFormat(type2);

                    int string_size = 0;
                    if (format == lms2012.DataFormat.DATAS)
                    {
                        string_size = (int)fs.ReadByte();
                    }
                    string string_size_str = "";
                    if (string_size > 0)
                    {
                        string_size_str = $" {string_size}";
                    }
                    if (DEBUG_PRINT_LEVEL >= 4) Console.WriteLine($"\t{type2} LOCAL{arg_bytes}{string_size_str}");
                    obj.Arguments.Add((type2, arg_bytes, string_size));

                    arg_bytes += string_size + lms2012._DATAFormat_size[format];
                }
                if (DEBUG_PRINT_LEVEL >= 4) Console.WriteLine();
            }
            if (header.local_bytes - arg_bytes > 0)
            {
                for (int i = arg_bytes; i < header.local_bytes; i++)
                {
                    if (DEBUG_PRINT_LEVEL >= 4) Console.WriteLine($"\tDATA8 LOCAL{i}");
                    obj.Locals.Add(i);
                }
                if (DEBUG_PRINT_LEVEL >= 4) Console.WriteLine();
            }
            while (true)
            {
                var offset = fs.Position;
                LMSStatement linest = parse_ops(fs, obj, header.offset, id);
                linest.LineIndex = obj.Statements.Count;
                obj.Statements.Add(linest);

                //if (DEBUG_PRINT >= 2) Console.WriteLine($"OFFSET{id}_{offset - header.offset}:");
                if (DEBUG_PRINT_LEVEL >= 2 || id == 1 && DEBUG_PRINT_LEVEL >= 1) Console.Write($"{id}_{offset - header.offset}:");
                linest.Offset = offset - header.offset;

                if (linest.Operator == lms2012.Op.OBJECT_END) break;

                // OMIT this
                //// skip printing "RETURN()" if it is the last op in an object
                //if (linest.Operator == lms2012.Op.RETURN)
                //{
                //    //        peek = ord(infile.read(1))
                //    //        infile.seek(-1, os.SEEK_CUR)
                //    //        if peek == Op.OBJECT_END.value:
                //    //            continue
                //}
                if (DEBUG_PRINT_LEVEL >= 3) Console.WriteLine($"\t{linest.Operator.ToString()}({string.Join(", ", linest.Params.Select(elem => $"{elem.handle}{elem.scope}{elem.data}{elem.postfix} {elem.data.GetType().Name}").ToArray())})");
                else if (DEBUG_PRINT_LEVEL >= 2 || id == 1 && DEBUG_PRINT_LEVEL >= 1) Console.WriteLine($"\t{linest.ToString()}");
            }
            if (DEBUG_PRINT_LEVEL >= 2 || id == 1 && DEBUG_PRINT_LEVEL >= 1) Console.WriteLine("}");

            fs.Seek(save_position, SeekOrigin.Begin);

            return obj;
        }

        LMSStatement parse_ops(Stream fs, LMSObject obj, int start, int id)
        {
            LMSStatement retline = new LMSStatement(obj);
            var op = retline.Operator = (lms2012.Op)fs.ReadByte();

            if (op == lms2012.Op.OBJECT_END) return retline;

            foreach (var param in lms2012._op_code_params[op])
            {
                if (param is lms2012.Subparam)
                {
                    LMSParam param1 = parse_param(param, fs);
                    LMSParam[] subparam1 = parse_subparam(param, param1.ToString(), fs);
                    retline.Params.AddRange(subparam1);
                }
                else
                {
                    LMSParam param1 = parse_param(param, fs);
                    retline.Params.Add(param1);
                }
                //    # special handling for CALL
                if (op == lms2012.Op.CALL && param is lms2012.Param && (lms2012.Param)param == lms2012.Param.PAR16)
                {
                    retline.Params[retline.Params.Count - 1].handle = "OBJECT";

                }
                //    # special handling for varargs
                if (param is lms2012.Param && (lms2012.Param)param == lms2012.Param.PARNO)
                {
                    var numvarargs = (int)retline.Params.Last().data;
                    retline.Params.RemoveAt(retline.Params.Count - 1);
                    for (int i = 0; i < numvarargs; i++)
                    {
                        LMSParam param1 = parse_param(lms2012.Param.PARV, fs);
                        retline.Params.Add(param1);
                    }
                }
            }

            //# special handling for jump ops
            if (op.ToString().StartsWith("JR"))
            {
                var offspar = retline.Params.Last();
                try
                {
                    var offset = Int32.Parse(offspar.ToString());
                    retline.Params.RemoveAt(retline.Params.Count - 1);
                    var offs2 = fs.Position - start + offset;
                    retline.Params.Add(new LMSParam() { handle = "OFFSET", data = id, offsetdata = offs2, postfix = $"_{offs2.ToString()}", scope = null });
                }
                catch { }
            }

            return retline;
        }

        LMSParam[] parse_subparam(Enum type1, string value, Stream fs)
        {
            List<LMSParam> retpars = new List<LMSParam>();

            try
            {
                lms2012.Subparam subcode1 = (lms2012.Subparam)type1;
                //int subcodecode1 = Int32.Parse(value);
                Type subcode_type0 = lms2012._subcode_enums[subcode1];
                Enum subcode_type1 = (Enum)Enum.Parse(subcode_type0, value);
                retpars.Add(new LMSParam() { data = subcode_type1, scope = null });
                var values = -1;

                lms2012.Param[] subcode_type_params = lms2012._subcode_params[subcode1][subcode_type1];
                foreach (var param in subcode_type_params)
                {
                    //    # special handling for arrays
                    if (param == lms2012.Param.PARVALUES)
                    {
                        values = (int)retpars.Last().data;
                        //        # print("PARVALUES = ", values)
                    }
                    else if (values >= 0)
                    {
                        while (values > 0)
                        {
                            values -= 1;
                            LMSParam param2 = parse_param(param, fs);
                            retpars.Add(param2);
                        }
                    }
                    else
                    {
                        LMSParam param2 = parse_param(param, fs);
                        retpars.Add(param2);
                        // # special handling for varargs
                        if (param == lms2012.Param.PARNO)
                        {
                            int lastvalue = (int)retpars.Last().data;
                            if (lastvalue == 0)
                            {
                                retpars.RemoveAt(retpars.Count - 1);
                            }
                            else
                            {
                                for (int i = 0; i < lastvalue; i++)
                                {
                                    LMSParam param3 = parse_param(lms2012.Param.PARV, fs);
                                    retpars.Add(param3);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return retpars.ToArray();
        }

        LMSParam parse_param(Enum param, Stream infile)
        {
            var first_byte = infile.ReadByte();
            LMSParam retdata = new LMSParam() { scope = null };
            if ((first_byte & lms2012.PRIMPAR_LONG) != 0)
            {
                if ((first_byte & lms2012.PRIMPAR_VARIABLE) != 0)
                {
                    if ((first_byte & lms2012.PRIMPAR_GLOBAL) != 0)
                        retdata.scope = "GLOBAL";
                    else
                        retdata.scope = "LOCAL";
                    var size = first_byte & lms2012.PRIMPAR_BYTES;
                    retdata.data = null;
                    if (size == lms2012.PRIMPAR_1_BYTE)
                    {
                        var datasize = 1;
                        byte[] data1 = new byte[datasize];
                        infile.Read(data1, 0, datasize);
                        retdata.data = unchecked((sbyte)data1[0]);
                    }
                    else if (size == lms2012.PRIMPAR_2_BYTES)
                    {
                        var datasize = 2;
                        byte[] data1 = new byte[datasize];
                        infile.Read(data1, 0, datasize);
                        retdata.data = BitConverter.ToUInt16(data1, 0);
                    }
                    else if (size == lms2012.PRIMPAR_4_BYTES)
                    {
                        var datasize = 4;
                        byte[] data1 = new byte[datasize];
                        infile.Read(data1, 0, datasize);
                        //retdata.data = BitConverter.ToUInt32(data1, 0);
                        retdata.data = BitConverter.ToSingle(data1, 0);
                    }
                    if ((first_byte & lms2012.PRIMPAR_HANDLE) != 0)
                        retdata.handle = "@";
                    else if ((first_byte & lms2012.PRIMPAR_ADDR) != 0)
                        throw new NotImplementedException();
                    return retdata;
                }
                else //# PRIMPAR_CONST
                {
                    if ((first_byte & lms2012.PRIMPAR_LABEL) != 0)
                    {
                        retdata.data = infile.ReadByte();
                        retdata.handle = "LABEL";
                        return retdata;
                    }
                    var size = first_byte & lms2012.PRIMPAR_BYTES;
                    if (param is lms2012.Param && (lms2012.Param)param == lms2012.Param.PARF)
                    {
                        if (size != lms2012.PRIMPAR_4_BYTES)
                            throw new Exception("Expecting float value");
                        byte[] data1 = new byte[4];
                        infile.Read(data1, 0, 4);
                        int int_value = BitConverter.ToInt32(data1, 0);
                        if (int_value == lms2012.DATAF_MAX) { retdata.data = "DATAF_MAX"; }
                        else if (int_value == lms2012.DATAF_MIN) { retdata.data = "DATAF_MIN"; }
                        else if (int_value == lms2012.DATAF_NAN) { retdata.data = "DATAF_NAN"; }
                        else
                        {
                            retdata.data = BitConverter.ToSingle(data1, 0);
                            retdata.postfix = "F";
                        }
                        return retdata;
                    }
                    if (size == lms2012.PRIMPAR_STRING_OLD || size == lms2012.PRIMPAR_STRING)
                    {
                        string retval = parse_string(infile);
                        retdata.data = retval;
                        return retdata;
                    }
                    if (size == lms2012.PRIMPAR_1_BYTE)
                    {
                        var datasize = 1;
                        byte[] data1 = new byte[datasize];
                        infile.Read(data1, 0, datasize);
                        retdata.data = unchecked((sbyte)data1[0]);
                        return retdata;
                    }
                    else if (size == lms2012.PRIMPAR_2_BYTES)
                    {
                        var datasize = 2;
                        byte[] data1 = new byte[datasize];
                        infile.Read(data1, 0, datasize);
                        retdata.data = BitConverter.ToInt16(data1, 0);
                        return retdata;
                    }
                    else if (size == lms2012.PRIMPAR_4_BYTES)
                    {
                        var datasize = 4;
                        byte[] data1 = new byte[datasize];
                        infile.Read(data1, 0, datasize);
                        //retdata.data = BitConverter.ToUInt32(data1, 0);
                        retdata.data = BitConverter.ToSingle(data1, 0);

                        return retdata;
                    }
                }
            }
            else // # PRIMPAR_SHORT
            {
                if ((first_byte & lms2012.PRIMPAR_VARIABLE) != 0)
                {
                    if ((first_byte & lms2012.PRIMPAR_GLOBAL) != 0)
                        retdata.scope = "GLOBAL";
                    else
                        retdata.scope = "LOCAL";
                    retdata.data = first_byte & lms2012.PRIMPAR_INDEX;
                    return retdata;
                }
                else
                {
                    if ((first_byte & lms2012.PRIMPAR_CONST_SIGN) != 0)
                    {
                        // # special handling for negative numbers
                        retdata.data = (first_byte & lms2012.PRIMPAR_VALUE) - (lms2012.PRIMPAR_VALUE + 1);
                        return retdata;
                    };
                    retdata.data = first_byte & lms2012.PRIMPAR_VALUE;
                    return retdata;
                }
            }
            throw new Exception("Impossible value combination");
        }

        string parse_string(Stream infile)
        {
            string value = "";
            while (true)
            {
                Char ch = (Char)infile.ReadByte();
                if (ch == 0) break;
                value += ch;
            }
            value = value.Replace("\t", "\\t");
            value = value.Replace("\r", "\\r");
            value = value.Replace("\n", "\\n");
            value = value.Replace("'", "\\q");
            return $"'{value}'";
        }

        lms2012.ObjectHeader parse_object_header(Stream fs)
        {
            int count = Marshal.SizeOf(typeof(lms2012.ObjectHeader));
            byte[] readBuffer = new byte[count];
            BinaryReader reader = new BinaryReader(fs);
            readBuffer = reader.ReadBytes(count);
            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            var header = (lms2012.ObjectHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(lms2012.ObjectHeader));
            handle.Free();

            return header;
        }
        (string version, Int16 num_objs, Int32 global_bytes) parse_program_header(Stream fs, long size)
        {
            int count = Marshal.SizeOf(typeof(lms2012.ProgramHeader));
            byte[] readBuffer = new byte[count];
            BinaryReader reader = new BinaryReader(fs);
            readBuffer = reader.ReadBytes(count);
            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            var header = (lms2012.ProgramHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(lms2012.ProgramHeader));
            handle.Free();

            if (header.lego != "LEGO")
                throw new Exception("Bad file - does not start with 'LEGO'");
            if (header.size != size)
                throw new Exception("Bad file - size is incorrect");

            return (header.byte_code_version, header.num_objects, header.global_bytes);
        }
    }
}
