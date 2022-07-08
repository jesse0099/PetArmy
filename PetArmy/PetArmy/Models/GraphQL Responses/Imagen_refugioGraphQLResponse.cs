using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class Imagen_refugioGraphQLResponse
    {
        public List<Imagen_refugio> imagen_refugio { get; set; }
        public Imagen_refugio insert_imagen_refugio { get; set; }

        public Imagen_refugioGraphQLResponse()
        {
        }
        public Imagen_refugioGraphQLResponse(List<Imagen_refugio> imagen_refugio, Imagen_refugio insert_imagen_refugio)
        {
            this.imagen_refugio = imagen_refugio;
            this.insert_imagen_refugio = insert_imagen_refugio;
        }
    }
}
