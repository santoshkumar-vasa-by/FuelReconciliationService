
namespace BYEsoDomainModelKernel.Models.Language
{
  public static class LanguageFactory
  {
    public static Language CreateLanguage(int internalID, string name)//, IsoLanguage isolanguage)
    {
      return new Language
      {
        ID = internalID,
        Name = name//,
        //IsoLanguage = isolanguage
      };
    }
  }
}
