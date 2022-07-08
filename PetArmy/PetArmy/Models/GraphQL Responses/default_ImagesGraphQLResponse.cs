using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class default_ImagesGraphQLResponse
    {
        public Default_Images default_Images { get; set; }

        public default_ImagesGraphQLResponse(Default_Images image)
        {
            this.default_Images = image;
        }
    }
}
