using System;
using System.Reflection;

namespace ByEsoDomainInfraStructure.UnitOfWork
{
  public class UnitOfWorkFactory
  {
    private static IUnitOfWork _CurrentUnitOfWork { get; set; }

    public static IUnitOfWork Current
    {
      get
      {
        var unitOfWork = _CurrentUnitOfWork;
        if (unitOfWork == null)
        {
          throw new InvalidOperationException("You are not in a unit of work");
        }
        return unitOfWork;
      }
    }

    public static bool HasCurrentUnitOfWork
    {
      get
      {
        var unitOfWork = _CurrentUnitOfWork;
        return unitOfWork != null;
      }
    }

    private static object mutex = new object();
    //public static IUnitOfWork Start(ISessionData sessionData, DiagnosticExceptionType requestedExceptionType =
    //  DiagnosticExceptionType.NoException)
    //{
    //  lock (mutex)
    //  {
    //    if (_CurrentUnitOfWork != null)
    //      throw new InvalidOperationException("You cannot start more than one unit of work at the same time.");

    //    var unitOfWorkStartExceptionRequested =
    //      requestedExceptionType.Equals(DiagnosticExceptionType.ExceptionFromUnitOfWorkStart);

    //    if (unitOfWorkStartExceptionRequested)
    //    {
    //      //throw new EsoGenericException("Exception thrown from Unit of Work start.");
    //    }

    //    var unitOfWorkUnauthorizedAccessExceptionRequested = requestedExceptionType.Equals(
    //      DiagnosticExceptionType.UnauthorizedAccessExceptionFromUnitOfWorkStart);

    //    if (unitOfWorkUnauthorizedAccessExceptionRequested)
    //    {
    //      throw new UnauthorizedAccessException("UnauthorizedAccessException thrown from UnitOfWork Start.");
    //    }

    //    var unitOfWork = InstantiateUnitOfWork(sessionData);

    //    _CurrentUnitOfWork = unitOfWork;
    //    return unitOfWork;
    //  }
    //}
    private static IUnitOfWork InstantiateUnitOfWork(ISessionData sessionData)
    {
      lock (mutex)
      {
        Type uowType = typeof(UnitOfWork);
        const BindingFlags protectedFlag = BindingFlags.Instance | BindingFlags.NonPublic;
        var parameterTypeArray = new Type[] { typeof(ISessionData) };
        ConstructorInfo cinfo = uowType.GetConstructor(protectedFlag, null, parameterTypeArray, null);

        return (IUnitOfWork)cinfo.Invoke(new object[] { sessionData });
      }
    }

    public static void Dispose()
    {
      if (HasCurrentUnitOfWork)
      {
        var uow = _CurrentUnitOfWork;
        _CurrentUnitOfWork = null;
        uow.Dispose();
      }
    }
  }
}
