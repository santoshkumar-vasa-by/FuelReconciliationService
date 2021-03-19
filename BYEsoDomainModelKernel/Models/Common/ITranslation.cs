namespace BYEsoDomainModelKernel.Models.Common
{
  public interface ITranslation
  {
    int LanguageId { get; }
    string Name { get; }
    string GetUniqueKey();
  }
}
