using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2.Charting.O2Jam
{
    public interface IChartSample
    {
        short Id       { get; set; }
        string Name    { get; set; }
        byte[] Payload { get; set; }
    }
}
