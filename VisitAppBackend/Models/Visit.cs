using System;
using System.Runtime.Serialization;

namespace VisitAppBackend.Models
{
    [DataContract]
    public class Visit
    {
        public Visit()
        {
        }

        [DataMember(Name = "nome_place")]
        public String NomePlace
        {
            get;
            set;
        }

        [DataMember(Name = "endereco_place")]
        public String EnderecoPlace
        {
            get;
            set;
        }

        [DataMember(Name = "id_place")]
        public String PlaceId
        {
            get;
            set;
        }

        [DataMember(Name = "latitude")]
        public double Latitude
        {
            get;
            set;
        }

        [DataMember(Name = "longitude")]
        public double Longitude
        {
            get;
            set;
        }

        [DataMember(Name = "data_visita")]
        public String DataVisita
        {
            get;
            set;
        }

        [DataMember(Name = "hora_inicio")]
        public int HoraInicioVisita
        {
            get;
            set;
        }

        [DataMember(Name = "minuto_inicio")]
        public int MinutoInicioVisita
        {
            get;
            set;
        }

        [DataMember(Name = "hora_fim")]
        public int HoraFimVisita
        {
            get;
            set;
        }

        [DataMember(Name = "minuto_fim")]
        public int MinutoFimVisita
        {
            get;
            set;
        }

        [DataMember(Name = "acompanhar_amigos")]
        public Boolean AcompanharAmigos
        {
            get;
            set;
        }

        [DataMember(Name = "id_facebook")]
        public String IdFacebook
        {
            get;
            set;
        }


        [DataMember(Name = "objectId")]
        public String ObjectId
        {
            get;
            set;
        }


        [DataMember(Name = "updatedAt")]
        public String UpdatedAt
        {
            get;
            set;
        }

        [DataMember(Name = "createdAt")]
        public String CreatedAt
        {
            get;
            set;
        }
    }
}