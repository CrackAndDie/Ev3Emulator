
using System.Collections.Generic;

// auto generated file
namespace EV3ModelLib
{
    public partial class BlockInfo
    {
        public static readonly Dictionary<string, BlockInfo> BlockMapByRef =
new Dictionary<string, BlockInfo>
{
    {
        "AdvDistanceCM",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.Centimeters",
            Reference = "AdvDistanceCM",
            IconName = "PolyGroup_UltrasonicSensor_Mode_Centimeters_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Distance",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Measuring_Mode",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "MeasuringMode" }
                }
            }
        }
    },
    {
        "AdvDistanceInches",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.Inches",
            Reference = "AdvDistanceInches",
            IconName = "PolyGroup_UltrasonicSensor_Mode_Inches_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "DistanceInches",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Measuring_Mode",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "MeasuringMode" }
                }
            }
        }
    },
    {
        "ArrayBuild",
        new BlockInfo
        {
            ShortName = "ArrayOperations.Append_Numeric",
            Reference = "ArrayBuild",
            IconName = "PolyGroup_ArrayOperations_Mode_Append_Numeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInNumeric",
                    new BlockParamInfo { DataType = "Single[]", CallDirectionInput = true }
                },
                {
                    "arrayOutNumeric",
                    new BlockParamInfo { DataType = "Single[]" }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ArrayBuildBoolean",
        new BlockInfo
        {
            ShortName = "ArrayOperations.Append_Boolean",
            Reference = "ArrayBuildBoolean",
            IconName = "PolyGroup_ArrayOperations_Mode_Append_Boolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInBoolean",
                    new BlockParamInfo { DataType = "Boolean[]", CallDirectionInput = true }
                },
                {
                    "arrayOutBoolean",
                    new BlockParamInfo { DataType = "Boolean[]" }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ArrayGetSize",
        new BlockInfo
        {
            ShortName = "ArrayOperations.Length_Numeric",
            Reference = "ArrayGetSize",
            IconName = "PolyGroup_ArrayOperations_Mode_Length_Numeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInNumeric",
                    new BlockParamInfo { DataType = "Single[]", CallDirectionInput = true }
                },
                {
                    "Size",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "ArrayGetSizeBoolean",
        new BlockInfo
        {
            ShortName = "ArrayOperations.Length_Boolean",
            Reference = "ArrayGetSizeBoolean",
            IconName = "PolyGroup_ArrayOperations_Mode_Length_Boolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInBoolean",
                    new BlockParamInfo { DataType = "Boolean[]", CallDirectionInput = true }
                },
                {
                    "Size",
                    new BlockParamInfo { DataType = "Single" }
                }
            }
        }
    },
    {
        "ArrayReadAtIndex",
        new BlockInfo
        {
            ShortName = "ArrayOperations.ReadAtIndex_Numeric",
            Reference = "ArrayReadAtIndex",
            IconName = "PolyGroup_ArrayOperations_Mode_ReadAtIndex_Numeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInNumeric",
                    new BlockParamInfo { DataType = "Single[]", CallDirectionInput = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Index",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ArrayReadAtIndexBoolean",
        new BlockInfo
        {
            ShortName = "ArrayOperations.ReadAtIndex_Boolean",
            Reference = "ArrayReadAtIndexBoolean",
            IconName = "PolyGroup_ArrayOperations_Mode_ReadAtIndex_Boolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInBoolean",
                    new BlockParamInfo { DataType = "Boolean[]", CallDirectionInput = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1 }
                },
                {
                    "Index",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ArrayWriteAtIndex",
        new BlockInfo
        {
            ShortName = "ArrayOperations.WriteAtIndex_Numeric",
            Reference = "ArrayWriteAtIndex",
            IconName = "PolyGroup_ArrayOperations_Mode_WriteAtIndex_Numeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInNumeric",
                    new BlockParamInfo { DataType = "Single[]", CallDirectionInput = true }
                },
                {
                    "arrayOutNumeric",
                    new BlockParamInfo { DataType = "Single[]" }
                },
                {
                    "Index",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ArrayWriteAtIndexBoolean",
        new BlockInfo
        {
            ShortName = "ArrayOperations.WriteAtIndex_Boolean",
            Reference = "ArrayWriteAtIndexBoolean",
            IconName = "PolyGroup_ArrayOperations_Mode_WriteAtIndex_Boolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "arrayInBoolean",
                    new BlockParamInfo { DataType = "Boolean[]", CallDirectionInput = true }
                },
                {
                    "arrayOutBoolean",
                    new BlockParamInfo { DataType = "Boolean[]" }
                },
                {
                    "Index",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "BluetoothMessagingClose",
        new BlockInfo
        {
            ShortName = "Bluetooth.Clear",
            Reference = "BluetoothMessagingClose",
            IconName = "PolyGroup_Bluetooth_Mode_Clear_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "Connect_To", new BlockParamInfo { DataType = "String", CallIndex = 4, CallDirectionInput = true } } }
        }
    },
    {
        "BluetoothMessagingInitiate",
        new BlockInfo
        {
            ShortName = "Bluetooth.Initiate",
            Reference = "BluetoothMessagingInitiate",
            IconName = "PolyGroup_Bluetooth_Mode_Initiate_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "Connect_To", new BlockParamInfo { DataType = "String", CallDirectionInput = true } } }
        }
    },
    {
        "BluetoothMessagingOff",
        new BlockInfo
        {
            ShortName = "Bluetooth.Off",
            Reference = "BluetoothMessagingOff",
            IconName = "PolyGroup_Bluetooth_Mode_Off_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "BluetoothMessagingOn",
        new BlockInfo
        {
            ShortName = "Bluetooth.On",
            Reference = "BluetoothMessagingOn",
            IconName = "PolyGroup_Bluetooth_Mode_On_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "ButtonChange",
        new BlockInfo
        {
            ShortName = "BrickButton.ChangeBrickButton",
            Reference = "ButtonChange",
            IconName = "PolyGroup_BrickButton_Mode_ChangeBrickButton_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, Identification = "OutputBrickButtonID" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, IsResult = true }
                }
            }
        }
    },
    {
        "ButtonCompare",
        new BlockInfo
        {
            ShortName = "BrickButton.Compare",
            Reference = "ButtonCompare",
            IconName = "PolyGroup_BrickButton_Mode_Compare_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Buttons",
                    new BlockParamInfo { DataType = "Single[]", CallDirectionInput = true, Identification = "OutputBrickButtonID" }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, Identification = "OutputBrickButtonID" }
                },
                {
                    "Action",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "TouchState" }
                }
            }
        }
    },
    {
        "ButtonValue",
        new BlockInfo
        {
            ShortName = "BrickButton.Measure",
            Reference = "ButtonValue",
            IconName = "PolyGroup_BrickButton_Mode_Measure_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { { "Value", new BlockParamInfo { DataType = "Single", Identification = "OutputBrickButtonID", /* added later */ IsResult = true } } }
        }
    },
    {
        "ColorAmbientIntensity",
        new BlockInfo
        {
            ShortName = "ColorSensor.MeasureAmbientLight",
            Reference = "ColorAmbientIntensity",
            IconName = "PolyGroup_ColorSensor_Mode_MeasureAmbientLight_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "ColorAmbientIntensityChange",
        new BlockInfo
        {
            ShortName = "ColorSensor.ChangeAmbientLight",
            Reference = "ColorAmbientIntensityChange",
            IconName = "PolyGroup_ColorSensor_Mode_ChangeAmbientLight_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, IsResult = true }
                }
            }
        }
    },
    {
        "ColorAmbientIntensityCompare",
        new BlockInfo
        {
            ShortName = "ColorSensor.CompareAmbientLight",
            Reference = "ColorAmbientIntensityCompare",
            IconName = "PolyGroup_ColorSensor_Mode_CompareAmbientLight_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ColorCalibrateDefault",
        new BlockInfo
        {
            ShortName = "ColorSensor.CalibrateResetColor",
            Reference = "ColorCalibrateDefault",
            IconName = "PolyGroup_ColorSensor_Mode_CalibrateResetColor_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "ColorCalibrateMax",
        new BlockInfo
        {
            ShortName = "ColorSensor.CalibrateMaxColor",
            Reference = "ColorCalibrateMax",
            IconName = "PolyGroup_ColorSensor_Mode_CalibrateMaxColor_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { { "CalibrateValueMax", new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true } } }
        }
    },
    {
        "ColorCalibrateMin",
        new BlockInfo
        {
            ShortName = "ColorSensor.CalibrateMinColor",
            Reference = "ColorCalibrateMin",
            IconName = "PolyGroup_ColorSensor_Mode_CalibrateMinColor_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { { "CalibrateValueMin", new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true } } }
        }
    },
    {
        "ColorChange",
        new BlockInfo
        {
            ShortName = "ColorSensor.ChangeColor",
            Reference = "ColorChange",
            IconName = "PolyGroup_ColorSensor_Mode_ChangeColor_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Color",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, Identification = "Color" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, IsResult = true }
                }
            }
        }
    },
    {
        "ColorCompare",
        new BlockInfo
        {
            ShortName = "ColorSensor.CompareColor",
            Reference = "ColorCompare",
            IconName = "PolyGroup_ColorSensor_Mode_CompareColor_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Color",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, Identification = "Color" }
                },
                {
                    "Set_of_colors",
                    new BlockParamInfo { DataType = "Single[]", CallIndex = 1, CallDirectionInput = true, Identification = "Color" }
                }
            }
        }
    },
    {
        "ColorReflectedIntensity",
        new BlockInfo
        {
            ShortName = "ColorSensor.MeasureReflectedLight",
            Reference = "ColorReflectedIntensity",
            IconName = "PolyGroup_ColorSensor_Mode_MeasureReflectedLight_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "ColorReflectedIntensityChange",
        new BlockInfo
        {
            ShortName = "ColorSensor.ChangeReflectedLight",
            Reference = "ColorReflectedIntensityChange",
            IconName = "PolyGroup_ColorSensor_Mode_ChangeReflectedLight_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, IsResult = true }
                }
            }
        }
    },
    {
        "ColorReflectedIntensityCompare",
        new BlockInfo
        {
            ShortName = "ColorSensor.CompareReflectedLight",
            Reference = "ColorReflectedIntensityCompare",
            IconName = "PolyGroup_ColorSensor_Mode_CompareReflectedLight_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ColorValue",
        new BlockInfo
        {
            ShortName = "ColorSensor.MeasureColor",
            Reference = "ColorValue",
            IconName = "PolyGroup_ColorSensor_Mode_MeasureColor_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Color",
                    new BlockParamInfo { DataType = "Single", Identification = "Color" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "DeltaWaitDistanceCM",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.ChangeDistanceCm",
            Reference = "DeltaWaitDistanceCM",
            IconName = "PolyGroup_UltrasonicSensor_Mode_ChangeDistanceCm_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Distance",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Change",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DeltaWaitDistanceInches",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.ChangeDistanceInch",
            Reference = "DeltaWaitDistanceInches",
            IconName = "PolyGroup_UltrasonicSensor_Mode_ChangeDistanceInch_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "DistanceInches",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Change",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DeltaWaitGyroAngle",
        new BlockInfo
        {
            ShortName = "Gyro.ChangeAngle",
            Reference = "DeltaWaitGyroAngle",
            IconName = "PolyGroup_Gyro_Mode_ChangeAngle_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Angle",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Change",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DeltaWaitGyroRate",
        new BlockInfo
        {
            ShortName = "Gyro.ChangeRate",
            Reference = "DeltaWaitGyroRate",
            IconName = "PolyGroup_Gyro_Mode_ChangeRate_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Rate",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Change",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DeltaWaitVolume",
        new BlockInfo
        {
            ShortName = "SoundSensor.ChangedB",
            Reference = "DeltaWaitVolume",
            IconName = "PolyGroup_SoundSensor_Mode_ChangedB_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Change",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DeltaWaitVolumedBa",
        new BlockInfo
        {
            ShortName = "SoundSensor.ChangedBa",
            Reference = "DeltaWaitVolumedBa",
            IconName = "PolyGroup_SoundSensor_Mode_ChangedBa_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ValuedBa",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Change",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DisplayCircle",
        new BlockInfo
        {
            ShortName = "Display.Circle",
            Reference = "DisplayCircle",
            IconName = "PolyGroup_Display_Mode_Circle_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Clear_Screen",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Radius",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Fill",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true }
                },
                {
                    "Invert_Color",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, CallDirectionInput = true, Identification = "InvertColor" }
                }
            }
        }
    },
    {
        "DisplayClear",
        new BlockInfo
        {
            ShortName = "Display.Clear",
            Reference = "DisplayClear",
            IconName = "PolyGroup_Display_Mode_Clear_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "DisplayFile",
        new BlockInfo
        {
            ShortName = "Display.File",
            Reference = "DisplayFile",
            IconName = "PolyGroup_Display_Mode_File_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Filename",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Clear_Screen",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DisplayLine",
        new BlockInfo
        {
            ShortName = "Display.Line",
            Reference = "DisplayLine",
            IconName = "PolyGroup_Display_Mode_Line_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Clear_Screen",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "X1",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Y1",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "X2",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Y2",
                    new BlockParamInfo { DataType = "Single", CallIndex = 4, CallDirectionInput = true }
                },
                {
                    "Invert_Color",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, CallDirectionInput = true, Identification = "InvertColor" }
                }
            }
        }
    },
    {
        "DisplayPoint",
        new BlockInfo
        {
            ShortName = "Display.Point",
            Reference = "DisplayPoint",
            IconName = "PolyGroup_Display_Mode_Point_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Clear_Screen",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Invert_Color",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3, CallDirectionInput = true, Identification = "InvertColor" }
                }
            }
        }
    },
    {
        "DisplayRect",
        new BlockInfo
        {
            ShortName = "Display.Rectangle",
            Reference = "DisplayRect",
            IconName = "PolyGroup_Display_Mode_Rectangle_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Clear_Screen",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Width",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Height",
                    new BlockParamInfo { DataType = "Single", CallIndex = 4, CallDirectionInput = true }
                },
                {
                    "Fill",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, CallDirectionInput = true }
                },
                {
                    "Invert_Color",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 6, CallDirectionInput = true, Identification = "InvertColor" }
                }
            }
        }
    },
    {
        "DisplayString",
        new BlockInfo
        {
            ShortName = "Display.String",
            Reference = "DisplayString",
            IconName = "PolyGroup_Display_Mode_String_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Text",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true }
                },
                {
                    "Clear_Screen",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Invert_Color",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "InvertColor" }
                },
                {
                    "Size",
                    new BlockParamInfo { DataType = "Single", CallIndex = 5, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DisplayStringGrid",
        new BlockInfo
        {
            ShortName = "Display.StringGrid",
            Reference = "DisplayStringGrid",
            IconName = "PolyGroup_Display_Mode_StringGrid_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Text",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true }
                },
                {
                    "Clear_Screen",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Column",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Row",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Invert_Color",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "InvertColor" }
                },
                {
                    "Size",
                    new BlockParamInfo { DataType = "Single", CallIndex = 5, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DistanceCM",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.MeasureCentimeters",
            Reference = "DistanceCM",
            IconName = "PolyGroup_UltrasonicSensor_Mode_MeasureCentimeters_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Distance",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "DistanceCMCompare",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.CompareCentimeters",
            Reference = "DistanceCMCompare",
            IconName = "PolyGroup_UltrasonicSensor_Mode_CompareCentimeters_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Distance",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DistanceInches",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.MeasureInches",
            Reference = "DistanceInches",
            IconName = "PolyGroup_UltrasonicSensor_Mode_MeasureInches_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "DistanceInches",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "DistanceInchesCompare",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.CompareInches",
            Reference = "DistanceInchesCompare",
            IconName = "PolyGroup_UltrasonicSensor_Mode_CompareInches_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "DistanceInches",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "DistanceListen",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.MeasurePresence",
            Reference = "DistanceListen",
            IconName = "PolyGroup_UltrasonicSensor_Mode_MeasurePresence_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Heard",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "DistanceListenCompare",
        new BlockInfo
        {
            ShortName = "UltrasonicSensor.ComparePresence",
            Reference = "DistanceListenCompare",
            IconName = "PolyGroup_UltrasonicSensor_Mode_ComparePresence_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Heard",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "EnergyMeterChangeInCurrent",
        new BlockInfo
        {
            ShortName = "EnergyMeter.ChangeInCurrent",
            Reference = "EnergyMeterChangeInCurrent",
            IconName = "PolyGroup_EnergyMeter_Mode_ChangeInCurrent_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Current",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountCurr",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterChangeInVoltage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.ChangeInVoltage",
            Reference = "EnergyMeterChangeInVoltage",
            IconName = "PolyGroup_EnergyMeter_Mode_ChangeInVoltage_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Volts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountVolt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterChangeInWatt",
        new BlockInfo
        {
            ShortName = "EnergyMeter.ChangeInWatt",
            Reference = "EnergyMeterChangeInWatt",
            IconName = "PolyGroup_EnergyMeter_Mode_ChangeInWatt_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Watts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountWatt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 3, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3, IsResult = true }
                }
            }
        }
    },
    {
        "EnergyMeterChangeJoule",
        new BlockInfo
        {
            ShortName = "EnergyMeter.ChangeJoule",
            Reference = "EnergyMeterChangeJoule",
            IconName = "PolyGroup_EnergyMeter_Mode_ChangeJoule_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Joule",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterChangeOutCurrent",
        new BlockInfo
        {
            ShortName = "EnergyMeter.ChangeOutCurrent",
            Reference = "EnergyMeterChangeOutCurrent",
            IconName = "PolyGroup_EnergyMeter_Mode_ChangeOutCurrent_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Current",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountCurr",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterChangeOutVoltage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.ChangeOutVoltage",
            Reference = "EnergyMeterChangeOutVoltage",
            IconName = "PolyGroup_EnergyMeter_Mode_ChangeOutVoltage_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Volts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountVolt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterChangeOutWatt",
        new BlockInfo
        {
            ShortName = "EnergyMeter.ChangeOutWatt",
            Reference = "EnergyMeterChangeOutWatt",
            IconName = "PolyGroup_EnergyMeter_Mode_ChangeOutWatt_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Watts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountWatt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterCompareInCurrent",
        new BlockInfo
        {
            ShortName = "EnergyMeter.CompareInCurrent",
            Reference = "EnergyMeterCompareInCurrent",
            IconName = "PolyGroup_EnergyMeter_Mode_CompareInCurrent_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Current",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdCurr",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterCompareInVoltage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.CompareInVoltage",
            Reference = "EnergyMeterCompareInVoltage",
            IconName = "PolyGroup_EnergyMeter_Mode_CompareInVoltage_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Volts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdVolt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterCompareInWatt",
        new BlockInfo
        {
            ShortName = "EnergyMeter.CompareInWatt",
            Reference = "EnergyMeterCompareInWatt",
            IconName = "PolyGroup_EnergyMeter_Mode_CompareInWatt_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Watts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdWatt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterCompareJoule",
        new BlockInfo
        {
            ShortName = "EnergyMeter.CompareOutJoule",
            Reference = "EnergyMeterCompareJoule",
            IconName = "PolyGroup_EnergyMeter_Mode_CompareOutJoule_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Joule",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdJoule",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterCompareOutCurrent",
        new BlockInfo
        {
            ShortName = "EnergyMeter.CompareOutCurrent",
            Reference = "EnergyMeterCompareOutCurrent",
            IconName = "PolyGroup_EnergyMeter_Mode_CompareOutCurrent_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ComparisonOut",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Current",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdCurr",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterCompareOutVoltage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.CompareOutVoltage",
            Reference = "EnergyMeterCompareOutVoltage",
            IconName = "PolyGroup_EnergyMeter_Mode_CompareOutVoltage_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ComparisonOut",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Volts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdVolt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterCompareOutWatt",
        new BlockInfo
        {
            ShortName = "EnergyMeter.CompareOutWatt",
            Reference = "EnergyMeterCompareOutWatt",
            IconName = "PolyGroup_EnergyMeter_Mode_CompareOutWatt_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ComparisonOut",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Watts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdWatt",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "EnergyMeterInCurrent",
        new BlockInfo
        {
            ShortName = "EnergyMeter.MeasureInCurrent",
            Reference = "EnergyMeterInCurrent",
            IconName = "PolyGroup_EnergyMeter_Mode_MeasureInCurrent_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Current",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "EnergyMeterInVoltage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.MeasureInVoltage",
            Reference = "EnergyMeterInVoltage",
            IconName = "PolyGroup_EnergyMeter_Mode_MeasureInVoltage_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Volts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "EnergyMeterInWattage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.MeasureInWatts",
            Reference = "EnergyMeterInWattage",
            IconName = "PolyGroup_EnergyMeter_Mode_MeasureInWatts_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Watts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "EnergyMeterJoule",
        new BlockInfo
        {
            ShortName = "EnergyMeter.MeasureJoule",
            Reference = "EnergyMeterJoule",
            IconName = "PolyGroup_EnergyMeter_Mode_MeasureJoule_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Joule",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "EnergyMeterOutCurrent",
        new BlockInfo
        {
            ShortName = "EnergyMeter.MeasureOutCurrent",
            Reference = "EnergyMeterOutCurrent",
            IconName = "PolyGroup_EnergyMeter_Mode_MeasureOutCurrent_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Current",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "EnergyMeterOutVoltage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.MeasureOutVoltage",
            Reference = "EnergyMeterOutVoltage",
            IconName = "PolyGroup_EnergyMeter_Mode_MeasureOutVoltage_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Volts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "EnergyMeterOutWattage",
        new BlockInfo
        {
            ShortName = "EnergyMeter.MeasureOutWatts",
            Reference = "EnergyMeterOutWattage",
            IconName = "PolyGroup_EnergyMeter_Mode_MeasureOutWatts_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Watts",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "FileClose",
        new BlockInfo
        {
            ShortName = "FileAccess.Close",
            Reference = "FileClose",
            IconName = "PolyGroup_FileAccess_Mode_Close_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "FileName", new BlockParamInfo { DataType = "String", CallDirectionInput = true } } }
        }
    },
    {
        "FileDelete",
        new BlockInfo
        {
            ShortName = "FileAccess.Delete",
            Reference = "FileDelete",
            IconName = "PolyGroup_FileAccess_Mode_Delete_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "FileName", new BlockParamInfo { DataType = "String", CallDirectionInput = true } } }
        }
    },
    {
        "FileReadNumeric",
        new BlockInfo
        {
            ShortName = "FileAccess.Numeric",
            Reference = "FileReadNumeric",
            IconName = "PolyGroup_FileAccess_Mode_Numeric_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "FileName",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true }
                },
                {
                    "Numeric",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "FileReadText",
        new BlockInfo
        {
            ShortName = "FileAccess.Text",
            Reference = "FileReadText",
            IconName = "PolyGroup_FileAccess_Mode_Text_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "FileName",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true }
                },
                {
                    "Text",
                    new BlockParamInfo { DataType = "String" }
                }
            }
        }
    },
    {
        "FileWriteText",
        new BlockInfo
        {
            ShortName = "FileAccess.Write",
            Reference = "FileWriteText",
            IconName = "PolyGroup_FileAccess_Mode_Write_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "FileName",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true }
                },
                {
                    "TextIn",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "GyroAngleAndRate",
        new BlockInfo
        {
            ShortName = "Gyro.MeasureAngleAndRate",
            Reference = "GyroAngleAndRate",
            IconName = "PolyGroup_Gyro_Mode_MeasureAngleAndRate_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Angle",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Rate",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "GyroAngleCompare",
        new BlockInfo
        {
            ShortName = "Gyro.CompareAngle",
            Reference = "GyroAngleCompare",
            IconName = "PolyGroup_Gyro_Mode_CompareAngle_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Angle",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "GyroDegrees",
        new BlockInfo
        {
            ShortName = "Gyro.MeasureAngle",
            Reference = "GyroDegrees",
            IconName = "PolyGroup_Gyro_Mode_MeasureAngle_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Angle",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "GyroRate",
        new BlockInfo
        {
            ShortName = "Gyro.MeasureRate",
            Reference = "GyroRate",
            IconName = "PolyGroup_Gyro_Mode_MeasureRate_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Rate",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "GyroRateCompare",
        new BlockInfo
        {
            ShortName = "Gyro.CompareRate",
            Reference = "GyroRateCompare",
            IconName = "PolyGroup_Gyro_Mode_CompareRate_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Rate",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "GyroReset",
        new BlockInfo
        {
            ShortName = "Gyro.Reset",
            Reference = "GyroReset",
            IconName = "PolyGroup_Gyro_Mode_Reset_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { { "Port", new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" } } }
        }
    },
    {
        "InvertMotor",
        new BlockInfo
        {
            ShortName = "InvertMotor",
            Reference = "InvertMotor",
            IconName = "PolyGroup_InvertMotor_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Invert",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "IRProximity",
        new BlockInfo
        {
            ShortName = "InfraredSensor.MeasureProximity",
            Reference = "IRProximity",
            IconName = "PolyGroup_InfraredSensor_Mode_MeasureProximity_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Proximity",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "IRProximityChange",
        new BlockInfo
        {
            ShortName = "InfraredSensor.ChangeProximity",
            Reference = "IRProximityChange",
            IconName = "PolyGroup_InfraredSensor_Mode_ChangeProximity_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Proximity",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, IsResult = true }
                }
            }
        }
    },
    {
        "IRProximityCompare",
        new BlockInfo
        {
            ShortName = "InfraredSensor.CompareProximity",
            Reference = "IRProximityCompare",
            IconName = "PolyGroup_InfraredSensor_Mode_CompareProximity_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Proximity",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "IRRemote",
        new BlockInfo
        {
            ShortName = "InfraredSensor.MeasureBeaconRemote",
            Reference = "IRRemote",
            IconName = "PolyGroup_InfraredSensor_Mode_MeasureBeaconRemote_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                },
                {
                    "Button",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, Identification = "OutputButtonID", /* added later */ IsResult=true  }
                }
            }
        }
    },
    {
        "IRRemoteChange",
        new BlockInfo
        {
            ShortName = "InfraredSensor.ChangeRemote",
            Reference = "IRRemoteChange",
            IconName = "PolyGroup_InfraredSensor_Mode_ChangeRemote_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Button",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, Identification = "OutputButtonID" }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                }
            }
        }
    },
    {
        "IRRemoteCompare",
        new BlockInfo
        {
            ShortName = "InfraredSensor.CompareRemote",
            Reference = "IRRemoteCompare",
            IconName = "PolyGroup_InfraredSensor_Mode_CompareRemote_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                },
                {
                    "Set_of_remote_button_IDs",
                    new BlockParamInfo { DataType = "Single[]", CallIndex = 2, CallDirectionInput = true, Identification = "OutputButtonID" }
                },
                {
                    "Button",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, Identification = "OutputButtonID" }
                }
            }
        }
    },
    {
        "IRSeeker",
        new BlockInfo
        {
            ShortName = "InfraredSensor.MeasureBeaconSeeker",
            Reference = "IRSeeker",
            IconName = "PolyGroup_InfraredSensor_Mode_MeasureBeaconSeeker_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Heading",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                },
                {
                    "Proximity",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Valid",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3 }
                }
            }
        }
    },
    {
        "IRSeekerDataloggingHeading",
        new BlockInfo
        {
            ShortName = "InfraredSensor.MeasureBeaconSeekerDataloggingHeading",
            Reference = "IRSeekerDataloggingHeading",
            IconName = "PolyGroup_InfraredSensor_Mode_MeasureBeaconSeekerDataloggingHeading_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Heading",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "IRSeekerDataloggingProximity",
        new BlockInfo
        {
            ShortName = "InfraredSensor.MeasureBeaconSeekerDataloggingProximity",
            Reference = "IRSeekerDataloggingProximity",
            IconName = "PolyGroup_InfraredSensor_Mode_MeasureBeaconSeekerDataloggingProximity_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Proximity",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "IRTrackerChangeHeading",
        new BlockInfo
        {
            ShortName = "InfraredSensor.ChangeHeading",
            Reference = "IRTrackerChangeHeading",
            IconName = "PolyGroup_InfraredSensor_Mode_ChangeHeading_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                },
                {
                    "Heading",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "HeadingAmount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "IRTrackerChangeProximity",
        new BlockInfo
        {
            ShortName = "InfraredSensor.ChangeBeaconProximity",
            Reference = "IRTrackerChangeProximity",
            IconName = "PolyGroup_InfraredSensor_Mode_ChangeBeaconProximity_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                },
                {
                    "Proximity",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "UInt32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 5, IsResult = true }
                }
            }
        }
    },
    {
        "IRTrackerCompareHeading",
        new BlockInfo
        {
            ShortName = "InfraredSensor.CompareBeaconSeekerHeading",
            Reference = "IRTrackerCompareHeading",
            IconName = "PolyGroup_InfraredSensor_Mode_CompareBeaconSeekerHeading_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Heading",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "HeadingThreshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "IRTrackerCompareProximity",
        new BlockInfo
        {
            ShortName = "InfraredSensor.CompareBeaconSeekerProximity",
            Reference = "IRTrackerCompareProximity",
            IconName = "PolyGroup_InfraredSensor_Mode_CompareBeaconSeekerProximity_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Proximity",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Channel",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "SetOfChannels" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "KeepAlive",
        new BlockInfo
        {
            ShortName = "KeepAlive",
            Reference = "KeepAlive",
            IconName = "PolyGroup_KeepAlive_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "Time_until_sleep_(ms)", new BlockParamInfo { DataType = "Single" } } }
        }
    },
    {
        "LedOff",
        new BlockInfo
        {
            ShortName = "LED.Off",
            Reference = "LedOff",
            IconName = "PolyGroup_LED_Mode_Off_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "LedOn",
        new BlockInfo
        {
            ShortName = "LED.On",
            Reference = "LedOn",
            IconName = "PolyGroup_LED_Mode_On_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Color",
                    new BlockParamInfo { DataType = "Int32", CallDirectionInput = true, Identification = "LEDColor" }
                },
                {
                    "Pulse",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "LedReset",
        new BlockInfo
        {
            ShortName = "LED.Reset",
            Reference = "LedReset",
            IconName = "PolyGroup_LED_Mode_Reset_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "MediumMotorDistance",
        new BlockInfo
        {
            ShortName = "MediumMotor.Degrees",
            Reference = "MediumMotorDistance",
            IconName = "PolyGroup_MediumMotor_Mode_Degrees_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Degrees",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MediumMotorDistanceRotations",
        new BlockInfo
        {
            ShortName = "MediumMotor.Rotations",
            Reference = "MediumMotorDistanceRotations",
            IconName = "PolyGroup_MediumMotor_Mode_Rotations_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Rotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MediumMotorStop",
        new BlockInfo
        {
            ShortName = "MediumMotor.Stop",
            Reference = "MediumMotorStop",
            IconName = "PolyGroup_MediumMotor_Mode_Stop_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MediumMotorTime",
        new BlockInfo
        {
            ShortName = "MediumMotor.Time",
            Reference = "MediumMotorTime",
            IconName = "PolyGroup_MediumMotor_Mode_Time_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Seconds",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MediumMotorUnlimited",
        new BlockInfo
        {
            ShortName = "MediumMotor.Unlimited",
            Reference = "MediumMotorUnlimited",
            IconName = "PolyGroup_MediumMotor_Mode_Unlimited_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "MessageChangeValueBoolean",
        new BlockInfo
        {
            ShortName = "Messaging.ChangeBooleanValue",
            Reference = "MessageChangeValueBoolean",
            IconName = "PolyGroup_Messaging_Mode_ChangeBooleanValue_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2 }
                }
            }
        }
    },
    {
        "MessageChangeValueNumeric",
        new BlockInfo
        {
            ShortName = "Messaging.ChangeNumericValue",
            Reference = "MessageChangeValueNumeric",
            IconName = "PolyGroup_Messaging_Mode_ChangeNumericValue_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Change",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "MessageChangeValueText",
        new BlockInfo
        {
            ShortName = "Messaging.ChangeTextValue",
            Reference = "MessageChangeValueText",
            IconName = "PolyGroup_Messaging_Mode_ChangeTextValue_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "String", CallIndex = 1 }
                }
            }
        }
    },
    {
        "MessageCompareBoolean",
        new BlockInfo
        {
            ShortName = "Messaging.CompareBoolean",
            Reference = "MessageCompareBoolean",
            IconName = "PolyGroup_Messaging_Mode_CompareBoolean_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2, IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3 }
                }
            }
        }
    },
    {
        "MessageCompareNumeric",
        new BlockInfo
        {
            ShortName = "Messaging.CompareNumeric",
            Reference = "MessageCompareNumeric",
            IconName = "PolyGroup_Messaging_Mode_CompareNumeric_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 3, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 4, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "MessageCompareText",
        new BlockInfo
        {
            ShortName = "Messaging.CompareText",
            Reference = "MessageCompareText",
            IconName = "PolyGroup_Messaging_Mode_CompareText_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "String", CallIndex = 1 }
                },
                {
                    "Comparison2",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 2, CallDirectionInput = true, Identification = "ComparisonType2" }
                },
                {
                    "ComparisonText",
                    new BlockParamInfo { DataType = "String", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "MessageUpdateBoolean",
        new BlockInfo
        {
            ShortName = "Messaging.ChangeBooleanUpdate",
            Reference = "MessageUpdateBoolean",
            IconName = "PolyGroup_Messaging_Mode_ChangeBooleanUpdate_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1 }
                }
            }
        }
    },
    {
        "MessageUpdateNumeric",
        new BlockInfo
        {
            ShortName = "Messaging.ChangeNumericUpdate",
            Reference = "MessageUpdateNumeric",
            IconName = "PolyGroup_Messaging_Mode_ChangeNumericUpdate_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "MessageUpdateText",
        new BlockInfo
        {
            ShortName = "Messaging.ChangeTextUpdate",
            Reference = "MessageUpdateText",
            IconName = "PolyGroup_Messaging_Mode_ChangeTextUpdate_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "String", CallIndex = 1 }
                }
            }
        }
    },
    {
        "MotorDistance",
        new BlockInfo
        {
            ShortName = "Motor.Degrees",
            Reference = "MotorDistance",
            IconName = "PolyGroup_Motor_Mode_Degrees_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Degrees",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MotorDistanceRotations",
        new BlockInfo
        {
            ShortName = "Motor.Rotations",
            Reference = "MotorDistanceRotations",
            IconName = "PolyGroup_Motor_Mode_Rotations_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Rotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 3, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MotorSpeedChange",
        new BlockInfo
        {
            ShortName = "RotationSensor.ChangeSpeed",
            Reference = "MotorSpeedChange",
            IconName = "PolyGroup_RotationSensor_Mode_ChangeSpeed_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, IsResult = true }
                }
            }
        }
    },
    {
        "MotorSpeedCompare",
        new BlockInfo
        {
            ShortName = "RotationSensor.CompareCurrentSpeed",
            Reference = "MotorSpeedCompare",
            IconName = "PolyGroup_RotationSensor_Mode_CompareCurrentSpeed_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "ThresholdSpeed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "MotorSpeedSensor",
        new BlockInfo
        {
            ShortName = "RotationSensor.MeasureCurrentSpeed",
            Reference = "MotorSpeedSensor",
            IconName = "PolyGroup_RotationSensor_Mode_MeasureCurrentSpeed_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                }
            }
        }
    },
    {
        "MotorStop",
        new BlockInfo
        {
            ShortName = "Motor.Stop",
            Reference = "MotorStop",
            IconName = "PolyGroup_Motor_Mode_Stop_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MotorTime",
        new BlockInfo
        {
            ShortName = "Motor.Time",
            Reference = "MotorTime",
            IconName = "PolyGroup_Motor_Mode_Time_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Seconds",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MotorUnlimited",
        new BlockInfo
        {
            ShortName = "Motor.Unlimited",
            Reference = "MotorUnlimited",
            IconName = "PolyGroup_Motor_Mode_Unlimited_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "MoveDistance",
        new BlockInfo
        {
            ShortName = "Move.Degrees",
            Reference = "MoveDistance",
            IconName = "PolyGroup_Move_Mode_Degrees_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Steering",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Degrees",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveDistanceRotations",
        new BlockInfo
        {
            ShortName = "Move.Rotations",
            Reference = "MoveDistanceRotations",
            IconName = "PolyGroup_Move_Mode_Rotations_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Steering",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Rotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveStop",
        new BlockInfo
        {
            ShortName = "Move.Stop",
            Reference = "MoveStop",
            IconName = "PolyGroup_Move_Mode_Stop_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveTankDistance",
        new BlockInfo
        {
            ShortName = "MoveTank.Degrees",
            Reference = "MoveTankDistance",
            IconName = "PolyGroup_MoveTank_Mode_Degrees_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Speed_Left",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Speed_Right",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Degrees",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveTankDistanceRotations",
        new BlockInfo
        {
            ShortName = "MoveTank.Rotations",
            Reference = "MoveTankDistanceRotations",
            IconName = "PolyGroup_MoveTank_Mode_Rotations_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Speed_Left",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Speed_Right",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Rotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveTankMode",
        new BlockInfo
        {
            ShortName = "MoveTank.Unlimited",
            Reference = "MoveTankMode",
            IconName = "PolyGroup_MoveTank_Mode_Unlimited_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Speed_Left",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Speed_Right",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "MoveTankStop",
        new BlockInfo
        {
            ShortName = "MoveTank.Stop",
            Reference = "MoveTankStop",
            IconName = "PolyGroup_MoveTank_Mode_Stop_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveTankTime",
        new BlockInfo
        {
            ShortName = "MoveTank.Time",
            Reference = "MoveTankTime",
            IconName = "PolyGroup_MoveTank_Mode_Time_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Speed_Left",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Speed_Right",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Seconds",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveTime",
        new BlockInfo
        {
            ShortName = "Move.Time",
            Reference = "MoveTime",
            IconName = "PolyGroup_Move_Mode_Time_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Steering",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Seconds",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Brake_At_End",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, CallDirectionInput = true, Identification = "BrakeAtEnd" }
                }
            }
        }
    },
    {
        "MoveUnlimited",
        new BlockInfo
        {
            ShortName = "Move.Unlimited",
            Reference = "MoveUnlimited",
            IconName = "PolyGroup_Move_Mode_Unlimited_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Ports",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "TwoOutputPorts" }
                },
                {
                    "Steering",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Speed",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "PlayNote",
        new BlockInfo
        {
            ShortName = "Sound.Note",
            Reference = "PlayNote",
            IconName = "PolyGroup_Sound_Mode_Note_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Note",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true }
                },
                {
                    "Duration",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Volume",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Play_Type",
                    new BlockParamInfo { DataType = "UInt16", CallIndex = 4, CallDirectionInput = true, Identification = "PlayType" }
                }
            }
        }
    },
    {
        "PlaySoundFile",
        new BlockInfo
        {
            ShortName = "Sound.File",
            Reference = "PlaySoundFile",
            IconName = "PolyGroup_Sound_Mode_File_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Volume",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Play_Type",
                    new BlockParamInfo { DataType = "UInt16", CallIndex = 2, CallDirectionInput = true, Identification = "PlayType" }
                }
            }
        }
    },
    {
        "PlaySoundStop",
        new BlockInfo
        {
            ShortName = "Sound.Stop",
            Reference = "PlaySoundStop",
            IconName = "PolyGroup_Sound_Mode_Stop_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "PlayTone",
        new BlockInfo
        {
            ShortName = "Sound.Tone",
            Reference = "PlayTone",
            IconName = "PolyGroup_Sound_Mode_Tone_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Frequency",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Duration",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Volume",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Play_Type",
                    new BlockParamInfo { DataType = "UInt16", CallIndex = 3, CallDirectionInput = true, Identification = "PlayType" }
                }
            }
        }
    },
    {
        "RandomBoolean",
        new BlockInfo
        {
            ShortName = "Random.Boolean",
            Reference = "RandomBoolean",
            IconName = "PolyGroup_Random_Mode_Boolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "Percent_True",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                }
            }
        }
    },
    {
        "RandomSingle",
        new BlockInfo
        {
            ShortName = "Random.Numeric",
            Reference = "RandomSingle",
            IconName = "PolyGroup_Random_Mode_Numeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Lower",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Number",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Upper",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "RawMotorOn",
        new BlockInfo
        {
            ShortName = "UnregulatedMotor",
            Reference = "RawMotorOn",
            IconName = "PolyGroup_UnregulatedMotor_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Power",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "RawSensorValue",
        new BlockInfo
        {
            ShortName = "RawSensorValue",
            Reference = "RawSensorValue",
            IconName = "PolyGroup_RawSensorValue_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port_Number",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Raw_Value",
                    new BlockParamInfo { DataType = "Single" }
                }
            }
        }
    },
    {
        "ReceiveMessage",
        new BlockInfo
        {
            ShortName = "Messaging.ReceiveText",
            Reference = "ReceiveMessage",
            IconName = "PolyGroup_Messaging_Mode_ReceiveText_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "String" }
                },
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true, IsVisibilitySpecial = true }
                }
            }
        }
    },
    {
        "ReceiveMessageBoolean",
        new BlockInfo
        {
            ShortName = "Messaging.ReceiveBoolean",
            Reference = "ReceiveMessageBoolean",
            IconName = "PolyGroup_Messaging_Mode_ReceiveBoolean_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true, IsVisibilitySpecial = true }
                }
            }
        }
    },
    {
        "ReceiveMessageNumeric",
        new BlockInfo
        {
            ShortName = "Messaging.ReceiveNumeric",
            Reference = "ReceiveMessageNumeric",
            IconName = "PolyGroup_Messaging_Mode_ReceiveNumeric_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "ReceivedMessage",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true, IsVisibilitySpecial = true }
                }
            }
        }
    },
    {
        "RotationDegreesChange",
        new BlockInfo
        {
            ShortName = "RotationSensor.ChangeDegrees",
            Reference = "RotationDegreesChange",
            IconName = "PolyGroup_RotationSensor_Mode_ChangeDegrees_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Degrees",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, IsResult = true }
                }
            }
        }
    },
    {
        "RotationDegreesCompare",
        new BlockInfo
        {
            ShortName = "RotationSensor.CompareDegrees",
            Reference = "RotationDegreesCompare",
            IconName = "PolyGroup_RotationSensor_Mode_CompareDegrees_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Degrees",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdDegrees",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "RotationReset",
        new BlockInfo
        {
            ShortName = "RotationSensor.Reset",
            Reference = "RotationReset",
            IconName = "PolyGroup_RotationSensor_Mode_Reset_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { { "MotorPort", new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" } } }
        }
    },
    {
        "RotationRotationsChange",
        new BlockInfo
        {
            ShortName = "RotationSensor.ChangeRotation",
            Reference = "RotationRotationsChange",
            IconName = "PolyGroup_RotationSensor_Mode_ChangeRotation_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Rotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountRotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, IsResult = true }
                }
            }
        }
    },
    {
        "RotationRotationsCompare",
        new BlockInfo
        {
            ShortName = "RotationSensor.CompareRotation",
            Reference = "RotationRotationsCompare",
            IconName = "PolyGroup_RotationSensor_Mode_CompareRotation_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Rotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdRotations",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "RotationValue",
        new BlockInfo
        {
            ShortName = "RotationSensor.MeasureDegrees",
            Reference = "RotationValue",
            IconName = "PolyGroup_RotationSensor_Mode_MeasureDegrees_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Degrees",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                }
            }
        }
    },
    {
        "RotationValueRotations",
        new BlockInfo
        {
            ShortName = "RotationSensor.MeasureRotation",
            Reference = "RotationValueRotations",
            IconName = "PolyGroup_RotationSensor_Mode_MeasureRotation_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Rotations",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "MotorPort",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneOutputPort" }
                }
            }
        }
    },
    {
        "Round_Down",
        new BlockInfo
        {
            ShortName = "Round.Down",
            Reference = "Round_Down",
            IconName = "PolyGroup_Round_Mode_Down_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Output_Result",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Input",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Round_Nearest",
        new BlockInfo
        {
            ShortName = "Round.Nearest",
            Reference = "Round_Nearest",
            IconName = "PolyGroup_Round_Mode_Nearest_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Output_Result",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Input",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Round_Truncate",
        new BlockInfo
        {
            ShortName = "Round.Truncate",
            Reference = "Round_Truncate",
            IconName = "PolyGroup_Round_Mode_Truncate_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Output_Result",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Input",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Number_of_Decimals",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Round_Up",
        new BlockInfo
        {
            ShortName = "Round.Up",
            Reference = "Round_Up",
            IconName = "PolyGroup_Round_Mode_Up_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Output_Result",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Input",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                }
            }
        }
    },
    {
        "SendMessage",
        new BlockInfo
        {
            ShortName = "Messaging.SendText",
            Reference = "SendMessage",
            IconName = "PolyGroup_Messaging_Mode_SendText_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Receiving_Brick_Name",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "SentMessage",
                    new BlockParamInfo { DataType = "String", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "SendMessageBoolean",
        new BlockInfo
        {
            ShortName = "Messaging.SendBoolean",
            Reference = "SendMessageBoolean",
            IconName = "PolyGroup_Messaging_Mode_SendBoolean_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Receiving_Brick_Name",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "SentMessage",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "SendMessageNumeric",
        new BlockInfo
        {
            ShortName = "Messaging.SendNumeric",
            Reference = "SendMessageNumeric",
            IconName = "PolyGroup_Messaging_Mode_SendNumeric_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Message_Title",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Receiving_Brick_Name",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "SentMessage",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "StopBlock",
        new BlockInfo
        {
            ShortName = "StopBlock",
            Reference = "StopBlock",
            IconName = "PolyGroup_StopBlock_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "TemperatureChangeCelsius",
        new BlockInfo
        {
            ShortName = "TemperatureSensor.ChangeCelsius",
            Reference = "TemperatureChangeCelsius",
            IconName = "PolyGroup_TemperatureSensor_Mode_ChangeCelsius_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Celsius",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "AmountC",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "TemperatureChangeFahrenheit",
        new BlockInfo
        {
            ShortName = "TemperatureSensor.ChangeFahrenheit",
            Reference = "TemperatureChangeFahrenheit",
            IconName = "PolyGroup_TemperatureSensor_Mode_ChangeFahrenheit_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Direction",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "WaitForChange" }
                },
                {
                    "Fahrenheit",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "TemperatureCompareCelsius",
        new BlockInfo
        {
            ShortName = "TemperatureSensor.CompareCelsius",
            Reference = "TemperatureCompareCelsius",
            IconName = "PolyGroup_TemperatureSensor_Mode_CompareCelsius_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Celsius",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdC",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "TemperatureCompareFahrenheit",
        new BlockInfo
        {
            ShortName = "TemperatureSensor.CompareFahrenheit",
            Reference = "TemperatureCompareFahrenheit",
            IconName = "PolyGroup_TemperatureSensor_Mode_CompareFahrenheit_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Fahrenheit",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "ThresholdF",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "TemperatureValueCelsius",
        new BlockInfo
        {
            ShortName = "TemperatureSensor.MeasureCelsius",
            Reference = "TemperatureValueCelsius",
            IconName = "PolyGroup_TemperatureSensor_Mode_MeasureCelsius_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Celsius",
                    new BlockParamInfo { DataType = "Single" }
                }
            }
        }
    },
    {
        "TemperatureValueFahrenheit",
        new BlockInfo
        {
            ShortName = "TemperatureSensor.MeasureFahrenheit",
            Reference = "TemperatureValueFahrenheit",
            IconName = "PolyGroup_TemperatureSensor_Mode_MeasureFahrenheit_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Fahrenheit",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "TimeCompare",
        new BlockInfo
        {
            ShortName = "Wait.Timer",
            Reference = "TimeCompare",
            IconName = "Polygroup_Wait_Mode_Timer_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "How_Long",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                }
            }
        }
    },
    {
        "TimeCompareLoop",
        new BlockInfo
        {
            ShortName = "LoopCondition.Time",
            Reference = "TimeCompareLoop",
            IconName = "PolyGroup_LoopCondition_Mode_Time_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "How_Long",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                }
            }
        }
    },
    {
        "TimerChange",
        new BlockInfo
        {
            ShortName = "Timer.ChangeTime",
            Reference = "TimerChange",
            IconName = "PolyGroup_Timer_Mode_ChangeTime_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Timer",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Timer_Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Amount",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 4, IsResult = true }
                }
            }
        }
    },
    {
        "TimerCompare",
        new BlockInfo
        {
            ShortName = "Timer.CompareTime",
            Reference = "TimerCompare",
            IconName = "PolyGroup_Timer_Mode_CompareTime_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Timer",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Timer_Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Threshold",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "TimerReset",
        new BlockInfo
        {
            ShortName = "Timer.Reset",
            Reference = "TimerReset",
            IconName = "PolyGroup_Timer_Mode_Reset_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { { "Timer", new BlockParamInfo { DataType = "Single", CallDirectionInput = true } } }
        }
    },
    {
        "TimerValue",
        new BlockInfo
        {
            ShortName = "Timer.MeasureTime",
            Reference = "TimerValue",
            IconName = "PolyGroup_Timer_Mode_MeasureTime_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Timer",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Timer_Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "TouchChange",
        new BlockInfo
        {
            ShortName = "TouchSensor.ChangeState",
            Reference = "TouchChange",
            IconName = "PolyGroup_TouchSensor_Mode_ChangeState_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "State",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1 }
                }
            }
        }
    },
    {
        "TouchCompare",
        new BlockInfo
        {
            ShortName = "TouchSensor.Compare",
            Reference = "TouchCompare",
            IconName = "PolyGroup_TouchSensor_Mode_Compare_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Pressed__Released_or_Bumped",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, Identification = "TouchState" }
                }
            }
        }
    },
    {
        "TouchValue",
        new BlockInfo
        {
            ShortName = "TouchSensor.Measure",
            Reference = "TouchValue",
            IconName = "PolyGroup_TouchSensor_Mode_Measure_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "State",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "VolumeCompare",
        new BlockInfo
        {
            ShortName = "SoundSensor.comparedB",
            Reference = "VolumeCompare",
            IconName = "PolyGroup_SoundSensor_Mode_comparedB_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold_Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "VolumeCompareDBA",
        new BlockInfo
        {
            ShortName = "SoundSensor.comparedBa",
            Reference = "VolumeCompareDBA",
            IconName = "PolyGroup_SoundSensor_Mode_comparedBa_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                },
                {
                    "Comparison",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true, Identification = "ComparisonType" }
                },
                {
                    "ValuedBa",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Threshold_Value",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "VolumeValue",
        new BlockInfo
        {
            ShortName = "SoundSensor.measuredB",
            Reference = "VolumeValue",
            IconName = "PolyGroup_SoundSensor_Mode_measuredB_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Value",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "VolumeValuedBa",
        new BlockInfo
        {
            ShortName = "SoundSensor.measuredBa",
            Reference = "VolumeValuedBa",
            IconName = "PolyGroup_SoundSensor_Mode_measuredBa_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "ValuedBa",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "Port",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true, Identification = "OneInputPort" }
                }
            }
        }
    },
    {
        "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMaster",
        new BlockInfo
        {
            ShortName = "Datalogging.OnForTimeSeconds",
            Reference = "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMaster",
            IconName = "PolyGroup_Datalogging_Mode_OnForTimeSeconds_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Duration",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Rate",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Rate_Mode",
                    new BlockParamInfo { DataType = "Single", CallIndex = 4, CallDirectionInput = true, Identification = "RateUnit" }
                }
            }
        }
    },
    {
        "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMasterMinutes",
        new BlockInfo
        {
            ShortName = "Datalogging.OnForTimeMinute",
            Reference = "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMasterMinutes",
            IconName = "PolyGroup_Datalogging_Mode_OnForTimeMinute_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Duration",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Rate",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Rate_Mode",
                    new BlockParamInfo { DataType = "Single", CallIndex = 4, CallDirectionInput = true, Identification = "RateUnit" }
                }
            }
        }
    },
    {
        "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMasterOn",
        new BlockInfo
        {
            ShortName = "Datalogging.On",
            Reference = "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMasterOn",
            IconName = "PolyGroup_Datalogging_Mode_On_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "Rate",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Rate_Mode",
                    new BlockParamInfo { DataType = "Single", CallIndex = 4, CallDirectionInput = true, Identification = "RateUnit" }
                }
            }
        }
    },
    {
        "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMasterSingle",
        new BlockInfo
        {
            ShortName = "Datalogging.SingleMeasurement",
            Reference = "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogMasterSingle",
            IconName = "PolyGroup_Datalogging_Mode_SingleMeasurement_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "Name", new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true } } }
        }
    },
    {
        "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogStop",
        new BlockInfo
        {
            ShortName = "Datalogging.Stop",
            Reference = "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_DataLogStop",
            IconName = "PolyGroup_Datalogging_Mode_Stop_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "Name", new BlockParamInfo { DataType = "String", CallIndex = 5, CallDirectionInput = true, IsVisibilitySpecial = true } } }
        }
    },
    {
        "Arithmetic_AbsoluteValue",
        new BlockInfo
        {
            ShortName = "Math.AbsoluteValue",
            Reference = "Arithmetic_AbsoluteValue",
            IconName = "PolyGroup_Math_Mode_AbsoluteValue_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo> { { "X", new BlockParamInfo { DataType = "Single", CallDirectionInput = true } }, { "Result", new BlockParamInfo { DataType = "Single" } } }
        }
    },
    {
        "Arithmetic_Add",
        new BlockInfo
        {
            ShortName = "Math.Add",
            Reference = "Arithmetic_Add",
            IconName = "PolyGroup_Math_Mode_Add_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Arithmetic_Divide",
        new BlockInfo
        {
            ShortName = "Math.Divide",
            Reference = "Arithmetic_Divide",
            IconName = "PolyGroup_Math_Mode_Divide_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Arithmetic_Multiply",
        new BlockInfo
        {
            ShortName = "Math.Multiply",
            Reference = "Arithmetic_Multiply",
            IconName = "PolyGroup_Math_Mode_Multiply_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Arithmetic_Power",
        new BlockInfo
        {
            ShortName = "Math.Exponent",
            Reference = "Arithmetic_Power",
            IconName = "PolyGroup_Math_Mode_Exponent_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Base",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Exponent",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Arithmetic_SquareRoot",
        new BlockInfo
        {
            ShortName = "Math.SquareRoot",
            Reference = "Arithmetic_SquareRoot",
            IconName = "PolyGroup_Math_Mode_SquareRoot_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo> { { "X", new BlockParamInfo { DataType = "Single", CallDirectionInput = true } }, { "Result", new BlockParamInfo { DataType = "Single" } } }
        }
    },
    {
        "Arithmetic_Subtract",
        new BlockInfo
        {
            ShortName = "Math.Subtract",
            Reference = "Arithmetic_Subtract",
            IconName = "PolyGroup_Math_Mode_Subtract_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Boolean_And",
        new BlockInfo
        {
            ShortName = "BooleanOperations.And",
            Reference = "Boolean_And",
            IconName = "PolyGroup_BooleanOperations_Mode_And_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "A",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "B",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Boolean_Not",
        new BlockInfo
        {
            ShortName = "BooleanOperations.Not",
            Reference = "Boolean_Not",
            IconName = "PolyGroup_BooleanOperations_Mode_Not_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo> { { "Result", new BlockParamInfo { DataType = "Boolean" } }, { "A", new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true } } }
        }
    },
    {
        "Boolean_Or",
        new BlockInfo
        {
            ShortName = "BooleanOperations.Or",
            Reference = "Boolean_Or",
            IconName = "PolyGroup_BooleanOperations_Mode_Or_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "A",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "B",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Boolean_XOr",
        new BlockInfo
        {
            ShortName = "BooleanOperations.XOR",
            Reference = "Boolean_XOr",
            IconName = "PolyGroup_BooleanOperations_Mode_XOR_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "A",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "B",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "CaseSelector_Boolean",
        new BlockInfo
        {
            ShortName = "CaseSelector.Boolean",
            Reference = "CaseSelector_Boolean",
            IconName = "PolyGroup_CaseSelector_Mode_Boolean_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, IsResult = true }
                },
                {
                    "Boolean",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "CaseSelector_Numeric",
        new BlockInfo
        {
            ShortName = "CaseSelector.Numeric",
            Reference = "CaseSelector_Numeric",
            IconName = "PolyGroup_CaseSelector_Mode_Numeric_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Number",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Int32", CallIndex = 1, IsResult = true }
                }
            }
        }
    },
    {
        "CaseSelector_String",
        new BlockInfo
        {
            ShortName = "CaseSelector.String",
            Reference = "CaseSelector_String",
            IconName = "PolyGroup_CaseSelector_Mode_String_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "String",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, IsResult = true }
                }
            }
        }
    },
    {
        "CommentBlock",
        new BlockInfo
        {
            ShortName = "CommentBlock",
            Reference = "CommentBlock",
            IconName = "PolyGroup_CommentBlock_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Comment",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                    //Special handling, not serialzed as ConfigurableMethodTerminal, but as "Comment" property
                }
            }
        }
    },
    {
        "Comparison_Equal",
        new BlockInfo
        {
            ShortName = "Compare.Equal",
            Reference = "Comparison_Equal",
            IconName = "PolyGroup_Compare_Mode_Equal_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "x",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 2 }
                }
            }
        }
    },
    {
        "Comparison_Greater",
        new BlockInfo
        {
            ShortName = "Compare.GreaterThan",
            Reference = "Comparison_Greater",
            IconName = "PolyGroup_Compare_Mode_GreaterThan_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "x",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Comparison_GreaterEqual",
        new BlockInfo
        {
            ShortName = "Compare.GreaterOrEqual",
            Reference = "Comparison_GreaterEqual",
            IconName = "PolyGroup_Compare_Mode_GreaterOrEqual_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "x",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Comparison_Less",
        new BlockInfo
        {
            ShortName = "Compare.LessThan",
            Reference = "Comparison_Less",
            IconName = "PolyGroup_Compare_Mode_LessThan_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "x",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Comparison_LessEqual",
        new BlockInfo
        {
            ShortName = "Compare.LessOrEqual",
            Reference = "Comparison_LessEqual",
            IconName = "PolyGroup_Compare_Mode_LessOrEqual_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "x",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "Comparison_NotEqual",
        new BlockInfo
        {
            ShortName = "Compare.NotEqual",
            Reference = "Comparison_NotEqual",
            IconName = "PolyGroup_Compare_Mode_NotEqual_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "x",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "ConcatenateStrings",
        new BlockInfo
        {
            ShortName = "Text.Merge",
            Reference = "ConcatenateStrings",
            IconName = "PolyGroup_Text_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "String" }
                },
                {
                    "A",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true }
                },
                {
                    "B",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "C",
                    new BlockParamInfo { DataType = "String", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "InsideRange",
        new BlockInfo
        {
            ShortName = "Range.Inside",
            Reference = "InsideRange",
            IconName = "PolyGroup_Range_Mode_Inside_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Test_Value",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "Lower_Bound",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Upper_Bound",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "LoopIndex",
        new BlockInfo
        {
            ShortName = "LoopIndex",
            Reference = "LoopIndex",
            IconName = "PolyGroup_LoopIndex_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo> { { "Loop_Index", new BlockParamInfo { DataType = "Single" } } }
        }
    },
    {
        "OutsideRange",
        new BlockInfo
        {
            ShortName = "Range.Outside",
            Reference = "OutsideRange",
            IconName = "PolyGroup_Range_Mode_Outside_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Test_Value",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean" }
                },
                {
                    "Lower_Bound",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "Upper_Bound",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "StopAfterNumberIterations",
        new BlockInfo
        {
            ShortName = "LoopCondition.Count",
            Reference = "StopAfterNumberIterations",
            IconName = "PolyGroup_LoopCondition_Mode_Count_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Iterations_To_Run",
                    new BlockParamInfo { DataType = "Int32", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                }
            }
        }
    },
    {
        "StopIfTrue",
        new BlockInfo
        {
            ShortName = "LoopCondition.Boolean",
            Reference = "StopIfTrue",
            IconName = "PolyGroup_LoopCondition_Mode_Boolean_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Do_Stop",
                    new BlockParamInfo { DataType = "Boolean", CallDirectionInput = true }
                },
                {
                    "Result",
                    new BlockParamInfo { DataType = "Boolean", IsResult = true }
                }
            }
        }
    },
    {
        "StopNever",
        new BlockInfo
        {
            ShortName = "LoopCondition.Unlimited",
            Reference = "StopNever",
            IconName = "PolyGroup_LoopCondition_Mode_Unlimited_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo> { { "Result", new BlockParamInfo { DataType = "Boolean", IsResult = true } } }
        }
    },
    {
        "ToggleInterrupt",
        new BlockInfo
        {
            ShortName = "Interrupt",
            Reference = "ToggleInterrupt",
            IconName = "PolyGroup_Interrupt_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo> { { "InterruptName", new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true, IsVisibilitySpecial = true } } }
        }
    },
    {
        "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_MathEquation",
        new BlockInfo
        {
            ShortName = "Math.Advanced",
            Reference = "X3Placeholder_2A058539-ED76-4476-93FE-CCE8AA559C5A_MathEquation",
            IconName = "PolyGroup_Math_Mode_Advanced_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "Result",
                    new BlockParamInfo { DataType = "Single" }
                },
                {
                    "X",
                    new BlockParamInfo { DataType = "Single", CallDirectionInput = true }
                },
                {
                    "Y",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                },
                {
                    "C",
                    new BlockParamInfo { DataType = "Single", CallIndex = 2, CallDirectionInput = true }
                },
                {
                    "D",
                    new BlockParamInfo { DataType = "Single", CallIndex = 3, CallDirectionInput = true }
                },
                {
                    "Equation",
                    new BlockParamInfo { DataType = "String", CallIndex = 4, CallDirectionInput = true, IsVisibilitySpecial = true }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalGetString",
        new BlockInfo
        {
            ShortName = "Variable.ReadText",
            Reference = "X3.Lib:GlobalGetString",
            IconName = "PolyGroup_Variable_Mode_ReadText_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "String", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalGetSingle",
        new BlockInfo
        {
            ShortName = "Variable.ReadNumeric",
            Reference = "X3.Lib:GlobalGetSingle",
            IconName = "PolyGroup_Variable_Mode_ReadNumeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalGetBoolean",
        new BlockInfo
        {
            ShortName = "Variable.ReadBoolean",
            Reference = "X3.Lib:GlobalGetBoolean",
            IconName = "PolyGroup_Variable_Mode_ReadBoolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalGetNumericArray",
        new BlockInfo
        {
            ShortName = "Variable.ReadNumericArray",
            Reference = "X3.Lib:GlobalGetNumericArray",
            IconName = "PolyGroup_Variable_Mode_ReadNumericArray_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Single[]", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalGetBooleanArray",
        new BlockInfo
        {
            ShortName = "Variable.ReadBooleanArray",
            Reference = "X3.Lib:GlobalGetBooleanArray",
            IconName = "PolyGroup_Variable_Mode_ReadBooleanArray_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Boolean[]", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalSetString",
        new BlockInfo
        {
            ShortName = "Variable.WriteText",
            Reference = "X3.Lib:GlobalSetString",
            IconName = "PolyGroup_Variable_Mode_WriteText_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalSetSingle",
        new BlockInfo
        {
            ShortName = "Variable.WriteNumeric",
            Reference = "X3.Lib:GlobalSetSingle",
            IconName = "PolyGroup_Variable_Mode_WriteNumeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalSetBoolean",
        new BlockInfo
        {
            ShortName = "Variable.WriteBoolean",
            Reference = "X3.Lib:GlobalSetBoolean",
            IconName = "PolyGroup_Variable_Mode_WriteBoolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalSetNumericArray",
        new BlockInfo
        {
            ShortName = "Variable.WriteNumericArray",
            Reference = "X3.Lib:GlobalSetNumericArray",
            IconName = "PolyGroup_Variable_Mode_WriteNumericArray_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Single[]", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalSetBooleanArray",
        new BlockInfo
        {
            ShortName = "Variable.WriteBooleanArray",
            Reference = "X3.Lib:GlobalSetBooleanArray",
            IconName = "PolyGroup_Variable_Mode_WriteBooleanArray_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "name",
                    new BlockParamInfo { DataType = "String", CallDirectionInput = true, IsVisibilitySpecial = true }
                },
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Boolean[]", CallIndex = 1, CallDirectionInput = true }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalConstString",
        new BlockInfo
        {
            ShortName = "Constant.Text",
            Reference = "X3.Lib:GlobalConstString",
            IconName = "PolyGroup_Constant_Mode_Text_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "String", CallIndex = 1, CallDirectionInput = true, 
                        /* added later */ IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "String", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalConstSingle",
        new BlockInfo
        {
            ShortName = "Constant.Numeric",
            Reference = "X3.Lib:GlobalConstSingle",
            IconName = "PolyGroup_Constant_Mode_Numeric_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1, CallDirectionInput = true, 
                        /* added later */ IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Single", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalConstBoolean",
        new BlockInfo
        {
            ShortName = "Constant.Boolean",
            Reference = "X3.Lib:GlobalConstBoolean",
            IconName = "PolyGroup_Constant_Mode_Boolean_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1, CallDirectionInput = true, 
                        /* added later */ IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Boolean", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalConstNumericArray",
        new BlockInfo
        {
            ShortName = "Constant.NumericArray",
            Reference = "X3.Lib:GlobalConstNumericArray",
            IconName = "PolyGroup_Constant_Mode_NumericArray_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Single[]", CallIndex = 1, CallDirectionInput = true, 
                        /* added later */ IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Single[]", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:GlobalConstBooleanArray",
        new BlockInfo
        {
            ShortName = "Constant.BooleanArray",
            Reference = "X3.Lib:GlobalConstBooleanArray",
            IconName = "PolyGroup_Constant_Mode_BooleanArray_Diagram.png",
            BlockFamily = "DataOperations",
            Params = new Dictionary<string, BlockParamInfo>
            {
                {
                    "valueIn",
                    new BlockParamInfo { DataType = "Boolean[]", CallIndex = 1, CallDirectionInput = true, 
                        /* added later */ IsVisibilitySpecial = true }
                },
                {
                    "valueOut",
                    new BlockParamInfo { DataType = "Boolean[]", CallIndex = 1 }
                }
            }
        }
    },
    {
        "X3.Lib:StartBlockTest",
        new BlockInfo
        {
            ShortName = "StartBlock",
            Reference = "X3.Lib:StartBlockTest",
            IconName = "PolyGroup_StartBlock_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo> {
                // removed: manually
                //{
                //    "Result",
                //    new BlockParamInfo { DataType = "Boolean", CallIndex = 0, IsResult = true }
                //}
            }
        }
    },
    {
        "neverUsed_D04426AB-522F-477F-86FA-4A1EEEC45449",
        new BlockInfo
        {
            ShortName = "HWPageManualInput",
            Reference = "neverUsed_D04426AB-522F-477F-86FA-4A1EEEC45449",
            IconName = "PolyGroup_HWPageManualInput_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "neverUsed_24800E4C-0E91-4B6C-A3B7-2C75465F04B8",
        new BlockInfo
        {
            ShortName = "HWPageManualOutput",
            Reference = "neverUsed_24800E4C-0E91-4B6C-A3B7-2C75465F04B8",
            IconName = "PolyGroup_HWPageManualOutput_Diagram.png",
            BlockFamily = "Sensor",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "X3CustomDfirSimple_2A058539-ED76-4476-93FE-CCE8AA559C5A_StartBlock",
        new BlockInfo
        {
            ShortName = "StartBlock",
            Reference = "X3CustomDfirSimple_2A058539-ED76-4476-93FE-CCE8AA559C5A_StartBlock",
            IconName = "PolyGroup_StartBlock_Diagram.png",
            BlockFamily = "FlowControl",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "PlayRecordedSoundFile",
        new BlockInfo
        {
            ShortName = "Sound.RecordedFile",
            Reference = "PlayRecordedSoundFile",
            IconName = "PolyGroup_Sound_Mode_File_Diagram.png",
            BlockFamily = "Action",
            Params = new Dictionary<string, BlockParamInfo> { }
        }
    },
    {
        "CommentBox",
        new BlockInfo
        {
            ShortName = "CommentBox",
            IconName = "PolyGroup_CommentBlock_Diagram.png",
            BlockFamily = "Advanced",
            Params = new Dictionary<string, BlockParamInfo> { { "Comment", new BlockParamInfo { DataType = "String", CallDirectionInput = true } } }
        }
    }
}

;
    }
}