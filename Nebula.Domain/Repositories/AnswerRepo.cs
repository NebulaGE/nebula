using Nebula.Domain.Abstract;
using Nebula.Domain.Entities;

namespace Nebula.Domain.Repositories
{
    public class AnswerRepo : BaseRepository<Answer>
    {
        public AnswerRepo(IDbContextNebula t) : base(t)
        {
        } 
    }
}