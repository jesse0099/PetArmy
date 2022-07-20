using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public  class Imagen_MascotaGraphQLResponse
    {
        public List<Imagen_Mascota> imagen_mascota { get; set; }
        public Imagen_Mascota insert_imagen_mascota { get; set; }
    }
}
