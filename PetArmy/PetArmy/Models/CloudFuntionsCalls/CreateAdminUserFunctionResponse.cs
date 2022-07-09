namespace PetArmy.Models.CloudFuntionsCalls
{
    public class ApprovedBy
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class CreatedOn
    {
        public TimestampValue timestampValue { get; set; }
        public string valueType { get; set; }
    }

    public class Email
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class Fields
    {
        public FirstName firstName { get; set; }
        public LastName lastName { get; set; }
        public Password password { get; set; }
        public PhoneNumber phoneNumber { get; set; }
        public Role role { get; set; }
        public Email email { get; set; }
    }

    public class Enabled
    {
        public bool booleanValue { get; set; }
        public string valueType { get; set; }
    }

    public class AccessGrantedBy
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class FirstName
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class LastName
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class MapValue
    {
        public Fields fields { get; set; }
    }

    public class Password
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class PhoneNumber
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class Role
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class AdminCreationResponse
    {
        public ApprovedBy approvedBy { get; set; }
        public DocId docId{ get; set; }
        public Motive motive { get; set; }
        public UserEmail userEmail { get; set; }
        public CreatedOn createdOn { get; set; }
        public UserDetails userDetails { get; set; }
        public Status status { get; set; }
        public Enabled enabled { get; set; }
        public AccessGrantedBy accessGrantedBy { get; set; }
    }

    public class DocId
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class Motive
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class Status
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }

    public class TimestampValue
    {
        public string seconds { get; set; }
        public int nanos { get; set; }
    }

    public class UserDetails
    {
        public MapValue mapValue { get; set; }
        public string valueType { get; set; }
    }

    public class UserEmail
    {
        public string stringValue { get; set; }
        public string valueType { get; set; }
    }
}
