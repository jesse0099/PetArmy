using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace PetArmy.Models
{
    public class MockGraphQLResponse
    {

        public MockString Data { get; }

        public MockGraphQLResponse(MockString data) =>  Data = data;

    }
}
