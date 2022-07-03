using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace PetArmy.Models
{
    public class MockGraphQLResponse
    {

        public List<MockTable> mockTable { get; set; }
        public MockTable mockTable_by_pk { get; set; }
        public MockTable insert_mockTable { get; set; }

        public MockGraphQLResponse() { }

        public MockGraphQLResponse(List<MockTable> mockTable)
        {
            this.mockTable = mockTable;
        }

        public MockGraphQLResponse(MockTable mockTable)
        {
            this.mockTable_by_pk = mockTable;
            this.insert_mockTable = mockTable;
        }

    }
}
