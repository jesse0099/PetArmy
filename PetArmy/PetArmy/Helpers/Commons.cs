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
                                                          near_pets_by_tags:mascota(where: {_and: [ {mascota_tags: {tag: {nombre_tag: {_in: $tags}}}}
    						                                                                        {refugio: {ubicaciones_refugios: {g_location: {_st_d_within: {distance: $distance, from: $from}}}}}
    										                                                        {estado: {_eq: true}}
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
                                                            edad
                                                            refugio {
                                                              id_refugio
                                                            }
                                                            mascota_tags {
                                                              tag {
                                                                nombre_tag
                                                              }
                                                            }
                                                            imagenes_mascota{
                                                              imagen:image
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
    }
}
