using api.CQS;

namespace api.services;

public interface IRapportService :
ICommandHandler<CreateRapportCommand>
{

}