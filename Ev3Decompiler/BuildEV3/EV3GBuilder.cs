using EV3ModelLib;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV3DecompilerLib.BuildEV3
{
    public class EV3GBuilder
    {
        public void BuildEV3(Dictionary<string, Node> blocks, string description, Stream outStream)
        {
            BuildEV3(blocks, null, description, outStream);
        }

        public void BuildEV3(Dictionary<string, byte[]> blocksdata, string description, Stream outStream)
        {
            BuildEV3(null, blocksdata, description, outStream);
        }

        public void BuildEV3(Dictionary<string, Node> blocks, Dictionary<string, byte[]> blockdata, string description, Stream outStream)
        {
            var zipblocks = new List<IEV3GZipBlock>()
            {
                    new CreateProjectTitle(),
                    new CreateProjectDescription() { ProjectDescription = description }
            };

            var proj = new CreateProject();
            zipblocks.Add(proj);

            //TODO: escape for filename
            //-- generate XML and add (rbf restore)
            if (blocks != null)
            foreach (var prgnode in blocks)
            {
                var prog = new CreateProgramFromNode(prgnode.Value, prgnode.Key);
                zipblocks.Add(prog);
                proj.EV3PFiles.Add(prog.Name);
            }

            //-- add the already existing xml (ev3 extract)
            if (blockdata != null)
                foreach (var prgnode in blockdata)
                {
                    switch (Path.GetExtension(prgnode.Key))
                    {
                        case ".ev3p":
                            var prog = new CreateProgramFromXml(prgnode.Value, prgnode.Key);
                            zipblocks.Add(prog);
                            proj.EV3PFiles.Add(prog.Name);
                            break;
                        case ".mbxml":
                            var mbxml = new CreateMyBlockXmlFromXml(prgnode.Value, prgnode.Key);
                            zipblocks.Add(mbxml);
                            proj.EV3MBXMLPFiles.Add(mbxml.Name);
                            break;

                    }
                }

            using (var zipStream = new ZipOutputStream(outStream))
            {
                zipStream.SetLevel(9);
                foreach (var zipblock in zipblocks)
                {
                    var str = zipblock.GetContent();

                    using (var ms1 = new MemoryStream())
                    {
                        using (var sw = new StreamWriter(ms1, new UTF8Encoding(true), 4096, true))
                            sw.Write(str);
                    ZipEntry ze = new ZipEntry(zipblock.Name);
                        ze.Size = ms1.Length;

                    zipStream.PutNextEntry(ze);

                        ms1.Position = 0;
                        ms1.CopyTo(zipStream);

                    zipStream.CloseEntry();
                }

                }
                zipStream.IsStreamOwner = false;
                zipStream.Finish();
            }
        }
    }
}
