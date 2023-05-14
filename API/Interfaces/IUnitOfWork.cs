namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}
        IMessageRepository MessageRepository {get;}
        ILikesRepository LikesRepository {get;}
        Task<bool> Complete();  //if complete is not true then rollback
        bool HasChanges();  //will track current change

        
    }
}