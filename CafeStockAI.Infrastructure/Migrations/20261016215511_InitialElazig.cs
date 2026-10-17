using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeStockAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialElazig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cafestock");

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessSettings",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MigrationHistory",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceFingerprint = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductCount = table.Column<int>(type: "int", nullable: false),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false),
                    MovementCount = table.Column<int>(type: "int", nullable: false),
                    CountCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MigrationHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockAreas",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Area = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAreas", x => x.Id);
                    table.CheckConstraint("CK_StockAreas_Kind", "[Area] IN (1,2)");
                    table.ForeignKey(
                        name: "FK_StockAreas_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "cafestock",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "cafestock",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinimumStock = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SourceCell = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ImageKey = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.CheckConstraint("CK_Products_MinimumStock", "[MinimumStock] >= 0");
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "cafestock",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "cafestock",
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportedLocations",
                schema: "cafestock",
                columns: table => new
                {
                    ImportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceWarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedLocations", x => new { x.ImportId, x.SourceWarehouseId });
                    table.ForeignKey(
                        name: "FK_ImportedLocations_MigrationHistory_ImportId",
                        column: x => x.ImportId,
                        principalSchema: "cafestock",
                        principalTable: "MigrationHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImportedLocations_StockAreas_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "cafestock",
                        principalTable: "StockAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeBranches",
                schema: "cafestock",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBranches", x => new { x.EmployeeId, x.BranchId });
                    table.ForeignKey(
                        name: "FK_EmployeeBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "cafestock",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeBranches_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "cafestock",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockCounts",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceWarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReviewedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsDemo = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCounts", x => x.Id);
                    table.CheckConstraint("CK_StockCounts_Status", "[Status] IN (1,2,3)");
                    table.ForeignKey(
                        name: "FK_StockCounts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "cafestock",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockCounts_Employees_ReviewedById",
                        column: x => x.ReviewedById,
                        principalSchema: "cafestock",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockCounts_Employees_SubmittedById",
                        column: x => x.SubmittedById,
                        principalSchema: "cafestock",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsDemo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.CheckConstraint("CK_Transfers_DifferentAreas", "[SourceAreaId] <> [TargetAreaId]");
                    table.ForeignKey(
                        name: "FK_Transfers_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "cafestock",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_StockAreas_SourceAreaId",
                        column: x => x.SourceAreaId,
                        principalSchema: "cafestock",
                        principalTable: "StockAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_StockAreas_TargetAreaId",
                        column: x => x.TargetAreaId,
                        principalSchema: "cafestock",
                        principalTable: "StockAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                schema: "cafestock",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    UsernameKey = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "cafestock",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockCountLines",
                schema: "cafestock",
                columns: table => new
                {
                    CountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpectedBar = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    CountedBar = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    ExpectedDepot = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    CountedDepot = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    Expected = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Counted = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCountLines", x => new { x.CountId, x.ProductId });
                    table.CheckConstraint("CK_StockCountLines_Quantities", "[Expected] >= 0 AND [Counted] >= 0 AND ([ExpectedBar] IS NULL OR [ExpectedBar] >= 0) AND ([ExpectedDepot] IS NULL OR [ExpectedDepot] >= 0) AND ([CountedBar] IS NULL OR [CountedBar] >= 0) AND ([CountedDepot] IS NULL OR [CountedDepot] >= 0)");
                    table.ForeignKey(
                        name: "FK_StockCountLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "cafestock",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockCountLines_StockCounts_CountId",
                        column: x => x.CountId,
                        principalSchema: "cafestock",
                        principalTable: "StockCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                schema: "cafestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ActorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDemo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovements_Employees_ActorId",
                        column: x => x.ActorId,
                        principalSchema: "cafestock",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "cafestock",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_StockAreas_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "cafestock",
                        principalTable: "StockAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_StockCounts_CountId",
                        column: x => x.CountId,
                        principalSchema: "cafestock",
                        principalTable: "StockCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalSchema: "cafestock",
                        principalTable: "Transfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferLines",
                schema: "cafestock",
                columns: table => new
                {
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferLines", x => new { x.TransferId, x.ProductId });
                    table.CheckConstraint("CK_TransferLines_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_TransferLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "cafestock",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferLines_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalSchema: "cafestock",
                        principalTable: "Transfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Name",
                schema: "cafestock",
                table: "Branches",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                schema: "cafestock",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBranches_BranchId",
                schema: "cafestock",
                table: "EmployeeBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                schema: "cafestock",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedLocations_AreaId",
                schema: "cafestock",
                table: "ImportedLocations",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationHistory_SourceFingerprint",
                schema: "cafestock",
                table: "MigrationHistory",
                column: "SourceFingerprint",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId_Name",
                schema: "cafestock",
                table: "Products",
                columns: new[] { "CategoryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                schema: "cafestock",
                table: "Products",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "cafestock",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockAreas_BranchId_Area",
                schema: "cafestock",
                table: "StockAreas",
                columns: new[] { "BranchId", "Area" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockCountLines_ProductId",
                schema: "cafestock",
                table: "StockCountLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_BranchId",
                schema: "cafestock",
                table: "StockCounts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_ReviewedById",
                schema: "cafestock",
                table: "StockCounts",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_SubmittedById",
                schema: "cafestock",
                table: "StockCounts",
                column: "SubmittedById");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ActorId",
                schema: "cafestock",
                table: "StockMovements",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_AreaId_ProductId_OccurredAt",
                schema: "cafestock",
                table: "StockMovements",
                columns: new[] { "AreaId", "ProductId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CountId",
                schema: "cafestock",
                table: "StockMovements",
                column: "CountId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductId",
                schema: "cafestock",
                table: "StockMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_TransferId",
                schema: "cafestock",
                table: "StockMovements",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferLines_ProductId",
                schema: "cafestock",
                table: "TransferLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CreatedById",
                schema: "cafestock",
                table: "Transfers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SourceAreaId",
                schema: "cafestock",
                table: "Transfers",
                column: "SourceAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TargetAreaId",
                schema: "cafestock",
                table: "Transfers",
                column: "TargetAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Name",
                schema: "cafestock",
                table: "Units",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UsernameKey",
                schema: "cafestock",
                table: "UserAccounts",
                column: "UsernameKey",
                unique: true,
                filter: "[IsEnabled] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessSettings",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "EmployeeBranches",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "ImportedLocations",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "StockCountLines",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "StockMovements",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "TransferLines",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "UserAccounts",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "MigrationHistory",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "StockCounts",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "Transfers",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "StockAreas",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "cafestock");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "cafestock");
        }
    }
}
