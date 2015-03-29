using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash
{
    public interface ICacher
    {
        void Save(Key key, object value);
        bool TryRetrieve(Key key, out object value);
    }
}
