using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class MockString
    {

        public string ResponseString { get; set; }


        public MockString(string responseString)
        {
            ResponseString = responseString;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
