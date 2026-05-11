using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public interface IComportementIA
    {
        Action CalculerCoup(Partie partie);
    }
}
