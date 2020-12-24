using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tidy.Tests.Mocks
{
    public class MockParameter
    {
        public void Mark()
        {
            IsMarked = true;
        }
        public bool IsMarked { get; private set; }
    }
}
