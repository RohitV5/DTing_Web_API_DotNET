using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        //Entity framework is convention based and it will make Id as primary key in table
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }


        public DateOnly DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }


        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<Photo> Photos { get; set; } = new();  //shorthand for new List<Photo>()


        //Automapper is smart to find Age from properties then it will look for \Get+property name()\ and map the value
        //Using this logic we can create mapping logic for fields which do not match
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }






    }
}