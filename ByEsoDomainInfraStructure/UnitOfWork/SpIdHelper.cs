using NHibernate;

namespace ByEsoDomainInfraStructure.UnitOfWork
{
  public class SpIdHelper
  {
    public static void RegisterUserInSpIdTable(ITransaction trans, ISession session, ISessionData sessionData)
    {
      var command = session.Connection.CreateCommand();

      trans.Enlist(command);

      command.CommandText = GetRadSysSpIdSQL(sessionData);
      command.ExecuteNonQuery();
    }

    private static string GetRadSysSpIdSQL(ISessionData sessionData)
    {
      return @"DELETE FROM rad_sys_spid where spid = @@spid

              INSERT INTO rad_sys_spid
              (
                spid,
                current_hier_id,
                current_hier_level,
                current_user_id,
                min_bu_date,
                max_bu_date,
                current_language_id,
                current_client_id,
                template_client_id,
                disable_trigger
              )

              SELECT
                  @@spid,
	                NULL,
	                2," +
             sessionData.UserId + @",
	                NULL,
	                NULL," +
             sessionData.DefaultLanguageId + "," +
             sessionData.ClientId + @",
	                NULL,
	                NULL";
    }
  }
}
