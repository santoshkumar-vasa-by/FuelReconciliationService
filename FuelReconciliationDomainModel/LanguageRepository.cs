using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using NHibernate;

namespace BYEsoDomainModelKernel.Models.Language
{
  public class LanguageRepository : ILanguageRepository
  {
    private readonly ISession _Session;

    public LanguageRepository(IUnitOfWork uow)
    {
      _Session = uow.GetSession();
    }

    public async Task Add(Language agg)
    {
      await _Session.SaveAsync(agg);
    }

    public async Task<Language> GetByID(int key)
    {
      return await _Session.LoadAsync<Language>(key);
    }
  }
}
