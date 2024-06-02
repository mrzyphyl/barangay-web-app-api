using Core.Service.Base.DataSets;

namespace Core.Service.Models
{
    public class User : BaseModel<UserState> {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string MaritalStatus { get; set; }
    }

    public enum UserState {
        Active,
        Inactive
    }
}
