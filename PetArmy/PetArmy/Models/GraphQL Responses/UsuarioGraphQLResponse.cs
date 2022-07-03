using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class UsuarioGraphQLResponse
    {
        public List<Usuario> usuario { get; set; }
        public Usuario usuario_by_pk { get; set; }
        public Usuario insert_usuario { get; set; }

    }
}
