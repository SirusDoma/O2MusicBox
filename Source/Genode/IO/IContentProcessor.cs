using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genode.IO
{
    public interface IContentProcessor<T>
    {
        T Parse(Stream stream);
    }
}
