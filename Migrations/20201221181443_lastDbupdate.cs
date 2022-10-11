using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ShawkanyDb.Migrations
{
    public partial class lastDbupdate : Migration
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
                    CurrencyID = table.Column<int>(nullable: false),
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
                name: "DealerType",
                columns: table => new
                {
                    DealerTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dealer = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerType", x => x.DealerTypeID);
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
                name: "ExpiredItem",
                columns: table => new
                {
                    ExpireID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Company = table.Column<string>(maxLength: 50, nullable: true),
                    Country = table.Column<string>(maxLength: 50, nullable: true),
                    Currency = table.Column<string>(maxLength: 50, nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "date", nullable: true),
                    Item = table.Column<string>(maxLength: 50, nullable: true),
                    LatineName = table.Column<string>(maxLength: 50, nullable: true),
                    ManufactureDate = table.Column<DateTime>(type: "date", nullable: true),
                    PurchasePrice = table.Column<double>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    SalePrice = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiredItem", x => x.ExpireID);
                });

            migrationBuilder.CreateTable(
                name: "ExpiredItemsDueToDays",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Days = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiredItemsDueToDays", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpoyeeID = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    NoteStatus = table.Column<string>(maxLength: 10, nullable: true),
                    RemindDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime", nullable: true)
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
                name: "Unit",
                columns: table => new
                {
                    UnitID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Unit = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.UnitID);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoginDate = table.Column<DateTime>(nullable: true),
                    LogoutDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginDetail", x => x.DetailId);
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
                name: "Dealler",
                columns: table => new
                {
                    DealerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    DealerName = table.Column<string>(maxLength: 50, nullable: true),
                    DealerTypeID = table.Column<int>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealler", x => x.DealerID);
                    table.ForeignKey(
                        name: "FK_Dealler_DealerType",
                        column: x => x.DealerTypeID,
                        principalTable: "DealerType",
                        principalColumn: "DealerTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalaryPayment",
                columns: table => new
                {
                    SalaryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeID = table.Column<int>(nullable: true),
                    PaidAmount = table.Column<int>(nullable: true),
                    PaidBy = table.Column<int>(nullable: true),
                    PaidDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryPayment", x => x.SalaryID);
                    table.ForeignKey(
                        name: "FK_Salary_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryPayment_Employee",
                        column: x => x.PaidBy,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Expence",
                columns: table => new
                {
                    ExpenceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Detail = table.Column<string>(maxLength: 200, nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    ExpenceAmount = table.Column<int>(nullable: false),
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
                    AmountInPackage = table.Column<int>(nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ItemID = table.Column<int>(nullable: true),
                    ManufictureDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PurchasePrice = table.Column<int>(nullable: true),
                    SalePrice = table.Column<int>(nullable: true),
                    TotalQuantity = table.Column<int>(nullable: true),
                    UnitID = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Item2_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deal",
                columns: table => new
                {
                    DealID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Credit = table.Column<double>(nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    DealTypeID = table.Column<int>(nullable: true),
                    DealerID = table.Column<int>(nullable: true),
                    Debit = table.Column<double>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    Loan = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deal", x => x.DealID);
                    table.ForeignKey(
                        name: "FK_Deal_Currency",
                        column: x => x.CurrencyID,
                        principalTable: "Currency",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deal_DealType",
                        column: x => x.DealTypeID,
                        principalTable: "DealType",
                        principalColumn: "DealTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deal_Dealler",
                        column: x => x.DealerID,
                        principalTable: "Dealler",
                        principalColumn: "DealerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deal_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    PurchaseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountInPackage = table.Column<int>(nullable: true),
                    Bill_No = table.Column<string>(maxLength: 50, nullable: true),
                    CompanyID = table.Column<int>(nullable: true),
                    CountryID = table.Column<int>(nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    DealerID = table.Column<int>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ItemID = table.Column<int>(nullable: true),
                    ManufactureDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "date", nullable: true),
                    PurchasePrice = table.Column<double>(nullable: true),
                    PurchaseState = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    SalePrice = table.Column<double>(nullable: true),
                    StockID = table.Column<int>(nullable: true),
                    UnitID = table.Column<int>(nullable: true)
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
                        name: "FK_Purchase_Dealler",
                        column: x => x.DealerID,
                        principalTable: "Dealler",
                        principalColumn: "DealerID",
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
                    table.ForeignKey(
                        name: "FK_Purchase_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    SaleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountInPackage = table.Column<int>(nullable: true),
                    Bill_No = table.Column<string>(maxLength: 50, nullable: true),
                    CompanyID = table.Column<int>(nullable: true),
                    CountryID = table.Column<int>(nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    DealerID = table.Column<int>(nullable: true),
                    Discount = table.Column<double>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ItemID = table.Column<int>(nullable: true),
                    ManufactureDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Note = table.Column<string>(maxLength: 200, nullable: true),
                    PurchasePrice = table.Column<double>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    SaleDate = table.Column<DateTime>(type: "date", nullable: true),
                    SalePrice = table.Column<double>(nullable: true),
                    SaleState = table.Column<string>(maxLength: 50, nullable: true),
                    SaleType = table.Column<string>(maxLength: 50, nullable: true),
                    StockID = table.Column<int>(nullable: true),
                    tempCustomerName = table.Column<string>(maxLength: 50, nullable: true),
                    UnitID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.SaleID);
                    table.ForeignKey(
                        name: "FK_Sale_Company",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Country",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Currency",
                        column: x => x.CurrencyID,
                        principalTable: "Currency",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Dealler1",
                        column: x => x.DealerID,
                        principalTable: "Dealler",
                        principalColumn: "DealerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Item",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Stock",
                        column: x => x.StockID,
                        principalTable: "Stock",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deal_CurrencyID",
                table: "Deal",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_DealTypeID",
                table: "Deal",
                column: "DealTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_DealerID",
                table: "Deal",
                column: "DealerID");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_EmployeeID",
                table: "Deal",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Dealler_DealerTypeID",
                table: "Dealler",
                column: "DealerTypeID");

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
                name: "IX_Item2_UnitID",
                table: "Item2",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CompanyID",
                table: "Purchase",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CountryID",
                table: "Purchase",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CurrencyID",
                table: "Purchase",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_CorporationID",
                table: "Purchase",
                column: "DealerID");

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
                name: "IX_Purchase_UnitID",
                table: "Purchase",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayment_EmployeeID",
                table: "SalaryPayment",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPayment_PaidBy",
                table: "SalaryPayment",
                column: "PaidBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CompanyID",
                table: "Sale",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CountryID",
                table: "Sale",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CurrencyID",
                table: "Sale",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_DealerID",
                table: "Sale",
                column: "DealerID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_EmployeeID",
                table: "Sale",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ItemID",
                table: "Sale",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_StockID",
                table: "Sale",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_UnitID",
                table: "Sale",
                column: "UnitID");
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
                name: "ExpiredItem");

            migrationBuilder.DropTable(
                name: "ExpiredItemsDueToDays");

            migrationBuilder.DropTable(
                name: "Item2");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "SalaryPayment");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "UserLoginDetail");

            migrationBuilder.DropTable(
                name: "DealType");

            migrationBuilder.DropTable(
                name: "ExpenceType");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Dealler");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "DealerType");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
