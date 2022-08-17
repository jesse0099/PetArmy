using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public static class Commons
    {

        public static byte[] StreamToByteArray(Stream input)
        {
            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Stream GetImageSourceStream(ImageSource imgSource)
        {
            if (imgSource is StreamImageSource)
            {
                try
                {
                    StreamImageSource strImgSource = (StreamImageSource)imgSource;
                    System.Threading.CancellationToken cToken = System.Threading.CancellationToken.None;
                    Task<Stream> returned = strImgSource.Stream(cToken);
                    return returned.Result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string GetPetsByShelter = @"query get_pets_by_shelter($adminId: String!) {
          pets_by_shelter: mascota(where: {refugio: {administrador: {_eq: $adminId}} }) {
            id_mascota
            nombre
            especie
            raza
            peso
            descripcion
            discapacidad
            alergias
            castrado
            vacunado
            enfermedad
            edad
            estado
            refugio{
              id_refugio
            }
            mascota_tags {
              tag {
                nombre_tag
              }
            }
            imagenes_mascota {
              imagen: image
            }
          }
        }";

        public static string UpdatePetByPk = @"mutation UpdatePetByPK(   $alergias: Boolean!, $castrado: Boolean!, $discapacidad: Boolean!,
											                             $enfermedad: Boolean!, $vacunado: Boolean!, $estado: Boolean!,
											                             $descripcion: String!, $edad: numeric!, $id_refugio: Int!, 
										                                 $nombre: String!, $especie: String!, $raza: String!, $peso: numeric!,
											                             $id_mascota: Int!) {
                                                                            update_mascota_by_pk(_set: { alergias: $alergias, castrado: $castrado, discapacidad: $discapacidad,
    											                                                         enfermedad: $enfermedad, vacunado: $vacunado,  estado: $estado, 
    													                                                 descripcion: $descripcion edad: $edad, id_refugio: $id_refugio, 
     													                                                 nombre: $nombre, especie: $especie, raza: $raza, peso: $peso}, 
                                                                                                  pk_columns: {id_mascota: $id_mascota}) {
                                                                                                    id_mascota
                                                                                                  }
                                                                        }";
    }
}
