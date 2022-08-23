using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public static class Commons
    {
        public const string DefaultPhoneNumber = "+506 0000 0000";
        public const string FirebaseProjectId = "538291567160-era9d1f0o9qutsssrf6fcqmi45n9abfe.apps.googleusercontent.com";
        public const string AdminCreationRequestFunction = "adminCreationRequest";
        public const string AdminCreationApprovalFunction = "adminCreationApproval";
        public const string AdminCreationRejectionFunction = "adminCreationRejection";
        public const string AdminCreationRequestsFunction = "adminCreationRequests";
        public const string AdminAccessStateUpdateFunction = "adminAccessStateUpdate";
        public const string AdminApprovalEmailExceptionMessage = "This email is already in use by another account";
        public const string AdminApprovalTreatedExceptionMessage = "Already Treated Account";
        public const string AdminRequestTreatedState = "Treated";
        public const string AdminRequestRejectedState = "Rejected";
        public const string AdminRequestPendingState = "Pending";
        public const string AdminRequestProcessingState = "Processing";
        public const string GetNearPetsByTagsQuery = @"query near_pets_by_tags($distance: Float!, $from: geography!, $tags: [String]) {
        near_pets_by_tags: mascota(where: {_and: [{mascota_tags: {tag: {nombre_tag: {_in: $tags}}}}, 
                                                  {refugio: {ubicaciones_refugios: {g_location: {_st_d_within: {distance: $distance, from: $from}}}}}, 
                                                  {estado: {_eq: true}},
		                                          {id_mascota: {_in: [6, 4, 1, 2, 3]}}
                                                  ]}) {
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
                                            refugio {
                                              id_refugio
                                              ubicacion: ubicaciones_refugios{	
                                                lalitud: latitud
                                                longitud
                                              }
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
        public const string RequestPetAdoption = @"mutation RequestPetAdoption($uid: String!, $mascota: Int!, $refugio: Int!) {
          insert_solicitudes_adopcion(objects: {id_mascota: $mascota, id_refugio: $refugio, fecha_solicitud: now, adoptante: $uid, aprobacion: false}) {
            returning {
              id_mascota
              refugio {
                id_refugio
              }
            }
          }
        }";

        const Int16 minimum_password_length = 6;
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        const string phoneRegex = @"\+\d{3} \d{4} \d{4}";

        public static bool IsValidEmail(string email)
        {
            return (Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
        }

        public static bool IsValidPassword(string password)
        {
            return password.Length >= minimum_password_length;
        }

        public static bool IsValidPhone(string phone)
        {
            if (phone == null || phone == string.Empty || phone == DefaultPhoneNumber)
                return false;
            return Regex.IsMatch(phone, phoneRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        public static string UpdateCampCastraMutation = @"mutation update_an_article($nombre_camp: String!, $descripcion: String!, $direccion: String!, $tel_contacto: String!, $id_campana: Int!) {
          update_camp_castracion_by_pk (
            pk_columns: {id_campana: $id_campana}, 
            _set: { nombre_camp: $nombre_camp
    				        descripcion: $descripcion
    				        direccion: $direccion
    				        tel_contacto: $tel_contacto}
          ) {
            id_campana
          }
        }";

        public static string DeleteCampCastraMutation = @"mutation delete_an_article($id_campana: Int!) {
            delete_camp_castracion_by_pk(
              id_campana: $id_campana
              ){
                nombre_camp
               }
             }";
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
              id_imagen
              id_mascota
            }
          }
        }";

        public static string UpdatePetByPk = @"mutation UpdatePetByPK($alergias: Boolean!, $castrado: Boolean!, $discapacidad: Boolean!, $enfermedad: Boolean!, $vacunado: Boolean!, $estado: Boolean!, $descripcion: String!, $edad: numeric!, $id_refugio: Int!, $nombre: String!, $especie: String!, $raza: String!, $peso: numeric!, $id_mascota: Int!, $addedImages: [imagen_mascota_insert_input!]!, $updatedImages: [imagen_mascota_updates!]!, $deletedImages: [Int!]!) {
                              delete_imagen_mascota(where: {_and: [{id_mascota: {_eq: $id_mascota}}, {id_imagen: {_in: $deletedImages}}]}) {
                                deleted_images: affected_rows
                              }
                              update_imagen_mascota_many(updates: $updatedImages) {
                                affected_rows
                              }
                              insert_imagen_mascota(objects: $addedImages) {
                                added_images: affected_rows
                              }
                              update_mascota_by_pk(_set: {alergias: $alergias, castrado: $castrado, discapacidad: $discapacidad, enfermedad: $enfermedad, vacunado: $vacunado, estado: $estado, descripcion: $descripcion, edad: $edad, id_refugio: $id_refugio, nombre: $nombre, especie: $especie, raza: $raza, peso: $peso}, pk_columns: {id_mascota: $id_mascota}) {
                                id_mascota
                              }
                            }";

        public static string DeletePetByPlk = @"mutation DeletePetById($mascotaId: Int!, $refugioId: Int!) {
          delete_imagen_mascota(where: {mascotum: {id_mascota: {_eq: $mascotaId}}}) {
            affected_rows
          }
          delete_solicitudes_adopcion(where: {_and: [{id_mascota: {_eq: $mascotaId}}, {id_refugio: {_eq: $refugioId}}]}) {
            affected_rows
          }
          delete_registro_adopcion(where: {_and: [{id_mascota: {_eq: $mascotaId}}, {id_refugio: {_eq: $refugioId}}]}) {
            affected_rows
          }
          delete_mascota_casa_cuna(where: {_and: [{id_mascota: {_eq: $mascotaId}}, {id_refugio: {_eq: $refugioId}}]}) {
            affected_rows
          }
          delete_mascota_tag(where: {_and: [{id_mascota: {_eq: $mascotaId}}]}) {
            affected_rows
          }
          delete_mascota(where: {id_mascota: {_eq: $mascotaId}}) {
            returning {
              castrado
              descripcion
              discapacidad
              enfermedad
              especie
              estado
              id_mascota
              nombre
              peso
              raza
              vacunado
              id_refugio
            }
          }
        }";

        public static string InsertPet = @"mutation InsertNewPet($alergias: Boolean!, $castrado: Boolean!, $discapacidad: Boolean!, $enfermedad: Boolean!, $vacunado: Boolean!, $estado: Boolean!, $descripcion: String!,$id_refugio: Int!, $nombre: String!, $especie: String!, $raza: String!, $peso: numeric!) {
          insert_mascota(objects: {alergias: $alergias, castrado: $castrado, descripcion: $descripcion, discapacidad: $discapacidad, enfermedad: $enfermedad, especie: $especie, estado: $estado, nombre: $nombre, peso: $peso, raza: $raza, vacunado: $vacunado, id_refugio: $id_refugio}) {
            returning {
              id_mascota
            }
          }
        }";
    }
}
