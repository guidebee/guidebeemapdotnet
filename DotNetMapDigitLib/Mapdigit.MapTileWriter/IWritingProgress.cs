﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mapdigit.MapTileWriter
{
    public interface IWritingProgressListener
    {
        void Progress(long index, int x, int y, int z, bool failed);

        void FinishWriting();
    }
}
