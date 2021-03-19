namespace BYEsoDomainModelKernel.Models
{
  public class State : BaseAuditEntity
  {
  public virtual int Id { get; set; }
  public virtual string StateCode { get; set; }
  public virtual string StateName { get; set; }
  public virtual Country Country { get; set; }
  public State()
  {
  }
  public State(string stateCode, string stateName, Country country)
  {
    StateCode = stateCode;
    StateName = stateName;
    Country = country;
  }
  public override bool Equals(object obj)
  {
    var ot = obj as State;
    if (ot == null)
    {
      return false;
    }
    return ot.Id == Id;
  }

  public override int GetHashCode()
  {
    return Id.GetHashCode();
  }
  }
}