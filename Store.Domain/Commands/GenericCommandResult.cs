using Store.Domain.Commands.Interfaces;

namespace Store.Domain.Commands
{
    // retorno dos commands para a tela
    public class GenericCommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public GenericCommandResult(bool success, string mesasge, object data)
        {
            Success = success;
            Message = mesasge;
            Data = data;
        }
    }
    
}
