using System.Collections.Generic;

namespace PetArmy.Models.GraphQL_Responses
{
    public class FeedGrapQLResponse
    {
        public IEnumerable<Mascota> near_pets_by_tags { get; set; }

        public FeedGrapQLResponse(IEnumerable<Mascota> near_pets_by_tags)
        {
            this.near_pets_by_tags = near_pets_by_tags;
        }
        public FeedGrapQLResponse()
        {

        }
    }
}

