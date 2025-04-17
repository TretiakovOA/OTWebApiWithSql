namespace OTWebApiWithSql.Models;

public class CreditApplication
{
    public int Id { get; set; }
    public string? LoanPurpose { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal ClientIncome { get; set; }
    public int CreditProductId { get; set; }
}
