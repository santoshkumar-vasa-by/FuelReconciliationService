using System.Threading.Tasks;

namespace BYEsoDomainModelKernel.Models.Language
{
  public interface ILanguageRepository
  {
    Task Add(Language agg);

    Task<Language> GetByID(int key);
  }
}
