using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PetArmy.Models.GraphQL_Responses
{
    public class MascotaGraphQLResponse
    {
        //This might not work
        public MascotaGraphQLResponse(IEnumerable<Mascota> mascotas) => Mascotas = mascotas.ToList();
        public IReadOnlyList<Mascota> Mascotas { get; }

        public List<Mascota> mascota { get; set; }
        public Mascota mascota_by_pk { get; set; }
        public Mascota insert_mascota { get; set; }
        public Mascota delete_mascota { get; set; }


        public Mascota mascota_casa_cuna { get; set; }
        public Mascota mascota_casa_cuna_agregate { get; set; }
        public Mascota mascota_casa_cuna_by_pk { get; set; }
        public Mascota mascota_tag { get; set; }
        public Mascota mascota_tag_agreagate { get; set; }
        public Mascota mascota_tag_pk { get; set; }


        public MascotaGraphQLResponse()
        {
        }

        public MascotaGraphQLResponse(List<Mascota> mascota,
                                        Mascota mascota_agregate,
                                        Mascota mascota_by_pk,
                                        Mascota mascota_casa_cuna,
                                        Mascota mascota_casa_cuna_agregate,
                                        Mascota mascota_casa_cuna_by_pk,
                                        Mascota mascota_tag,
                                        Mascota mascota_tag_agreagate,
                                        Mascota mascota_tag_pk)
        {
            this.mascota = mascota;
            //this.mascota_agregate = mascota_agregate;
            this.mascota_by_pk = mascota_by_pk;
            this.mascota_casa_cuna = mascota_casa_cuna;
            this.mascota_casa_cuna_agregate = mascota_casa_cuna_agregate;
            this.mascota_casa_cuna_by_pk = mascota_casa_cuna_by_pk;
            this.mascota_tag = mascota_tag;
            this.mascota_tag_agreagate = mascota_tag_agreagate;
            this.mascota_tag_pk = mascota_tag_pk;
        }
    }


}
