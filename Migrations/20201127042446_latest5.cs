using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ShawkanyDb.Migrations
{
    public partial class latest5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Company = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "Corporation",
                columns: table => new
                {
                    CorporationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Corporation = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporation", x => x.CorporationID);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Currency = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyID);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeRate",
                columns: table => new
                {
                    CurrencyExchangeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AfghaniRateToRupe = table.Column<double>(nullable: true),
                    AfhaniRateToDoller = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeRate", x => x.CurrencyExchangeID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "DealType",
                columns: table => new
                {
                    DealTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DealType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealType", x => x.DealTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    HireDate = table.Column<DateTime>(type: "date", nullable: true),
                    Image = table.Column<string>(maxLength: 200, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Salary = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "ExpenceType",
                columns: table => new
                {
                    ExpenceTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExpenceType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenceType", x => x.ExpenceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpoyeeID = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    RemindDate = table.Column<DateTime>(type: "date", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteID);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StockID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Stock = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockID);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: true),
                    CompanyID = table.Column<int>(nullable: true),
                    CountryID = table.Column<int>(nullable: true),
                    LatinName = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    OldPrice = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Item_Category",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_Company",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_Country",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loan",
                columns: table => new
                {
                    LoanID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrencyID = table.Column<int>(nullable: true),
                    CustomerID = table.Column<int>(nullable: true),
                    LoanDate = table.Column<DateTime>(type: "date", nullable: true),
                    Quantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.LoanID);
                    table.ForeignKey(
                        name: "FK_Loan_Currency",
                        column: x => x.CurrencyID,
                        principalTable: "Currency",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loan_Customer",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deal",
                columns: table => new
                {
                    DealID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CorporationID = table.Column<int>(nullable: true),
                    Credit = table.Column<double>(nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    CustomerID = table.Column<int>(nullable: true),
                    DealTypeID = table.Column<int>(nullable: true),
                    Debit = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deal", x => x.DealID);
                    table.ForeignKey(
                        name: "FK_Deal_Corporation",
                        column: x => x.CorporationID,
                        principalTable: "Corporation",
                        principalColumn: "CorporationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deal_Currency",
                        column: x => x.CurrencyID,
                        principalTable: "Currency",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deal_Customer",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deal_DealType",
                        column: x => x.DealTypeID,
                        principalTable: "DealType",
                        principalColumn: "DealTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalaryPayment",
                columns: table => new
                {
                    SalaryPaymentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeID = table.Column<int>(nullable: false),
                    PaidAmount = table.Column<int>(nullable: false),
                    PaidDate = table.Column<DateTime>(nullable: false),
                    Remain = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryPayment", x => x.SalaryPaymentID);
                    table.ForeignKey(
                        name: "FK_Salary_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expence",
                columns: table => new
                {
                    ExpenceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Detail = table.Column<string>(maxLength: 200, nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    ExpenceAmount = table.Column<double>(nullable: true),
                    ExpenceDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpenceTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expence", x => x.ExpenceID);
                    table.ForeignKey(
                        name: "FK_Expence_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expence_ExpenceType",
                        column: x => x.ExpenceTypeID,
                        principalTable: "ExpenceType",
                        principalColumn: "ExpenceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Item2",
                columns: table => new
                {
                    Item2ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrencyID = table.Column<int>(nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "date", nullable: true),
                    ItemID = table.Column<int>(nullable: true),
                    ManufictureDate = table.Column<DateTime>(type: "date", nullable: true),
                    PurchasePrice = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    SalePrice = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item2", x => x.Item2ID);
                    table.ForeignKey(
                        name: "FK_Item2_Currency",
                        column: x => x.CurrencyID,
                        principalTable: "Currency",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item2_Item",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    PurchaseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bill_No = table.Column<string>(maxLength: 50, nullable: true),
                    CompanyID = table.Column<int>(nullable: true),
                    CorporationID = table.Column<int>(nullable: true),
                    CountryID = table.Column<int>(nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "date", nullable: true),
                    ItemID = table.Column<int>(nullable: true),
                    ManufictureDate = table.Column<DateTime>(type: "date", nullable: true),
                    PurchasePrice = table.Column<double>(nullable: true),
                    PurchaseState = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    SalePrice = table.Column<double>(nullable: true),
                    StockID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.PurchaseID);
                    table.ForeignKey(
                        name: "FK_Purchase_Company",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Corporation",
                        column: x => x.CorporationID,
                        principalTable: "Corporation",
                        principalColumn: "CorporationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Country",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Currency",
                        column: x => x.CurrencyID,
                        principalTable: "Currency",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Item",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Stock",
                        column: x => x.StockID,
                        principalTable: "Stock",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    SaleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bill_No = table.Column<string>(maxLength: 50, nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    CustomerID = table.Column<int>(nullable: true),
                    Discount = table.Column<double>(nullable: true),
                    ItemID = table.Column<int>(nullable: true),
                    Note = table.Column<string>(maxLength: 200, nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    SaleDate = table.Column<DateTime>(type: "date", nullable: true),
                    SaleState = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.SaleID);
                    table.ForeignKey(
                        name: "FK_Sale_Currency",
                        column: x => x.CurrencyID,
                        principalTable: "Currency",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Customer",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Item",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deal_CorporationID",
                table: "Deal",
                column: "CorporationID");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_CurrencyID",
                table: "Deal",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_CustomerID",
                table: "Deal",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_DealTypeID",
                table: "Deal",
                column: "DealTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Expence_EmployeeID",
                table: "Expence",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Expence_ExpenceTypeID",
                table: "Expence",
                column: "ExpenceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CategoryID",
                table: "Item",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CompanyID",
                table: "Item",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CountryID",
                table: "Item",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Item2_CurrencyID",
                table: "Item2",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Item2_ItemID",
                table: "Item2",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_CurrencyID",
                table: "Loan",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_CustomerID",
                table: "Loan",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CompanyID",
                table: "Purchase",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CorporationID",
                table: "Purchase",
                column: "CorporationID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CountryID",
                table: "Purchase",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CurrencyID",
                table: "Purchase",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_EmployeeID",
                table: "Purchase",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_ItemID",
                table: "Purchase",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_StockID",
                table: "Purchase",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayment_EmployeeID",
                table: "SalaryPayment",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CurrencyID",
                table: "Sale",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CustomerID",
                table: "Sale",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ItemID",
                table: "Sale",
                column: "ItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyExchangeRate");

            migrationBuilder.DropTable(
                name: "Deal");

            migrationBuilder.DropTable(
                name: "Expence");

            migrationBuilder.DropTable(
                name: "Item2");

            migrationBuilder.DropTable(
                name: "Loan");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "SalaryPayment");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "DealType");

            migrationBuilder.DropTable(
                name: "ExpenceType");

            migrationBuilder.DropTable(
                name: "Corporation");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
