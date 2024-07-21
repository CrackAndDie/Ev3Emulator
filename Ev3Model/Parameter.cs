using Newtonsoft.Json;
using System.Linq;
using YamlDotNet.Serialization;

namespace EV3ModelLib
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
        public string MyBlockParamDefaultValue { get; set; } // defaultvalue for myblock param
        public string Identification => Identification_stored ?? (Identification_stored = GetDataIdentification(Parent, Name));
        //TODO: cache this (with null handling effectivity)
        public bool IsInput { get; set; } = true;
        public string Variable { get; set; }
        public string ValueFormatted { get { return this.ConvertValueForPrinting(); } }
        //TODO: cache this (null handling not important)

        public bool IsControlParameter { get; set; } = false;

        [JsonIgnore]
        [YamlIgnore]
        public Node Parent { get; private set; }
        [JsonIgnore]
        [YamlIgnore]
        private string Identification_stored = null;

        public Parameter(string name, string value, Node parent)
        {
            Name = name;
            Value = value;
            Parent = parent;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //TODO: first / temporary solution // should change this to a more generic solution
            if (parent.NodeType == Node.NodeTypeEnum.CaseItem && this.Name == Node.PARAM_SWITCH_Pattern)
            {
                if (parent.Parent?.RefName != null)
                {
                    var parentFirstOutParam = BlockInfo.BlockMapByRef[parent.Parent?.RefName].Params.FirstOrDefault(pa => !pa.Value.CallDirectionInput).Value;
                    this.Identification_stored = parentFirstOutParam?.Identification;
                    this.DataType = parentFirstOutParam?.DataType;
                }
            }

            if (parent.NodeType == Node.NodeTypeEnum.Loop && name.In(Node.PARAM_LOOP_InterruptName, Node.PARAM_LOOP_LoopIndex)) this.IsControlParameter = true;
        }

        private static string GetDataIdentification(Node node, string propName)
        {
            try
            {
                if (node.RefName == null) return null;
                if (!BlockInfo.BlockMapByRef.ContainsKey(node.RefName)) return null;
                if (!BlockInfo.BlockMapByRef[node.RefName].Params.ContainsKey(propName)) return null;

                //propName is already transformed Brake_At_End vs "Brake At End"
                return BlockInfo.BlockMapByRef[node.RefName].Params[propName].Identification;
            }
            catch { }
            return null;
        }

        private string ConvertValueForPrinting()
        {
            try
            {
                //propName is already transformed Brake_At_End vs "Brake At End"
                if (Variable != null)
                {
                    return (IsInput ? "$" : "out $") + Variable;

                }
                else if (Identification != null)
                {
                    if (Value == null || !Value.StartsWith("["))
                    {
                        // simple value handling
                        string dispValue = ConvertSingleValueForPrinting(Identification, Value);
                        if (!string.IsNullOrEmpty(dispValue)) return dispValue;
                    }
                    else
                    {
                        // array handling
                        var valuearray = Value.Trim('[', ']').Split(',').Select(s => ConvertSingleValueForPrinting(Identification, s)).ToArray();
                        string dispValue = "[" +
                            string.Join(",", valuearray) +
                            "]";
                        return dispValue;
                    }
                }
            }
            catch { }

            return Value;
        }
        /// <summary>
        /// Convert a single value using identification for printing
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static string ConvertSingleValueForPrinting(string identification, string Value)
        {
            try
            {
                return IdentificationHelper.IdentificationValues[identification][Value];
            }
            catch
            {
                return Value;
            }
        }
    }
}
