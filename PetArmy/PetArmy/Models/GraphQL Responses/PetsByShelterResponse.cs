using System.Collections.Generic;

namespace PetArmy.Models.GraphQL_Responses
{
    public class PetsByShelterResponse
    {
        public IEnumerable<Mascota> pets_by_shelter { get; set; }

        public PetsByShelterResponse(IEnumerable<Mascota> pets_by_shelter)
        {
            this.pets_by_shelter = pets_by_shelter;
        }
        public PetsByShelterResponse()
        {

        }
    }
}
