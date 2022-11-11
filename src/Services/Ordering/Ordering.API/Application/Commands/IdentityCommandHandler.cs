namespace Ordering.API.Application.Commands;

public class IdentityCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
    where T : IRequest<R>
{
    readonly IMediator _mediator;
    readonly IRequestManager _requestManager;
    readonly ILogger<IdentityCommandHandler<T, R>> _logger;

    public IdentityCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentityCommandHandler<T, R>> logger)
    {
        _mediator = mediator;
        _requestManager = requestManager;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected virtual R CreateResultForDuplicateRequest()
    {
        return default(R);
    }

    public async Task<R> Handle(IdentifiedCommand<T, R> messsage, CancellationToken cancellationToken)
    {
        var alreadyExists = await _requestManager.ExistAsync(messsage.Id);
        if (alreadyExists)
        {
            return CreateResultForDuplicateRequest();
        }
        else
        {
            await _requestManager.CreateRequestForCommandAsync<T>(messsage.Id);
            try
            {
                var command = messsage.Command;
                var commandName = command.GetGenericTypeName();
                var idProperty = string.Empty;
                var commandId = string.Empty;

                switch (command)
                {
                    case CancelOrderCommand cancelOrderCommand:
                        idProperty = nameof(cancelOrderCommand.OrderNumber);
                        commandId = $"{cancelOrderCommand.OrderNumber}";
                        break;

                    case ShipOrderCommand shipOrderCommand:
                        idProperty = nameof(shipOrderCommand.OrderNumber);
                        commandId = $"{shipOrderCommand.OrderNumber}";
                        break;

                    default:
                        idProperty = "Id?";
                        commandId = "n/a";
                        break;
                }

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    commandName,
                    idProperty,
                    commandId,
                    command);

                var result = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation(
                    "----- Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    result,
                    commandName,
                    idProperty,
                    commandId,
                    command);

                return result;
            }
            catch
            {
                return default(R);
            }
        }
    }
}