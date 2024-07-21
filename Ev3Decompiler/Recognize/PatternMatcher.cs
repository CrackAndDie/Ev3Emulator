using EV3DecompilerLib.Decompile;
using EV3ModelLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static EV3DecompilerLib.Decompile.lms2012;

namespace EV3DecompilerLib.Recognize
{
    /// <summary>
    /// Pattern matcher to recognize EV3-G blocks from LMS disassembly
    /// </summary>
    public class PatternMatcher : IProjectConverter
    {
        public static string BYTECODE_057_DECODING_VERSION = "0.57";
        public static int DEBUG_PRINT_LEVEL = 0;
        internal List<EV3GBlock> pmlCollectedMyBlocks = new List<EV3GBlock>();
        internal Dictionary<int, EV3GBlock> dictCollectedMyBlocks_by_MyBlockID = new Dictionary<int, EV3GBlock>();
        internal Dictionary<int, int> OBJECTID_2_MyBlockID = new Dictionary<int, int>();
        internal Dictionary<int, string> GlobalVariables_GLOBALID_2_VariableId = new Dictionary<int, string>();
        internal Dictionary<string, string> GlobalVariables_VariableId_2_VariableType = new Dictionary<string, string>();
        internal Dictionary<string, (string value, CallparamExt type)> CollectedGlobalParams = new Dictionary<string, (string, CallparamExt)>();
        internal Dictionary<string, CallparamExt> CollectedArrayTypes = new Dictionary<string, CallparamExt>();
        internal Dictionary<int, EV3GBlock> CollectedForkObjects = new Dictionary<int, EV3GBlock>();
        internal Dictionary<string, string> CollectedLoopInterrupts = new Dictionary<string, string>();


        /// <summary>
        /// Main entry point to recognize EV3 calls
        /// </summary>
        /// <param name="prg"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public (EV3GBlock program, List<EV3GBlock> myblocks) RecognizeEV3Calls(LMSProgram prg, string filename)
        {
            LMSObject vmmain = prg.Objects[0];

            //-- collect GLOBAL constants (strings and arrays) from MAIN object (vmthread)
            CollectGlobalParameters(vmmain);

            //-- collect all loop names
            CollectLoopNames(prg);

            //-- start main program processing
            vmmain.Comment = new LMSComment("Main program");
            var retval = AnalyzeTopLevelObject(prg, vmmain, null, 0, false, true);
            retval.Name = Path.GetFileNameWithoutExtension(filename);
            retval.NodeType = EV3GBlock.GNodeType.TopLevel;

            //-- version check
            if (prg.version != BYTECODE_057_DECODING_VERSION)
            {
                if (retval.Count == 0)
                {
                    EV3GBlock gmessage = new EV3GBlock(null, "ERROR");
                    gmessage.Parameters.Add("Message", $"Incompatible byte code version {prg.version}");

                    retval.Add(gmessage);
                }
            }

            if (DEBUG_PRINT_LEVEL >= 1) Console.WriteLine();
            return (retval, pmlCollectedMyBlocks);
        }

        /// <summary>
        /// Main entry point to recognize EV3 calls
        /// </summary>
        /// <param name="prg"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ProjectResultData RecognizeEV3Calls2(LMSProgram prg, string filename)
        {
            (EV3GBlock ev3gProgram, List<EV3GBlock> ev3gMyblocks) = RecognizeEV3Calls(prg, filename);

            var program = NodeConversion.Convert(ev3gProgram);
            var myblocks = ev3gMyblocks.Select(elem => NodeConversion.Convert(elem)).ToList();

            //-- add all blocks to a dictionary
            Dictionary<string, Node> retblocks = new Dictionary<string, Node>();
            retblocks.Add(program.Name, program);
            myblocks.ForEach(elem => retblocks.Add(elem.Name, elem));

            //-- project creation
            Project project = new Project(Path.GetFileName(filename));
            project.CreateProject(retblocks.Values.ToList());
            //RBFProject project = new RBFProject(Path.GetFileName(filename));
            //project.CreateProject(this, ev3gProgram, ev3gMyblocks);

            //-- return result
            return (retblocks, project);
        }

        /// <summary>
        /// Extension based conversion
        /// </summary>
        /// <param name="instream"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        ProjectResultData IProjectConverter.Convert(Stream instream, string filename)
        {
            var lmsprogram = new EV3DecompilerLib.Decompile.Decompiler().BuildByteCodeForFile(instream, filename);
            return RecognizeEV3Calls2(lmsprogram, filename);
        }

        /// <summary>
        /// static ctor
        /// </summary>
        static PatternMatcher()
        {
            RegisterConversion();
        }
        public static void RegisterConversion()
        {
            ProjectConversion.RegisterConverter(".rbf", typeof(PatternMatcher));
        }

        /// <summary>
        /// Collect all loop names from loop interrupts (this contains string reference only) and globals
        /// </summary>
        /// <param name="prg"></param>
        private void CollectLoopNames(LMSProgram prg)
        {
            // collect all loop interrupt blocks (there should be 0 or 1, NI does not seem to duplicate this block, but let us be sure...)
            var patternLoopInterrupt = Pattern.PATTERNS.FirstOrDefault(elem => elem.Name == Pattern.BLOCK_Interrupt); if (patternLoopInterrupt == null) return;
            var objidsLoopInterrupts = prg.Objects
                .Where(obj => CheckSubPatternMatch(obj, 0, patternLoopInterrupt.Elems, out int _))
                .Select(obj => obj.Id)
                .ToList();
            if (objidsLoopInterrupts.Count > 0)
            {
                prg.Objects.ForEach(obj => obj.Statements.ForEach(
                    line =>
                    {
                        if (line.Operator == lms2012.Op.CALL)
                        {
                            int objid = Int32.Parse(line.Params[0].data.ToString());
                            if (objidsLoopInterrupts.Contains(objid))
                            {
                                // this is a loop interrupt line --> store value
                                // loopid,@cmd.globals @out.loopname // duplicate from pattern, optimize this later
                                var loopid = line.Params[1].ToString();
                                (var loopname, _) = _GlobalScopeValue(line.Params[2], Callparam.IN_S);
                                CollectedLoopInterrupts[loopid] = loopname;
                            }
                        }
                    }
                    ));
            }
        }

        /// <summary>
        /// Collect all global string and array constants from main object
        /// </summary>
        /// <param name="vmmain"></param>
        private void CollectGlobalParameters(LMSObject vmmain)
        {
            vmmain.Statements.ForEach(line =>
            {
                switch (line.Operator)
                {
                    case lms2012.Op.STRINGS:
                        if (line.Params[0].data.ToString() == "DUPLICATE")
                        {
                            var key = line.Params[2].ToStringWOHandle();
                            var value = line.Params[1].ToString().Trim('\'',' ');
                            var vartype = new CallparamExt(lms2012.Callparam.IN_S, true);
                            CollectedGlobalParams[key] = (value, vartype);
                            CollectedArrayTypes[key] = vartype;
                        }
                        break;
                    case lms2012.Op.ARRAY:
                        if (line.Params[0].data.ToString() == "INITF" || line.Params[0].data.ToString() == "INIT8")
                        {
                            var key = line.Params[1].ToStringWOHandle();
                            // variable number of out parameters - array
                            int numvalues = Int32.Parse(line.Params[3].ToString());
                            var paramData_sub = new List<string>();
                            for (int ivalue = 0; ivalue < numvalues; ivalue++)
                                paramData_sub.Add(line.Params[3 + 1 + ivalue].ToString(false, true)); //exclude postfix from GLOBALS (not needed for recognition, just output)
                                                                                                      // "array" + line.Params[0].data.ToString().ToLower().Last()
                            var datatype = line.Params[0].data.ToString().Last();
                            lms2012.Callparam datatype_cp =
                                    datatype == '8' ? lms2012.Callparam.IN_8 :
                                    /*datatype == 'F' ?*/ lms2012.Callparam.IN_F;
                            var value = $"[{string.Join(", ", paramData_sub)}]";
                            CollectedGlobalParams[key] = (value, new CallparamExt(datatype_cp, true));
                        }
                        else if (line.Params[0].data.ToString() == "CREATEF" || line.Params[0].data.ToString() == "CREATE8")
                        {
                            var key = line.Params[2].ToStringWOHandle();
                            var datatype = line.Params[0].data.ToString().Last();
                            var datatype_str = datatype == '8' ? Pattern.BLOCK_DataType_Boolean :
                                    datatype == 'F' ? Pattern.BLOCK_DataType_Numeric :
                                    null;
                            CollectedArrayTypes[key] = new CallparamExt(datatype == '8' ? Callparam.IN_8 : Callparam.IN_F, true);
                        }
                        break;
                }
            });
        }

        /// <summary>
        /// Search a top level object (VM, TRIG/FORK, MyBlock) for patterns
        /// </summary>
        /// <param name="prg"></param>
        /// <param name="lmsobj"></param>
        /// <param name="printdepth"></param>
        /// <returns></returns>
        internal EV3GBlock AnalyzeTopLevelObject(LMSProgram prg, LMSObject lmsobj, EV3GBlock pmdroot, int printdepth,
            bool isMyBlock, bool isLookingForTopSignatureWithJR)
        {
            EV3GBlock pmdlretval = new EV3GBlock(pmdroot);
            EV3GBlock pmdltoplevel = pmdlretval;
            int lineidx = 0;
            bool isThisMainObject = lmsobj.Id == 1 && lmsobj.header.is_vmthread;

            //-- if vmthread or MyBLOCK (but not in LEGO subcalls) advance to first "JR"
            //-- once in a vmthread or in a myblock there must be a initial (locking) block ending with JR
            //-- vmthread starts with constants (text and arrays)
            bool isTopSignatureFoundWithJR = false;
            if (!isLookingForTopSignatureWithJR) isTopSignatureFoundWithJR = true;
            if (isLookingForTopSignatureWithJR)
            {
                LMSStatement line = null;
                do
                {
                    line = lmsobj.Statements[lineidx++];
                } while (line.Operator != lms2012.Op.JR && lineidx < lmsobj.Statements.Count);

                //-- if JR is still needed and is not found (example: DuplicateFORKS.rbf), rewind and start processing from the beginning
                if (isTopSignatureFoundWithJR || line.Operator != lms2012.Op.JR) { lineidx = 0; }
                else { isTopSignatureFoundWithJR = true; }
            }

            //-- process all lines
            while (lineidx < lmsobj.Statements.Count)
            {
                var line = lmsobj.Statements[lineidx];

                #region PATTERN MATCHING
                //============================================================================================================
                int matched_lines;

                //==================================================================================
                //== LOOP_START_PATTERN
                if (CheckSubPatternMatch(lmsobj, lineidx, Pattern.LOOP_START_PATTERN, out matched_lines))
                {
                    //-- if LOOP pattern is found
                    var offsetEnd = lmsobj.Statements[lineidx + matched_lines - 1].Offset;
                    // second line contains loop interrupt mask -> AND32(LOCAL48,2,LOCAL44)
                    var loop_interrupt_id = int.Parse(lmsobj.Statements[lineidx + 1].Params[1].data.ToString());
                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found LOOP_START pattern with internal id {loop_interrupt_id} at offset: {line.Offset}-{offsetEnd}", lmsobj.Id);

                    //TODO: handle Index block -- probably only show if used in loop or later

                    //-- temporary: start loop will be replaced by the end loop condition later
                    var pmd = new EV3GBlock(pmdltoplevel)
                    {
                        Name = Pattern.BLOCK_StartLoop,
                        Offset = line.Offset,
                        OffsetLineId = lineidx,
                        OffsetEndLineId = lineidx + matched_lines - 1,
                        NodeType = EV3GBlock.GNodeType.Loop
                    };
                    var sloopid = loop_interrupt_id.ToString();
                    pmd.Parameters.Add(Node.PARAM_LOOP_InterruptId, sloopid);
                    if (CollectedLoopInterrupts.ContainsKey(sloopid))
                        pmd.Parameters.Add(Node.PARAM_LOOP_InterruptName, CollectedLoopInterrupts[sloopid]);
                    pmdltoplevel.Add(pmd);
                    pmdltoplevel = pmd;
                    printdepth++;

                    // set comment for matched lines
                    SetCommentForMatchedLines(lmsobj, lineidx, matched_lines, pmd);

                    lineidx += matched_lines;
                    continue;
                }

                //==================================================================================
                //== WAITFOR_LOOPEND_PATTERN
                else if (CheckSubPatternMatch(lmsobj, lineidx, Pattern.WAITFOR_LOOPEND_PATTERN, out matched_lines))
                {
                    //-- ERROR if there is absolutely no statements within loop (not even waitfor/stop conditions)
                    if (pmdltoplevel.Count == 0) { _WriteLine(printdepth, $"ERROR: WaitFor/LoopEnd missing in {lmsobj.ToString()} # {line.Offset}"); lineidx++; continue; }
                    //-- if WAITFOR pattern is found, add this to the LAST statement as it is positioned AFTER the sensor block
                    pmdltoplevel.Last().Name = Pattern.BLOCK_WaitForPrefix + pmdltoplevel.Last().Name;
                    pmdltoplevel.Last().HasWait = true;
                    // set comment for matched lines
                    SetCommentForMatchedLines(lmsobj, lineidx, matched_lines, pmdltoplevel.Last());

                    // assume JR_TRUE statement is first e.g JR_TRUE(LOCAL24,OFFSET1_193) --> first variable (compareresult typically) can be removed from the result as it is consumed
                    var jrtrueline = line;
                    var jtruearg = line.Params[0];
                    var matchedarg = pmdltoplevel.Last().Parameters.FirstOrDefault(elem => elem.Value == jtruearg.ToString());
                    if (matchedarg.Key != null)
                        pmdltoplevel.Last().Parameters.Remove(matchedarg.Key);

                    // find JR statement, (now assume it is statement #3) and check jump
                    var jmpline = lmsobj.Statements[lineidx + 2];
                    if (jmpline.Operator != lms2012.Op.JR) { if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, "ERROR: MISSING JR statement!"); }
                    long jmpoffset = (long)jmpline.Params[0].offsetdata;
                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found WAITFOR_LOOPEND pattern at offset: {line.Offset}-{lmsobj.Statements[lineidx + matched_lines - 1].Offset} to {jmpoffset}", lmsobj.Id);
                    //IDEA: consider also: waitfor points to the previous lmsobj[pmdltoplevel.Last().OffsetLineId].Offset == jmpoffset
                    if (pmdltoplevel.NodeType == EV3GBlock.GNodeType.Loop &&
                        pmdltoplevel.Offset <= jmpoffset && jmpoffset <=
                        lmsobj.Statements[Math.Min(pmdltoplevel.OffsetEndLineId, lmsobj.Statements.Count - 1)].Offset)
                    {
                        //-- if jump points somewhere at the start of the Loop (LoopStart Sequence or next statement) - this OK
                        //-- this waitfor is actually a loop_end condition
                        var lastnode = pmdltoplevel.Last();

                        //-- move this node up to the parent node, replacing StartLoop
                        var tmproot = pmdltoplevel;
                        var idxOfRootAtParent = pmdltoplevel.Index;
                        pmdltoplevel.Remove(lastnode); pmdltoplevel.Root[idxOfRootAtParent] = lastnode;
                        lastnode.NodeType = EV3GBlock.GNodeType.Loop; lastnode.HasWait = false;
                        lastnode.AddRange(pmdltoplevel);

                        // set comment for matched lines
                        SetCommentForMatchedLines(lmsobj, lineidx, matched_lines, lastnode);

                        //-- close the scope
                        pmdltoplevel = pmdltoplevel.Root;
                        printdepth--;
                        if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, "Loop END");
                    }

                    lineidx += matched_lines;
                    continue;
                }

                //==================================================================================
                //== CALL_BLOCK_PATTERN
                else if (CheckSubPatternMatch(lmsobj, lineidx, isThisMainObject ? Pattern.CALL_BLOCK_PATTERN_MAIN : Pattern.CALL_BLOCK_PATTERN, out matched_lines))
                {
                    //TODO: ADD32 exists in main, myblocks, block forked through main (block forks below main)

                    //--  if CALL_BLOCK pattern is found
                    //skip print, just annoying -> if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found CALL_BLOCK pattern at offset: {line.Offset}-{lmsobj.Statements[lineidx + matched_lines - 1].Offset}", lmsobj.Id);
                    var callline = lmsobj.Statements.Skip(lineidx).Take(matched_lines).First(elem => elem.Operator == lms2012.Op.CALL);
                    EV3GBlock pmd = CheckSubCall(prg, lmsobj, printdepth /*+ 1*/, pmdltoplevel, callline, lineidx + matched_lines - 1);

                    // add tracking global members
                    AddTrackingGlobals(lmsobj, lineidx, matched_lines, pmd);

                    // set comment for matched lines
                    if (pmd != null)
                        SetCommentForMatchedLines(lmsobj, lineidx, matched_lines, pmd);

                    lineidx += matched_lines;
                    continue;
                }

                //==================================================================================
                //== MATHADV_BLOCK_PATTERN
                else if (CheckSubPatternMatch(lmsobj, lineidx, Pattern.MATHADV_BLOCK_PATTERN, out matched_lines))
                {
                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found: MATHADV_BLOCK_PATTERN {/*pmd*/ ""}");

                    List<string> sops = new List<string>();
                    Dictionary<string, (string expression, int precedence)> sops1 = new Dictionary<string, (string, int)>();
                    /*static readonly */
                    IReadOnlyDictionary<string, (string friendly, int precedence, bool associative)> ExpandMathFunctions = new Dictionary<string, (string, int, bool)>()
                    { ["ADDF"] = ("+", 1, true), ["SUBF"] = ("-", 1, false), ["MULF"] = ("*", 2, true), ["DIVF"] = ("/", 2, false), ["MOD"] = ("%", 2, false), ["POW"] = ("^", 3, false) };
                    string friendlyformat = null;
                    string partarget = null;
                    /*static readonly */
                    IReadOnlyList<(string, string)> MATH_ADV_PARNAMES = new (string Param, string Display)[] {
                            ("X","a"), ("Y","b"), ("C","c"), ("D","d") };
                    var outboundParNames = new Queue<(string Param, string Display)>(MATH_ADV_PARNAMES);
                    var outboundParSubstitutions = new Dictionary<string, (string Param, string Display)>();
                    foreach (var line1 in lmsobj.Statements.Skip(lineidx).Take(matched_lines).ToArray())
                    {
                        bool ismath = line1.Operator == lms2012.Op.MATH;
                        var op = !ismath ? line1.Operator.ToString() : line1.Params[0].ToString();
                        var par1 = line1.Params[!ismath ? 0 : 1].ToString(false, true);
                        int par1prec = 99, par2prec = 99;
                        //handle unary ops (SQRT,LN,LOG) argument only 
                        var unary = ismath && line1.Params.Count == 3;
                        var par2 = unary ? null : line1.Params[!ismath ? 1 : 2].ToString(false, true);
                        bool par1_issimple = true;
                        bool par2_issimple = true;
                        partarget = line1.Params.Last().ToString(false, true); // this is the taret local variable - caarying over the result
                        sops.Add(op);
                        var precedence = 99;

                        void ProcessVariable(ref string par, ref int prec, ref bool issimple)
                        {
                            if (par.StartsWith("LOCAL"))
                            {
                                if (sops1.ContainsKey(par))
                                {
                                    prec = sops1[par].precedence;
                                    par = sops1[par].expression;
                                    issimple = false;
                                }
                                else
                                {
                                    if (!outboundParSubstitutions.ContainsKey(par)) 
                                        outboundParSubstitutions[par] = outboundParNames.Dequeue(); // TODO: CHECK it any remaining
                                    string newName = outboundParSubstitutions[par].Display;
                                    par = newName;
                                }
                            }
                        }
                        ProcessVariable(ref par1, ref par1prec, ref par1_issimple);
                        if (!unary) ProcessVariable(ref par2, ref par2prec, ref par2_issimple);

                        // problem a-b-c != a-(b-c)

                        friendlyformat = null;
                        if (ExpandMathFunctions.ContainsKey(op))
                        {
                            // par1, par2 precedence counts --> use that
                            var exfunc = ExpandMathFunctions[op];
                            precedence = exfunc.precedence;
                            op = exfunc.friendly;
                            if (par1prec < precedence) par1 = $"({par1})";
                            if (par2prec < precedence || (par2prec == precedence && !exfunc.associative && !par2_issimple)) par2 = $"({par2})";
                            friendlyformat = $"{par1}{op}{par2}";
                        }
                        else
                        {
                            friendlyformat = $"{op}({par1}{(unary ? null : ",")}{par2})";
                        }
                        sops1[partarget] = (friendlyformat, precedence);
                    }

                    var pmd = new EV3GBlock(pmdltoplevel)
                    {
                        Name = Pattern.BLOCK_MathAdvanced,
                        Offset = line.Offset,
                        OffsetLineId = lineidx,
                        OffsetEndLineId = lineidx + matched_lines - 1,
                    };
                    pmd.Parameters.Add("Equation", friendlyformat);
                    pmd.Parameters.Add("@Result", partarget);
                    // add params to the equation X,Y,C,D
                    foreach (var kvp in outboundParSubstitutions.ToList())
                        pmd.Parameters.Add(kvp.Value.Param, kvp.Key);

                    // add node to toplevel
                    pmdltoplevel.Add(pmd);

                    // set comment for matched lines
                    SetCommentForMatchedLines(lmsobj, lineidx, matched_lines, pmd);

                    lineidx += matched_lines;
                    continue;
                }

                //==================================================================================
                //== VARIABLE_WRITE_PATTERN
                else if (CheckSubPatternMatch(lmsobj, lineidx, Pattern.VARIABLE_WRITE_PATTERN, out matched_lines))
                {
                    //-- get move statement --> 1_67:   MOVEF_F(999F,GLOBAL0) --> approach from the end
                    var moveline = lmsobj.Statements.Skip(lineidx).Take(matched_lines).Reverse()
                        .First(elem => elem.Operator == lms2012.Op.MOVEF_F || elem.Operator == lms2012.Op.MOVE8_8 || elem.Operator == lms2012.Op.ARRAY);

                    CallparamExt vartype;
                    string varvalue = null;
                    int varev3gid = -1;
                    if (moveline.Operator == lms2012.Op.ARRAY)
                    {
                        // this is an array - check whether this is a global --> check desttype GLOBALS (INITF/INIT8/STRINGS(DUPLICATE))
                        vartype = CollectedArrayTypes[moveline.Params[2].ToStringWOHandle()];
                        //source
                        varvalue = _GlobalScopeValue(moveline.Params[1], vartype).value;
                        //destination
                        varev3gid = int.Parse(moveline.Params[2].data.ToString());
                    }
                    else
                    {
                        vartype = moveline.Operator == lms2012.Op.MOVE8_8 ? Callparam.IN_8 : Callparam.IN_F;
                        vartype.SetDirection(true);
                        //vartype = new CallparamExt((Callparam)((int)vartype.callparam & (int)~DataDirection.OUT | (int)DataDirection.IN),
                        //    vartype.IsArrayType);
                        //source
                        varvalue = moveline.Params[0].ToString();
                        //destination
                        varev3gid = int.Parse(moveline.Params[1].data.ToString());
                    }
                    var varname = GetEV3GVariableId(varev3gid, vartype.GetOutboundType());

                    var pmd = new EV3GBlock(pmdltoplevel)
                    {
                        Name = Pattern.BLOCK_WriteVariablePrefix + vartype.GetOutboundType(),
                        // Variable.Write.WriteText, Variable.Write.WriteNumeric, Variable.Write.WriteBoolean
                        // Variable.Write.WriteNumericArray, Variable.Write.WriteBooleanArray
                        Offset = line.Offset,
                        OffsetLineId = lineidx,
                        OffsetEndLineId = lineidx + matched_lines - 1,
                        Parameters = new Dictionary<string, string>()
                        {
                            ["name"] = varname,
                            ["valueIn"] = varvalue,
                        }
                    };
                    pmdltoplevel.Add(pmd);

                    // add tracking global members
                    AddTrackingGlobals(lmsobj, lineidx, matched_lines, pmd);

                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found: VARIABLE_WRITE_PATTERN {pmd}");

                    // set comment for matched lines
                    SetCommentForMatchedLines(lmsobj, lineidx, matched_lines, pmd);

                    lineidx += matched_lines;
                    continue;
                }

                //==================================================================================
                //== VARIABLE_READ_PATTERN
                else if (CheckSubPatternMatch(lmsobj, lineidx, Pattern.VARIABLE_READ_PATTERN, out matched_lines))
                {
                    //== Variable Read block
                    //1_97:   MOVEF_F(GLOBAL0,LOCAL0) / ARRAY(COPY,GLOBAL46,LOCAL12)

                    var moveline = line;
                    CallparamExt vartype;
                    LMSParam resultsourcevar =
                        (moveline.Operator == lms2012.Op.ARRAY) ? // vartype is "Text (or array)"
                        moveline.Params[1] : moveline.Params[0];
                    LMSParam resulttargetvar =
                        (moveline.Operator == lms2012.Op.ARRAY) ? // vartype is "Text (or array)"
                        moveline.Params[2] : moveline.Params[1];

                    if (moveline.Operator == lms2012.Op.ARRAY)
                    {
                        // this is an array - check whether this is a global --> check desttype GLOBALS (INITF/INIT8/STRINGS(DUPLICATE))
                        vartype = CollectedArrayTypes[resultsourcevar.ToStringWOHandle()];
                        //vartype = new CallparamExt((Callparam)((int)vartype.callparam & (int)~DataDirection.IN | (int)DataDirection.OUT),
                        //    vartype.IsArrayType);
                        vartype.SetDirection(false);
                    }
                    else
                    {
                        vartype = moveline.Operator == lms2012.Op.MOVE8_8 ? Callparam.OUT_8 : Callparam.OUT_F;
                    }

                    int varev3gid = int.Parse(resultsourcevar.data.ToString());
                    string varname = GetEV3GVariableId(varev3gid, vartype.GetOutboundType());

                    var pmd = new EV3GBlock(pmdltoplevel)
                    {
                        Name = Pattern.BLOCK_ReadVariablePrefix + vartype.GetOutboundType(),
                        // Variable.Read.ReadNumeric, Variable.Read.ReadText, Variable.Read.ReadBoolean
                        // Variable.Read.ReadNumericArray, Variable.Read.ReadBooleanArray
                        Offset = line.Offset,
                        OffsetLineId = lineidx,
                        OffsetEndLineId = lineidx + matched_lines - 1,
                        Parameters = new Dictionary<string, string>()
                        {
                            ["name"] = varname,
                            ["@Result.valueOut"] = resulttargetvar.ToString(),
                        }
                    };
                    pmdltoplevel.Add(pmd);

                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found: VARIABLE_READ_PATTERN {pmd}");

                    // set comment for matched lines
                    SetCommentForMatchedLines(lmsobj, lineidx, matched_lines, pmd);

                    lineidx += matched_lines;
                    continue;
                }

                //==================================================================================
                //== SWITCH: SWITCH Header and case offsets
                else if ((line.Operator == lms2012.Op.JR_EQ32 || line.Operator == lms2012.Op.JR_EQ8) &&
                    pmdltoplevel.Count > 0 && !pmdltoplevel.Last().HasSwitch)
                {
                    try
                    {  // temp!!!
                       //-- start only if switch handling is not in progress
                       // JR_EQ32(LOCAL0, 0, OFFSET1_77)
                       // start checking for switch node
                        var pmSwitchCondition = pmdltoplevel.Last();
                        if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found: SWITCH_JUMP_SECTION Condition:{pmSwitchCondition.Name}");

                        pmSwitchCondition.HasSwitch = true;
                        bool isCompareText = pmSwitchCondition.Name == Pattern.BLOCK_CaseSelectorString; //todo: more roboust solution
                        pmSwitchCondition.Name = Pattern.BLOCK_SwitchPrefix + pmSwitchCondition.Name;

                        //-------------------------------
                        //-- SWITCH: identify CASE offsets
                        var linesub = line;
                        var caseoffsets = new List<Tuple<string, long>>();
                        string last_case_value = null;
                        while (linesub != null && linesub.Operator == line.Operator)
                        {
                            var case_value = linesub.Params[1].data.ToString();
                            if (isCompareText)
                            {
                                // peek last STRINGS(COMPARE)
                                var linesubprev = linesub.Previous();
                                if (linesubprev.Operator != lms2012.Op.STRINGS) { if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, "ERROR: Missing STRINGS statement"); }
                                // expecting: STRINGS(COMPARE,@LOCAL12,'HELLO',LOCAL0)
                                case_value = linesubprev.Params[2].data.ToString();
                            }

                            var case_offset = linesub.Params[2].offsetdata;
                            caseoffsets.Add(new Tuple<string, long>(last_case_value, case_offset));

                            last_case_value = case_value;
                            linesub = linesub.Next();

                            if (isCompareText && linesub.Operator == lms2012.Op.STRINGS)
                            {
                                // if next statement is STRINGS(COMPARE) skip
                                linesub = linesub.Next();
                            }
                        }

                        // set comment for matched lines
                        SetCommentForMatchedLines(lmsobj, lineidx, linesub.LineIndex - lineidx + 1, pmSwitchCondition);

                        {
                            // last elem of any case (at offset) is JR --> this is the end of the full switch - add as last case
                            var line_JR = lmsobj.Statements[lmsobj.LineIndexOfOffset(caseoffsets[0].Item2)].Previous();
                            if (line_JR.Operator != lms2012.Op.JR) throw new Exception("JR operator was expected");
                            var case_offset = line_JR.Params[0].offsetdata;
                            caseoffsets.Add(new Tuple<string, long>(last_case_value, case_offset));
                        }

                        //-------------------------------
                        //-- SWITCH: switch body parts -> iterate through case body parts
                        foreach (var caseoffset_tup in caseoffsets)
                        {
                            var pmcasenode = new EV3GBlock(pmSwitchCondition)
                            {
                                Name = Pattern.BLOCK_SwitchCaseItem,
                                OffsetLineId = pmSwitchCondition.Count > 0 ? pmSwitchCondition.Last().OffsetEndLineId : linesub.LineIndex,
                                OffsetEndLineId = lmsobj.LineIndexOfOffset(caseoffset_tup.Item2) - 1
                            };
                            // get case_value attribute
                            //var case_value_attribute = pmSwitchCondition.Parameters.FirstOrDefault(elem => !elem.Key.StartsWith("@Result")).Key;
                            pmcasenode.Parameters = new Dictionary<string, string>()
                            {
                                [Node.PARAM_SWITCH_Pattern] = caseoffset_tup.Item1 ?? Pattern.CASE_DEFAULT_Value
                            };
                            var offset1 = pmcasenode.Offset = lmsobj.Statements[pmcasenode.OffsetLineId].Offset;
                            var offset2 = lmsobj.Statements[pmcasenode.OffsetEndLineId].Offset;
                            if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth + 1, $"Case({pmcasenode.Parameters["case_value"]}) Offset:{offset1}-{offset2}");
                            pmSwitchCondition.Add(pmcasenode);

                            // set comment for matched lines
                            SetCommentForMatchedLines(lmsobj, pmcasenode.OffsetLineId, pmcasenode.OffsetEndLineId - pmcasenode.OffsetLineId + 1, pmcasenode, false);
                        };

                        // move top level node to the first case
                        pmdltoplevel = pmSwitchCondition.First();
                        // (if matched) increase line idx to end of switch header
                        lineidx = lmsobj.Statements.IndexOf(linesub);

                        // if first statement is MOVE32_32(0,LOCAL) --> skip it //TODO: no idea what it does
                        linesub = lmsobj.Statements[lineidx];
                        if (linesub.Operator == lms2012.Op.MOVE32_32 &&
                                linesub.Params[0].ToString() == "0" && linesub.Params[1].ToString().StartsWith("LOCAL"))
                        {
                            lineidx++;
                        }
                        if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"CASE_START: {pmSwitchCondition.First()}");
                    }
                    catch (Exception)
                    {
                        //TODO: rewind switch modifications
                        if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, "SWITCH_JUMP_SECTION matching failed");
                        //pmdltoplevel.Last().Clear();
                        //pmdltoplevel.Last().IsSwitchNode = false;
                        //lineidx++;
                    }
                }

                //==================================================================================
                //== SWITCH: NEXT CASE BODY
                else if (pmdltoplevel.Root?.HasSwitch == true && line.Operator == lms2012.Op.JR)
                {
                    // this is CASE switch (end)
                    // find proper subcase node (already added)
                    // if there is one, make it top level, if this was the last one, pop to original context

                    var pmSwitchCondition = pmdltoplevel.Root;
                    var pmCaseNode = pmSwitchCondition.First(elem => elem.OffsetEndLineId == lineidx);
                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"CASE_END: {pmCaseNode}");
                    // set comment for matched lines
                    //SetCommentForMatchedLines(lmsobj, lineidx, 1, $"CASE_END: {pmCaseNode}", true); //!!

                    var idxOfCase = pmSwitchCondition.IndexOf(pmCaseNode);
                    if (idxOfCase + 1 <= pmSwitchCondition.Count - 1)
                    {
                        // next case exist -- descend to next case node
                        pmdltoplevel = pmSwitchCondition[idxOfCase + 1];
                        lineidx++;
                        if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"CASE_START: {pmdltoplevel}");

                        // if first statement is MOVE32_32(0,LOCAL) --> skip it //TODO: no idea what it does
                        LMSStatement linesub;
                        linesub = lmsobj.Statements[lineidx];
                        if (linesub.Operator == lms2012.Op.MOVE32_32 &&
                                linesub.Params[0].ToString() == "0" && linesub.Params[1].ToString().StartsWith("LOCAL"))
                        {
                            lineidx++;
                        }
                    }
                    else
                    {
                        // no more case nodes -- descend to original context (where switch was started)
                        pmdltoplevel = pmSwitchCondition.Root;
                        lineidx++;
                        if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"SWITCH_END");
                    }
                }

                //==================================================================================
                //== ANY OTHER LINES
                else
                {
                    //-- otherwise just skip line / increment line index
                    lineidx++;
                }
                #endregion PATTERN MATCHING

                #region FORK HANDLING
                //============================================================================================================

                //==================================================================================
                //== FORK: check if FORK is applied (OBJECT_TRIG/OBJECT_WAIT)
                if (line.Operator == lms2012.Op.OBJECT_TRIG) // OBJECT_TRIG = Triggers object and run the object if fully triggered
                {
                    int oldlineidx = lineidx - 1;//lineidx is already incremented
                    int target_objid = int.Parse(line.Params[0].data.ToString());
                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"FORK to OBJECT{target_objid}");

                    //-- if FORK is already registered, just indicate and skip it
                    if (CollectedForkObjects.ContainsKey(target_objid))
                    {
                        var pmd1 = new EV3GBlock(pmdltoplevel)
                        {
                            Name = Pattern.BLOCK_MergeToFork,
                            NodeType = EV3GBlock.GNodeType.ForkItem,
                            TargetObjectId = target_objid,
                            Offset = line.Offset,
                            OffsetLineId = oldlineidx,
                            //Parameters = new Dictionary<string, string>()
                            //{
                            //    ["objectid"] = target_objid.ToString()
                            //}
                        };
                        pmdlretval.Add(pmd1);
                        SetCommentForMatchedLines(lmsobj, oldlineidx, 1, pmd1);

                        if (prg.Objects[target_objid - 1] != null) prg.Objects[target_objid - 1].Comment.Text += $", {lmsobj.Id}_{line.Offset}";

                        return pmdlretval;
                    }

                    //-- if FORK is new, show all statements
                    prg.Objects[target_objid - 1].Comment = new LMSComment($"Triggered from: {lmsobj.Id}_{line.Offset}");
                    LMSObject targetobject = prg.Objects[target_objid - 1];
                    var pmd = new EV3GBlock(pmdltoplevel)
                    {
                        Name = Pattern.BLOCK_ForkItem,
                        NodeType = EV3GBlock.GNodeType.ForkItem,
                        TargetObjectId = target_objid,
                        Offset = line.Offset,
                        OffsetLineId = oldlineidx,
                    };
                    //pmd.Parameters["objectid"] = target_objid.ToString();
                    SetCommentForMatchedLines(lmsobj, oldlineidx, 1, pmd);

                    EV3GBlock retval2 = null;

                    // check for dummy loop
                    if (CheckSubPatternMatch(targetobject, 0, Pattern.LOOP_START_PATTERN, out int matched_lines2) &&
                        targetobject.Statements.Count == matched_lines2 + 1 /* LOOP_START + OBJ_END */ )
                    {
                        // this is a dummy loop (created by an error or due to unoptimized engineering)
                        retval2 = new EV3GBlock(pmd)
                        {
                            new EV3GBlock(pmd)
                            {
                                Name = Pattern.BLOCK_StartLoopDummy,
                                TargetObjectId = target_objid,
                                Offset = 0,
                                OffsetLineId = matched_lines2
                            }
                        };

                        // set comment for matched lines
                        SetCommentForMatchedLines(targetobject, 0, matched_lines2, retval2);
                        line.Comment.Text += " - " + Pattern.BLOCK_StartLoopDummy;
                    }
                    else
                    {
                        // check for top level object (what is inside the fork)
                        retval2 = AnalyzeTopLevelObject(prg, targetobject, pmd, printdepth + 1, false, !isTopSignatureFoundWithJR);
                    }

                    // if anything is found add to result
                    if (retval2 != null)
                    {
                        pmdltoplevel.Add(pmd);
                        pmd.AddRange(retval2);
                        CollectedForkObjects[target_objid] = pmd;
                    }
                }
                else if (line.Operator == Op.OBJECT_WAIT)
                {
                    // FORK objects return at object wait
                    // TODO: check what happens with FORM MERGE! special cases (e.g. variable datawire "wait"
                    // Wait for all fork objects to finish then post process them
                    if (line.Next()?.Operator != Op.OBJECT_WAIT)
                    {
                        // CloseAndReorderKids
                        // reorder forks - move short ones to the front
                        // upon first FORK reorder
                        var pmd = pmdltoplevel.FirstOrDefault(elem => elem.NodeType == EV3GBlock.GNodeType.ForkItem);
                        if (pmd != null)
                        {
                            //-- add fork parent first
                            var forkparent = new EV3GBlock(pmdltoplevel)
                            {
                                Name = Pattern.BLOCK_Fork,
                                NodeType = EV3GBlock.GNodeType.Fork
                            };

                            // reorder
                            var forks = pmdltoplevel.FindAll(elem => /*elem.Index >= pmd.Index && */ elem.NodeType == EV3GBlock.GNodeType.ForkItem)
                                .OrderBy(elem => elem.Count)
                                .ToList();
                            pmdltoplevel.RemoveAll(elem => elem.NodeType == EV3GBlock.GNodeType.ForkItem);
                            forkparent.AddRange(forks);

                            //-- add then the forkitems
                            pmdltoplevel.Add(forkparent);
                        }
                    }
                }
                #endregion FORK HANDLING
            }

            #region POST PROCESSING
            //============================================================================================================
            //== if top level has only one FORK NODE, skip it, use children only
            if (pmdlretval.Count == 1 && pmdlretval[0].NodeType == EV3GBlock.GNodeType.ForkItem)
            {
                var forknode = pmdlretval[0];
                pmdlretval.Clear();
                pmdlretval.AddRange(forknode);
            }

            //-- remove empty top level objects (e.g. dummy forks)
            if (pmdlretval.Count == 0 && !isThisMainObject)
            {
                pmdlretval = null;
            }

            //-- Add StartBlock
            if (pmdlretval != null && (lmsobj.header.is_vmthread || lmsobj.header.is_subcall))
            {
                var bkStart = new EV3GBlock(pmdroot); pmdlretval.Insert(0, bkStart);
                bkStart.Name = Pattern.BLOCK_StartBlock;

                //-- if MyBlock, copy all parameters as well
                if (OBJECTID_2_MyBlockID.ContainsKey(lmsobj.Id))
                {
                    var pmdMyBlock = dictCollectedMyBlocks_by_MyBlockID[OBJECTID_2_MyBlockID[lmsobj.Id]];

                    pmdMyBlock.Parameters.ToList().ForEach(pa => bkStart.Parameters.Add(pa.Key, pa.Value));
                }
            }

            #endregion POST PROCESSING

            return pmdlretval;
        }

        /// <summary>
        /// Add tracking global variables for debugging
        /// </summary>
        /// <param name="lmsobj"></param>
        /// <param name="startlineidx"></param>
        /// <param name="matched_line_count"></param>
        /// <param name="pmd"></param>
        private static void AddTrackingGlobals(LMSObject lmsobj, int startlineidx, int matched_line_count, EV3GBlock pmd)
        {
            //!! bool isThisMainObject = lmsobj.header.is_vmthread;

            // ADD32(GLOBAL204,1,GLOBAL204)
            if (/*isThisMainObject && */matched_line_count >= 3)
            {
                if (lmsobj[startlineidx].Operator == lms2012.Op.ADD32)
                {
                    //this is a tracking statement - parse and add to pmd
                    var globalid = int.Parse(lmsobj[startlineidx].Params[0].data.ToString());
                    pmd.TrackerGlobalStart = globalid;
                }
                if (lmsobj[startlineidx + matched_line_count - 1].Operator == lms2012.Op.ADD32)
                {
                    //this is a tracking statement - parse and add to pmd
                    var globalid = int.Parse(lmsobj[startlineidx + matched_line_count - 1].Params[0].data.ToString());
                    pmd.TrackerGlobalEnd = globalid;
                }
            }
        }

        /// <summary>
        /// Get target variable ID (EV3G globale variables)
        /// </summary>
        /// <param name="varev3gid"></param>
        /// <returns></returns>
        private string GetEV3GVariableId(int varev3gid, string vartype)
        {
            if (!GlobalVariables_GLOBALID_2_VariableId.TryGetValue(varev3gid, out string varname))
            {
                varname = $"VAR_{vartype}_{GlobalVariables_GLOBALID_2_VariableId.Count + 1}";
                GlobalVariables_GLOBALID_2_VariableId[varev3gid] = varname;
                GlobalVariables_VariableId_2_VariableType[varname] = vartype;
            }
            return varname;
        }

        /// <summary>
        /// Set Comment for matched lines
        /// </summary>
        /// <param name="lmsobj"></param>
        /// <param name="lineidx"></param>
        /// <param name="matched_lines"></param>
        /// <param name="pmd"></param>
        private static void SetCommentForMatchedLines(LMSObject lmsobj, int lineidx, int matched_lines, EV3GBlock pmd, bool replace = true)
        {
            string commentText = pmd.ToString();
            if (pmd.TrackerGlobalStart >= 0) commentText += $" [{pmd.TrackerGlobalStart}|{pmd.TrackerGlobalEnd}]";

            LMSComment comment = new LMSComment(commentText);
            LMSComment lastcomment = null;
            for (var idx = lineidx; idx < lineidx + matched_lines; idx++)
            {
                var line = lmsobj[idx];
                if (replace || line.Comment == null) line.Comment = comment;
                else if (lastcomment != lmsobj[idx].Comment)
                {
                    var prevline = line.Previous();
                    string newcomment = line.Comment.Text + " - " + commentText;
                    if (prevline == null || prevline.Comment != line.Comment)
                    {
                        line.Comment.Text = newcomment;
                    }
                    else
                    {
                        // split comment!
                        line.Comment = new LMSComment(newcomment);
                        //TODO: Check
                        for (int idx2 = idx; idx2 < lineidx + matched_lines; idx2++)
                        {
                            if (lmsobj[idx2].Comment == prevline.Comment) lmsobj[idx2].Comment = line.Comment;
                            else break;
                        }
                    }

                    lastcomment = line.Comment;
                }
            }
        }

        /// <summary>
        /// Check subroutine call
        /// </summary>
        /// <param name="prg"></param>
        /// <param name="lmsobj"></param>
        /// <param name="printdepth"></param>
        /// <param name="pmdltoplevel"></param>
        /// <param name="line"></param>
        /// <param name="offsetEnd"></param>
        private EV3GBlock CheckSubCall(LMSProgram prg, LMSObject lmsobj, int printdepth, EV3GBlock pmdltoplevel, LMSStatement line, int lastLineIdx = -1)
        {
            EV3GBlock retval = null;

            //Check for CALL op and OBJECT arg
            if (line.Operator != lms2012.Op.CALL || line.Params[0].handle != "OBJECT")
            {
                _WriteLine(printdepth, $"Illegal call {line.ToString()}", lmsobj.Id);
                return retval;
            }

            int target_objid = Int32.Parse(line.Params[0].data.ToString());
            int lineid = lmsobj.Statements.FindIndex(elem => elem == line);
            int matched_lines;
            LMSObject target_obj = prg.Objects[target_objid - 1];
            if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Checking OBJECT{target_objid} \t// {line.ToString()}");

            //-- Check if MyBlock pattern starts the subcall -> separate it to a myblock
            if (CheckSubPatternMatch(target_obj, 0, Pattern.MYBLOCK_PATTERN, out matched_lines) ||
                CheckSubPatternMatch(target_obj, 0, Pattern.MYBLOCK_PATTERN_WITH_INSTANT_FORK, out matched_lines))
            {
                //TODO: with instant fork -> consume semaphore for MYBLOCK at all of the objects (except for any dummy loops)

                var isNewMyBlock = !OBJECTID_2_MyBlockID.ContainsKey(target_obj.Id);
                if (isNewMyBlock)
                {
                    OBJECTID_2_MyBlockID[target_obj.Id] = OBJECTID_2_MyBlockID.Count + 1;
                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"MYBLOCK_{OBJECTID_2_MyBlockID[target_obj.Id]} == {target_obj.ToString()}");
                }

                // MYBLOCK: SUB starts with MYBLOCKPATTERN
                var pmd = retval = new EV3GBlock(pmdltoplevel)
                {
                    Name = Pattern.BLOCK_MyBlockPrefix + $"_{OBJECTID_2_MyBlockID[target_obj.Id]}",
                    TargetObjectId = target_objid,
                    Offset = line.Offset,
                    OffsetLineId = lineid,
                    OffsetEndLineId = lastLineIdx,
                    NodeType = EV3GBlock.GNodeType.MyBlockNode
                };

                //-- add myblock parameters
                pmd.Parameters = new Dictionary<string, string>();
                bool firstIntParamSkipped = false;
                Dictionary<string, string> param_locals = new Dictionary<string, string>();
                for (int idx = 1; idx <= line.Params.Count - 1; idx++)
                {
                    var elem = line.Params[idx];
                    // will skip first int param - this is the interrupt vector
                    //if (!firstIntParamSkipped && elem.data.GetType() == typeof(Int32))
                    //{
                    //    firstIntParamSkipped = true;
                    //    continue;
                    //}
                    lms2012.Callparam cpitem = target_obj.Arguments[idx - 1].Item1;
                    if (!firstIntParamSkipped && (cpitem == Callparam.IN_32))
                    {
                        firstIntParamSkipped = true;
                        continue;
                    }
                    (string value, CallparamExt valueType) = _GlobalScopeValue(elem, cpitem);

                    string skey = $"Param_{idx}";
                    pmd.Parameters[skey] = value;
                    pmd.ParameterTypes[skey] = valueType;
                    param_locals[skey] = "LOCAL" + target_obj.Arguments[idx - 1].Item2.ToString();
                    //TODO use Callparam
                }
                pmdltoplevel.Add(pmd);

                if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(printdepth, $"Found MYBLOCK@{line.Offset}");

                //-- MYBLOCK is new, add it to global myblock registry
                if (isNewMyBlock)
                {
                    //-- add parameters
                    EV3GBlock pmd_copy = pmd.Clone();
                    pmd_copy.Parameters = new Dictionary<string, string>();
                    foreach (var paramkvp in pmd_copy.ParameterTypes.ToArray())
                    {
                        //string s = pmd_copy.ParameterTypes[paramkvp.Key].ToString();
                        string s = param_locals[paramkvp.Key];
                        pmd_copy.Parameters[paramkvp.Key] = s;
                    }
                    pmd_copy.NodeType = EV3GBlock.GNodeType.MyBlockNode;

                    //-- save myblock node in dictionary (before the AnalyzeTopLevelObject)
                    dictCollectedMyBlocks_by_MyBlockID[OBJECTID_2_MyBlockID[target_obj.Id]] = pmd_copy;

                    // add new elements under the collected and separated myblock pmd, keep the original in the call tree empty
                    var retval2 = AnalyzeTopLevelObject(prg, target_obj, pmd_copy, printdepth + 1, true, true);
                    if (retval2 != null)
                        pmd_copy.AddRange(retval2);
                    pmlCollectedMyBlocks.Add(pmd_copy);
                }
                else
                {
                    //-- MYBLOCK already exists, so skip it
                }
            }
            else
            {
                // Normal subroutine - check for EV3 Block patterns inside
                List<(LMSObject, LMSStatement)> visitedObjects = new List<(LMSObject, LMSStatement)>() { (target_obj, line) };
                var pmi = new PatternsMatchTracker();
                if (_SearchAllEV3GPatternsInLine(line, 0, pmi, printdepth, -1) ||
                    (_SearchPatternInObject(prg, target_obj, pmi, printdepth + 1, 0, visitedObjects)))
                {
                    Pattern patMatching = pmi.GetMatchingPattern();
                    string sBlockName = patMatching.Name;
                    //_Write(depth, $"Found {p1.Name}: ");
                    string[] paout = patMatching.ParamsOut?.Split(',');
                    int maxparcnt = paout != null ? Math.Min(paout.Length, line.Params.Count() - 1) : 0;
                    Dictionary<string, string> paramouts_formatted = new Dictionary<string, string>();
                    //!!Dictionary<string, string> paramouts_types = new Dictionary<string, string>();
                    for (int i = 0; i < maxparcnt; i++)
                    {
                        string pout = paout[i];
                        if (string.IsNullOrEmpty(pout)) continue;
                        string paramData = line.Params[i + 1].ToString();
                        switch (pout)
                        {
                            case EV3GBlock.PORT_MOTOR when string.IsNullOrEmpty(line.Params[i + 1].scope):
                            case EV3GBlock.PORT_MOTORS when string.IsNullOrEmpty(line.Params[i + 1].scope):
                                {
                                    int port = int.Parse(paramData);
                                    port = port % 100;
                                    string portStr = "" + (char)((int)'A' + (port % 10) - 1);
                                    if (port % 100 / 10 != 0)
                                        portStr = "" + (char)((int)'A' + (port % 100 / 10) - 1) + "+" + portStr;
                                    paramData = portStr;
                                }
                                break;
                            case EV3GBlock.PORT_SENSOR when string.IsNullOrEmpty(line.Params[i + 1].scope):
                            case EV3GBlock.PORT_SENSOR2 when string.IsNullOrEmpty(line.Params[i + 1].scope):
                                {
                                    int port = int.Parse(paramData);
                                    port = port % 100;
                                    string portStr = "" + ((port % 10));
                                    if (port % 100 / 10 != 0)
                                        portStr = "" + (char)((port % 100 / 10)) + "+" + portStr;
                                    paramData = portStr;
                                }
                                break;
                            case string svalue when svalue.StartsWith("@cmd.globals"):
                                {
                                    var sa1 = svalue.Split(' '); //@cmd

                                    pout = sa1[1].Replace("@out.", "");
                                    var globalFound = _GlobalScopeValue(line.Params[i + 1]);
                                    paramData = globalFound.value;
                                    //TODO: channel out param type!

                                    // we can differentiate between ArrayOperations.Length.Length_Boolean and ArrayOperations.Length.Length_Numeric
                                    //  based on calling GLOBAL type (ARRAY8 or ARRAYF)
                                    if (sBlockName == "ArrayOperations.Length.Length_" && globalFound.valueType != null)
                                        sBlockName +=
                                            (((int)globalFound.valueType.callparam & (int)lms2012.DataFormat.DATA8) == (int)lms2012.DataFormat.DATA8) ? Pattern.BLOCK_DataType_Boolean :
                                            (((int)globalFound.valueType.callparam & (int)lms2012.DataFormat.DATAF) == (int)lms2012.DataFormat.DATAF) ? Pattern.BLOCK_DataType_Numeric :
                                            "";
                                }
                                break;
                            default:
                                //!! maybe this could go even to the top?
                                //!!TODO channel out ParamType!
                                //!! compareType if (line.Params[i + 1].ParamType == "SHORT") paramData = (line.Params[i + 1].ToString() != "0").ToString(); else 
                                paramData = line.Params[i + 1].ToString();
                                break;
                        }

                        paramouts_formatted[pout] = paramData;
                    }

                    //-- add found match record
                    // "@result" means outgoing parameter from the block //TODO: double check this
                    //.StartsWith("@result").ToString()
                    paramouts_formatted = paramouts_formatted
                        .OrderByDescending(elem => !elem.Key.StartsWith("@result"))
                        .ThenBy(elem => elem.Key)
                        .ToDictionary(elem => elem.Key, elem => elem.Value);
                    var pmd = retval = new EV3GBlock(pmdltoplevel)
                    {
                        Name = sBlockName,
                        Parameters = paramouts_formatted,
                        TargetObjectId = target_objid,
                        Offset = line.Offset,
                        OffsetLineId = lineid,
                        OffsetEndLineId = lastLineIdx,

                        VisitedObjects = visitedObjects,
                        IsLegoStandardSub = true,
                    };
                    pmdltoplevel.Add(pmd);
                    //-- print matches
                    if (DEBUG_PRINT_LEVEL >= 1)
                        _WriteLine(printdepth, $"{"Found " + pmd.Name + "(" + string.Join(", ", pmd.Parameters.Select(elemkvp => $"{elemkvp.Key}: {elemkvp.Value}").ToArray()) + ")"}",
                            target_objid);
                }
                else
                {
                    if (DEBUG_PRINT_LEVEL >= 1)
                        _WriteLine(printdepth, $"[No Match found] @{line.Offset}", target_objid);
                    var pmd = retval = new EV3GBlock(pmdltoplevel)
                    {
                        Name = Pattern.BLOCK_NoMatchFound,
                        TargetObjectId = target_objid,
                        Offset = line.Offset,
                        OffsetLineId = lineid,
                        OffsetEndLineId = lastLineIdx
                    };
                    pmdltoplevel.Add(pmd);
                }
            }

            // set comment for target object
            if (retval != null)
            {
                if (target_obj.Comment == null)
                {
                    string commentText;
                    commentText = retval.Name;
                    if (retval.Parameters.Count > 0) commentText += $"({string.Join(",", retval.Parameters.Keys)})";
                    commentText += $" - called from: {lmsobj.Id}_{line.Offset}";
                    target_obj.Comment = new LMSComment(commentText);

                    if (retval.IsLegoStandardSub)
                    {
                        // descend into all object touched if any - witout exact call params (e.g. LMOTOR rot blocks)
                        if (retval.VisitedObjects != null)
                        {
                            // skip first one (this will be the target_obj)
                            for (int idx = 1; idx < retval.VisitedObjects.Count; idx++)
                            {
                                var elem = retval.VisitedObjects[idx];
                                var line1 = elem.Item2;
                                var subline = retval.VisitedObjects[idx - 1].Item2;
                                var subobj = retval.VisitedObjects[idx - 1].Item1;
                                if (elem.Item1.Comment == null)
                                    elem.Item1.Comment = new LMSComment("Subblock used by " + retval.Name +
                                        $" - called from: {subobj.Id}_{line1.Offset}");
                                else
                                    elem.Item1.Comment.Text += $", {subobj.Id}_{line1.Offset}";
                            };
                        }
                    }
                }
                else
                {
                    // for myblocks show all callers
                    target_obj.Comment.Text += $", {lmsobj.Id}_{line.Offset}";
                }
            }
            return retval;
        }

        /// <summary>
        /// Global ARRAY/STRING values saved
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private (string value, CallparamExt valueType) _GlobalScopeValue(LMSParam elem, CallparamExt callparam = null)
        {
            (string value, CallparamExt valueType) ret = (value: "", valueType: null);
            if (elem.scope == "GLOBAL")
            {
                if (CollectedGlobalParams.ContainsKey(elem.ToString()))
                {
                    ret.valueType = CollectedGlobalParams[elem.ToString()].type;
                    ret.value = CollectedGlobalParams[elem.ToString()].Item1;
                }
                else
                {
                    ret.value = "MISSING_GLOBAL";
                    if (DEBUG_PRINT_LEVEL >= 1) _WriteLine(0, $"MISSING_GLOBAL {elem}");
                }
            }
            else
            {
                ret.value = elem.ToString();

                if (elem.scope == "LOCAL" && CollectedArrayTypes.ContainsKey(ret.value))
                {
                    ret.valueType = CollectedArrayTypes[ret.value];
                    // as this one is "local" --> global must be the input - this is an output
                    ret.valueType.SetDirection(false);
                }
                else
                {
                    if (callparam != null)
                    {
                        ret.valueType = callparam;
                    }
                    else
                    {
                        //TODO: how to map NumArr, BoolArr?
                        if (elem.data.GetType() == typeof(int)) ret.valueType = lms2012.Callparam.IN_8;
                        else if (elem.data.GetType() == typeof(float)) ret.valueType = lms2012.Callparam.IN_F;
                        // else 
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Match pattern to lines
        /// </summary>
        /// <param name="lmsobj"></param>
        /// <param name="lineidx"></param>
        /// <param name="patelems_to_match"></param>
        /// <param name="matched_lines"></param>
        /// <returns></returns>
        private static bool CheckSubPatternMatch(LMSObject lmsobj, int lineidx, List<PatternElem> patelems_to_match, out int matched_lines)
        {
            bool matched_subpattern = true;
            int idxsub_statement = 0;
            //for (int idxsub_pattern = 0; idxsub_pattern < pattern_to_match.Length; idxsub_pattern++)
            for (int idxsub_pattern = 0; idxsub_pattern < patelems_to_match.Count; idxsub_pattern++)
            {
                //var patt_op = pattern_to_match[idxsub_pattern];
                var patelem = patelems_to_match[idxsub_pattern];

                int instancesmatched = 0;
                do
                {
                    var line1 = lineidx + idxsub_statement < lmsobj.Statements.Count ?
                        lmsobj.Statements[lineidx + idxsub_statement] : null;
                    if (!_SearchPatternElementInLine(line1, patelem))
                    {
                        if (instancesmatched == 0)
                        {
                            if (patelem.Conditions.HasFlag(PatternElem.ConditionsFlags.Optional)) break; // continue to next idxsub_pattern
                            matched_lines = 0;
                            return false;
                        }
                        else
                        {
                            // anyhow this is a MoreThanOnce conditions, just break and return matches so far
                            break;
                        }
                    }
                    idxsub_statement++;
                    instancesmatched++;
                } while (patelem.Conditions.HasFlag(PatternElem.ConditionsFlags.MoreThanOnce));
            }

            if (matched_subpattern)
            {
                matched_lines = idxsub_statement;
                return matched_lines > 0; // if all is optional, match only if something was at least matched
            }

            matched_lines = 0;
            return false;
        }

        /// <summary>
        /// Search for all EV3G patterns starting from a line
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lmsobj"></param>
        /// <param name="lineidx"></param>
        /// <param name="pm"></param>
        /// <param name="printdepth"></param>
        /// <param name="calldepth"></param>
        /// <returns></returns>
        static bool _SearchAllEV3GPatternsInLine(LMSStatement line, int lineidx, PatternsMatchTracker pm,
            int printdepth, int calldepth)
        {
            if (!Pattern.PATTERNS_CACHE.ContainsKey(line.Operator.ToString())) return false;

            foreach (Pattern pattern in Pattern.PATTERNS_CACHE[line.Operator.ToString()])
            {
                // exact match patterns cannot descend
                if (calldepth > 0 &&
                    (pattern.MatchType == Pattern.MatchTypeEnum.NoSubCalls ||
                    pattern.MatchType == Pattern.MatchTypeEnum.NoSubCallsExact)) continue;

                bool bOpMatches = false;
                try
                {
                    var patternElem = pm.GetPatternElem(pattern);
                    bOpMatches = _SearchPatternElementInLine(line, patternElem);
                }
                catch { }

                if (bOpMatches &&
                    (pattern.MatchType != Pattern.MatchTypeEnum.NoSubCallsExact || (pattern.MatchType == Pattern.MatchTypeEnum.NoSubCallsExact && pm.matchpositions[pattern] == lineidx)))
                {
                    if (DEBUG_PRINT_LEVEL >= 2) _WriteLine(printdepth, $"Matched {line.Operator + "@" + line.Offset.ToString(),-20} for {pattern.Name} @ {pm.matchpositions[pattern] + 1}/{pattern.Elems.Count}");
                    if (pm.RegisterMatch(pattern)) return true;
                }
                else
                {
                    // if this is not a match but pattern is already partially matched and exact matching is requried
                    // let us start over as this OBJECT cannot me a fully matching one
                    if (pattern.MatchType == Pattern.MatchTypeEnum.NoSubCallsExact && lineidx > 0 && pm.matchpositions[pattern] > 0)
                    {
                        pm.matchpositions[pattern] = 0;
                    }
                }
            }

            return false;
        }

        private static bool _SearchPatternElementInLine(LMSStatement line, PatternElem patternElem)
        {
            // check line operator
            if (patternElem.Conditions.HasFlag(PatternElem.ConditionsFlags.EitherOr))
            {
                return patternElem.EitherOrPatternElements.Any(elemsub => _SearchPatternElementInLine(line, elemsub));
            }
            if (!line.Operator.ToString().StartsWith(patternElem.Op)) return false;
            // check line operator conditions - last elem practically means count-2 as OBJECT_END is at the end cannot be matched
            if (patternElem.Conditions.HasFlag(PatternElem.ConditionsFlags.AtObjectStart) && line.LineIndex != 0 ||
                patternElem.Conditions.HasFlag(PatternElem.ConditionsFlags.AtObjectEnd) && line.LineIndex != line.Object.Statements.Count - 2) return false;


            //-- check value conditions
            if (patternElem.ValueConditions != null && patternElem.ValueConditions.Count > 0)
            {
                foreach (var valueCondition in patternElem.ValueConditions)
                {
                    if (!(line.Params.Count > valueCondition.Key &&
                        line.Params[valueCondition.Key].ToString().StartsWith(valueCondition.Value)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Search for pattern in an LMSObject
        /// </summary>
        /// <param name="prg"></param>
        /// <param name="obj"></param>
        /// <param name="pmi"></param>
        /// <param name="printdepth"></param>
        /// <param name="calldepth"></param>
        /// <returns></returns>
        static bool _SearchPatternInObject(LMSProgram prg, LMSObject obj, PatternsMatchTracker pmi,
            int printdepth, int calldepth,
            List<(LMSObject, LMSStatement)> visitedObjects)
        {
            if (DEBUG_PRINT_LEVEL >= 2) _WriteLine(printdepth, $"Searching in {obj.Id}");

            //-- check all statements for match
            for (int idx = 0; idx < obj.Statements.Count; idx++)
            {
                var line = obj.Statements[idx];
                if (_SearchAllEV3GPatternsInLine(line, idx, pmi, printdepth, calldepth)) return true;

                //-- follow call to match pattern
                if (line.Operator == lms2012.Op.CALL)
                {
                    int objid = Int32.Parse(line.Params[0].data.ToString());
                    LMSObject objcall = prg.Objects[objid - 1];
                    if (DEBUG_PRINT_LEVEL >= 2) _WriteLine(printdepth, $"Descending to object{objcall.Id}");
                    visitedObjects.Add((objcall, line));

                    if (_SearchPatternInObject(prg, objcall, pmi, printdepth + 1, calldepth + 1, visitedObjects))
                        return true;
                }
            }
            return false;
        }

        #region PrintOut helpers
        static bool _depthIndentNeeded = true;
        static int _Write(int depth, string s)
        {
            int len = 0;
            if (_depthIndentNeeded) { Console.Write("".PadLeft(depth * 2)); _depthIndentNeeded = false; len += depth * 2; }
            len += s.Length;

            Console.Write(s);
            return len;
        }
        static void _WriteLine(int depth, string s, int? objectid = null)
        {
            var len = _Write(depth, s);
            if (objectid.HasValue) { _Write(depth, (len < 100 ? "".PadLeft(100 - len) : null) + $"in OBJECT{objectid}"); }
            Console.WriteLine();
            _depthIndentNeeded = true;
        }
        #endregion PrintOut helpers
    }
}
