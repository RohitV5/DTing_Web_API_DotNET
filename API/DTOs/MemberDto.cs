

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PhotoUrl { get; set; } 
        //In case automapper doesnt find any matchinf property it will be set to null
        //For such properties we need to write logic in creatMap in autoMapper profile.

        public int Age { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }


        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<PhotoDto> Photos { get; set; }



    }
}