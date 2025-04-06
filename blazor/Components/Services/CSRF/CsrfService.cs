
namespace blazor.services;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireCsrfTokenAttribute : Attribute { }

public class CsrfContext
{
  public string HeaderToken {get;set;}="";
}
