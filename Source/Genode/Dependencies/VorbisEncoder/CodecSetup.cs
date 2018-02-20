﻿using System.Collections.Generic;
using VorbisEncoder.Setup;

namespace VorbisEncoder
{
    public class CodecSetup
    {
        public CodecSetup(EncodeSetup encodeSetup)
        {
            EncodeSetup = encodeSetup;
        }

        public EncodeSetup EncodeSetup { get; }

        public int[] BlockSizes { get; } = new int[2];

        public CodeBook[] FullBooks { get; set; }
        public IList<IStaticCodeBook> BookParams { get; } = new List<IStaticCodeBook>();
        public IList<Mode> ModeParams { get; } = new List<Mode>();
        public IList<Mapping> MapParams { get; } = new List<Mapping>();
        public IList<Floor> FloorParams { get; } = new List<Floor>();
        public IList<Residue> ResidueParams { get; } = new List<Residue>();
        public IList<PsyInfo> PsyParams { get; } = new List<PsyInfo>();
        public PsyGlobal PsyGlobalParam { get; set; }
    }
}