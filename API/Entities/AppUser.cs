using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>  
    {
        //Entity framework is convention based and it will make Id as primary key in table

        //Identity will take care of these 
        public int UserId { get; set; }

        // public string UserName { get; set; }

        // public byte[] PasswordHash { get; set; }

        // public byte[] PasswordSalt { get; set; }


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

        
        public List<UserLike> LikedByUsers { get; set; }
        public List<UserLike> LikedUsers { get; set; }

        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }


        // List of roles for user
        public ICollection<AppUserRole> UserRoles { get; set; }

        


        //Automapper is smart to find Age from properties then it will look for \Get+property name()\ and map the value
        //Using this logic we can create mapping logic for fields which do not match

        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }

        //removing and moving it to automapper in createMap other place because projections are still fetching all the values instead of DTO type






    }
}