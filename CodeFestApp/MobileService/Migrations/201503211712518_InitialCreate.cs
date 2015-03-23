using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;

namespace CodeFestApp.MobileService.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "codefestapp.LectureAttitudes",
                c => new
                    {
                        Id = c.String(nullable: false,
                                      maxLength: 128,
                                      annotations: new Dictionary<string, AnnotationValues>
                                          {
                                              {
                                                  "ServiceTableColumn",
                                                  new AnnotationValues(oldValue: null, newValue: "Id")
                                              },
                                          }),
                        LectureId = c.Int(nullable: false),
                        Attitude = c.Int(nullable: false),
                        DeviceIdentity = c.String(),
                        Version = c.Binary(nullable: false,
                                           fixedLength: true,
                                           timestamp: true,
                                           storeType: "rowversion",
                                           annotations: new Dictionary<string, AnnotationValues>
                                               {
                                                   {
                                                       "ServiceTableColumn",
                                                       new AnnotationValues(oldValue: null, newValue: "Version")
                                                   },
                                               }),
                        CreatedAt = c.DateTimeOffset(nullable: false,
                                                     precision: 7,
                                                     annotations: new Dictionary<string, AnnotationValues>
                                                         {
                                                             {
                                                                 "ServiceTableColumn",
                                                                 new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                                             },
                                                         }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                                                     annotations: new Dictionary<string, AnnotationValues>
                                                         {
                                                             {
                                                                 "ServiceTableColumn",
                                                                 new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                                             },
                                                         }),
                        Deleted = c.Boolean(nullable: false,
                                            annotations: new Dictionary<string, AnnotationValues>
                                                {
                                                    {
                                                        "ServiceTableColumn",
                                                        new AnnotationValues(oldValue: null, newValue: "Deleted")
                                                    },
                                                }),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreatedAt, clustered: true);

        }
        
        public override void Down()
        {
            DropIndex("codefestapp.LectureAttitudes", new[] { "CreatedAt" });
            DropTable("codefestapp.LectureAttitudes",
                      removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                          {
                              {
                                  "CreatedAt",
                                  new Dictionary<string, object>
                                      {
                                          { "ServiceTableColumn", "CreatedAt" },
                                      }
                              },
                              {
                                  "Deleted",
                                  new Dictionary<string, object>
                                      {
                                          { "ServiceTableColumn", "Deleted" },
                                      }
                              },
                              {
                                  "Id",
                                  new Dictionary<string, object>
                                      {
                                          { "ServiceTableColumn", "Id" },
                                      }
                              },
                              {
                                  "UpdatedAt",
                                  new Dictionary<string, object>
                                      {
                                          { "ServiceTableColumn", "UpdatedAt" },
                                      }
                              },
                              {
                                  "Version",
                                  new Dictionary<string, object>
                                      {
                                          { "ServiceTableColumn", "Version" },
                                      }
                              },
                          });
        }
    }
}
