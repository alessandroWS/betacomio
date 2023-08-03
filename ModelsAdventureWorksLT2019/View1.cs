using System;
using System.Collections.Generic;

namespace betacomio.ModelsAdventureWorksLT2019;

public partial class View1
{
    public int SalesOrderId { get; set; }

    public byte RevisionNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public byte Status { get; set; }

    public bool OnlineOrderFlag { get; set; }

    public string SalesOrderNumber { get; set; } = null!;

    public string? PurchaseOrderNumber { get; set; }

    public string? AccountNumber { get; set; }

    public int CustomerId { get; set; }

    public int? ShipToAddressId { get; set; }

    public int? BillToAddressId { get; set; }

    public string ShipMethod { get; set; } = null!;

    public string? CreditCardApprovalCode { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmt { get; set; }

    public decimal Freight { get; set; }

    public decimal TotalDue { get; set; }

    public string? Comment { get; set; }

    public Guid Rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }

    public int SalesOrderDetailId { get; set; }

    public short OrderQty { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal UnitPriceDiscount { get; set; }

    public decimal LineTotal { get; set; }

    public Guid Expr1 { get; set; }

    public DateTime Expr2 { get; set; }

    public bool Expr3 { get; set; }

    public string? Expr4 { get; set; }

    public string Expr5 { get; set; } = null!;

    public string? Expr6 { get; set; }

    public string Expr7 { get; set; } = null!;

    public string? Expr8 { get; set; }

    public string? Expr9 { get; set; }

    public string? Expr10 { get; set; }

    public string? Expr11 { get; set; }

    public string? Expr12 { get; set; }

    public string Expr13 { get; set; } = null!;

    public string Expr14 { get; set; } = null!;

    public Guid Expr15 { get; set; }

    public DateTime Expr16 { get; set; }
}
