namespace Web.API.Core.Responses;

public class StatusResponse<TStatusEnum>(TStatusEnum status) where TStatusEnum : Enum
{
	public TStatusEnum Status { get; set; } = status;
}