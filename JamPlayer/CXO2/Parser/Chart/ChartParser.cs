////////////////////////////////////////
// Chart Parser                       //
// Version              : 2.0         //
// Author               : CXO2        //
//////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Text;
using CXO2;
using CXO2.Data;
using CXO2.Parser;
using CXO2.IO;

namespace CXO2.Parser
{
    // ~ Chart Parser
    public static class ChartParser
    {
        public static bool parse(ref Chart chart, string path, Chart.Modes mode, string alternatepath = "")
        {
            StreamEngine buffer = new StreamEngine(path);
            byte[] data = buffer.getAllBuffer();
            buffer.Free();

            byte[] alternatedata = null;
            if (alternatepath != "")
            {
                buffer = new StreamEngine(alternatepath);
                alternatedata = buffer.getAllBuffer();
                buffer.Free();
            }

            return parse(ref chart, data, mode, alternatedata, path);
        }

        public static bool parse(ref Chart chart, byte[] data, Chart.Modes mode, byte[] alternatedata = null, string path = "")
        {
            try
            {
                OJN h = new OJN();

                if (OJNParser.isOJN(data, ref h))
                {
                    h.file = path;
                    h.ojm = h.file.Replace(".ojn", ".ojm");

                    if (alternatedata != null)
                        OJMParser.parse(ref chart, alternatedata);
                    else
                        OJMParser.parse(ref chart, h.ojm);

                    OJNParser.parse(ref chart, ref h, data, mode);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool parseHeader(ref Chart chart, string path)
        {
            StreamEngine buffer = new StreamEngine(path);
            byte[] data = buffer.getAllBuffer();
            buffer.Free();

            return parseHeader(ref chart, data);
        }

        public static bool parseHeader(ref Chart chart, byte[] data)
        {
            OJN h = new OJN();

            if (OJNParser.isOJN(data, ref h))
                return OJNParser.parseHeaderToChart(ref chart, h);

            return false;
        }
    }
}
