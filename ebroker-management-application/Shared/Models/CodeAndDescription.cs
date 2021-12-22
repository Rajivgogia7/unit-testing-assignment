namespace EBroker.Management.Application.Shared.Models
{
    public class CodeAndDescription<TCode, TDesc>
    {
        public CodeAndDescription(TCode code, TDesc desc)
        {
            Code = code;
            Description = desc;
        }
        public TCode Code { get; set; }
        public TDesc Description { get; set; }
    }
}
