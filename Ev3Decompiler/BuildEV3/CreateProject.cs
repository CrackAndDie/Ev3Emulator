using EV3ModelLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EV3DecompilerLib.BuildEV3
{
    public class CreateProject : IEV3GZipBlock
    {
        public string Name { get { return "Project.lvprojx"; } }
        public List<string> EV3PFiles = new List<string>();
        public List<string> EV3MBXMLPFiles = new List<string>();
        public string GetContent()
        {
            var xTarget = XElement.Parse(@"
<Target DocumentTypeIdentifier='VIVMTarget' Name='VI\ Virtual\ Machine'>
    <ProjectReference ReferenceName='NationalInstruments.VI.VirtualMachine.Runtime, Version=0.0.0.0' ReferencePath='' />
    <ProjectReference ReferenceName='NationalInstruments.LabVIEW.CoreRuntime, Version=0.0.0.0' ReferencePath='' />
    <DefinitionReference DocumentTypeIdentifier='NationalInstruments.X3.App.X3FolderLoaderDefinition' Name='vi\.lib_' Bindings='Envoy,DefinitionReference,EmbeddedReference' />
    <DefinitionReference DocumentTypeIdentifier='NationalInstruments.ExternalFileSupport.Modeling.ExternalFileType' Name='___ProjectTitle' Bindings='Envoy,DefinitionReference,EmbeddedReference,ProjectItemDragDropDefaultService' />
    <DefinitionReference DocumentTypeIdentifier='NationalInstruments.ExternalFileSupport.Modeling.ExternalFileType' Name='___ProjectDescription' Bindings='Envoy,DefinitionReference,EmbeddedReference,ProjectItemDragDropDefaultService' />
    <DefinitionReference DocumentTypeIdentifier='NationalInstruments.ExternalFileSupport.Modeling.ExternalFileType' Name='___CopyrightYear' Bindings='Envoy,DefinitionReference,EmbeddedReference,ProjectItemDragDropDefaultService' />
    <DefinitionReference DocumentTypeIdentifier='NationalInstruments.X3.App.X3FolderLoaderDefinition' Name='vi\.lib_PBR' Bindings='Envoy,DefinitionReference,EmbeddedReference' />
</Target>");
            var xProjectSettings = XElement.Parse("<ProjectSettings><NamedGlobalData xmlns='http://www.ni.com/X3NamedGlobalData.xsd' /><ProjectOrigin Path='en-GB/Internal/FreePlayProgram.ev3' xmlns='http://www.ni.com/X3ProjectOrigin.xsd' /><DaisyChainMode On='False' xmlns='http://www.ni.com/X3ProjectPropertiesModel.xsd' /></ProjectSettings>");

            // -- add Program
            foreach (var file1 in EV3PFiles.Concat(EV3MBXMLPFiles))
            {
                //var ev3p = CreateProgramFromNode.FileNameConverter(prgname);
                var xb1 = new XElement("SourceFileReference").AddAttributes(new Dictionary<string, string>()
                {
                    ["StoragePath"] = file1,
                    ["RelativeStoragePath"] = file1,
                    ["OverridingDocumentTypeIdentifier"] = "X3VIDocument",
                    ["DocumentTypeIdentifier"] = "NationalInstruments.LabVIEW.VI.Modeling.VirtualInstrument",
                    ["Name"] = (char.IsNumber(file1.FirstOrDefault()) ? @"\" : null) + Utils.EscapeVIX(file1),
                    ["Bindings"] = "Envoy,DefinitionReference,EmbeddedReference,ProjectItemDragDropDefaultService"
                });
                xTarget.Add(xb1);
                //xb1.Add(XElement.Parse("<X3DocumentSettings ShowFileOnStartup='False' IsTeacherOnlyFile='False' IsHiddenDependency='False' xmlns='http://www.ni.com/X3DocumentSettings.xsd'/>"));
            }
            foreach (var file1 in EV3MBXMLPFiles)
            {
                var xb1 = new XElement("DefinitionReference").AddAttributes(new Dictionary<string, string>()
                {
                    ["DocumentTypeIdentifier"] = "NationalInstruments.ExternalFileSupport.Modeling.ExternalFileType",
                    ["Name"] = (char.IsNumber(file1.FirstOrDefault()) ? @"\" : null) + Utils.EscapeVIX(file1),
                    ["Bindings"] = "Envoy,DefinitionReference,EmbeddedReference,ProjectItemDragDropDefaultService"
                });
                xTarget.Add(xb1);
            }

            // -- project document
            var doc = new XDocument();
            var xelem2 = XExt.CreateXElementWithNS("SourceFile", "http://www.ni.com/SourceModel.xsd",
                        new XAttribute("Version", "1.0.2.10"),
                        new XElement("Namespace", new XAttribute("Name", "Default"),
                            XExt.CreateXElementWithNS("Project", "http://www.ni.com/Project.xsd",
                                xTarget, xProjectSettings)
                        ),
                        new XElement("Namespace", new XAttribute("Name", @"VI\ Virtual\ Machine"),
                            XExt.CreateXElementWithNS("VIVMTarget", "http://www.ni.com/VIVMTarget.xsd")),
                        new XElement("Namespace", new XAttribute("Name", @"vi\.lib_"),
                            XExt.CreateXElementWithNS("LoaderDefinition", "http://www.ni.com/LoaderDefinition.xsd",
                                new XElement("Type", "FolderLoaderDefinition"),
                                new XElement("Name", "vi.lib_"),
                                new XElement("Location")
                            )
                        ),
                        new XElement("Namespace", new XAttribute("Name", @"___ProjectTitle"),
                            XExt.CreateXElementWithNS("ExternalFile", "http://www.ni.com/ExternalFile.xsd",
                                new XElement("RelativeStoragePath", "___ProjectTitle"),
                                new XElement("StoragePath")
                            )
                        ),
                        new XElement("Namespace", new XAttribute("Name", @"___ProjectDescription"),
                            XExt.CreateXElementWithNS("ExternalFile", "http://www.ni.com/ExternalFile.xsd",
                                new XElement("RelativeStoragePath", "___ProjectDescription"),
                                new XElement("StoragePath")
                            )
                        ),
                        new XElement("Namespace", new XAttribute("Name", @"___CopyrightYear"),
                            XExt.CreateXElementWithNS("ExternalFile", "http://www.ni.com/ExternalFile.xsd",
                                new XElement("RelativeStoragePath", "___CopyrightYear"),
                                new XElement("StoragePath")
                            )
                        ),
                        new XElement("Namespace", new XAttribute("Name", @"vi\.lib_PBR"),
                            XExt.CreateXElementWithNS("LoaderDefinition", "http://www.ni.com/LoaderDefinition.xsd",
                                new XElement("Type", "FolderLoaderDefinition"),
                                new XElement("Name", "vi.lib_PBR"),
                                new XElement("Location")
                            )
                        )
                );
            foreach (var file1 in EV3MBXMLPFiles)
            {
                xelem2.Add(
                    XExt.CreateXElementWithNS("Namespace", "http://www.ni.com/SourceModel.xsd",
                        new XAttribute("Name", (char.IsNumber(file1.FirstOrDefault()) ? @"\" : null) + Utils.EscapeVIX(file1)),
                        XExt.CreateXElementWithNS("ExternalFile", "http://www.ni.com/ExternalFile.xsd",
                            new XElement("RelativeStoragePath", file1),
                            new XElement("StoragePath")
                        )
                    ));
            }

            doc.Add(xelem2);
            return
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                doc.ToString();
        }
    }
}
