namespace NaijaStartupWeb.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class createDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddOnServices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServiceName = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        Registration_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company_Registration", t => t.Registration_Id)
                .Index(t => t.Registration_Id);
            
            CreateTable(
                "dbo.Company_Registration",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CacRegistrationNumber = c.String(),
                        CompanyName = c.String(),
                        CompanyType = c.String(),
                        AlternateCompanyName = c.String(),
                        AlternateCompanyType = c.String(),
                        FinancialYearEnd = c.String(),
                        BusinessActivity = c.String(),
                        ApprovalStatus = c.String(),
                        SndBusinessActivity = c.String(),
                        IsAddressRegistered = c.Boolean(nullable: false),
                        LocalResidentDirector = c.Boolean(nullable: false),
                        LocalResidentDirectorPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Postcode = c.String(),
                        LocalDirector = c.Boolean(nullable: false),
                        CompanyCapitalCurrency = c.String(),
                        ShareHolderName = c.String(),
                        NoOfSharesIssue = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegCompleted = c.Boolean(nullable: false),
                        IsCacAvailable = c.Boolean(nullable: false),
                        SharePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SharesAllocated = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        Package_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Packages", t => t.Package_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Package_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Comp_Incentives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        Incentive_Id = c.Int(),
                        Registration_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Incentives", t => t.Incentive_Id)
                .ForeignKey("dbo.Company_Registration", t => t.Registration_Id)
                .Index(t => t.Incentive_Id)
                .Index(t => t.Registration_Id);
            
            CreateTable(
                "dbo.Incentives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncentiveName = c.String(),
                        Description = c.String(),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Company_Officers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(),
                        Gender = c.String(),
                        Designation = c.String(),
                        Id_Type = c.String(),
                        Id_Number = c.String(),
                        Nationality = c.String(),
                        Birth_Country = c.String(),
                        Phone_No = c.String(),
                        Dob = c.String(),
                        Email = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        PostalCode = c.String(),
                        MobileNo = c.String(),
                        Identification = c.Binary(),
                        Proficiency = c.Binary(),
                        CerficationOfBirth = c.Binary(),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        Registration_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company_Registration", t => t.Registration_Id)
                .Index(t => t.Registration_Id);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApiRequest = c.String(),
                        ApiResponse = c.String(),
                        Status = c.Boolean(nullable: false),
                        Message = c.String(),
                        PaymentType = c.String(),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        Registration_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company_Registration", t => t.Registration_Id)
                .Index(t => t.Registration_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(),
                        LastName = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Address = c.String(),
                        IsActive = c.Boolean(),
                        Role = c.String(),
                        CreationTime = c.DateTime(),
                        CreatorUserId = c.String(),
                        ModificationTime = c.DateTime(),
                        ModificationUserId = c.String(),
                        IsDeleted = c.Boolean(),
                        DeletionUserId = c.String(),
                        DeletionTime = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ChatHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostIncooperationName = c.String(),
                        Group = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        IsTicket = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        Company_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company_Registration", t => t.Company_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Company_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ChatThreads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        document = c.Binary(),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        Chat_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChatHeaders", t => t.Chat_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Chat_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Message = c.String(),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        description = c.String(),
                        field1 = c.String(),
                        field2 = c.String(),
                        CreationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        CreatorUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        ModificationTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        ModificationUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        DeletionTime = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "(getutcdate())")
                                },
                            }),
                        DeletionUserId = c.String(nullable: false, maxLength: 50, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "('')")
                                },
                            }),
                        IsDeleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ChatHeaders", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatHeaders", "Company_Id", "dbo.Company_Registration");
            DropForeignKey("dbo.ChatThreads", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatThreads", "Chat_Id", "dbo.ChatHeaders");
            DropForeignKey("dbo.Company_Registration", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "Registration_Id", "dbo.Company_Registration");
            DropForeignKey("dbo.Company_Registration", "Package_Id", "dbo.Packages");
            DropForeignKey("dbo.Company_Officers", "Registration_Id", "dbo.Company_Registration");
            DropForeignKey("dbo.Comp_Incentives", "Registration_Id", "dbo.Company_Registration");
            DropForeignKey("dbo.Comp_Incentives", "Incentive_Id", "dbo.Incentives");
            DropForeignKey("dbo.AddOnServices", "Registration_Id", "dbo.Company_Registration");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ChatThreads", new[] { "User_Id" });
            DropIndex("dbo.ChatThreads", new[] { "Chat_Id" });
            DropIndex("dbo.ChatHeaders", new[] { "User_Id" });
            DropIndex("dbo.ChatHeaders", new[] { "Company_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Payments", new[] { "Registration_Id" });
            DropIndex("dbo.Company_Officers", new[] { "Registration_Id" });
            DropIndex("dbo.Comp_Incentives", new[] { "Registration_Id" });
            DropIndex("dbo.Comp_Incentives", new[] { "Incentive_Id" });
            DropIndex("dbo.Company_Registration", new[] { "User_Id" });
            DropIndex("dbo.Company_Registration", new[] { "Package_Id" });
            DropIndex("dbo.AddOnServices", new[] { "Registration_Id" });
            DropTable("dbo.Settings",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Contacts",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.ChatThreads",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.ChatHeaders",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Payments",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.Packages",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.Company_Officers",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.Incentives",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.Comp_Incentives",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.Company_Registration",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
            DropTable("dbo.AddOnServices",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "CreatorUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "DeletionTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "DeletionUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "0" },
                        }
                    },
                    {
                        "ModificationTime",
                        new Dictionary<string, object>
                        {
                            { "Default", "(getutcdate())" },
                        }
                    },
                    {
                        "ModificationUserId",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                    {
                        "ServiceName",
                        new Dictionary<string, object>
                        {
                            { "Default", "('')" },
                        }
                    },
                });
        }
    }
}
