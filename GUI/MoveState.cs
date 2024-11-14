using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksConsole.GUI
{
    internal enum MoveState
    {
        Success,
        Invalid_Input,
        Invalid_Position,
        New,
        Changed_Piece,
        Win,
    }
}

