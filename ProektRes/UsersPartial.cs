using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotov_ProektRes
{
    public partial class users
    {
        public bool isBoy { get => gender == 1; }
        public bool isGirl { get => gender == 2; }
        public bool isAlien { get => gender == 3; }
    }
}
