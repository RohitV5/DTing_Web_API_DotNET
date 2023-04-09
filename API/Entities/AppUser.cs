namespace API.Entities
{
    public class AppUser
    {
        //Entity framework is convention based and it will make Id as primary key in table
        public int Id {get;set;}

        public string UserName { get; set; }

    }
}