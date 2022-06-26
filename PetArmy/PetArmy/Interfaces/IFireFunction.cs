using PetArmy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Interfaces
{
    public interface IFireFunction
    {
        void CallFunction(string function, Dictionary<string,object> data, Action<Object, string> _onCallComplete);
    }
}
