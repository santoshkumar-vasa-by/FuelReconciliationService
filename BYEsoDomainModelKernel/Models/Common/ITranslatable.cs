using System.Collections.Generic;

namespace BYEsoDomainModelKernel.Models.Common
{
  public interface ITranslatable<out T> where T : ITranslation
  {
    IEnumerable<T> Translations { get; }
    string Name { get; }
    string GetUniqueKey(int languageID);
  }
}
