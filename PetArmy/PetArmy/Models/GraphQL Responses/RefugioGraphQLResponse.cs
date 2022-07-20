using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class RefugioGraphQLResponse
    {

        public List<Refugio> refugio { get; set; }
        public Refugio refugio_by_pk { get; set; }
        public Refugio insert_refugio { get; set; }
        public Refugio update_refugio_by_pk { get; set; }

        public RefugioGraphQLResponse()
        {

        }

        public RefugioGraphQLResponse(List<Refugio> refugio, Refugio refugio_by_pk, Refugio insert_refugio, Refugio update_refugio_by_pk)
        {
            this.refugio = refugio;
            this.refugio_by_pk = refugio_by_pk;
            this.insert_refugio = insert_refugio;
            this.update_refugio_by_pk = update_refugio_by_pk;
        }
    }
}
