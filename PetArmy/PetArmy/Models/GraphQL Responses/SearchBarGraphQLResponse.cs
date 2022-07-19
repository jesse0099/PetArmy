using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class SearchBarGraphQLResponse
    {

        public IEnumerable<Tag> tag { get; set; }
        public IEnumerable<Mascota_Tag> mascota_tag { get; set; }
        public List<Mascota> mascota { get; set; }

        public SearchBarGraphQLResponse()
        {
        }

        public SearchBarGraphQLResponse(IEnumerable<Tag> tag, IEnumerable<Mascota_Tag> mascota_tag, List<Mascota> mascota)
        {
            this.tag = tag;
            this.mascota_tag = mascota_tag;
            this.mascota = mascota;
        }
    }
}
