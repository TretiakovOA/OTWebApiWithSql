﻿namespace OTWebApiWithSql.Models;

public class Client
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public int Age { get; set; }
    public string? Workplace { get; set; }
    public string? Phone { get; set; }
}