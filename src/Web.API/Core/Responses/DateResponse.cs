namespace Web.API.Core.Responses;

public class DateResponse(DateTime date)
{
	public DateTime Date { get; set; } = date;
}